using System;
using System.Collections.Generic;

namespace SetDuration
{
    /// <summary>
    /// Defines a test result in terms of start time and how many sub test results it contains.
    /// A test result can have more than one sub test result.
    /// Each sub test result are test results themselves.
    /// The duration of a test result is computed as follows:
    ///     Duration = Sum the duration of each sub testresult.
    /// </summary>
    /// <example>A test result A thats start at 2, and containing two sub test results each
    /// starting at 3 and 6 will yield the following sequence of durations:
    /// Duration of A = 3
    /// Duration of B = 3
    /// Duration of C = 0
    /// </example>
    class TestResult
    {
        public bool addSubResult(ref TestResult iTestResult)
        {
            bool wSuccess = true;

            // add the new sub 
            if (iTestResult != null) 
            {
                mSubResults.Add(iTestResult);

                // the top result must have the same start time as the first sub
                if (mSubResults.Count == 1) mStartTime = mSubResults[0].mStartTime;
            }
            else  wSuccess = false;

            // Compute end times

            // recompute duration whenever the subs change,
            // this is recursive through to the subs
            foreach (TestResult iSubResult in mSubResults)
            {
                m_Duration += Math.Abs(iSubResult.mStartTime - mStartTime);
            }

            return wSuccess;
        }

        public double mStartTime { get; set; } = 0.0;
        public Double mDuration
        {
            get { return m_Duration; }
        }

        private Double mEndTime { get; set; } = 0.0;
        private Double m_Duration { get; set; } = 0.0;
        private List<TestResult> mSubResults = new List<TestResult>();


    }

    class Program
    {
        private static void Assert(bool iInput, string iID)
        {
            if (iInput)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine(iID + " -> Success");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine(iID + " -> Fail");
            }
        }

        static void Main(string[] args)
        {
            // test 1: duration (D) of single first top result (TR) is 0.0
            bool wSuccess = true;
            TestResult wTR0 = new TestResult();
            wTR0.mStartTime = 2.0;
            Assert(wTR0.mDuration.Equals(0.0), (string)"Test 1");


            // test 2:
            // A: D(TR[0]) is 0.0 when 1 Sub TR (STR) exists
            // B: D(TR[0].STR[0]) is 0.0 when single sub
            wSuccess = true;

            TestResult wSTR0 = new TestResult();
            wSTR0.mStartTime = 3.0;
            wSuccess = wTR0.addSubResult(ref wSTR0);

            wSuccess &= wSTR0.mDuration.Equals(0.0);
            Assert(wSuccess, (string)"Test 2.A");

            wSuccess &= wSTR0.mDuration.Equals(0.0);
            Assert(wSuccess, (string)"Test 2.B");


            // test 3:
            // A test result A thats start at 2, and containing two sub test results each
            // starting at 3 and 6 will yield the following sequence of durations:
            // Duration of A = 3
            // Duration of B = 3
            // Duration of C = 0
            wSuccess = true;
            const Double wADuration = 3.0;
            const Double wBDuration = 3.0;
            const Double wCDuration = 0.0;

            TestResult wSTR1 = new TestResult();
            wSTR1.mStartTime = 6.0;
            wSuccess = wTR0.addSubResult(ref wSTR1);
            wSuccess = wTR0.mDuration.Equals(wADuration);
            Assert(wSuccess, (string)"Test 3.A");

            wSuccess = wSTR0.mDuration.Equals(wBDuration);
            Assert(wSuccess, (string)"Test 3.B");

            wSuccess = wSTR1.mDuration.Equals(wCDuration);
            Assert(wSuccess, (string)"Test 3.C");

            // The End
            Console.WriteLine("< Press Any Key >");
            Console.ReadKey();
        }
    }
}