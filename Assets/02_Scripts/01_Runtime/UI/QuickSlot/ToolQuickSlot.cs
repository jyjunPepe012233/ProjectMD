using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MinD.SO.Item;
using TMPro;
using UnityEngine.Serialization;

namespace MinD.Runtime.UI
{
    public class ToolQuickSlot : MonoBehaviour
    {
        public TextMeshProUGUI toolName;
        public Image[] slotImages;
        private List<Tool> toolList = new();
        private int currentIndex = 0;

        public void Initialize(List<Tool> toolSlots)
        {
            if (toolSlots == null)
            {
                Debug.LogError("Tool slots are null during initialization.");
                toolList = new List<Tool>();
            }
            else
            {
                toolList = new List<Tool>(toolSlots);
                toolList.RemoveAll(tool => tool == null);
            }

            currentIndex = 0;
            UpdateUI();
        }

        public void UpdateUI()
        {
            if (toolList.Count == 0)
            {
                foreach (var image in slotImages)
                {
                    image.enabled = false;
                }
                return;
            }
            toolName.text = toolList[currentIndex].itemName;
            for (int i = 0; i < slotImages.Length; i++)
            {
                int index = (currentIndex + i - 2 + toolList.Count) % toolList.Count;
                slotImages[i].sprite = toolList[index].itemImage;
                slotImages[i].enabled = true;
            }
        }

        public void Rotate(int direction)
        {
            if (toolList.Count == 0) return;

            currentIndex = (currentIndex + direction + toolList.Count) % toolList.Count;
            UpdateUI();
        }

        public Tool GetCurrentTool()
        {
            if (toolList.Count == 0) return null;

            return toolList[currentIndex];
        }
    }
}