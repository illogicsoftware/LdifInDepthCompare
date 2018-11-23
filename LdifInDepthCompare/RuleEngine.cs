using System;
using System.IO;
using System.Collections.Generic;

namespace LdifInDepthCompare
{
    public class RuleEngine
    {



        private readonly Constants.EXPLAIN explain;
        private SortedList<String,Rule> rules;

        public RuleEngine(String aRuleFile,String explainParm)
        {

            if (explainParm == null) explain = Constants.EXPLAIN.NONE;
            else if (explainParm.Trim().ToUpper() == "STRATEGY") explain = Constants.EXPLAIN.STRATEGY;
            else if (explainParm.Trim().ToUpper() == "LINE_BY_LINE") explain = Constants.EXPLAIN.LINE_BY_LINE;
            else if (explainParm.Trim().ToUpper() == "FULL") explain = Constants.EXPLAIN.FULL;
            else throw new Exception("Unknown value for option explain="+explainParm);
            //let's load the rules!
            rules=new RuleLoader(aRuleFile).loadRules();

        }



    }
}
