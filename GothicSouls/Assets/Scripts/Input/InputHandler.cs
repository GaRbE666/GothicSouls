using UnityEngine;

namespace SG
{
    public class InputHandler : MonoBehaviour
    {
        #region FIELDS
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_Input;
        public bool a_Input;
        public bool x_Input;
        public bool y_Input;

        public bool tap_rb_Input;
        public bool hold_rb_Input;
        public bool tap_rt_Input;

        public bool lb_Input;
        public bool tap_lb_Input;
        public bool tap_lt_Input;

        public bool jump_Input;
        public bool inventory_Input;

        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;
        public bool lockOnInput;
        public bool right_Stick_Right_Input;
        public bool right_Stick_Left_Input;

        public bool rollFlag;
        public bool twoHandFlag;
        public bool sprintFlag;
        public bool comboFlag;
        public bool lockOnFlag;
        public bool fireFlag;
        public bool inventoryFlag;
        public float rollInputTimer;

        public Transform critialAttackRayCastStartPoint;

        PlayerControls inputActions;
        PlayerCombatManager playerCombatManager;
        PlayerInventoryManager playerInventoryManager;
        PlayerManager playerManager;
        PlayerAnimatorManager playerAnimatorManager;
        PlayerEffectsManager playerEffectsManager;
        PlayerStatsManager playerStatsManager;
        BlockingCollider blockingCollider;
        PlayerWeaponSlotManager weaponSlotManager;
        CameraHandler cameraHandler;
        UIManager uiManager;

        Vector2 movementInput;
        Vector2 cameraInput;
        #endregion

        private void Awake()
        {
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerManager = GetComponent<PlayerManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            uiManager = FindObjectOfType<UIManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            weaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            blockingCollider = GetComponentInChildren<BlockingCollider>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        }

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;

                inputActions.PlayerActions.LT.performed += i => tap_lt_Input = true;
                inputActions.PlayerActions.TapLB.performed += i => tap_lb_Input = true;
                inputActions.PlayerActions.LB.performed += i => lb_Input = true; //Si va mal cambiar por started
                inputActions.PlayerActions.LB.canceled += i => lb_Input = false;
                inputActions.PlayerActions.RB.performed += i => tap_rb_Input = true;
                inputActions.PlayerActions.HoldRB.performed += i => hold_rb_Input = true;
                inputActions.PlayerActions.HoldRB.canceled += i => hold_rb_Input = false;
                inputActions.PlayerActions.HoldRB.canceled += i => fireFlag = true;
                inputActions.PlayerActions.RT.performed += i => tap_rt_Input = true;
                inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
                inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
                inputActions.PlayerActions.A.performed += i => a_Input = true;
                inputActions.PlayerActions.X.performed += i => x_Input = true;
                inputActions.PlayerActions.Roll.performed += i => b_Input = true;
                inputActions.PlayerActions.Roll.canceled += i => b_Input = false;
                //inputActions.PlayerActions.Jump.performed += inputActions => jump_Input = true; //Jump               
                inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
                inputActions.PlayerActions.Y.performed += i => y_Input = true;
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            if (playerStatsManager.isDead)
            {
                return;
            }
            HandleMoveInput();
            HandleRollInput();

            HandleHoldRBInput();
            HandleHoldLBInput();

            HandleTapLBInput();
            HandleTapRBInput();
            HandleTapRTInput();
            HandleTapLTInput();

            HandleQuickSlotsInput();
            HandleInventoryInput();

            HandleLockOnInput();
            HandleTwoHandInput();
            HandleUseConsumableInput();
            
        }

        public void HandleMoveInput()
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        private void HandleRollInput()
        {
            if (b_Input)
            {
                rollInputTimer += Time.deltaTime;

                if (playerStatsManager.currentStamina <= 0)
                {
                    b_Input = false;
                    sprintFlag = false;
                }

                if (moveAmount > 0.5f && playerStatsManager.currentStamina > 0)
                {
                    sprintFlag = true;
                }
            }
            else
            {
                sprintFlag = false;
                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }

        private void HandleTapRBInput()
        {
            if (tap_rb_Input)
             {
                tap_rb_Input = false;

                if (playerInventoryManager.rightWeapon.tap_RB_Action != null)
                {
                    playerManager.UpdateWhichHandCharacterIsUsing(true);
                    playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                    playerInventoryManager.rightWeapon.tap_RB_Action.PerformAction(playerManager);
                }
            }
        }

        private void HandleHoldRBInput()
        {
            if (hold_rb_Input)
            {
                if (playerInventoryManager.rightWeapon.hold_RB_Action != null)
                {
                    playerManager.UpdateWhichHandCharacterIsUsing(true);
                    playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                    playerInventoryManager.rightWeapon.hold_RB_Action.PerformAction(playerManager);
                }
            }
        }

        private void HandleTapRTInput()
        {
            if (tap_rt_Input)
            {
                tap_rt_Input = false;

                if (playerInventoryManager.rightWeapon.tap_RT_Action != null)
                {
                    playerManager.UpdateWhichHandCharacterIsUsing(true);
                    playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                    playerInventoryManager.rightWeapon.tap_RT_Action.PerformAction(playerManager);
                }
            }
        }

        private void HandleTapLTInput()
        {
            if (tap_lt_Input)
            {
                tap_lt_Input = false;
                if (playerManager.isTwoHandingWeapon)
                {
                    if (playerInventoryManager.rightWeapon.tap_LT_Action != null)
                    {
                        playerManager.UpdateWhichHandCharacterIsUsing(true);
                        playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                        playerInventoryManager.rightWeapon.tap_LT_Action.PerformAction(playerManager);
                    }
                }
                else
                {
                    if (playerInventoryManager.leftWeapon.tap_LT_Action != null)
                    {
                        playerManager.UpdateWhichHandCharacterIsUsing(false);
                        playerInventoryManager.currentItemBeingUsed = playerInventoryManager.leftWeapon;
                        playerInventoryManager.leftWeapon.tap_LT_Action.PerformAction(playerManager);
                    }
                }
            }
        }

        private void HandleHoldLBInput()
        {
            if (playerManager.isInAir || playerManager.isFiringSpell)
            {
                lb_Input = false;
                return;
            }

            if (lb_Input)
            {
                if (playerManager.isTwoHandingWeapon)
                {
                    if (playerInventoryManager.rightWeapon.hold_LB_Action != null)
                    {
                        playerManager.UpdateWhichHandCharacterIsUsing(true);
                        playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                        playerInventoryManager.rightWeapon.hold_LB_Action.PerformAction(playerManager);
                    }
                }
                else
                {
                    if (playerInventoryManager.leftWeapon.hold_LB_Action != null)
                    {
                        playerManager.UpdateWhichHandCharacterIsUsing(false);
                        playerInventoryManager.currentItemBeingUsed = playerInventoryManager.leftWeapon;
                        playerInventoryManager.leftWeapon.hold_LB_Action.PerformAction(playerManager);
                    }
                }
            }
            else if (lb_Input == false)
            {
                if (blockingCollider.blockingCollider.enabled)
                {
                    playerManager.isBlocking = false;
                    blockingCollider.DisableBlockingCollider();
                }
            }
        }

        private void HandleTapLBInput()
        {
            if (tap_lb_Input)
            {
                tap_lb_Input = false;
                if (playerManager.isTwoHandingWeapon)
                {
                    if (playerInventoryManager.rightWeapon.tap_LB_Action != null)
                    {
                        playerManager.UpdateWhichHandCharacterIsUsing(true);
                        playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                        playerInventoryManager.rightWeapon.tap_LB_Action.PerformAction(playerManager);
                    }
                }
                else
                {
                    if (playerInventoryManager.leftWeapon.tap_LB_Action != null)
                    {
                        playerManager.UpdateWhichHandCharacterIsUsing(false);
                        playerInventoryManager.currentItemBeingUsed = playerInventoryManager.leftWeapon;
                        playerInventoryManager.leftWeapon.tap_LB_Action.PerformAction(playerManager);
                    }
                }
            }
        }

        private void HandleQuickSlotsInput()
        {
            if (d_Pad_Right)
            {
                playerInventoryManager.ChangeRightWeapon();
            }
            else if (d_Pad_Left)
            {
                playerInventoryManager.ChangeLeftWeapon();
            }
        }

        private void HandleInventoryInput()
        {
            if (inventory_Input)
            {
                inventoryFlag = !inventoryFlag;

                if (inventoryFlag)
                {
                    uiManager.OpenSelectWindow();
                    uiManager.UpdateUI();
                    uiManager.hudWindow.SetActive(false);
                }
                else
                {
                    uiManager.CloseSelectWindow();
                    uiManager.CloseAllInventoryWindows();
                    uiManager.hudWindow.SetActive(true);
                }
            }
        }

        private void HandleLockOnInput()
        {
            if (lockOnInput && lockOnFlag == false)
            {
                lockOnInput = false;
                cameraHandler.HandleLockOn();

                if (cameraHandler.nearestLockOntarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOntarget;
                    lockOnFlag = true;
                }
            }
            else if (lockOnInput && lockOnFlag)
            {
                lockOnInput = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTargets();
            }

            if (lockOnFlag && right_Stick_Left_Input)
            {
                right_Stick_Left_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.leftLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                }
            }

            if (lockOnFlag && right_Stick_Right_Input)
            {
                right_Stick_Right_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.rightLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                }
            }

            cameraHandler.SetCameraHeight();
        }

        private void HandleTwoHandInput()
        {
            if (y_Input)
            {
                y_Input = false;

                twoHandFlag = !twoHandFlag;

                if (twoHandFlag && !playerInventoryManager.rightWeapon.isUnarmed)
                {
                    playerManager.isTwoHandingWeapon = true;
                    weaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
                    weaponSlotManager.LoadTwoHandIKTarget(true);
                }
                else
                {
                    playerManager.isTwoHandingWeapon = false;
                    weaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
                    weaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);
                    weaponSlotManager.LoadTwoHandIKTarget(false);
                }
            }
        }

        private void HandleUseConsumableInput()
        {
            if (x_Input)
            {
                x_Input = false;
                playerInventoryManager.currentConsumable.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
            }
        }
    }
}
