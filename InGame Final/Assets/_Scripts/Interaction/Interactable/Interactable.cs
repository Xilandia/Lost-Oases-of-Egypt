using UnityEngine;

namespace _Scripts.Interaction.Interactable
{
    public abstract class Interactable : MonoBehaviour
    {
        public bool isInteracting = false;
        public GameObject highlight = null;

        public virtual void Awake()
        {
            highlight.SetActive(false);
        }

        public virtual void OnInteractEnter()
        {
            highlight.SetActive(true);
            isInteracting = true;
        }

        public virtual void OnInteractExit()
        {
            highlight.SetActive(false);
            isInteracting = false;
        }
    }
}