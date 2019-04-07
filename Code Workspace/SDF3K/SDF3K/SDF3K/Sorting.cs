/*################# Sorting.cs #######################################
# Eklektik Design
# Micah Richards
# 2/6/2018
#           
# Purpose: Parses date strings into other data forms
#
###############################################################*/

namespace SDF3K
{
    /*################# Libraries #################################*/
    using System;
    using System.Collections.Generic;

    internal class Sorting
    {

        /*################# Public Functions ##########################*/

        /*################# GetDayFromDateString ########################
        # Purpose: Returns the day number from the date string
        #                  
        ###############################################################*/
        public static int GetDayFromDateString(String date)
        {
            DateTime dt = Convert.ToDateTime(date);
            return dt.Day;
        }

        /*################# GetHourFromDateString #######################
        # Purpose: Returns the hour number from the date string
        #                  
        ###############################################################*/
        public static int GetHourFromDateString(String date)
        {
            DateTime dt = Convert.ToDateTime(date);
            return dt.Hour;
        }

        /*################# GetMonthFromDateString ######################
        # Purpose: Returns the month number from the date string
        #                  
        ###############################################################*/
        public static int GetMonthFromDateString(String date)
        {
            DateTime dt = Convert.ToDateTime(date);
            return dt.Month;
        }

        /*################# SortByDay ###################################
        # Purpose: Takes a list of date strings and sorts them by day
        #                  
        ###############################################################*/
        public static Object[][] SortByDay(Object[] input)
        {
            int day;
            List<Object>[] output = new List<Object>[31];

            for (int i = 0; i < 31; i++)
            {
                output[i] = new List<Object>();
            }
            for (int i = 0; i <= input.Length - 1; i++)
            {
                day = (int)GetDayFromDateString((String)input[i]);
                output[day - 1].Add(input[i]);
            }

            return output.ListArrayToArray();
        }

        /*################# SortByHour ##################################
        # Purpose:  Takes a list of date strings and sorts them by hour
        #                  
        ###############################################################*/
        public static Object[][] SortByHour(Object[] input)
        {
            int hour;
            List<Object>[] output = new List<Object>[24];

            for (int i = 0; i < 24; i++)
            {
                output[i] = new List<Object>();
            }
            for (int i = 0; i <= input.Length - 1; i++)
            {
                hour = (int)GetHourFromDateString((String)input[i]);
                output[hour].Add(input[i]);
            }
            return output.ListArrayToArray();
        }

        /*################# SortByMonth #################################
        # Purpose:  Takes a list of date strings and sorts them by month
        #                  
        ###############################################################*/
        public static Object[][] SortByMonth(Object[] input)
        {
            int month;
            List<Object>[] output = new List<Object>[12];

            for (int i = 0; i < 12; i++)
            {
                output[i] = new List<Object>();
            }
            for (int i = 0; i <= input.Length - 1; i++)
            {
                month = (int)GetMonthFromDateString((String)input[i]);
                output[month - 1].Add(input[i]);
            }
            return output.ListArrayToArray();
        }
    }
}
