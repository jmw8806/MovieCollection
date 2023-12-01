using DataObjects;
using System.Collections.Generic;

namespace LogicLayer
{
    internal interface ICollectionManager
    {
        List<CollectionVM> GetCollectionsByUserID(int userID);
        List<int> GetMovieIDsByCollectionID(int collectionID);
        bool AddUserCollection(int userID, string collectionName);
        bool AddMovieToCollection(int movieID, int collectionID);
        bool RemoveUserCollection(int userID, int collectionID);
        bool RemoveMovieFromCollection(int movieID, int collectionID);
    }
}
