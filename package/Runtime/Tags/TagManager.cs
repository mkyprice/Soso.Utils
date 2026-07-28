using System;
using System.Collections.Generic;
using Soso.Utils.Framework;
using UnityEngine;

namespace Soso.Utils.Tags
{
    public class TagManager : BaseSingleton<TagManager>
    {
        public Action<GameObject> OnObjectSpawned;

        private Dictionary<string, HashSet<GameObject>> _taggedInstances = new Dictionary<string, HashSet<GameObject>>();

        public void RegisterInstance(GameObject instance)
        {
            var instanceTag = instance.tag;

            if (_taggedInstances.TryGetValue(instanceTag, out var instances) == false)
            {
                instances = new HashSet<GameObject>();
                _taggedInstances[instanceTag] = instances;
            }
            instances.Add(instance);
            
            OnObjectSpawned?.Invoke(instance);
        }

        public void UnregisterInstance(GameObject instance)
        {
            var instanceTag = instance.tag;

            if (_taggedInstances.TryGetValue(instanceTag, out var instances))
            {
                instances.Remove(instance);
            }
        }

        public IEnumerable<GameObject> GetTaggedInstances(string tag)
        {
            if (_taggedInstances.TryGetValue(tag, out var instances))
            {
                foreach (var instance in instances)
                {
                    yield return instance;
                }
            }
        }
    }
}
