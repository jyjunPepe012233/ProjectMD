using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MinD.SO.Item;
using TMPro;

namespace MinD.Runtime.UI
{
    public class MagicQuickSlot : MonoBehaviour
    {
        public TextMeshProUGUI itemName;
        public Image[] slotImages;
        private List<Magic> magicList = new();
        private int currentIndex = 0;

        public void Initialize(List<Magic> magicSlots)
        {
            if (magicSlots == null)
            {
                Debug.LogError("Magic slots are null during initialization.");
                magicList = new List<Magic>();
            }
            else
            {
                magicList = new List<Magic>(magicSlots);
                magicList.RemoveAll(magic => magic == null); // Null 항목 제거
            }
            
            currentIndex = 0;
            UpdateUI();
        }

        public void UpdateUI()
        {
            if (magicList.Count == 0)
            {
                // 슬롯이 비어 있는 경우
                foreach (var image in slotImages)
                {
                    image.enabled = false;
                }
                return;
            }
            itemName.text = magicList[currentIndex].itemName;
            for (int i = 0; i < slotImages.Length; i++)
            {
                int index = (currentIndex + i - 2 + magicList.Count) % magicList.Count; // 순환 인덱스 계산
                slotImages[i].sprite = magicList[index].itemImage;
                slotImages[i].enabled = true;
            }
        }

        public void Rotate(int direction)
        {
            if (magicList.Count == 0) return;
        
            currentIndex = (currentIndex + direction + magicList.Count) % magicList.Count;
            UpdateUI();
        }

        public Magic GetCurrentMagic()
        {
            if (magicList.Count == 0) return null;

            return magicList[currentIndex];
        }
    }
}