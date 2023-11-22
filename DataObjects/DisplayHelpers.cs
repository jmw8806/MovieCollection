using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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

        public static string displayList(List<string> list)
        {
            string output = "";
            foreach(string item in list)
            {
                output += item + " ";
            }
            return output;
        }

       public static List<int> getYears(int start)
        {
            int end = DateTime.Now.Year;
            List<int> years = new List<int>();
            for(int year = start; year <= end; year++) 
            {
                years.Add(year);
            }
            return years;
        }


    }
}
