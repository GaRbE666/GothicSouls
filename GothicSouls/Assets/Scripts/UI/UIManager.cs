using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JS
{
    public class UIManager : MonoBehaviour
    {
        #region FIELDS
        public PlayerManager player;
        public EquipmentWindowUI equipmentWindowUI;
        public QuickSlotsUI quickSlotsUI;

        [Header("HUD")]
        public Text soulCount;

        [Header("UI Windows")]
        public GameObject hudWindow;
        public GameObject selectWindow;
        public GameObject equipmentScreenWindow;
        public GameObject weaponInventoryWindow;
        public GameObject optionsWindow;
        public GameObject levelUpWindow;
        public GameObject headEquipmentWindow;
        public GameObject bodyEquipmentWindow;
        public GameObject legEquipmentWindow;
        public GameObject handEquipmentWindow;
        public GameObject deathTextWindow;

        [Header("Equipment Window Slot Selected")]
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;
        public bool headEquipmentSlotSelected;
        public bool bodyEquipmentSlotSelected;
        public bool legEquipmentSlotSelected;
        public bool handEquipmentSlotSelected;

        [Header("Weapon Inventory")]
        public GameObject weaponInventorySlotPrefab;
        public Transform weaponInevntorySlotsParent;
        WeaponInventorySlot[] weaponInventorySlots;

        [Header("Head Equipment Inventory")]
        public GameObject headEquipmentInventorySlotPrefab;
        public Transform headEquipmentInventorySlotParent;
        HeadEquipmentInventorySlot[] headEquipmentInventorySlots;

        [Header("Body Equipment Inventory")]
        public GameObject bodyEquipmentInventorySlotPrefab;
        public Transform bodyEquipmentInventorySlotParent;
        BodyEquipmentInventorySlot[] bodyEquipmentInventorySlots;

        [Header("Leg Equipment Inventory")]
        public GameObject legEquipmentInventorySlotPrefab;
        public Transform legEquipmentInventorySlotParent;
        LegEquipmentInventorySlot[] legEquipmentInventorySlots;

        [Header("Hand Equipment Inventory")]
        public GameObject handEquipmentInventorySlotPrefab;
        public Transform handEquipmentInventorySlotParent;
        HandEquipmentInventorySlot[] handEquipmentInventorySlots;
        #endregion

        private void Awake()
        {
            quickSlotsUI = GetComponentInChildren<QuickSlotsUI>();
            player = FindObjectOfType<PlayerManager>();

            weaponInventorySlots = weaponInevntorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
            headEquipmentInventorySlots = headEquipmentInventorySlotParent.GetComponentsInChildren<HeadEquipmentInventorySlot>();
            bodyEquipmentInventorySlots = bodyEquipmentInventorySlotParent.GetComponentsInChildren<BodyEquipmentInventorySlot>();
            legEquipmentInventorySlots = legEquipmentInventorySlotParent.GetComponentsInChildren<LegEquipmentInventorySlot>();
            handEquipmentInventorySlots = handEquipmentInventorySlotParent.GetComponentsInChildren<HandEquipmentInventorySlot>();

        }

        private void Start()
        {
            equipmentWindowUI.LoadWeaponOnEquipmentScreen(player.playerInventoryManager);
            equipmentWindowUI.LoadArmorOnEquipmentScreen(player.playerInventoryManager);

            if (player.playerInventoryManager.currentSpell != null 
                && player.playerInventoryManager.leftWeapon.weaponType == WeaponType.FaithCaster 
                || player.playerInventoryManager.leftWeapon.weaponType == WeaponType.PyromancyCaster
                || player.playerInventoryManager.rightWeapon.weaponType == WeaponType.FaithCaster
                || player.playerInventoryManager.rightWeapon.weaponType == WeaponType.PyromancyCaster)
            {
                quickSlotsUI.UpdateCurrentSpellIcon(player.playerInventoryManager.currentSpell);
            }

            if (player.playerInventoryManager.currentConsumable != null)
            {
                quickSlotsUI.UpdateCurrentConsumableIcon(player.playerInventoryManager.currentConsumable);
                quickSlotsUI.UpdateCurrentConsumableText(player.playerInventoryManager.currentConsumable.currentItemAmount);
            }

            soulCount.text = player.playerStatsManager.currentSoulCount.ToString();
        }

        public void UpdateUI()
        {
            //WEAPONS INVENTORY SLOTS
            for (int i = 0; i < weaponInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.weaponsInventory.Count)
                {
                    if (weaponInventorySlots.Length < player.playerInventoryManager.weaponsInventory.Count)
                    {
                        Instantiate(weaponInventorySlotPrefab, weaponInevntorySlotsParent);
                        weaponInventorySlots = weaponInevntorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }

                    weaponInventorySlots[i].AddItem(player.playerInventoryManager.weaponsInventory[i]);
                }
                else
                {
                    weaponInventorySlots[i].ClearInventorySlot();
                }
            }

            //HEAD EQUIPMENT INVENTORY SLOTS
            for (int i = 0; i < headEquipmentInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.headEquipmentInventory.Count)
                {
                    if (headEquipmentInventorySlots.Length < player.playerInventoryManager.headEquipmentInventory.Count)
                    {
                        Instantiate(headEquipmentInventorySlotParent, headEquipmentInventorySlotParent);
                        headEquipmentInventorySlots = headEquipmentInventorySlotParent.GetComponentsInChildren<HeadEquipmentInventorySlot>();
                    }

                    headEquipmentInventorySlots[i].AddItem(player.playerInventoryManager.headEquipmentInventory[i]);
                }
                else
                {
                    headEquipmentInventorySlots[i].ClearInventorySlot();
                }
            }

            //BODY EQUIPMENT INVENTORY SLOTS
            for (int i = 0; i < bodyEquipmentInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.bodyEquipmentInventory.Count)
                {
                    if (bodyEquipmentInventorySlots.Length < player.playerInventoryManager.bodyEquipmentInventory.Count)
                    {
                        Instantiate(bodyEquipmentInventorySlotParent, bodyEquipmentInventorySlotParent);
                        bodyEquipmentInventorySlots = bodyEquipmentInventorySlotParent.GetComponentsInChildren<BodyEquipmentInventorySlot>();
                    }

                    bodyEquipmentInventorySlots[i].AddItem(player.playerInventoryManager.bodyEquipmentInventory[i]);
                }
                else
                {
                    bodyEquipmentInventorySlots[i].ClearInventorySlot();
                }
            }
            //LEG EQUIPMENT INVENTORY SLOTS
            for (int i = 0; i < legEquipmentInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.legEquipmentInventory.Count)
                {
                    if (legEquipmentInventorySlots.Length < player.playerInventoryManager.legEquipmentInventory.Count)
                    {
                        Instantiate(legEquipmentInventorySlotParent, legEquipmentInventorySlotParent);
                        legEquipmentInventorySlots = legEquipmentInventorySlotParent.GetComponentsInChildren<LegEquipmentInventorySlot>();
                    }

                    legEquipmentInventorySlots[i].AddItem(player.playerInventoryManager.legEquipmentInventory[i]);
                }
                else
                {
                    legEquipmentInventorySlots[i].ClearInventorySlot();
                }
            }
            //HAND EQUIPMENT INVENTORY SLOTS
            for (int i = 0; i < handEquipmentInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.handEquipmentInventory.Count)
                {
                    if (handEquipmentInventorySlots.Length < player.playerInventoryManager.handEquipmentInventory.Count)
                    {
                        Instantiate(handEquipmentInventorySlotParent, handEquipmentInventorySlotParent);
                        handEquipmentInventorySlots = handEquipmentInventorySlotParent.GetComponentsInChildren<HandEquipmentInventorySlot>();
                    }

                    handEquipmentInventorySlots[i].AddItem(player.playerInventoryManager.handEquipmentInventory[i]);
                }
                else
                {
                    handEquipmentInventorySlots[i].ClearInventorySlot();
                }
            }
        }

        public void OpenSelectWindow()
        {
            selectWindow.SetActive(true);
        }

        public void CloseSelectWindow()
        {
            selectWindow.SetActive(false);
        }

        public void CloseAllInventoryWindows()
        {
            ResetAllSelectedSlots();
            weaponInventoryWindow.SetActive(false);
            equipmentScreenWindow.SetActive(false);
            hudWindow.SetActive(false);
            selectWindow.SetActive(false);
            levelUpWindow.SetActive(false);
            headEquipmentWindow.SetActive(false);
            bodyEquipmentWindow.SetActive(false);
            legEquipmentWindow.SetActive(false);
            handEquipmentWindow.SetActive(false);
            optionsWindow.SetActive(false);
        }

        public void ResetAllSelectedSlots()
        {
            rightHandSlot01Selected = false;
            rightHandSlot02Selected = false;
            leftHandSlot01Selected = false;
            leftHandSlot02Selected = false;

            headEquipmentSlotSelected = false;
            bodyEquipmentSlotSelected = false;
            legEquipmentSlotSelected = false;
            handEquipmentSlotSelected = false;
        }

    }
}
