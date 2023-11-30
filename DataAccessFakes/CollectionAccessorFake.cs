using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFakes
{
    public class CollectionAccessorFake : ICollectionAccessor

    {
        private List<CollectionVM> fakeCollections = new List<CollectionVM>();
 

        public CollectionAccessorFake() 
        {
            fakeCollections.Add(new CollectionVM()
            {
                collectionID = 1,
                collectionName = "Favorites",
                userID = 1,
                movieIDs = new List<int>()
            });
            fakeCollections.Add(new CollectionVM()
            {
                collectionID = 2,
                collectionName = "Comedy",
                userID = 1,
                movieIDs = new List<int>()
            });
            fakeCollections.Add(new CollectionVM()
            {
                collectionID = 1,
                collectionName = "Favorites",
                userID = 2,
                movieIDs = new List<int>()
            });

            fakeCollections[0].movieIDs.Add(1);
            fakeCollections[0].movieIDs.Add(2);
            fakeCollections[0].movieIDs.Add(3);

            fakeCollections[1].movieIDs.Add(1);
            fakeCollections[1].movieIDs.Add(2);

            fakeCollections[2].movieIDs.Add(3);


        }

        public int AddMovieToCollection(int movieID, int collectionID)
        {
            throw new NotImplementedException();
        }

        public int AddNewCollection(int userID, string collectionName)
        {
            throw new NotImplementedException();
        }

        public List<CollectionVM> GetCollectionsByUserID(int userID)
        {
            throw new NotImplementedException();
        }

        public List<int> GetMovieIDsByCollectionID(int collectionID)
        {
            throw new NotImplementedException();
        }
    }
}
