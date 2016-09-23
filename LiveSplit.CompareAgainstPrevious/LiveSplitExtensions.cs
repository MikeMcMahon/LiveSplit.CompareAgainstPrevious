using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.CompareAgainstPrevious
{
    public static class LiveSplitExtensions
    {
        public static Time CompareAgainstPrevious(this ISegment segment)
        {
            return segment.Comparisons[CompareAgainstPreviousComparisonGenerator.ComparisonName];
        }

        public static void CompareAgainstPrevious(this ISegment segment, Time newTime)
        {
            segment.Comparisons[CompareAgainstPreviousComparisonGenerator.ComparisonName] = newTime;
        }
    }
}
