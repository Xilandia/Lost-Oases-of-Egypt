using _Scripts.Utility.Entity;
using _Scripts.Interaction.Action;
using _Scripts.Player.Structure;
using UnityEngine;

namespace _Scripts.Interaction.Interactable
{
    public class InteractableBarracks : Interactable
    {
        public PlayerBarracks pB; // combine as part of refactor

        public override void OnInteractEnter()
        {
            base.OnInteractEnter();
            if (pB.isPrototype)
            {
                highlight.SetActive(false);
            }

            if (pB.isPlaced)
            {
                OnInteractExit();
            }

            if (pB.isComplete)
            {
                ActionFrame.instance.SetActionButtons(pB.trainableUnitNames);
            }
        }

        public override void OnInteractExit()
        {
            if (pB.isComplete)
            {
                ActionFrame.instance.ClearAction();
            }

            base.OnInteractExit();
        }
    }
}