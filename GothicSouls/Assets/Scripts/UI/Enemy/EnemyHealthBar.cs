using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JS
{
    public class EnemyHealthBar : MonoBehaviour
    {
        Slider slider;
        float timeUntilBarIsHidden = 0;

        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
        }

        public void SetHealth(float health)
        {
            slider.value = health;
            timeUntilBarIsHidden = 3;
        }

        public void SetMaxHealth(float maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        public void Update()
        {
            transform.rotation = Camera.main.transform.rotation;
            timeUntilBarIsHidden = timeUntilBarIsHidden - Time.deltaTime;

            if (slider != null)
            {
                if (timeUntilBarIsHidden <= 0)
                {
                    timeUntilBarIsHidden = 0;
                    slider.gameObject.SetActive(false);
                }
                else
                {
                    if (!slider.gameObject.activeInHierarchy)
                    {
                        slider.gameObject.SetActive(true);
                    }
                }

                if (slider.value <= 0)
                {
                    Destroy(slider.gameObject);
                }
            }
        }
    }
}
