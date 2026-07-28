using UnityEngine;

namespace Soso.Utils.Framework
{
    public abstract class BaseSingleton<T> : MonoBehaviour 
        where T : BaseSingleton<T>
    {
        public enum BEHAVIOR
        {
            DestroyNewest,
            DestroyOldest
        }

        [Header("Singleton")]
        [SerializeField] public new bool DontDestroyOnLoad;
        [SerializeField] public bool InitializeOnAwake;
        [SerializeField] public bool ShutdownOnDestroy;
        [SerializeField] public BEHAVIOR Behavior = BEHAVIOR.DestroyNewest;

        public static T GetInstance()
        {
            return _instance;
        }

        public static bool TryGetInstance(out T instance)
        {
            instance = _instance;
            return instance != null;
        }

        private static T _instance = null;
        private bool _isInitialized = false;

        private async void Awake()
        {
            if (isActiveAndEnabled == false)
            {
                Destroy(this);
                return;
            }
            if (_instance != null)
            {
                if (Behavior == BEHAVIOR.DestroyOldest)
                {
                    Destroy(_instance);
                }
                else
                {
                    Destroy(this);
                    return;
                }
            }
            if (_instance != this)
            {
                _instance = this as T;
                if (DontDestroyOnLoad)
                {
                    DontDestroyOnLoad(_instance);
                }
            }
            
            if (InitializeOnAwake)
            {
                await InitializeAsync();
            }
        }

        private async void OnDestroy()
        {
            if (ShutdownOnDestroy)
            {
                await ShutDown();
            }
        }

        public async Awaitable Initialize()
        {
            if (_isInitialized)
            {
                return;
            }
            _isInitialized = true;

            await InitializeAsync();
        }

        public async Awaitable ShutDown()
        {
            await ShutdownAsync();
        }

        protected virtual Awaitable InitializeAsync()
        {
            return Awaitable.NextFrameAsync();
        }

        protected virtual Awaitable ShutdownAsync()
        {
            return Awaitable.NextFrameAsync();
        }

        protected bool IsInitialized()
        {
            return _isInitialized;
        }
    }
}