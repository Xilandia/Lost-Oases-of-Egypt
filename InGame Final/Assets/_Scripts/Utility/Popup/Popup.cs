using TMPro;
using UnityEngine;

namespace _Scripts.Utility.Popup
{
    public class Popup : MonoBehaviour
    {
        public delegate void DisableAction(Popup Instance);

        public event DisableAction disable;

        public TextMeshProUGUI text;
        public float timeAlive;
        private Camera cam;

        void Awake()
        {
            cam = PopupHandler.instance.cam;
        }
        
        void Update()
        {
            timeAlive += Time.deltaTime;
            transform.position += Vector3.up * (Time.deltaTime * 4f);
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
                cam.transform.rotation * Vector3.up);
            
            if (timeAlive >= 5f)
            {
                disable?.Invoke(this);
            }
        }
    }
}
