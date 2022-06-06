using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JS
{
    public class PoisonBuildUpBar : MonoBehaviour
    {
        public Slider slider;

        private void Start()
        {
            slider = GetComponent<Slider>();
            slider.maxValue = 100;
            slider.value = 0;
            gameObject.SetActive(false);
        }

        public void SetPoisonBuilUpAmount(float currentPoisonBuildUp)
        {
            slider.value = currentPoisonBuildUp;
        }
    }
}
