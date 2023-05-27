using UnityEngine;

public class Action : MonoBehaviour
{
    public void OnClick()
    {
        InputHandler.instance.selectedStructure.AddToQueue(name);
    }
}
