using System;
using System.Collections.Generic;
using MinD.Runtime.Entity;
using MinD.SO.Item;
using UnityEngine;
using UnityEngine.UIElements;

namespace MinD.Runtime.UI
{
    public class QuickSlotUIManager : MonoBehaviour
    {
        public MagicQuickSlot magicQuickSlotUI;
        public ToolQuickSlot toolQuickSlotUI;

        private PlayerInventoryHandler inventoryHandler;

        private void Start()
        {
            inventoryHandler = FindObjectOfType<PlayerInventoryHandler>();
            InitializeQuickSlots();
        }

        public void InitializeQuickSlots()
        {
            magicQuickSlotUI.Initialize(new List<Magic>(inventoryHandler.magicSlots));
            toolQuickSlotUI.Initialize(new List<Tool>(inventoryHandler.toolSlots));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                RotateToolQuickSlot(1);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                RotateMagicQuickSlot(1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                RotateMagicQuickSlot(-1);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                RotateToolQuickSlot(-1);
            }
        }

        public void RotateMagicQuickSlot(int direction)
        {
            magicQuickSlotUI.Rotate(direction);
        }

        public void RotateToolQuickSlot(int direction)
        {
            toolQuickSlotUI.Rotate(direction);
        }
    }
}

