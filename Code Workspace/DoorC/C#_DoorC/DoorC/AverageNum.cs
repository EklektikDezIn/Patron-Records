/*################# QuickBox ####################################
# Eklektik Design
# Micah Richards
# 11/21/16
# Purpose: Collects numbers and outputs there average
#           for general purpose
#
###############################################################*/


/*################# Libraries #################################*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorC
{
    class AverageNum
    {
        /*################# Variables #################################*/
        double[] recentVals;                                //## Collection of values
        int currInd = 0;                                    //## Next index to replace
        int listSize = 0;                                   //## Size of collection


        /*################# Public Functions ##########################*/

        /*################# AverageNum ####################################
        # Purpose: Constructor for AverageNum class
        #          
        # Inputs:  int - size of AverageNum collection
        # 	   
        # Outputs: AverageNum - object
        #          
        ###############################################################*/
        public AverageNum(int size)
        {
            listSize = size;
            recentVals = new double[listSize];
        }

        /*################# AddValue ####################################
        # Purpose: Adds a value to the AverageNum
        #          
        # Inputs:  Double - number to add
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void AddValue(double num)
        {
            recentVals[currInd] = num / listSize;
            Increment();
        }

        /*################# GetValue ####################################
        # Purpose: Returns the value of the AverageNum
        #          
        # Inputs:  None
        # 	   
        # Outputs: Double - average of the collection of inputs
        #          
        ###############################################################*/
        public double GetValue()
        {
            double total = 0;
            foreach (double i in recentVals)
            {
                total += i;
            }
            return total;
        }

        /*################# Increment ####################################
        # Purpose: Tracks the next input for the AverageNum
        #          
        # Inputs:  None
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void Increment()
        {
            currInd++;
            if (currInd >= listSize)
            {
                currInd = 0;
            }
        }
    }
}
