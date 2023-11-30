using DataAccessInterfaces;
using DataAccessLayer;
using DataObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class CollectionManager : ICollectionManager
    {
        ICollectionAccessor _collectionAccessor = null;
        public CollectionManager() 
        {
            _collectionAccessor = new CollectionAccessor();
        }

        public CollectionManager(ICollectionAccessor collectionAccessor)
        {
            _collectionAccessor = collectionAccessor;
        }

        public bool AddMovieToCollection(int movieID, int collectionID)
        {
            bool result = false;

            try
            {
                int rows = 0;
                rows = _collectionAccessor.AddMovieToCollection(movieID, collectionID);
                if (rows != 0)
                {
                    result = true;
                }
                else
                {
                    throw new ArgumentException("Error finding data");
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error adding movie to collection", ex);
            }

            return result;
        }

        public bool AddUserCollection(int userID, string collectionName)
        {
            bool result = false;

            try
            {
                int newID = 0;
                int rows = 0;
                newID = _collectionAccessor.AddNewCollection(userID, collectionName);
                if(newID != 0)
                {
                    rows = _collectionAccessor.AddMovieToCollection(10000, newID);

                }
                
                if(rows != 0)
                {
                    result = true;

                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Not able to add collection", ex);
            }

            return result;
        }

        public List<CollectionVM> GetCollectionsByUserID(int userID)
        {
            List<CollectionVM> collections = null;

            try
            {
                collections = _collectionAccessor.GetCollectionsByUserID(userID);
                if(collections != null)
                {
                    foreach(var collection in collections)
                    {
                        List<int> movieIDs = GetMovieIDsByCollectionID(collection.collectionID);
                        collection.movieIDs = movieIDs;
                    }
                }
            }
            catch(Exception ex)
            {
                throw new ApplicationException("Failed to fetch collections", ex);
            }

            return collections;
        }

        public List<int> GetMovieIDsByCollectionID(int collectionID)
        {
            List<int> movieIDs = null;

            try 
            {
                movieIDs = _collectionAccessor.GetMovieIDsByCollectionID(collectionID);
                if(movieIDs == null)
                {
                    movieIDs.Add(10000);
                }
            } 
            catch (Exception ex) 
            { 
                throw new ApplicationException("Error retrieving collection items", ex); 
            }

            return movieIDs;
        }
    }
}
