using UnityEngine;

public class ITrainer : Interactable
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
