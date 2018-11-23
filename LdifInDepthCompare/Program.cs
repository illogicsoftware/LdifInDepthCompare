/**Author: Simonluca Scillitani
 * Company: Illogic 
 * http://www.illogic.software
 * summary: This program normalizes and compare 2 ldif exports
 */
using System;
using System.Collections.Generic;



namespace LdifInDepthCompare
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Logger.write(Constants.DEBUG_LEVELS.INFO,"---------------------------------------");
            Logger.write(Constants.DEBUG_LEVELS.INFO, "       Ldiff LdifInDepthCompare");
            Logger.write(Constants.DEBUG_LEVELS.INFO, "     Author @Simonluca Scillitani");
            Logger.write(Constants.DEBUG_LEVELS.INFO, "       Copyright 2018@Illogic");
            Logger.write(Constants.DEBUG_LEVELS.INFO, "     http://www.illogic.software");
            Logger.write(Constants.DEBUG_LEVELS.INFO, "---------------------------------------");

            var watch = System.Diagnostics.Stopwatch.StartNew();


            if (args.Length == 3)
            {
                List<LdifContainer> containers = new List<LdifContainer>();
                for (int iargs = 0; iargs < 2;iargs++)
                {

                    LdifContainer currentContainer = new Normalizer().normalize(args[iargs]);
                    containers.Add(currentContainer);
                    Logger.write(Constants.DEBUG_LEVELS.INFO, "---------------------------------------");
                    Logger.write(Constants.DEBUG_LEVELS.INFO, "Container Statistics");
                    Logger.write(Constants.DEBUG_LEVELS.INFO, "Container: " + args[iargs]);
                    Logger.write(Constants.DEBUG_LEVELS.INFO, "Number of entries: " + currentContainer.Count);
                    Logger.write(Constants.DEBUG_LEVELS.INFO, "---------------------------------------");
                }

                ContainersMatcher cm = (args.Length == 4) ?
                    //we have a rule file!
                    new ContainersMatcher(new Results(args[2]), new RuleEngine(args[3],args.Length==5?args[4]:null)) :
                    //no rules no party, will not output "processed" file in the result folder
                    new ContainersMatcher(new Results(args[2]), null);

                cm.match(containers);


                watch.Stop();
                TimeSpan ts = watch.Elapsed;

                Logger.write(Constants.DEBUG_LEVELS.INFO, "Completed in: "+Utils.tsToStr(ts));

            }
            else
            {
                Logger.write(Constants.DEBUG_LEVELS.FATAL, "Usage LdifInDepthCompare file1.ldif file2.ldif resultsDirectory <optional ruleFile> <optional explain>");
                Logger.write(Constants.DEBUG_LEVELS.FATAL, "<optional ruleFile> if you specify the rule file a new file named processed.txt will be created");
                Logger.write(Constants.DEBUG_LEVELS.FATAL, "<optional explain> if you add the explain option an explained_rules.txt will be created");
                Logger.write(Constants.DEBUG_LEVELS.FATAL, "valid values are:");
                Logger.write(Constants.DEBUG_LEVELS.FATAL, "STRATEGY -> output the rule tag applied but no data");
                Logger.write(Constants.DEBUG_LEVELS.FATAL, "LINE_BY_LINE -> output rule tag(s) + details by the ruleEngine but without data");
                Logger.write(Constants.DEBUG_LEVELS.FATAL, "FULL -> output ALL!! use only for debug unless you want to burn your computer!");
                Logger.write(Constants.DEBUG_LEVELS.FATAL, "Note: explain options could produce a very huge file and take a lot of time, use only if really needed!");
                System.Environment.Exit(1);
            }

        }
    }
}

