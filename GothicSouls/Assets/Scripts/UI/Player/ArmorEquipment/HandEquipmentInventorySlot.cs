using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JS
{
    public class HandEquipmentInventorySlot : MonoBehaviour
    {
        UIManager uiManager;
        PlayerManager player;

        public Image icon;
        HandEquipment item;

        private void Awake()
        {
            uiManager = GetComponentInParent<UIManager>();
            player = FindObjectOfType<PlayerManager>();
        }

        public void AddItem(HandEquipment newItem)
        {
            item = newItem;
            icon.sprite = item.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearInventorySlot()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        public void EquipThisItem()
        {
            if (uiManager.handEquipmentSlotSelected)
            {
                if (uiManager.player.playerInventoryManager.currentHandEquipment != null)
                {
                    player.playerAudioManager.PlayEquipArmor();
                    uiManager.player.playerInventoryManager.handEquipmentInventory.Add(uiManager.player.playerInventoryManager.currentHandEquipment);
                }

                uiManager.player.playerInventoryManager.currentHandEquipment = item;
                uiManager.player.playerInventoryManager.handEquipmentInventory.Remove(item);
                uiManager.player.playerEquipmentManager.EquipAllEquipmentModelsOnStart();
            }
            else
            {
                return;
            }

            uiManager.equipmentWindowUI.LoadArmorOnEquipmentScreen(uiManager.player.playerInventoryManager);
            uiManager.ResetAllSelectedSlots();
        }
    }
}
