using System;
using System.IO;
using System.Collections.Generic;
namespace LdifInDepthCompare
{
    public class RuleLoader
    {
        private String ruleFile;
        public RuleLoader(String aFile){
            this.ruleFile = aFile;
        }
        public SortedList<String, Rule> loadRules()
        {
            SortedList<String, Rule> rules = new SortedList<string, Rule>();
            StreamReader file = new StreamReader(this.ruleFile);
            String line;
            int lineNumber = 0;
            while ((line = file.ReadLine()) != null)
            {
                lineNumber++;
                System.Console.WriteLine(line);
                //we take only rules, let's exclude the comments
                if (!line.Trim().StartsWith("#"))
                {
                    String[] lineFields = line.Split(';');
                    if (lineFields.Length != 7)
                    {
                        throw new Exception("line number:" + lineNumber + " is NOT a valid rule as it has " + lineFields.Length + " columns. Remember that the separator is ; ");
                    }
                    else
                    {
                        if (lineFields[Constants.TAG].Trim().Length == 0) throw new Exception("Line:" + lineNumber + " Tag cannot be empty!");
                        if (!rules.ContainsKey(lineFields[Constants.TAG]))
                        {
                            Rule newRule = new Rule(lineFields,lineNumber);

                            //then add to the list
                            rules.Add(lineFields[Constants.TAG], newRule);
                        }
                        else throw new Exception("Duplicate tag at line:" + lineNumber + " tag:" + lineFields[0] + " already exist!");
                    }
                }
            }

            file.Close();
            return rules;
        }
    }
}
