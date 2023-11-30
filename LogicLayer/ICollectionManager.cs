using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    internal interface ICollectionManager
    {
        List<CollectionVM> GetCollectionsByUserID(int userID);
        List<int> GetMovieIDsByCollectionID(int collectionID);
        bool AddUserCollection(int userID, string collectionName);
        bool AddMovieToCollection(int movieID, int collectionID);
        bool RemoveUserCollection(int userID, int collectionID);

    }
}
