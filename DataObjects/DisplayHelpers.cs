using System;
using System.Collections.Generic;


namespace DataObjects
{
    public static class DisplayHelpers
    {
        //Method takes in a bool and outputs "yes" if true, "no" if false
        public static string criterionOutputConverter(bool criterionResult)
        {
            string isCriterion = "";
            if (criterionResult)
            {
                isCriterion = "Yes";
            }
            else
            {
                isCriterion = "No";
            }
            return isCriterion;
        }

        // join a list of items into a string
        public static string displayList(List<string> list)
        {
            string output = "";
            foreach (string item in list)
            {
                output += item + " ";
            }
            return output;
        }

        //generate a list of ints to represent years. From the start parameter to the current year.
        public static List<int> getYears(int start)
        {
            int end = DateTime.Now.Year;
            List<int> years = new List<int>();
            for (int year = start; year <= end; year++)
            {
                years.Add(year);
            }
            return years;
        }


    }
}
