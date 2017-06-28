using System;

namespace Grades
{
    internal class GradeStatistics
    {
        public GradeStatistics()
        {
            AverageGrade = 0;
            HighestGrade = 0;
            LowestGrade = float.MaxValue;
        }
        public float AverageGrade;
        public float HighestGrade;
        public float LowestGrade;
    }
}