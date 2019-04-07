/*################# Constants.cs ###########################
# Eklektik Design
# Micah Richards
# 2/7/2018
#           
# Purpose: Stores constants for SDF3K
#
###############################################################*/


namespace SDF3K
{

    /*################# Libraries #################################*/
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class Constants
    {
        /*## Integer Error Code ##*/
        public const int ERROR = -1;

        /*## String Error Code ##*/
        public const String BLANK = "";

        /*## Enum of Months ##*/
        public enum MONTHS { January, February, March, April, May, June, July, August, September, October, November, December};

        /*## Enum of Days ##*/
        public enum DAYS { Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday };
    }
}
