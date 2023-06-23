using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Utility.Popup
{
    public class PopupHandler : MonoBehaviour
    {
        public static PopupHandler instance;
        
        public Camera cam;

        void Awake()
        {
            instance = this;
        }

        public void CreatePopup(string text, Color color, Vector3 position)
        {
            Popup popup = PopupPoolHandler.instance.popupPool.Get();
            popup.text.text = text;
            popup.text.color = color;
            popup.transform.position = position;
        }
    }
}
