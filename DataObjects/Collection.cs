using System.Collections.Generic;

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
