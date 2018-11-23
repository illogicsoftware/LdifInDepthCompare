using System;
namespace LdifInDepthCompare
{
    public class Utils
    {

        public static String tsToStr(TimeSpan ts)
        {
            return ts.Days + " day(s) " + ts.Hours + " hour(s) " + ts.Minutes + " minute(s) " + ts.Seconds + " second(s) " + ts.Milliseconds + " milli(s)";  
        }
    }
}
