using _Scripts.Utility.Entity;
using _Scripts.Interaction.Action;
using _Scripts.Player.Structure;

namespace _Scripts.Interaction.Interactable
{
    public class InteractableTrainer : Interactable
    {
        public Entity playerTrainer; // squish into pT
        public PlayerTrainer pT; // combine as part of refactor

        public override void OnInteractEnter()
        {
            base.OnInteractEnter();
            if (pT.isPrototype)
            {
                highlight.SetActive(false);
            }

            if (pT.isPlaced)
            {
                OnInteractExit();
            }

            if (pT.isComplete)
            {
                ActionFrame.instance.SetActionButtons(playerTrainer);
            }
        }

        public override void OnInteractExit()
        {
            ActionFrame.instance.ClearAction();
            base.OnInteractExit();
        }
    }
}