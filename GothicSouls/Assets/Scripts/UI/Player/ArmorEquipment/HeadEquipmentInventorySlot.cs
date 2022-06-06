using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JS
{
    public class HeadEquipmentInventorySlot : MonoBehaviour
    {
        UIManager uiManager;
        PlayerManager player;

        public Image icon;
        HelmetEquipment item;

        private void Awake()
        {
            uiManager = GetComponentInParent<UIManager>();
            player = FindObjectOfType<PlayerManager>();
        }

        public void AddItem(HelmetEquipment newItem)
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
            if (uiManager.headEquipmentSlotSelected)
            {
                if (uiManager.player.playerInventoryManager.currentHelmetEquipment != null)
                {
                    player.playerAudioManager.PlayEquipArmor();
                    uiManager.player.playerInventoryManager.headEquipmentInventory.Add(uiManager.player.playerInventoryManager.currentHelmetEquipment);
                }

                uiManager.player.playerInventoryManager.currentHelmetEquipment = item;
                uiManager.player.playerInventoryManager.headEquipmentInventory.Remove(item);
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
