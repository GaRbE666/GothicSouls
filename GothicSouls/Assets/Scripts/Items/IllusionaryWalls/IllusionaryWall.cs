using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class IllusionaryWall : MonoBehaviour
    {
        public bool wallHasBeenHit;
        public Material illusionaryWallMaterial;
        public float alpha;
        public float fadeTimer = 2.5f;
        public BoxCollider wallCollider;

        public AudioSource audioSource;
        public AudioClip illusionaryWallSound;

        private void Update()
        {
            if (wallHasBeenHit)
            {
                FadeIllusionaryWall();
            }
        }

        public void FadeIllusionaryWall()
        {
            alpha = illusionaryWallMaterial.color.a;
            alpha -= Time.deltaTime / fadeTimer;
            Color fadeWallColor = new Color(1, 1, 1, alpha);
            illusionaryWallMaterial.color = fadeWallColor;

            if (wallCollider.enabled)
            {
                wallCollider.enabled = false;
                audioSource.PlayOneShot(illusionaryWallSound);
            }

            if (alpha <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
