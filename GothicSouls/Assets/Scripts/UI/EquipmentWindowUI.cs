using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JS
{
    public class EquipmentWindowUI : MonoBehaviour
    {
        public WeaponEquipmentSlotUI[] weaponEquipmentSlotsUI;
        public HeadEquipmentSlotUI headEquipmentSlotUI;
        public BodyEquipmentSlotUI bodyEquipmentSlotUI;
        public LegEquipmentSlotUI legEquipmentSlotUI;
        public HandEquipmentSlotUI handEquipmentSlotUI;

        public void LoadWeaponOnEquipmentScreen(PlayerInventoryManager playerInventory)
        {
            for (int i = 0; i < weaponEquipmentSlotsUI.Length; i++)
            {
                if (weaponEquipmentSlotsUI[i].rightHandSlot01)
                {
                    weaponEquipmentSlotsUI[i].AddItem(playerInventory.weaponsInRightHandSlot[0]);
                }
                else if (weaponEquipmentSlotsUI[i].rightHandSlot02)
                {
                    weaponEquipmentSlotsUI[i].AddItem(playerInventory.weaponsInRightHandSlot[1]);
                }
                else if (weaponEquipmentSlotsUI[i].leftHandSlot01)
                {
                    weaponEquipmentSlotsUI[i].AddItem(playerInventory.weaponsInLeftHandSlot[0]);
                }
                else
                {
                    weaponEquipmentSlotsUI[i].AddItem(playerInventory.weaponsInLeftHandSlot[1]);
                }
            }
        }

        public void LoadArmorOnEquipmentScreen(PlayerInventoryManager playerInventory)
        {
            if (playerInventory.currentHelmetEquipment != null)
            {
                headEquipmentSlotUI.AddItem(playerInventory.currentHelmetEquipment);
            }
            else
            {
                headEquipmentSlotUI.ClearItem();
            }

            if (playerInventory.currentBodyEquipment != null)
            {
                bodyEquipmentSlotUI.AddItem(playerInventory.currentBodyEquipment);
            }
            else
            {
                bodyEquipmentSlotUI.ClearItem();
            }

            if (playerInventory.currentLegEquipment != null)
            {
                legEquipmentSlotUI.AddItem(playerInventory.currentLegEquipment);
            }
            else
            {
                legEquipmentSlotUI.ClearItem();
            }

            if (playerInventory.currentHandEquipment != null)
            {
                handEquipmentSlotUI.AddItem(playerInventory.currentHandEquipment);
            }
            else
            {
                handEquipmentSlotUI.ClearItem();
            }
        }

    }
}
