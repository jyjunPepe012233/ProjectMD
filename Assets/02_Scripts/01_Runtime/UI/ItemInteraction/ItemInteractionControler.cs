using MinD.Runtime.Entity;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Input = UnityEngine.Windows.Input;

namespace MinD.Runtime.UI
{

    public class InteractionPanelController : MonoBehaviour
    {
        public GameObject interactionPanel;



        public void RefreshInteractionPanel()
        {
            if (Player.player.interaction.currentInteractables.Count == 0)
            {
                UnDisplayItemInteractionPanel();
            }
            else
            {
                DisplayItemInteractionPanel();
                interactionPanel.GetComponentInChildren<TextMeshProUGUI>().text = Player.player.interaction.currentInteractables[0].interactionText;
            }
        }

        public void DisplayItemInteractionPanel()
        {
            interactionPanel.SetActive(true);
        }

        public void UnDisplayItemInteractionPanel()
        {
            interactionPanel.SetActive(false);
        }
    }
}