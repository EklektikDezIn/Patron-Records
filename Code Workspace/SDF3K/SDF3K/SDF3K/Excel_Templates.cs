/*################# Excel_Templates #######################################
# Eklektik Design
# Micah Richards
# 07/11/17
# Purpose: Combines the base functions of the Excel class for more 
#           specific use cases.
#
###############################################################*/

namespace SDF3K
{
    /*################# Libraries #####################################*/
    using System;
    using static SDF3K.Constants;

    internal static class Excel_Templates
    {
        /*################# Public Functions ##########################*/

        /*################# Calendar #######################################
        # Purpose: Creates a calendar with labeled month and days
        #                  
        ###############################################################*/
        public static void Calendar(this Excel excelIn, int startCol, int startRow, int firstOfWeek, int monthId)
        {
            String day;
            String month = ((MONTHS)monthId).ToString();

            excelIn.MergeCells(excelIn.CreateRange(startCol, startRow, startCol + 6, startRow));
            excelIn.SetBorder(1, excelIn.CreateRange(startCol, startRow, startCol + 6, startRow + 6));
            excelIn.WriteCell(month, startCol, startRow, false);

            for (int i = 0; i < 7; i++)
            {
                day = ((DAYS)((firstOfWeek + i) % 7)).ToString();
                excelIn.WriteCell(day, startCol + i, startRow + 1, true);
            }
        }
    }
}
