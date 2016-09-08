using LiveSplit.Model.Comparisons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveSplit.Model;
using LiveSplit.Options;

namespace LiveSplit.CompareAgainstPrevious
{
    class CompareAgainstPreviousComparisonGenerator : IComparisonGenerator
    {
        public static string ComparisonName = "My Previous Run";
        public const string ShortComparisonName = "Previous Run";
        public string Name => ComparisonName;
        private IRun _Run;

        public event EventHandler RunChanged;
        private IList<Time> _OverrideSegments = new List<Time>();
        
        public bool IsReset { get; set; }

        public IRun Run
        {
            get { return _Run; }
            set
            {
                _Run = value;
                RunChanged?.Invoke(this, new EventArgs());
            }
        }

        public CompareAgainstPreviousComparisonGenerator(IRun run)
        {
            Run = run;
        }

        public void SetOverrideSegments(IList<Time> segments)
        {
            _OverrideSegments = segments;
        }

        public void Generate(ISettings settings)
        {
            if (IsReset)
            {
                IsReset = false;
                return;
            }

            TimeSpan? currentSegmentRTA = TimeSpan.Zero;
            TimeSpan? previousSplitTimeRTA = TimeSpan.Zero;
            TimeSpan? currentSegmentGameTime = TimeSpan.Zero;
            TimeSpan? previousSplitTimeGameTime = TimeSpan.Zero;

            var runs = Run.Skip(0).Take(Run.Count).ToArray();
            var index = 0;

            if (_OverrideSegments.Count() == 0)
            {
                foreach (var segment in Run)
                {

                    var newPrevSplit = new Time(realTime: null, gameTime: null);
                    if (runs[index].CompareAgainstPrevious().RealTime != null)
                    {
                        currentSegmentRTA = previousSplitTimeRTA + runs[index].CompareAgainstPrevious().RealTime;
                        previousSplitTimeRTA = currentSegmentRTA;
                        newPrevSplit.RealTime = currentSegmentRTA;
                    }
                    if (runs[index].CompareAgainstPrevious().GameTime != null)
                    {
                        currentSegmentGameTime = previousSplitTimeGameTime + runs[index].CompareAgainstPrevious().GameTime;
                        previousSplitTimeGameTime = currentSegmentGameTime;
                        newPrevSplit.GameTime = currentSegmentGameTime;
                    }

                    segment.Comparisons[Name] = newPrevSplit;
                    index++;
                }
            }
            else
            {
                foreach (var segment in Run)
                {
                    TimeSpan? gameTimeSegment = null;
                    TimeSpan? rtaSegment = null;
                    if (_OverrideSegments[index].GameTime != null)
                    {
                        currentSegmentGameTime = previousSplitTimeGameTime + _OverrideSegments[index].GameTime;
                        previousSplitTimeGameTime = currentSegmentGameTime;

                        gameTimeSegment = currentSegmentGameTime;
                    }


                    if (_OverrideSegments[index].RealTime != null)
                    {
                        currentSegmentRTA = previousSplitTimeRTA + _OverrideSegments[index].RealTime;
                        previousSplitTimeRTA = currentSegmentRTA;

                        rtaSegment = currentSegmentRTA;
                    }

                    segment.Comparisons[Name] = new Time(
                        rtaSegment,
                        gameTimeSegment);
                    index++;
                }
            }
            _OverrideSegments.Clear();
        }
    }
}
