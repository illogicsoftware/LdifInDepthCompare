/**Author: Simonluca Scillitani
 * Company: Illogic 
 * http://www.illogic.software
 * summary: This class contains the costants 
 */

using System;
namespace LdifInDepthCompare
{
    public class Constants
    {
        public enum MATCH_DIRECTION { FORWARD, REVERSE }
        public enum DEBUG_LEVELS { DEBUG, INFO, WARNING, ERROR, FATAL };
        public static DEBUG_LEVELS CURRENT_DEBUG_LEVEL = DEBUG_LEVELS.INFO;
        public enum EXPLAIN {NONE, STRATEGY, LINE_BY_LINE, FULL };


        public enum RULE_DIRECTIONS { FORWARD, BACKWARD, NONE };
        public enum RULE_ACTIONS { COPY, DELETE, CONCATENATE, MD5, SHA256, FIX };
        public enum RULE_LEVEL { FIELD, RECORD };
        public enum RULE_SELECTOR { BY_NAME, BY_REGEX };

        //positional fields
        public static readonly int TAG=0;
        public static readonly int DIRECTION = 1;
        public static readonly int ACTION = 2;
        public static readonly int LEVEL = 3;
        public static readonly int SELECTOR = 4;
        public static readonly int MATCH = 5;
        public static readonly int DEF = 6;
    }
}
