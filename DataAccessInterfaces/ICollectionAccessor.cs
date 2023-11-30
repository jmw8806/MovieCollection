using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterfaces
{
    public interface ICollectionAccessor
    {
        List<CollectionVM> GetCollectionsByUserID(int userID);
        List<int> GetMovieIDsByCollectionID(int collectionID);
        int AddNewCollection(int userID, string collectionName);
        int AddMovieToCollection(int movieID, int collectionID);
        int RemoveCollection(int userID, int collectionID);

    }
}
