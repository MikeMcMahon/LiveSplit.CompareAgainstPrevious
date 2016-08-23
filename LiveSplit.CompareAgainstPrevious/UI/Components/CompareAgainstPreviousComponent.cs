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

namespace LiveSplit.CompareAgainstPrevious
{
    class CompareAgainstPreviousComponent : LogicComponent
    {

        //public CompareAgainstPreviousSettings Settings;
        public LiveSplitState State;
        private CompareAgainstPreviousComparisonGenerator _Generator;

        public CompareAgainstPreviousComponent(LiveSplitState state)
        {
            //Settings = new CompareAgainstPreviousSettings();

            State = state;
            _Generator = new CompareAgainstPreviousComparisonGenerator(state.Run);
            _Generator.RunChanged += _Generator_RunChanged;
            LoadLastRunFromFile(state.Run.FilePath);

            State.Run.ComparisonGenerators.Add(_Generator);
            state.OnSplit += State_OnSplit;
            state.OnReset += State_OnReset;

            
        }

        private void LoadLastRunFromFile(string filePath)
        {
            XDocument document;
            using (var stream = File.OpenRead(filePath))
            {                
                document = XDocument.Load(stream);
            }

            var segments = document.Descendants("Run").Descendants("Segments").Descendants("Segment");
            foreach (var segment in segments)
            {
                // Look at the segment history and take the final node (ID should be the greatest)
                var segmentHistories = segment.Descendants("Time");
                var latestRunId = segmentHistories.Max(e => int.Parse(e.Attribute("id").Value));
                var latestRun = segmentHistories.Where(item => int.Parse(item.Attribute("id").Value) == latestRunId).First();

                // Take the latest run and stuff it into the comparison generator as the -latest- run so it can be compared against
            }
        }

        private void _Generator_RunChanged(object sender, EventArgs e)
        {
            LoadLastRunFromFile(State.Run.FilePath);
        }

        private void State_OnReset(object sender, TimerPhase value)
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
                foreach (Segment split in State.Run)
                {
                    var newPrevSplit = new Time(split.CompareAgainstPrevious());
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
                return "Compare Against Previous Splits";
            }
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        
        public override Control GetSettingsControl(LayoutMode mode)
        {
            return null;
            //return Settings;
        }

        public override XmlNode GetSettings(XmlDocument document)
        {
            return null;
            //return Settings.GetSettings(document);
        }

        public override void SetSettings(XmlNode settings)
        {
            //Settings.SetSettings(settings);
        }

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            // Doing nothing with this
        }
    }
}
