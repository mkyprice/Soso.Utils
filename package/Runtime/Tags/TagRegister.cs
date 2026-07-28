using System;
using UnityEngine;

namespace Soso.Utils.Tags
{
    public class TagRegister : MonoBehaviour
    {
        [Serializable]
        public enum EVENT
        {
            OnEnable,
            OnAwake,
            OnStart,
            Custom,
        }

        [SerializeField] public EVENT RegisterMode = EVENT.OnAwake;

        private void Awake()
        {
            if (RegisterMode == EVENT.OnAwake)
            {
                Register();
            }
        }

        private void Start()
        {
            if (RegisterMode == EVENT.OnStart)
            {
                Register();
            }
        }

        private void OnEnable()
        {
            if (RegisterMode == EVENT.OnEnable)
            {
                Register();
            }
        }

        private void OnDisable()
        {
            if (RegisterMode == EVENT.OnEnable)
            {
                Unregister();
            }
        }

        private void OnDestroy()
        {
            if (RegisterMode == EVENT.OnAwake || RegisterMode == EVENT.OnStart)
            {
                Unregister();
            }
        }

        public void Register()
        {
            if (TagManager.TryGetInstance(out var tagManager))
            {
                tagManager.RegisterInstance(gameObject);
            }
        }

        public void Unregister()
        {
            if (TagManager.TryGetInstance(out var tagManager))
            {
                tagManager.UnregisterInstance(gameObject);
            }
        }
    }
}