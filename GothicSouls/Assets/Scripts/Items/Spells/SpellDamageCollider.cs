using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JS
{
    public class SpellDamageCollider : DamageCollider
    {
        #region FIELDS
        public GameObject impactParticles;
        public GameObject projectileParticles;
        public GameObject muzzleParticles;

        bool hasCollided = false;

        CharacterStatsManager spellTarget;
        Rigidbody rigidbody;

        Vector3 impactNormal; //Used to rotate the impact particles
        #endregion

        protected override void Awake()
        {
            base.Awake();
            rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            projectileParticles = Instantiate(projectileParticles, transform.position, transform.rotation);
            projectileParticles.transform.parent = transform;

            if (muzzleParticles)
            {
                muzzleParticles = Instantiate(muzzleParticles, transform.position, transform.rotation);
                Destroy(muzzleParticles, 2f);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!hasCollided)
            {
                spellTarget = collision.transform.GetComponent<CharacterStatsManager>();

                if (spellTarget != null && spellTarget.teamIDNumber != teamIDNumber)
                {
                    spellTarget.TakeDamage(0, fireDamage, currentDamageAnimation);
                }

                hasCollided = true;
                impactParticles = Instantiate(impactParticles, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal));

                Destroy(projectileParticles);
                Destroy(impactParticles, 5f);
                Destroy(gameObject, 5f);
            }
        }
    }
}
