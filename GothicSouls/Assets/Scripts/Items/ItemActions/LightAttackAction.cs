using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    [CreateAssetMenu(menuName = "Item Actions/Light Attack Action")]
    public class LightAttackAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.playerStatsManager.currentStamina <= 0)
            {
                return;
            }
            player.playerAnimatorManager.EraseHandIKForWeapon();
            player.playerEffectsManager.PlayWeaponFX(false);

            if (player.canDoCombo)
            {
                player.inputHandler.comboFlag = true;
                HandleLightWeaponCombo(player);
                player.inputHandler.comboFlag = false;
            }
            else
            {
                if (player.isInteracting)
                {
                    return;
                }
                if (player.canDoCombo)
                {
                    return;
                }
                HandleLightAttack(player);
            }
        }

        public void HandleLightAttack(PlayerManager player)
        {
            if (player.isUsingLeftHand)
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_01, true, false);
                player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_01;

            }
            else if (player.isUsingRightHand)
            {
                if (player.inputHandler.twoHandFlag)
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_light_attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.th_light_attack_01;
                }
                else
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_01;
                }
            }
        }

        public void HandleLightWeaponCombo(PlayerManager player)
        {
            if (player.inputHandler.comboFlag)
            {
                player.animator.SetBool("canDoCombo", false);

                if (player.isUsingLeftHand)
                {
                    if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_light_attack_01)
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_02, true, false);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_02;
                    }
                    else
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_01, true, false);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_01;
                    }
                }
                else if (player.isUsingRightHand)
                {
                    if (player.isTwoHandingWeapon)
                    {
                        if (player.playerCombatManager.lastAttack == player.playerCombatManager.th_light_attack_01)
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_light_attack_02, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.th_light_attack_02;
                        }
                        else
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_light_attack_01, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.th_light_attack_01;
                        }
                    }
                    else
                    {
                        if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_light_attack_01)
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_02, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_02;
                        }
                        else
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_01, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_01;
                        }
                    }
                }
            }
        }
    }
}
