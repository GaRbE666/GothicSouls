using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JS
{
    public class RotateTowardsTargetState : State
    {
        public CombatStanceState combatStanceState;

        public override State Tick(EnemyManager enemy)
        {
            enemy.animator.SetFloat("Vertical", 0);
            enemy.animator.SetFloat("Horizontal", 0);

            Vector3 targetDrection = enemy.currentTarget.transform.position - enemy.transform.position;
            float viewableAngle = Vector3.SignedAngle(targetDrection, enemy.transform.forward, Vector3.up);

            if (enemy.isInteracting)
            {
                return this; //When we enter the state we will still be interacting from the attack animation so we pause here unti it has finished
            }

            if (viewableAngle >= 100 && viewableAngle <= 100 && !enemy.isInteracting)
            {
                Debug.Log("Entro");
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Behind", true);
                return combatStanceState;
            }
            else if (viewableAngle <= -101 && viewableAngle >= -180 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Behind", true);
                return combatStanceState;
            }
            else if (viewableAngle <= -45 && viewableAngle >= -100 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Right", true);
                return combatStanceState;
            }
            else if (viewableAngle >= 45 && viewableAngle <= 100 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Left", true);
                return combatStanceState;
            }

            return combatStanceState;
        }
    }
}
