using System;
namespace LdifInDepthCompare
{
    public class Rule
    {
        private readonly String tag;
        private readonly Constants.RULE_DIRECTIONS direction;
        private readonly Constants.RULE_ACTIONS action;
        private readonly Constants.RULE_LEVEL Level;
        private readonly Constants.RULE_SELECTOR selector;
        private readonly String match;
        private readonly String def;

        public Rule(String[] fields,int lineNumber)
        {
            this.tag = fields[Constants.TAG];
            this.direction = checkRuleDirection(fields[Constants.DIRECTION], lineNumber);
            this.action = checkRuleActions(fields[Constants.ACTION], lineNumber);
            this.Level = checkRuleLevel(fields[Constants.LEVEL], lineNumber);
            this.selector = checkRuleSelector(fields[Constants.SELECTOR], lineNumber);
            this.match = fields[Constants.MATCH];
            this.def = fields[Constants.DEF];

            //first let's check if the rule is valid
            checkPermittedCombinations();
        } 


        public void checkActionsFix()
        {
            if (def == null || def.Trim().Length == 0) throw new Exception("With Action=FIX you have to put a default value");
        }

        public void checkActionsDelete()
        {
            if (match == null || match.Trim().Length == 0) throw new Exception("With Action=DELETE you have to put a match value");
            if (def != null || def.Length != 0) Logger.write(Constants.DEBUG_LEVELS.WARNING, "The default value " + def + " will be ignored with the DELETE action");
        }

        private Constants.RULE_DIRECTIONS checkRuleDirection(String aDirection, int lineNumber)
        {
            if (aDirection.Trim() == Constants.RULE_DIRECTIONS.FORWARD.ToString()) return Constants.RULE_DIRECTIONS.FORWARD;
            if (aDirection.Trim() == Constants.RULE_DIRECTIONS.BACKWARD.ToString()) return Constants.RULE_DIRECTIONS.BACKWARD;
            if (aDirection.Trim() == Constants.RULE_DIRECTIONS.NONE.ToString()) return Constants.RULE_DIRECTIONS.NONE;
            throw new Exception("line:" + lineNumber + " invalid direction: " + aDirection);
        }

        private Constants.RULE_ACTIONS checkRuleActions(String anAction, int lineNumber)
        {
            if (anAction.Trim() == Constants.RULE_ACTIONS.CONCATENATE.ToString()) return Constants.RULE_ACTIONS.CONCATENATE;
            if (anAction.Trim() == Constants.RULE_ACTIONS.COPY.ToString()) return Constants.RULE_ACTIONS.COPY;
            if (anAction.Trim() == Constants.RULE_ACTIONS.DELETE.ToString()) return Constants.RULE_ACTIONS.DELETE;
            if (anAction.Trim() == Constants.RULE_ACTIONS.FIX.ToString()) return Constants.RULE_ACTIONS.FIX;
            if (anAction.Trim() == Constants.RULE_ACTIONS.MD5.ToString()) return Constants.RULE_ACTIONS.MD5;
            if (anAction.Trim() == Constants.RULE_ACTIONS.SHA256.ToString()) return Constants.RULE_ACTIONS.SHA256;

            throw new Exception("line:" + lineNumber + " invalid action: " + anAction);
        }

        private Constants.RULE_LEVEL checkRuleLevel(String aLevel, int lineNumber)
        {
            if (aLevel.Trim() == Constants.RULE_LEVEL.FIELD.ToString()) return Constants.RULE_LEVEL.FIELD;
            if (aLevel.Trim() == Constants.RULE_LEVEL.RECORD.ToString()) return Constants.RULE_LEVEL.RECORD;
            throw new Exception("line:" + lineNumber + " invalid level: " + aLevel);
        }

        private Constants.RULE_SELECTOR checkRuleSelector(String aSelector, int lineNumber)
        {
            if (aSelector.Trim() == Constants.RULE_SELECTOR.BY_NAME.ToString()) return Constants.RULE_SELECTOR.BY_NAME;
            if (aSelector.Trim() == Constants.RULE_SELECTOR.BY_REGEX.ToString()) return Constants.RULE_SELECTOR.BY_REGEX;
            throw new Exception("line:" + lineNumber + " invalid selector: " + aSelector);
        }

        private void checkPermittedCombinations()
        {
            switch (direction)
            {
                case Constants.RULE_DIRECTIONS.NONE:
                    switch (action)
                    {
                        case Constants.RULE_ACTIONS.FIX: checkActionsFix(); break;
                        case Constants.RULE_ACTIONS.DELETE: checkActionsDelete(); break;
                        default: throw new Exception("With direction NONE only FIX and DELETE actions allowed, instead found:" + action.ToString());
                    }
                    break;
                case Constants.RULE_DIRECTIONS.FORWARD:
                case Constants.RULE_DIRECTIONS.BACKWARD:
                    switch (action)
                    {
                        case Constants.RULE_ACTIONS.COPY:break;
                        case Constants.RULE_ACTIONS.CONCATENATE:break;
                        case Constants.RULE_ACTIONS.MD5: break;
                        case Constants.RULE_ACTIONS.SHA256: break;

                        default: throw new Exception("With direction FORWARD/BACKWARD this action is not permitted:" + action.ToString());
                    }
                    break;
            }
        }
    }
}
