using System;
using System.Collections.Generic;

namespace DirectoryManagerTest
{
    public interface ICache
    {
        bool PutItem<T>(T value, int id, params CacheDependency[] dependencies);

        T FetchItem<T>(int id);

        bool PutCollection<T>(IEnumerable<T> collection, Func<T, int> identifierField);

        bool PutAllCollection<T>(IEnumerable<T> collection, Func<T, int> identifierField);

        IEnumerable<T> FetchCollection<T>(Func<T, int> identifierField, IEnumerable<int> ids, Func<IEnumerable<int>, IEnumerable<T>> callback);

        IEnumerable<T> FetchAllCollection<T>(Func<T, int> identifierField, Func<IEnumerable<int>, IEnumerable<T>> callback);

        bool Remove<T>(T value, int id);
    }
}