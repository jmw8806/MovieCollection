using System;
using System.Collections.Generic;
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

        
    }
}
