using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

public class ColletionManager : MonoBehaviour{
    //public GameObject coll;
    const int QueryResultMaxCount = 512, DisplayResultMaxCount = 1;

    public GameObject cube;
    public GameObject pictures;
    public GameObject videoMesh;
    public GameObject bedSet;
    public GameObject lamp;

    private List<GameObject> Furnitures = new List<GameObject>();
    private GameObject picInstance;
    private GameObject cubeInstance;
    private SpatialUnderstandingDllTopology.TopologyResult[] _resultsTopology = new SpatialUnderstandingDllTopology.TopologyResult[QueryResultMaxCount];
    private SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult[] _resultsObj = new SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult[QueryResultMaxCount];

    private bool tapAllowed;

    void ActivateTap()
    {
        Debug.Log("Activating tapAllowed.............");
        if (!tapAllowed)
        {
            tapAllowed = true;
        }
    }

    // called by CustumLevelSolver
    public void AddFurnitures(GameObject item)
    {
        Furnitures.Add(item);
        Debug.Log("Now we have " + Furnitures.Count + " number of furnitures.......");
    }

    private void ResetRoom()
    {
        foreach(GameObject item in Furnitures)
        {
            Destroy(item);
        }

        Furnitures.Clear();

        Debug.Log("Furniture list is clear and now have " + Furnitures.Count + " number of furnitures!!!!!!");
    }

    public void OnClick()
    {

        ResetRoom();

        Debug.Log("OnInputClick is detected!!!!!!!!!!!!!!!!!  tapAllowed is "+tapAllowed);
        // only do things when tap allowed 
        if (!tapAllowed)
            return;

        //Query
        var resultsTopologyPtr = SpatialUnderstanding.Instance.UnderstandingDLL.PinObject(_resultsTopology);

        //var locationCount = SpatialUnderstandingDllTopology.QueryTopology_FindPositionsOnFloor(
        //    minWidthOfWallSpace, minHeightAboveFloor,
        //    _resultsTopology.Length, resultsTopologyPtr);
        //var locationCount = SpatialUnderstandingDllTopology.QueryTopology_FindLargestPositionsOnFloor(_resultsTopology.Length, resultsTopologyPtr);

        //var locationCount = SpatialUnderstandingDllTopology.QueryTopology_FindLargePositionsOnWalls(minHeightOfWallSpace,minWidthOfWallSpace, minHeightAboveFloor, 1, _resultsTopology.Length, resultsTopologyPtr);
        //var locationCount = SpatialUnderstandingDllTopology.QueryTopology_FindPositionsOnWalls(minHeightOfWallSpace, minWidthOfWallSpace, minHeightAboveFloor, 0.1f, _resultsTopology.Length, resultsTopologyPtr);

        // output
        Debug.Log("Find Positions On Floor......");
        //Debug.Log("Now printing: locationCount: " + locationCount + ". and resultsTopologyPtr is " + resultsTopologyPtr + "~~~~~~~~~~~~~~~~~~~");


        // add boxes;
        //var lineInc = Mathf.CeilToInt((float)locationCount / (float)DisplayResultMaxCount);
        //StartCoroutine(BuildRoom());

        //StartCoroutine(AddBoxes(lineInc, locationCount));
        //StartCoroutine(AddPicture(lineInc, locationCount));
        StartCoroutine(BuildRoom());
    }

    IEnumerator BuildRoom()
    {
        CustomLevelSolver.Instance.Query_OnWall(pictures, 1f, 2f, false, new Vector3(0.8f,0.4f,0.05f));
        yield return new WaitForSeconds(0.1f);
        while (CustomLevelSolver.Instance.CurrentState() != CustomLevelSolver.QueryStates.None)
        {
            yield return new WaitForSeconds(0.1f);
        }

        CustomLevelSolver.Instance.Query_OnWall(videoMesh, 0.5f, 1f, true, new Vector3(0.65f,0.35f,0.05f));
        yield return new WaitForSeconds(0.1f);
        while (CustomLevelSolver.Instance.CurrentState() != CustomLevelSolver.QueryStates.None)
        {
            yield return new WaitForSeconds(0.1f);
        }

        CustomLevelSolver.Instance.Query_OnFloor(bedSet, true, new Vector3(0.65f,0.25f,1f));
        yield return new WaitForSeconds(0.1f);
        while (CustomLevelSolver.Instance.CurrentState() != CustomLevelSolver.QueryStates.None)
        {
            yield return new WaitForSeconds(0.1f);
        }

        CustomLevelSolver.Instance.Query_OnCeiling(lamp, new Vector3(0.5f,0.21f,0.5f));
        yield return null;
    }




    IEnumerator AddPicture(int lineInc, int locationCount)
    {
        for(var i = 0; i < locationCount; i += lineInc)
        {
            picInstance = Instantiate(pictures, _resultsTopology[i].position, Quaternion.identity);
            picInstance.GetComponent<ColliderCheck>().DisableAllMesh();
            picInstance.transform.forward = _resultsTopology[i].normal;
            picInstance.GetComponent<BoxCollider>().enabled = true;
            if (!picInstance.GetComponent<ColliderCheck>().isColliding)
            {
                Debug.Log("AddPicture......");
                picInstance.GetComponent<ColliderCheck>().EnableAllMesh();
                break;
            }
            else
            {
                Destroy(picInstance);
            }

            yield return new WaitForSeconds(0.1f);
        }

        yield return null;
    }

    IEnumerator AddBoxes(int lineInc, int locationCount)
    {
        Debug.Log("Adding Boxes in Coroutine!");
        for (var i = 0; i < locationCount; i += lineInc)
        {
            cubeInstance = Instantiate(cube, _resultsTopology[i].position, Quaternion.identity);
            cubeInstance.AddComponent<Rigidbody>();
            cubeInstance.GetComponent<Rigidbody>().AddForce(cubeInstance.transform.up * 2f);
            yield return new WaitForSeconds(0.5f);
        }
    }


	// Use this for initialization
	void Start () {
        tapAllowed = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
