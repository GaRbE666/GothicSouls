using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JS
{
    public class EnemyBossManager : MonoBehaviour
    {
        EnemyManager enemy;
        public string bossName;
        UIBossHealthBar bossHealthBar;
        BossCombatStanceState bossCombatStanceState;
        public WorldEventManager worldEventManager;
        public bool hasPhaseShifted;

        [Header("Second Phase FX")]
        public GameObject particleFX;

        private void Awake()
        {
            enemy = GetComponent<EnemyManager>();
            bossHealthBar = FindObjectOfType<UIBossHealthBar>();
            bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
        }

        private void Start()
        {
            bossHealthBar.SetBossName(bossName);
            bossHealthBar.SetBossMaxHealth(enemy.enemyStatsManager.maxHealth);
        }

        public void UpdateBossHealthBar(float currentHealth, float maxHealth)
        {
            bossHealthBar.SetBossCurrentHealth(currentHealth);

            if (currentHealth <= maxHealth / 2 && !hasPhaseShifted)
            {
                hasPhaseShifted = true;
                ShiftToSecondPhase();
            }

            if (currentHealth <= 0)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimation("Dead", true);
                worldEventManager.BossHasBeenDefeated();
                bossHealthBar.gameObject.SetActive(false);
            }
        }

        public void ShiftToSecondPhase()
        {
            enemy.animator.SetBool("isInvulnerable", true);
            enemy.animator.SetBool("isPhaseShifting", true);
            enemy.enemyAnimatorManager.PlayTargetAnimation("Boss Phase Shift", true);
            bossCombatStanceState.hasPhaseShifted = true;
        }
    }
}
