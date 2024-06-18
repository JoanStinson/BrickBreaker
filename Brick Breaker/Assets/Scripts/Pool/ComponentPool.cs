using UnityEngine;
using System.Collections.Generic;

namespace JGM.Game
{
    public class ComponentPool<T> : IComponentPool<T> where T : Component
    {
        private readonly Stack<T> m_poolStack = new Stack<T>();

        public ComponentPool() { }

        public ComponentPool(int poolSize, Transform poolParent)
        {
            for (int i = 0; i < poolSize; i++)
            {
                var pooledGO = new GameObject();
                pooledGO.SetName($"Pooled {typeof(T)} {i + 1}");
                pooledGO.transform.SetParent(poolParent);
                pooledGO.SetActive(false);
                var pooledComponent = pooledGO.AddComponent<T>();
                m_poolStack.Push(pooledComponent);
            }
        }

        public ComponentPool(int poolSize, Transform poolParent, T prefab)
        {
            for (int i = 0; i < poolSize; i++)
            {
                var pooledGO = GameObject.Instantiate(prefab);
                pooledGO.gameObject.SetName($"Pooled {typeof(T)} {i + 1}");
                pooledGO.transform.SetParent(poolParent);
                pooledGO.gameObject.SetActive(false);
                m_poolStack.Push(pooledGO);
            }
        }

        public virtual T Get()
        {
            if (m_poolStack.Count == 0)
            {
                return null;
            }

            T component = m_poolStack.Pop();
            component.gameObject.SetActive(true);
            return component;
        }

        public virtual void Return(T component)
        {
            component.gameObject.SetActive(false);
            m_poolStack.Push(component);
        }
    }
}