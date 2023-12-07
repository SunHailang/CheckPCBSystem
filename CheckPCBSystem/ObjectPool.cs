using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckPCBSystem
{
    public class ObjectPool<T> where T : IDisposable, new()
    {
        private Stack<T> m_pool;

        public ObjectPool(int minCount = 0, int maxCount = 1000)
        {
            m_pool = new Stack<T>(minCount);
        }

        public T Get()
        {
            if (m_pool.Count > 0) return m_pool.Pop();
            return new T();
        }

        public void Release(T data)
        {
            data.Dispose();
            m_pool.Push(data);
        }

        ~ObjectPool()
        {
            m_pool.Clear();
            m_pool = null;
        }
    }
}
