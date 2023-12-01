using DataObjects;
using System.Collections.Generic;

namespace DataAccessInterfaces
{
    public interface ICollectionAccessor
    {
        List<CollectionVM> GetCollectionsByUserID(int userID);
        List<int> GetMovieIDsByCollectionID(int collectionID);
        int AddNewCollection(int userID, string collectionName);
        int AddMovieToCollection(int movieID, int collectionID);
        int RemoveCollection(int userID, int collectionID);
        int RemoveMovieFromCollection(int movieID, int collectionID);
    }
}
