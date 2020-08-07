using System;


namespace Assets.Scripts.ClientUtilities.Extensions
{
    public static class StringExtensions
    {
        public static string FormatTime(long CurrentTime,long EpochTime)
        {
            if(CurrentTime <= 0)
                return "00:00:00";
            double seconds = CurrentTime -EpochTime;
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            if (seconds < 3600)
                return string.Format("{0:D2}:{1:D2}",
                    time.Minutes,
                    time.Seconds);
            else if (seconds < 86400)
                return string.Format("{0:D2}:{1:D2}:{2:D2}",
               time.Hours,
               time.Minutes,
               time.Seconds);
            else
                return string.Format("{0:D1}d {1:D2}:{2:D2}",
               time.Days,
               time.Hours,
               time.Minutes);
        }
    }
}