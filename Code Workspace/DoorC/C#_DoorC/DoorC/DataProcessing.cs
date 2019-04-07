/*################# DataProcessing #############################
# Eklektik Design
# Micah Richards
# 07/11/17
# Purpose: Handles data processing and calculations for DoorC
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
    class DataProcessing
    {
        /*################# Variables #################################*/
        private static bool isPerson = false;                      //## tracks presence of Patron

        /*################# Public Functions ##########################*/

        /*################# DataToPatron ################################
        # Purpose: Converts stream of data to a count of patrons
        #          
        # Inputs:  String - data to be analyzed
        # 	   
        # Outputs: Data to excel file
        #          
        ###############################################################*/
        public static void DataToPatron(String myCompleteMessage, Window window)
        {
            List<string> msg = myCompleteMessage.ToString().Split("\r\n".ToCharArray()).ToList();
            msg.RemoveAll(IsBlank);
            if (msg.Count > 0)
            {
                msg.RemoveAt(0);
            }
            if (msg.Count > 0)
            {
                msg.RemoveAt(msg.Count - 1);
            }


            //###### Itterate through all incoming messages ######//
            foreach (string dist in msg)
            {
                double val;
                double.TryParse(dist, out val);
                int number;
                if (Math.Ceiling(val) == Math.Round(val))
                {
                    number = (int)Math.Round(val);
                    window.SetAmbientLightValue(number);
                }

                else if (Math.Floor(val) == Math.Round(val))
                {
                    number = (int)Math.Round(val);
                    window.SetLaserLightValue(Math.Round(val));

                    //###### Check if light reading indicates a beam intercept ######//
                    if (number < window.GetTriggerVal())
                    {
                        window.IncStand();
                        //###### Print Raw data or Cleaned data ######//
                        if (window.GetRCP() == 0)
                        {
                            window.UpdateOutput(number + System.Environment.NewLine);
                        }
                        else if (window.GetRCP() == 1)
                        {
                            window.UpdateOutput("0" + System.Environment.NewLine);
                        }

                    }
                    else
                    {
                        window.IncPass();
                        //###### Print Raw data or Cleaned data ######//
                        if (window.GetRCP() == 0)
                        {
                            window.UpdateOutput(number + System.Environment.NewLine);
                        }
                        else if (window.GetRCP() == 1)
                        {
                            window.UpdateOutput("1" + System.Environment.NewLine);
                        }
                    }


                    if (!isPerson)
                    {
                        //###### A person is detected when they have been intercepted the beam for a period of time ######//
                        if (window.GetStand() > window.GetTriggerCountValid())
                        {
                            isPerson = true;
                            window.SetPass(0);
                            window.SetStand(0);
                        }
                        //###### The system is refershed to prevent accumulation of bugs ######//
                        if (window.GetPass() > window.GetTriggerCountReset())
                        {
                            window.SetPass(0);
                            window.SetStand(0);
                        }
                    }
                    if (isPerson)
                    {
                        //###### An object has passed when, after intercepting the beam, it moves on ######//
                        if (window.GetPass() > window.GetTriggerCountValid())
                        {
                            isPerson = false;
                            window.SetPass(0);
                            window.SetStand(0);

                            //###### Print only Patron data ######//
                            if (window.GetRCP() == 2)
                            {
                                window.UpdateOutput(DateTime.Now.ToString("h:mm:ss tt") + System.Environment.NewLine);
                            }
                            window.WriteToExcelData(DateTime.Now.ToString(@"MM\/dd\/yyyy hh:mm:ss tt"));
                        }
                        //###### The system is refershed to prevent accumulation of bugs ######//
                        if (window.GetStand() > window.GetTriggerCountReset())
                        {
                            window.SetPass(0);
                            window.SetStand(0);
                        }
                    }
                }
            }
        }


        /*################# Private Functions #########################*/

        /*################# IsBlank #####################################
        # Purpose: Checks if a given string is blank
        #          
        # Inputs:  Strring - text to test
        # 	   
        # Outputs: Boolean - text is blank
        #          
        ###############################################################*/
        private static bool IsBlank(String s)
        {
            return s.ToLower().Equals("");
        }
    }
}