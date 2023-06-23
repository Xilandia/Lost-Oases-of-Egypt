using UnityEngine;
using UnityEngine.Pool;

namespace _Scripts.Utility.Popup
{
    public class PopupPoolHandler : MonoBehaviour
    {
        public static PopupPoolHandler instance;

        public ObjectPool<Popup> popupPool;

        [SerializeField] private Popup popupPrefab;

        void Awake()
        {
            instance = this;
            popupPool = new ObjectPool<Popup>(CreatePooledObject, OnTakeFromPool, OnReturnToPool, OnDestroyObject,
                false, 20, 50);
        }

        private Popup CreatePooledObject()
        {
            Popup Instance = Instantiate(popupPrefab, Vector3.zero, Quaternion.identity);
            Instance.disable += ReturnObjectToPool;
            Instance.gameObject.SetActive(false);
            Instance.transform.parent = transform;

            return Instance;
        }

        private void ReturnObjectToPool(Popup Instance)
        {
            popupPool.Release(Instance);
        }

        private void OnTakeFromPool(Popup Instance)
        {
            Instance.gameObject.SetActive(true);
            Instance.timeAlive = 0f;
            // Do something with the instance as it is taken from the pool.
        }

        private void OnReturnToPool(Popup Instance)
        {
            Instance.gameObject.SetActive(false);
        }

        private void OnDestroyObject(Popup Instance)
        {
            Destroy(Instance.gameObject);
        }
    }
}
