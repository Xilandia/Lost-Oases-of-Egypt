using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionFrame : MonoBehaviour
{
    public static ActionFrame instance;

    [SerializeField] private Button actionButton = null;
    [SerializeField] Transform layoutGroup = null;
    
    private List<Button> buttons = new List<Button>();

    void Awake()
    {
        instance = this;
    }

    public void SetActionButtons(Entity trainer)
    {
        foreach (Entity pU in trainer.buildableUnits)
        {
            Button button = Instantiate(actionButton, layoutGroup);
            button.name = pU.entityName;
            buttons.Add(button);
        }
        /*foreach (PlayerTrainer pT in actions.playerTrainers)
        {
            Button button = Instantiate(actionButton, layoutGroup);
            button.name = pT.trainerName;
            buttons.Add(button);
        }*/
    }

    public void ClearAction()
    {
        // object pool buttons?
        foreach (Button button in buttons)
        {
            Destroy(button.gameObject);
        }
        buttons.Clear();
    }
    
    
}
