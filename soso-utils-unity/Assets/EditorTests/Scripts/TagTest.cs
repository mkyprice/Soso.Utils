using System;
using Soso.Utils.Editor.Tags;
using Soso.Utils.Tags;
using UnityEngine;

public class TagTest : MonoBehaviour
{
    [SerializeField, TagSelector] public string SpawnTag;
    [SerializeField, TagSelector] public string SubTag;

    private string _prevSubTag;

    private void Start()
    {
        SubscribeToTags();
    }

    private void OnValidate()
    {
        SubscribeToTags();
    }

    private void SubscribeToTags()
    {
        if (TagManager.TryGetInstance(out var tagManager))
        {
            if (string.IsNullOrEmpty(_prevSubTag) == false)
            {
                tagManager.Unsubscribe(_prevSubTag, OnObjectSpawned, OnObjectDespawned);
            }
            tagManager.Unsubscribe(SubTag, OnObjectSpawned, OnObjectDespawned);
            tagManager.Subscribe(SubTag, OnObjectSpawned, OnObjectDespawned);
            _prevSubTag = SubTag;
        }
    }

    public void SpawnInstance()
    {
        var go = new GameObject($"Obj {SpawnTag}");
        go.tag = SpawnTag;
        go.AddComponent<TagRegister>();
    }

    private void OnObjectSpawned(GameObject obj)
    {
        Debug.Log($"Spawned {obj} with tag {obj.tag}");
    }

    private void OnObjectDespawned(GameObject obj)
    {
        Debug.Log($"Despawned {obj} with tag {obj.tag}");
    }
}
