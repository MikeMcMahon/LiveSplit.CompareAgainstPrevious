using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.CompareAgainstPrevious
{
    class CompareAgainstPreviousComponent : LogicComponent
    {

        //public CompareAgainstPreviousSettings Settings;
        public LiveSplitState State;

        public CompareAgainstPreviousComponent(LiveSplitState state)
        {
            //Settings = new CompareAgainstPreviousSettings();

            State = state;
            State.Run.ComparisonGenerators.Add(new CompareAgainstPreviousComparisonGenerator(State.Run));

            state.OnSplit += State_OnSplit;
            state.OnReset += State_OnReset;
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
