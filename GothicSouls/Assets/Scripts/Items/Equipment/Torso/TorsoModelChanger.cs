using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JS
{
    public class TorsoModelChanger : MonoBehaviour
    {
        public List<GameObject> torsoModels;

        private void Awake()
        {
            GetAllTorsoModels();
        }

        private void GetAllTorsoModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                torsoModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnEquipAllTorsoModels()
        {
            foreach (GameObject torsoModel in torsoModels)
            {
                torsoModel.SetActive(false);
            }
        }

        public void EquipTorsoModelByNAme(string torsoName)
        {
            for (int i = 0; i < torsoModels.Count; i++)
            {
                if (torsoModels[i].name == torsoName)
                {
                    torsoModels[i].SetActive(true);
                }
            }
        }
    }
}
