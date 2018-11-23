/**Author: Simonluca Scillitani
 * Company: Illogic 
 * http://www.illogic.software
 * summary: This class is a DnItem container used to register all the ldif records
 */

using System;
using System.Collections.Generic;

namespace LdifInDepthCompare
{
    public class LdifContainer:List<DnItem>
    {
        String containerName;
        DnItem currentDn=null;
        public LdifContainer(String name)
        {
            Logger.write(Constants.DEBUG_LEVELS.DEBUG, "New container ->" + name);
            this.containerName = name;
        }

        public void beginTransaction(String dnName){
            Logger.write(Constants.DEBUG_LEVELS.DEBUG, "Begin transaction ->" + dnName);
            this.endTransaction();
            //now we can safely create a new DnItem
            currentDn = new DnItem();
            //open recording and assigne the dnName
            currentDn.openRecording(dnName);
        }

        public void addLineToDn(String aLine){
            //add an additional line to the list of the DnItem
            if (currentDn!=null) currentDn.addLine(aLine);
        }

        /**this method is invoked each time we create a newDn and MUST be invoked by The CALLER when we complete the last insertion!
        */
        public void endTransaction(){
            //first we check if we have a pending transaction
            if (currentDn != null)
            {
                Logger.write(Constants.DEBUG_LEVELS.DEBUG, "End current transaction ->");
                //yes we have, so close the recording 
                currentDn.closeRecording();
                //and finally add to the container
                this.Add(currentDn);
                currentDn = null;
            }
        }
    }
}
