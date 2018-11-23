/**Author: Simonluca Scillitani
 * Company: Illogic 
 * http://www.illogic.software
 * summary: This class parses a file, normalize the entries and put in a structure LdifContainer<DnItem> list
 * it also ensure that each record is registered atomically calling the "begin/end transaction"
 * 
 * @return: the LdifContainer object
 */

using System;
using System.IO;
using System.Text;

namespace LdifInDepthCompare
{
    public class Normalizer
    {
        public LdifContainer normalize(String filename){

            //create a new container
            LdifContainer newContainer = new LdifContainer(filename);

            Logger.write(Constants.DEBUG_LEVELS.INFO, "Reading input file ->" + filename);
            String fullLine = "";
            //reading the file line by line
            foreach (string line in File.ReadLines(filename, Encoding.UTF8))
            {
                //if it is NOT an empty line ....
                if (line.Trim().Length > 0)
                {
                    //if we find a space means that there was a line break so we have
                    //to build the complete line
                    if (line.StartsWith(" "))
                    {
                        //let's store the line joining with the previous one
                        fullLine += line.Trim();
                    }
                    else //if the line does NOT start with a space  
                    {
                        //as the line does NOT start with a space this means that we are adding a new field in the DnItem of the container
                        //so, if there is in the cache a previous field we have to commit it BEFORE OVERRIDING 
                        //the cache with the new line
                        if (fullLine.Length > 0) //write only if it is NOT empty
                        {
                            //if the line is the DN we have to call the beginTransaction which will commit the previous DnItem and create a new one
                            if (fullLine.ToLower().StartsWith("dn:"))
                            {
                                newContainer.beginTransaction(fullLine.Trim());
                            }
                            else
                            {
                                //if it's not a dn then we have to add the field to the DnItam
                                newContainer.addLineToDn(fullLine.Trim());
                            }
                        }

                        //put the line in the cache so we can join it with the next if needed
                        fullLine = line;
                    }

                }
            }

            //ensure that alslo the last transaction was committed
            newContainer.endTransaction();
            return newContainer;
        }
    }
}
