using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;

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
            int rows = 0;
            foreach (var collection in fakeCollections)
            {
                if (collection.collectionID == collectionID)
                {
                    collection.movieIDs.Add(movieID);
                    rows++;
                }
                if (rows == 0)
                {
                    throw new ArgumentException("No movies added to the collection");
                }
            }
            return rows;
        }


        public int AddNewCollection(int userID, string collectionName)
        {
            int rows = 0;
            int newCollectionID = fakeCollections.Count + 1;
            CollectionVM newCollection = new CollectionVM();
            foreach (var collection in fakeCollections)
            {
                if (collection.userID == userID)
                {

                    if (collection.collectionName.ToLower() == collectionName.ToLower())
                    {
                        throw new ArgumentException("Collection name already exists");
                    }
                    else
                    {
                        newCollection.collectionID = newCollectionID;
                        newCollection.collectionName = collectionName;
                        newCollection.userID = userID;
                        newCollection.movieIDs = new List<int>();
                        rows++;
                    }
                }
            }
            if (rows != 0)
            {
                fakeCollections.Add(newCollection);
            }
            return rows;
        }

        public List<CollectionVM> GetCollectionsByUserID(int userID)
        {
            List<CollectionVM> collections = new List<CollectionVM>();

            foreach (var fakeCollection in fakeCollections)
            {
                if (fakeCollection.userID == userID)
                {
                    collections.Add(fakeCollection);
                }
            }
            if (collections == null)
            {
                throw new ArgumentException();
            }
            return collections;
        }

        public List<int> GetMovieIDsByCollectionID(int collectionID)
        {
            List<int> movieIDs = new List<int>();

            foreach (var fakeCollection in fakeCollections)
            {
                if (fakeCollection.collectionID == collectionID)
                {
                    movieIDs = fakeCollection.movieIDs;
                }
            }

            if (movieIDs == null)
            {
                throw new ArgumentException();
            }

            return movieIDs;
        }

        public int RemoveCollection(int userID, int collectionID)
        {
            int indexToRemove = 0;
            int rows = 0;

            for (int i = 0; i < fakeCollections.Count; i++)
            {
                if (fakeCollections[i].userID == userID && fakeCollections[i].collectionID == collectionID)
                {
                    rows++;
                    indexToRemove = i;
                }
            }
            if (rows != 0)
            {
                fakeCollections.RemoveAt(indexToRemove);
            }

            return rows;
        }

        public int RemoveMovieFromCollection(int movieID, int collectionID)
        {
            int rows = 0;
            List<int> movieIDs = new List<int>();
            int collectionIndex = 0;
            int indexToRemove = 0;

            for (int i = 0; i < fakeCollections.Count; i++)
            {
                if (fakeCollections[i].collectionID == collectionID)
                {
                    collectionIndex = i;
                    for (int j = 0; j < fakeCollections[i].movieIDs.Count; j++)
                    {
                        if (fakeCollections[i].movieIDs[j] == movieID)
                        {
                            indexToRemove = j;
                            rows++;
                            break;
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("No collections found");
                }

            }
            if (rows != 0)
            {
                fakeCollections[collectionIndex].movieIDs.Remove(fakeCollections[collectionIndex].movieIDs[indexToRemove]);
            }

            return rows;

        }
    }
}
