/**Author: Simonluca Scillitani
 * Company: Illogic 
 * http://www.illogic.software
 * summary: This class is the container data comparator 
 */

using System;
using System.Collections.Generic;

namespace LdifInDepthCompare
{
    public class ContainersMatcher
    {
        private Results results;
        private RuleEngine theRuleEngine;

        public ContainersMatcher(Results results,RuleEngine aRuleEngine){
            this.results = results;
            this.theRuleEngine = aRuleEngine;
        }

        private void runMatcher(Constants.MATCH_DIRECTION direction, List<LdifContainer> containers)
        {

            //this is the only counter that changes in BACKWARD mode so it needs to be reset in order be correct.
            results.resetNotPresent();
            Logger.write(Constants.DEBUG_LEVELS.INFO, direction.ToString()+" Comparing");
            foreach (DnItem dni in containers[(direction == Constants.MATCH_DIRECTION.FORWARD) ? 0 : 1])
            {
                Logger.write(Constants.DEBUG_LEVELS.DEBUG, "Scanning for " + dni + " into destination");
                bool found = false;

                foreach (DnItem dn2 in containers[(direction == Constants.MATCH_DIRECTION.FORWARD) ? 1 : 0])
                {
                    if (dn2.getDN().Trim() == dni.getDN().Trim())
                    {
                        found = true;
                        if (dn2.getDnHash() != dni.getDnHash())
                        {
                            /*
                             * This should NEVER happen as the hash is generated on the trimmed dn there is no way that the dn is the same and the hash not!
                             * <<BE CAREFUL!!>>
                             * If this happens it means that we have a serious problem with the hashing function! 
                             * 
                             * We have to write just once this items or we get them duplicated into the results
                            */
                            Logger.write(Constants.DEBUG_LEVELS.DEBUG, "Hash does not match " + dni.getDN());
                            if (direction == Constants.MATCH_DIRECTION.FORWARD) results.addHashDoNotMatch(dni);
                        }
                        else if (dn2.getDnFullHash() != dni.getDnFullHash())
                        {
                            /*We have to write just once this items or we get them duplicated into the results
                            */
                            Logger.write(Constants.DEBUG_LEVELS.DEBUG, "FULL Hash does not match " + dni.getDN());
                            //add the object result from comparing A & B so we can write later the object differences field by field
                            if (direction == Constants.MATCH_DIRECTION.FORWARD)
                            {
                                results.addFullHashDoNotMatch(dni.compare(dn2));
                                results.writeFullHashDoNotMatchSpecials(dni, dn2);
                            }

                        }
                        else
                        {
                            Logger.write(Constants.DEBUG_LEVELS.DEBUG, "Found " + dni.getDN());
                            if (direction == Constants.MATCH_DIRECTION.FORWARD) results.addToPresent(dni);
                        }
                        break;
                    }
                }

                if (!found)
                 {
                    Logger.write(Constants.DEBUG_LEVELS.DEBUG, "NOT Found " + dni.getDN());
                    results.addNotPresent(direction, dni);
                  }
            }

            Logger.write(Constants.DEBUG_LEVELS.INFO, "---------------------------------------");
            Logger.write(Constants.DEBUG_LEVELS.INFO, "Result after direction ->" + direction.ToString());
            Logger.write(Constants.DEBUG_LEVELS.INFO, "Present:" + results.getPresent());
            Logger.write(Constants.DEBUG_LEVELS.INFO, "NOT present:" + results.getNotPresent());
            if (results.getHashDoNotMatch() > 0)
            {
                Logger.write(Constants.DEBUG_LEVELS.FATAL, "DN Hash mismatch is greater than 0, please check for bugs in the hash function");              
                Logger.write(Constants.DEBUG_LEVELS.FATAL, "DN Hash mismatch:" + results.getHashDoNotMatch());
                Logger.write(Constants.DEBUG_LEVELS.FATAL, "Results are compromised by a bugged hash function, exiting!");
                System.Environment.Exit(1);

            }
            Logger.write(Constants.DEBUG_LEVELS.INFO, "Full hash mismatch " + results.getFullHashDoNotMatch());
            Logger.write(Constants.DEBUG_LEVELS.INFO, "Total:" + results.getTotal()+" of "+containers[direction==Constants.MATCH_DIRECTION.FORWARD?0:1].Count);
            Logger.write(Constants.DEBUG_LEVELS.INFO, "---------------------------------------");
        }

        public void match(List<LdifContainer> containers){

            Logger.write(Constants.DEBUG_LEVELS.INFO, "---------------------------------------");
            Logger.write(Constants.DEBUG_LEVELS.INFO, "Total record A: " + containers[0].Count);
            Logger.write(Constants.DEBUG_LEVELS.INFO, "Total record B: " + containers[1].Count);
            Logger.write(Constants.DEBUG_LEVELS.INFO, "---------------------------------------");

            runMatcher(Constants.MATCH_DIRECTION.FORWARD, containers);
            runMatcher(Constants.MATCH_DIRECTION.REVERSE, containers);

          
        }
    }
}
