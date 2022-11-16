using System;
using System.Collections.Generic;

namespace Utils
{
    /// <summary>
    ///     creates a pool for any object that implements IPoolable
    /// </summary>
    public class Pool<T>
        where T : IPoolable
    {
        private readonly Func<T> _createObject;
        private readonly Queue<T> _freedObjects = new();

        public Pool(Func<T> createObject) => _createObject = createObject;

        public T Fetch() => _freedObjects.Count != 0 ? _freedObjects.Dequeue() : _createObject();

        public void Free(T t)
        {
            t.Reset();
            _freedObjects.Enqueue(t);
        }
    }
}