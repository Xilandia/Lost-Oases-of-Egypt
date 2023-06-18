using _Scripts.Interaction.Action;
using _Scripts.Player.Unit;

namespace _Scripts.Interaction.Interactable
{
    public class InteractableWorker : Interactable
    {
        public PlayerWorker pW;

        public override void OnInteractEnter()
        {
            if (!pW.isAttemptingToBuild)
            {
                base.OnInteractEnter();
                ActionFrame.instance.SetActionButtons(pW.buildableStructureInfo);
            }
        }

        public override void OnInteractExit()
        {
            ActionFrame.instance.ClearAction();
            base.OnInteractExit();
        }
    }
}