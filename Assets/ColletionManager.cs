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


    public void OnClick()
    {
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
        CustomLevelSolver.Instance.Query_OnWall();
        //StartCoroutine(AddBoxes(lineInc, locationCount));
        //StartCoroutine(AddPicture(lineInc, locationCount));

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
