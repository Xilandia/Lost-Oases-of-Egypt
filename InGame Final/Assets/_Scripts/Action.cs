using UnityEngine;

public class Action : MonoBehaviour
{
    public void OnClick()
    {
        InputHandler.instance.selectedStructure.gameObject.GetComponent<PlayerTrainer>().AddToQueue(name);
    }
}
