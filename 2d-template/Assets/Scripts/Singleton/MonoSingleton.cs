using UnityEngine;

namespace Singleton
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static readonly object LockObject = new();
        private static T _instance;
        private static bool _isQuitting;

        public static T Instance
        {
            get
            {
                lock (LockObject)
                {
                    if (_isQuitting)
                    {
                        return null;
                    }

                    if (_instance == null)
                    {
                        var findResult = FindAnyObjectByType<T>();
                        if (findResult)
                        {
                            _instance = findResult;
                        }
                        else
                        {
                            var obj = new GameObject(typeof(T).Name);
                            _instance = obj.AddComponent<T>();
                        }

                        DontDestroyOnLoad(_instance.gameObject);
                    }

                    return _instance;
                }
            }
        }

        private void OnDisable()
        {
            _isQuitting = true;
            _instance = null;
        }
    }
}