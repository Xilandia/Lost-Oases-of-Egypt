using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Interaction.Action
{
    public class ActionFrame : MonoBehaviour
    {
        public static ActionFrame instance;

        [SerializeField] private Button actionButton = null;
        [SerializeField] Transform layoutGroup = null;

        private List<Button> buttons = new List<Button>();

        void Awake()
        {
            instance = this;

            for (int i = 0; i < 9; i++)
            {
                Button button = Instantiate(actionButton, layoutGroup);
                buttons.Add(button);
                button.gameObject.SetActive(false);
            }
        }

        public void SetActionButtons(string[] listOfActions)
        {
            for (int i = 0; i < listOfActions.Length; i++)
            {
                buttons[i].gameObject.SetActive(true);
                buttons[i].name = listOfActions[i];
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