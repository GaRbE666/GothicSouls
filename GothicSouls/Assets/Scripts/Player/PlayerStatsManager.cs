using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        #region FIELDS
        PlayerManager playerManager;

        public HealthBar healthBar;
        StaminaBar staminaBar;
        FocusPointBar focusPointsBar;
        PlayerAnimatorManager playerAnimatorManager;

        public float staminaRegenerationAmount = 1;
        public float staminaRegenTimer = 0;
        #endregion

        protected override void Awake()
        {
            base.Awake();
            playerManager = GetComponent<PlayerManager>();
            staminaBar = FindObjectOfType<StaminaBar>();
            focusPointsBar = FindObjectOfType<FocusPointBar>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetCurrentHealth(currentHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
            staminaBar.SetCurrentStamina(currentStamina);

            maxFocusPoints = SetMaxFocusPointsFromFocusLevel();
            currentFocusPoints = maxFocusPoints;
            focusPointsBar.SetMaxFocusPoints(maxFocusPoints);
            focusPointsBar.SetCurrentFocusPoints(currentFocusPoints);
        }

        public override void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer -= Time.deltaTime;
            }
            else if(poiseResetTimer <= 0 && playerManager.isInteracting)
            {
                totalPoiseDefense = armorPoiseBonus;
            }
        }

        public override void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation)
        {

            if (playerManager.isInvulnerable)
            {
                return;
            }

            base.TakeDamage(physicalDamage, fireDamage, damageAnimation);

            healthBar.SetCurrentHealth(currentHealth);
            playerAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
                playerAnimatorManager.PlayTargetAnimation("Dead", true);
            }
        }

        public override void TakePoisonDamage(int damage)
        {
            if (isDead)
            {
                return;
            }

            base.TakePoisonDamage(damage);
            healthBar.SetCurrentHealth(currentHealth);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
                playerAnimatorManager.PlayTargetAnimation("Dead", true);
            }
        }

        public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
        {
            base.TakeDamageNoAnimation(physicalDamage, fireDamage);
            healthBar.SetCurrentHealth(currentHealth);
        }

        public void TakeStaminaDamage(float damage)
        {
            currentStamina -= damage;
            staminaBar.SetCurrentStamina(currentStamina);
        }

        public void RegenerateStamina()
        {
            if (playerManager.isInteracting)
            {
                staminaRegenTimer = 0;
            }
            else
            {
                staminaRegenTimer += Time.deltaTime;

                if (currentStamina <= maxStamina && staminaRegenTimer > 1f)
                {
                    currentStamina += staminaRegenerationAmount * Time.deltaTime;
                    staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                }
            }
        }

        public void HealPlayer(int healAmount)
        {
            currentHealth = currentHealth + healAmount;

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            healthBar.SetCurrentHealth(currentHealth);
        }

        public void DeductFocusPoints(int focusPoints)
        {
            currentFocusPoints = currentFocusPoints - focusPoints;
            
            if (currentFocusPoints < 0)
            {
                currentFocusPoints = 0;
            }

            focusPointsBar.SetCurrentFocusPoints(currentFocusPoints);
        }

        public void AddSouls(int souls)
        {
            currentSoulCount = currentSoulCount + souls;
        }
    }
}
