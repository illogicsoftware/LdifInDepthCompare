/**Author: Simonluca Scillitani
 * Company: Illogic 
 * http://www.illogic.software
 * summary: This class write the results to the disk
 */
using System;
using System.Collections.Generic;
namespace LdifInDepthCompare
{
    public class Results
    {
        private String fName;
        //present: means the record is both in A and B with no issue ( it's equal )
        private  long present = 0;
        //notpresent: mean the record is present in A but not in B or vice versa regarding the direction
        private  long notPresent = 0;
        //fullHashDoNotMatch: means that the record is present BUT the data differs (added/deleted/changed) so A!=B in values but not in keys
        private  long fullHashDoNotMatch = 0;
        //hashdonotmatch: means we have a big mistake! this should NEVER happen because simply states that the same DN function produced two different hashes.
        //if this counter is !=0 that check the Crypto hash function because there is a bug!!!
        private  long hashDoNotMatch  = 0;

        public  long getPresent() { return present;  }
        public  long getNotPresent() { return notPresent; }
        public  long getHashDoNotMatch() { return hashDoNotMatch; }
        public  long getFullHashDoNotMatch() { return fullHashDoNotMatch;  }

        public Results(String fName){
            this.fName = fName;
        }

        private void writeItem(String fSuffix,DnItem item,bool printFullLine){
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fName+fSuffix, true))
            {
                file.WriteLine(item.getDN());
                if (printFullLine)
                {
                    file.WriteLine(item.getFullLine());
                    file.WriteLine(item.getDnFullHash());
                } else file.WriteLine(item.getConcatenatedFields());
            }
        }

        public void addToPresent(DnItem item) {
            writeItem("_Present.txt", item,false);
            present++;
        }

        public void addNotPresent(Constants.MATCH_DIRECTION direction, DnItem item)
        {
            writeItem("_NotPresent.txt", item,false);
            notPresent++;
        }


        public void addFullHashDoNotMatch(DnItem item) {
            writeItem("_FullHashDoesNotMatch.txt", item,false);
            fullHashDoNotMatch++;
        }

        public void addHashDoNotMatch(DnItem item) {
            writeItem("_HashDoesNotMatch.txt", item,false);
            hashDoNotMatch++;
        }

        public void writeFullHashDoNotMatchSpecials(DnItem a,DnItem b){
            writeItem("_FullHashDoesNotMatchSpecial.txt", a, true);
            writeItem("_FullHashDoesNotMatchSpecial.txt", b, true);
        }

        /**This is the only counter which will be reset!
         * Why?
         * If we have A and B 
         * the intersection between A and B will be the "present" counter and, of course it does not change if is A B or B A
         * the same is fot the fullHashDoNotMatch and hashDoNotMatch because they count "present record with exceptions" so they exist in the same quantity 
         * if A B or B A ( doesn't matter the direction ).
         * the "notpresent" counter, instead, means that you have a key which is NOT present in B ( if the key is in A ) and vice versa. 
         * This number changes regarding the direction. A not in B != B not in A, of course!
        */
        public void resetNotPresent(){
            notPresent = 0;
        }

        public long getTotal(){
            return (present + notPresent + fullHashDoNotMatch + hashDoNotMatch);
        }
       
    }
}
