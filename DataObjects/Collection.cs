using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Collection
    {
        public int collectionID { get; set; }
        public string collectionName { get; set; }
        public int userID { get; set; }

    }
    public class CollectionVM : Collection
    {
        public List<int> movieIDs { get; set; }
    }
}
