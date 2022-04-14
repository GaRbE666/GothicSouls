using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class DamagePlayer : MonoBehaviour
    {
        public int damage = 25;
        private void OnTriggerEnter(Collider other)
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            Debug.Log("Entro en el trigger");
            if (playerStats != null)
            {
                Debug.Log("Hago da�o");
                playerStats.TakeDamage(damage);
            }
        }
    }
}
