using System.Collections.Generic;
using _Scripts.Utility.Entity;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Interaction.Action
{
    public class ActionFrame : MonoBehaviour
    {
        public static ActionFrame instance;

        [SerializeField] private Button actionButton = null;
        [SerializeField] Transform layoutGroup = null;

        private readonly List<Button> buttons = new List<Button>();
        private readonly List<ActionReferences> buttonReferences = new List<ActionReferences>();

        void Awake()
        {
            instance = this;

            for (int i = 0; i < 9; i++)
            {
                Button button = Instantiate(actionButton, layoutGroup);
                buttons.Add(button);
                buttonReferences.Add(button.GetComponent<ActionReferences>());
                button.gameObject.SetActive(false);
            }
        }

        public void SetActionButtons(EntityUI[] listOfActions)
        {
            for (int i = 0; i < listOfActions.Length; i++)
            {
                buttons[i].gameObject.SetActive(true);
                buttons[i].name = listOfActions[i].entityName;
                buttonReferences[i].image.sprite = listOfActions[i].entitySprite;
                buttonReferences[i].oreCost.text = listOfActions[i].entityCostOre.ToString();
                buttonReferences[i].woodCost.text = listOfActions[i].entityCostWood.ToString();
            }
        }

        public void ActivateButton(int i)
        {
            Button button = buttons[i];
            
            if (button.gameObject.activeSelf)
            {
                button.onClick.Invoke();
            }
        }

        public void ClearAction()
        {
            foreach (Button button in buttons)
            {
                button.gameObject.SetActive(false);
            }
        }
    }
}