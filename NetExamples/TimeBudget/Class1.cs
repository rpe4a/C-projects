using System;
using System.Diagnostics;
using System.Globalization;

namespace TimeBudget
{
    public class TimeBudget
    {
        public static readonly TimeBudget Infinite = new TimeBudget(TimeSpan.FromHours(1));
        public static readonly TimeBudget Expired = new TimeBudget(TimeSpan.Zero);

        public static TimeBudget StartNew(TimeSpan budget, TimeSpan precision)
        {
            return new TimeBudget(budget, precision).Start();
        }

        public static TimeBudget StartNew(TimeSpan budget)
        {
            return new TimeBudget(budget).Start();
        }

        public static TimeBudget StartNew(int budgetMs, int precisionMs)
        {
            return new TimeBudget(TimeSpan.FromMilliseconds(budgetMs), TimeSpan.FromMilliseconds(precisionMs)).Start();
        }

        public static TimeBudget StartNew(int budgetMs)
        {
            return new TimeBudget(TimeSpan.FromMilliseconds(budgetMs)).Start();
        }

        private readonly TimeSpan budget;
        private readonly TimeSpan precision;
        private readonly Stopwatch watch;

        public TimeBudget(TimeSpan budget, TimeSpan precision)
        {
            this.budget = budget;
            this.precision = precision;
            watch = new Stopwatch();
        }

        public TimeBudget(TimeSpan budget)
            : this(budget, TimeSpan.FromMilliseconds(5))
        {
        }

        public TimeSpan Budget
        {
            get { return budget; }
        }

        public TimeSpan Precision
        {
            get { return precision; }
        }

        public TimeBudget Start()
        {
            watch.Start();
            return this;
        }

        public TimeSpan Remaining()
        {
            var remaining = budget - watch.Elapsed;
            return remaining < precision
                ? TimeSpan.Zero
                : remaining;
        }

        public TimeSpan Elapsed()
        {
            return watch.Elapsed;
        }

        public TimeSpan TryAcquireTime(TimeSpan neededTime)
        {
            return TimeSpanExtensions.Min(neededTime, Remaining());
        }

        public bool HasExpired()
        {
            return Remaining() <= TimeSpan.Zero;
        }
    }

    public static class TimeSpanExtensions
    {
        public static TimeSpan Multiply(this TimeSpan ts, double multiplier)
        {
            return TimeSpan.FromTicks((long)(ts.Ticks * multiplier));
        }

        public static TimeSpan Divide(this TimeSpan ts, double divisor)
        {
            return TimeSpan.FromTicks((long)(ts.Ticks / divisor));
        }

        public static TimeSpan Min(TimeSpan ts1, TimeSpan ts2)
        {
            return ts1 <= ts2 ? ts1 : ts2;
        }

        public static TimeSpan Max(TimeSpan ts1, TimeSpan ts2)
        {
            return ts1 >= ts2 ? ts1 : ts2;
        }

        public static string ToPrettyString(this TimeSpan time)
        {
            if (time.TotalDays >= 1)
                return time.TotalDays.ToString("0.###", CultureInfo.InvariantCulture) + " days";

            if (time.TotalHours >= 1)
                return time.TotalHours.ToString("0.###", CultureInfo.InvariantCulture) + " hours";

            if (time.TotalMinutes >= 1)
                return time.TotalMinutes.ToString("0.###", CultureInfo.InvariantCulture) + " minutes";

            if (time.TotalSeconds >= 1)
                return time.TotalSeconds.ToString("0.###", CultureInfo.InvariantCulture) + " seconds";

            if (time.TotalMilliseconds >= 1)
                return time.TotalMilliseconds.ToString("0.###", CultureInfo.InvariantCulture) + " milliseconds";

            return time.ToString();
        }
    }
}

