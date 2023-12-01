using System.Collections.Generic;

namespace DataObjects
{
    public class Movie
    {
        public int titleID { get; set; }
        public string title { get; set; }
        public int year { get; set; }
        public string rating { get; set; }
        public int runtime { get; set; }
        public bool isCriterion { get; set; }
        public string notes { get; set; }
        public bool isActive { get; set; }
    }

    public class MovieVM : Movie
    {
        public List<string> genres { get; set; }
        public List<string> formats { get; set; }
        public string imgName { get; set; }
        public List<string> languages { get; set; }
    }
}
