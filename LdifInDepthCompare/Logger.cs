/**Author: Simonluca Scillitani
 * Company: Illogic 
 * http://www.illogic.software
 * summary: This class is a simple logger
 */

using System;
namespace LdifInDepthCompare
{
    public class Logger
    {
        public static void write(Constants.DEBUG_LEVELS level,string message)
        {
            if (level >= Constants.CURRENT_DEBUG_LEVEL){
                Console.WriteLine(level.ToString()+": "+message);
            }
        }
    }
}
