using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Windows.Forms;
using LiveSplit.CompareAgainstPrevious.UI;

namespace LiveSplit.CompareAgainstPrevious
{
    class CompareAgainstPreviousComponent : LogicComponent
    {

        //public CompareAgainstPreviousSettings Settings;
        public LiveSplitState State;
        private CompareAgainstPreviousComparisonGenerator _Generator;
        private bool _SuccessfulRun;
        public CompareAgainstPreviousSettings Settings { get; set; }

        public CompareAgainstPreviousComponent(LiveSplitState state)
        {
            //Settings = new CompareAgainstPreviousSettings();

            State = state;
            _Generator = new CompareAgainstPreviousComparisonGenerator(state.Run);
            _Generator.RunChanged += _Generator_RunChanged;

            if (File.Exists(state.Run.FilePath))
                LoadLastRunFromFile(state.Run.FilePath);

            State.Run.ComparisonGenerators.Add(_Generator);

            state.OnSplit += State_OnSplit;
            State.OnSkipSplit += State_OnSkipSplit;
            state.OnUndoSplit += State_OnUndoSplit;
            state.OnReset += State_OnReset;

            Settings = new CompareAgainstPreviousSettings();
            Settings.ComparisonNameChanged += Settings_ComparisonNameChanged;
        }

        private void Settings_ComparisonNameChanged(object sender, EventArgs e)
        {
            var settings = sender as CompareAgainstPreviousSettings;
            var name = CompareAgainstPreviousComparisonGenerator.ComparisonName;
            var newName = settings.ComparisonName;

            // Essentially stolen logic from the LayoutSettingsDialog
            // First thing first, change the currentcomparison to match what it should be
            if (State.CurrentComparison == name)
                State.CurrentComparison = newName;

            // Then for each segment we need to move over the comparison to the new name
            foreach (var seg in State.Run)
            {
                seg.Comparisons[newName] = seg.Comparisons[name];
                seg.Comparisons.Remove(CompareAgainstPreviousComparisonGenerator.ComparisonName);
            }

            // Then update the name the comparisongenerator responds to
            CompareAgainstPreviousComparisonGenerator.ComparisonName = newName;
        }

        private void LoadLastRunFromFile(string filePath)
        {
            XDocument document;
            using (var stream = File.OpenRead(filePath))
            {
                document = XDocument.Load(stream);
            }


            TimeSpan? currentSegmentRTA = TimeSpan.Zero;
            TimeSpan? previousSplitTimeRTA = TimeSpan.Zero;
            TimeSpan? currentSegmentGameTime = TimeSpan.Zero;
            TimeSpan? previousSplitTimeGameTime = TimeSpan.Zero;

            var segments = document.Descendants("Run").Descendants("Segments").Descendants("Segment");
            var segmentSplitTimes = new List<Time>();
            foreach (var segment in segments)
            {
                // Look at the segment history and take the final node (ID should be the greatest)
                var segmentHistories = segment.Descendants("Time");
                if (segmentHistories.Count() == 0)
                {
                    // oh gosh we don't have splits yet for this segment
                    segmentSplitTimes.Add(new Time(realTime: null, gameTime: null));
                    continue;
                }

                var latestRunId = segmentHistories.Max(e => int.Parse(e.Attribute("id").Value));
                var latestRunSplit = segmentHistories.Where(item => int.Parse(item.Attribute("id").Value) == latestRunId).First();

                // Take the latest run and stuff it into the comparison generator 
                // as the -latest- run so it can be compared against
                var realTimeNode = latestRunSplit.Descendants("RealTime");
                var gameTimeNode = latestRunSplit.Descendants("GameTime");

                var overrideSegmentTime = new Time();

                // Account for the skipped splits by nulling them
                if (realTimeNode.Count() == 0)
                    overrideSegmentTime.RealTime = null;
                else
                    overrideSegmentTime.RealTime = TimeSpan.Parse(realTimeNode.First().Value);

                if (gameTimeNode.Count() == 0)
                    overrideSegmentTime.GameTime = null;
                else
                    overrideSegmentTime.GameTime = TimeSpan.Parse(gameTimeNode.First().Value);

                segmentSplitTimes.Add(overrideSegmentTime);
            }
            _Generator.SetOverrideSegments(segmentSplitTimes);
        }

        private void _Generator_RunChanged(object sender, EventArgs e)
        {
            var generator = sender as CompareAgainstPreviousComparisonGenerator;

            if (!String.IsNullOrEmpty(generator.Run.FilePath) && File.Exists(generator.Run.FilePath))
                LoadLastRunFromFile(generator.Run.FilePath);
        }

        private void State_OnReset(object sender, TimerPhase value)
        {
            // If the run was reset before it was complete we don't want to compare against it... 
            if (!_SuccessfulRun)
                _Generator.IsReset = true;

            _SuccessfulRun = false;
        }

        private void State_OnUndoSplit(object sender, EventArgs e)
        {
        }

        private void State_OnSkipSplit(object sender, EventArgs e)
        {
        }

        private void State_OnSplit(object sender, EventArgs e)
        {
            
            TimeSpan? currentSegmentRTA = TimeSpan.Zero;
            TimeSpan? previousSplitTimeRTA = TimeSpan.Zero;
            TimeSpan? currentSegmentGameTime = TimeSpan.Zero;
            TimeSpan? previousSplitTimeGameTime = TimeSpan.Zero;

            // When a run ends we want to update the splits for the monitored split for real time or game time
            if (State.CurrentPhase == TimerPhase.Ended)
            {
                // When a run ends we hit this before reset, so we need to make sure
                // We don't ignore the run
                _SuccessfulRun = true;   
                foreach (Segment split in State.Run)
                {
                    // We need to look at the current splits so we can see if there was a skipped split
                    // Easier than trying to override the onskipsplit/unundosplit functionality
                    // Here we can just look at the currently ended run - because ultimately that's what
                    // we want to stuff into the previous split comparison
                    var newPrevSplit = new Time(realTime: null, gameTime: null);
                    if (split.SplitTime.RealTime != null)
                    {
                        currentSegmentRTA = split.SplitTime.RealTime - previousSplitTimeRTA;
                        previousSplitTimeRTA = split.SplitTime.RealTime;
                        newPrevSplit.RealTime = currentSegmentRTA;
                    }
                    if (split.SplitTime.GameTime != null)
                    {
                        currentSegmentGameTime = split.SplitTime.GameTime - previousSplitTimeGameTime;
                        previousSplitTimeGameTime = split.SplitTime.GameTime;
                        newPrevSplit.GameTime = currentSegmentGameTime;
                    }
                    split.CompareAgainstPrevious(newPrevSplit);
                }
            }
        }

        public override string ComponentName
        {
            get
            {
                return "Compare Against Previous Run";
            }
        }

        public override void Dispose()
        {
            State.Run.ComparisonGenerators.Remove(_Generator);

            State.OnSplit -= State_OnSplit;
            State.OnReset -= State_OnReset;
            State.OnSkipSplit -= State_OnSkipSplit;
            _Generator.RunChanged -= _Generator_RunChanged;
        }

        
        public override Control GetSettingsControl(LayoutMode mode)
        {
            return Settings;
        }

        public override XmlNode GetSettings(XmlDocument document)
        {
            return Settings.GetSettings(document);
        }

        public override void SetSettings(XmlNode settings)
        {
            Settings.SetSettings(settings);
        }

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            // Doing nothing with this
        }
    }
}
