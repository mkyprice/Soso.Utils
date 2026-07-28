using System;
using System.Collections.Generic;
using Soso.Utils.Framework;
using UnityEngine;

namespace Soso.Utils.Tags
{
    public class TagManager : BaseSingleton<TagManager>
    {
        private Dictionary<string, Action<GameObject>> _onObjectSpawned = new Dictionary<string, Action<GameObject>>();
        private Dictionary<string, Action<GameObject>> _onObjectDespawned = new Dictionary<string, Action<GameObject>>();
        private Dictionary<string, HashSet<GameObject>> _taggedInstances = new Dictionary<string, HashSet<GameObject>>();

        public void Subscribe(string instanceTag, Action<GameObject> onObjectSpawned = null, Action<GameObject> onObjectDespawned = null)
        {
            AddAction(instanceTag, onObjectSpawned, _onObjectSpawned);
            AddAction(instanceTag, onObjectDespawned, _onObjectDespawned);
        }

        public void Unsubscribe(string instanceTag, Action<GameObject> onObjectSpawned = null, Action<GameObject> onObjectDespawned = null)
        {
            RemoveAction(instanceTag, onObjectSpawned, _onObjectSpawned);
            RemoveAction(instanceTag, onObjectDespawned, _onObjectDespawned);
        }

        public void RegisterInstance(GameObject instance)
        {
            var instanceTag = instance.tag;

            if (_taggedInstances.TryGetValue(instanceTag, out var instances) == false)
            {
                instances = new HashSet<GameObject>();
                _taggedInstances[instanceTag] = instances;
            }
            
            instances.Add(instance);
            
            if (_onObjectSpawned.TryGetValue(instanceTag, out var action))
            {
                action?.Invoke(instance);
            }
        }

        public void UnregisterInstance(GameObject instance)
        {
            var instanceTag = instance.tag;

            if (_taggedInstances.TryGetValue(instanceTag, out var instances))
            {
                instances.Remove(instance);
            }
            
            if (_onObjectDespawned.TryGetValue(instanceTag, out var action))
            {
                action?.Invoke(instance);
            }
        }

        public IEnumerable<GameObject> GetTaggedInstances(string instanceTag)
        {
            if (_taggedInstances.TryGetValue(instanceTag, out var instances))
            {
                foreach (var instance in instances)
                {
                    yield return instance;
                }
            }
        }

        public void Clear()
        {
            _onObjectSpawned.Clear();
            _onObjectDespawned.Clear();
            _taggedInstances.Clear();
        }

        #region Internal

        protected override Awaitable ShutdownAsync()
        {
            Clear();
            
            return base.ShutdownAsync();
        }

        private void AddAction(string instanceTag, Action<GameObject> action, Dictionary<string, Action<GameObject>> actions)
        {
            if (action != null && actions.TryAdd(instanceTag, action) == false)
            {
                actions[instanceTag] += action;
            }
        }

        private void RemoveAction(string instanceTag, Action<GameObject> action, Dictionary<string, Action<GameObject>> actions)
        {
            if (action != null && actions.ContainsKey(instanceTag))
            {
                var currentAction = actions[instanceTag];
                currentAction -= action;
                if (currentAction == null)
                {
                    actions.Remove(instanceTag);
                }
                else
                {
                    actions[instanceTag] = currentAction;
                }
            }
        }

        #endregion
    }
}
