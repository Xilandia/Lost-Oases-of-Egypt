using UnityEngine;
using _Scripts.Interaction.Management;

namespace _Scripts.Interaction.Action
{
    public class Action : MonoBehaviour
    {
        public void OnClick()
        {
            ActionHandler.instance.TriggerAction(name);
        }
    }
}