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
        public const string ComparisonName = "My Previous Splits";
        public const string ShortComparisonName = "My Previous";
        public string Name => ComparisonName;
        
        public IRun Run { get; set; }

        public CompareAgainstPreviousComparisonGenerator(IRun run)
        {
            Run = run;
        }

        public void Generate(ISettings settings)
        {
            TimeSpan? currentSegmentRTA = TimeSpan.Zero;
            TimeSpan? previousSplitTimeRTA = TimeSpan.Zero;
            TimeSpan? currentSegmentGameTime = TimeSpan.Zero;
            TimeSpan? previousSplitTimeGameTime = TimeSpan.Zero;

            var runs = Run.Skip(0).Take(Run.Count).ToArray();
            var index = 0;
            foreach (var segment in Run)
            {
                currentSegmentGameTime = previousSplitTimeGameTime + runs[index].CompareAgainstPrevious().GameTime;
                previousSplitTimeGameTime = runs[index].CompareAgainstPrevious().GameTime;

                currentSegmentRTA = previousSplitTimeRTA + runs[index].CompareAgainstPrevious().RealTime;
                previousSplitTimeRTA = runs[index].CompareAgainstPrevious().RealTime;

                segment.Comparisons[Name] = new Time(
                    currentSegmentRTA,
                    currentSegmentGameTime);
                index++;
            }
        }
    }
}
