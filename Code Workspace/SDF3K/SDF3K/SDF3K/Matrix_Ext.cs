/*################# Matrix_Ext.cs ###############################
# Eklektik Design
# Micah Richards
# 2/6/2018
#           
# Purpose: Provides additional functionality to matrix objects.
#
###############################################################*/

namespace SDF3K
{

    /*################# Libraries #####################################*/
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class Matrix_Ext
    {

        /*################# Public Functions ##########################*/

        /*################# IntToObj ####################################
        # Purpose: Converts an int array to an object array
        #                  
        ###############################################################*/
        public static Object[] IntToObj(this int[] matrix)
        {
            return Array.ConvertAll<int, object>(matrix, (x) => (object)x); //https://social.msdn.microsoft.com/Forums/vstudio/en-US/da05048d-2e26-4d50-a460-c68acd03c600/cannot-convert-int-to-object?forum=csharpgeneral
        }

        /*################# IntToObj ####################################
        # Purpose: Converts a two dimensional int array to a two
        #           dimensional object array
        #                  
        ###############################################################*/
        public static Object[][] IntToObj(this int[][] matrix)
        {
            Object[][] output = new Object[matrix.Length][];

            for (int i = 0; i < matrix.Length; i++)
            {
                output[i] = Array.ConvertAll<int, object>(matrix[i], (x) => (object)x); //https://social.msdn.microsoft.com/Forums/vstudio/en-US/da05048d-2e26-4d50-a460-c68acd03c600/cannot-convert-int-to-object?forum=csharpgeneral
            }
            return output;
        }

        /*################# ListArrayToArray ############################
        # Purpose: Converts an array of Lists to a two dimensional array
        #                  
        ###############################################################*/
        public static Object[][] ListArrayToArray(this List<Object>[] matrix)
        {
            Object[][] output = new Object[matrix.Length][];

            for (int i = 0; i < output.Length - 1; i++)
            {
                output[i] = new Object[matrix[i].Count];
                for (int j = 0; j < output[i].Length - 1; j++)
                {
                    output[i][j] = matrix[i].ElementAt(j);
                }
            }
            return output;
        }

        /*################# OneToTwo ####################################
        # Purpose: Converts an array to a two dimensional array
        #                  
        ###############################################################*/
        public static int[][] OneToTwo(this int[] matrix, int xLength)
        {
            int[][] output = new int[(int)Math.Ceiling((double)matrix.Length / xLength)][];

            for (int i = 0; i < output.Length; i++)
            {
                output[i] = new int[xLength];
            }

            for (int i = 0; i < matrix.Length; i++)
            {
                output[(int)Math.Floor((double)i / xLength)][i % xLength] = matrix[i];
            }
            return output;
        }
    }
}
