using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoloToolkit.Sharing.Tests
{
    public class BuildingGroupManager : MonoBehaviour {

        private GameObject mainBuilding;
        private GameObject gym;
        private GameObject restaurant;
        private Dictionary<GameObject, bool> dict = new Dictionary<GameObject, bool>();

        // Use this for initialization
        void Start() {
            
            mainBuilding = transform.Search("MainBuilding").transform.gameObject;
            restaurant = transform.Search("McDonald").transform.gameObject;
            gym = transform.Search("GymBuilding").transform.gameObject;

            DisableMesh(mainBuilding);
            DisableMesh(gym);
            DisableMesh(restaurant);

            CustomMessages.Instance.MessageHandlers[CustomMessages.TestMessageID.buildingGroup] = this.ProcessRemoteBuildingGroup;
        }

        void DisableMesh(GameObject obj)
        {
            foreach (MeshRenderer mesh in obj.GetComponentsInChildren<MeshRenderer>())
            {
                mesh.enabled = false;
            }

            if (!dict.ContainsKey(obj))
            {
                dict.Add(obj, false);
            }
        }

        void EnableMesh(GameObject obj)
        {
            foreach (MeshRenderer mesh in obj.GetComponentsInChildren<MeshRenderer>())
            {
                mesh.enabled = true;
            }

            if(dict.ContainsKey(obj))
                dict[obj] = true;
        }

        // Update is called once per frame
        void Update() {

        }

        public void AddMainBuilding()
        {
            StartCoroutine(GrowBuiding(mainBuilding));
        }

        public void AddGym()
        {
            StartCoroutine(GrowBuiding(gym));
        }
        public void AddRestaurant()
        {
            StartCoroutine(GrowBuiding(restaurant));
        }

        private IEnumerator GrowBuiding(GameObject building)
        {
            Vector3 initPos = building.transform.localPosition;
            Vector3 initScale = building.transform.localScale;

            building.transform.localPosition = new Vector3(initPos.x, 0f, initPos.z);
            building.transform.localScale = Vector3.zero;//new Vector3(initScale.x, 0f, initScale.z);

            EnableMesh(building);

            while (building.transform.localScale.x < initScale.x)
            {
                building.transform.localPosition = Vector3.Lerp(building.transform.localPosition, initPos, 0.1f);
                building.transform.localScale = Vector3.Lerp(building.transform.localScale, initScale, 0.1f);

                yield return new WaitForSeconds(Time.deltaTime);

                CustomMessages.Instance.SendBuildingGroup(mainBuilding, gym, restaurant, dict);
            }

            building.transform.localScale = initScale;
            building.transform.localPosition = initPos;
            CustomMessages.Instance.SendBuildingGroup(mainBuilding, gym, restaurant, dict);
        }


        private void ProcessRemoteBuildingGroup(NetworkInMessage msg) {

            long userID = msg.ReadInt64();

            HelperReadBuildingInfo(mainBuilding,msg);
            HelperReadBuildingInfo(gym, msg);
            HelperReadBuildingInfo(restaurant, msg);
            
        }

        private void HelperReadBuildingInfo(GameObject building, NetworkInMessage msg)
        {
            building.transform.localPosition = CustomMessages.Instance.ReadVector3(msg);
            building.transform.localScale = CustomMessages.Instance.ReadVector3(msg);

            if (CustomMessages.Instance.ReadBool(msg))
            {
                EnableMesh(building);
            }
            else
            {
                DisableMesh(building);
            }
        }

    }
}