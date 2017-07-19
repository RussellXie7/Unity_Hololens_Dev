
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using HoloToolkit.Unity.SpatialMapping;

namespace HoloToolkit.Unity
{
    public class SpawnPortalAndCube : MonoBehaviour
    {
        public GameObject portal;
        public GameObject cube;

        private GameObject portalInstance;
        private GameObject cubeInstance;

        private bool portalSpawned;
        private bool cubeSpawned;

        private void Start()
        {
            portalSpawned = false;
            cubeSpawned = false;
        }

        private void Update()
        {
            if (portalSpawned && !cubeSpawned)
            {
                float delta = Vector3.Distance(Camera.main.transform.position, portalInstance.transform.position);
                Debug.Log("The delta distance is " + delta);
                if (delta < 1.2f)
                {
                    SpawnCube();
                }
            }
        }

        void SpawnPortal(SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult result)
        {
            if (portalSpawned)
            {
                return;
            }
            var pos = new Vector3(Camera.main.transform.position.x + 4f, -0.5f, Camera.main.transform.position.z);
            portalInstance = Instantiate(portal, result.Position, Quaternion.identity);
            portalInstance.AddComponent<Rigidbody>();
            portalInstance.GetComponent<Rigidbody>().isKinematic = true;

            portalSpawned = true;
        }

        void SpawnCube()
        {
            var pos = new Vector3(portalInstance.transform.position.x, portalInstance.transform.position.y + 0.25f, portalInstance.transform.position.z);
            StartCoroutine(SpawnCubeSequentially(pos));

            cubeSpawned = true;
        }

        IEnumerator SpawnCubeSequentially(Vector3 pos)
        {
            int count = 0;

            while (count < 6)
            {
                cubeInstance = Instantiate(cube, pos, Quaternion.identity);
                cubeInstance.AddComponent<Rigidbody>();
                cubeInstance.GetComponent<Rigidbody>().AddForce(cubeInstance.transform.up * 2f);
                count++;
                yield return new WaitForSeconds(0.2f);
            }
            yield return null;
        }
    }
}