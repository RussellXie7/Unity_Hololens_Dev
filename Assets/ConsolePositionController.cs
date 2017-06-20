using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsolePositionController : MonoBehaviour {

    //Reference to the console.
    public Transform userConsole;

    //Reference to the console edges.;
    public Transform[] consoleEdges;

    //Max distance trigger.
    //When console is off position by a distance greater than this, it will get moved.
    public float distanceTriggerMovement;

    //The Animation Utials to move the console.
    AnimationUtils au = new AnimationUtils();

    // Use this for initialization
    void Start () {
		
	}

    //The transform of the edge we should place the console.
    Transform prevTransEdgeToPlace = null;

    // Update is called once per frame
    void Update () {

        //Using a linear search for now.
        float minDistance = float.MaxValue;
        Transform transEdgeToPlace = null;
        //Get main cam transform.
        Transform camTrans = Camera.main.transform;

        //Check which edge is the closest to the user.
        foreach (Transform t in consoleEdges)
        {
            Vector3 edgePosition = t.position;

            float distToUser = Vector3.Distance(camTrans.position, edgePosition);

            //Update minimum distance.
            if (distToUser < minDistance)
            {
                minDistance = distToUser;
                transEdgeToPlace = t;
            }
        }

        if (prevTransEdgeToPlace != transEdgeToPlace)
        {
            prevTransEdgeToPlace = transEdgeToPlace;
        }

        //Project cam-edgePosition onto the edge.
        Vector3 projectOnEdge = Vector3.Project(camTrans.position - transEdgeToPlace.position, transEdgeToPlace.right);

        float lengthOfProj = Vector3.Distance(consoleEdges[0].position, consoleEdges[1].position) / Mathf.Sqrt(2);

        //Get the position to place the console.
        Vector3 posToPlace = transEdgeToPlace.position + Vector3.Normalize(projectOnEdge) * Mathf.Clamp( Vector3.Magnitude(projectOnEdge), Vector3.Magnitude(projectOnEdge), lengthOfProj);


        //If current position is too far from position to place, move it to destination.
        if (Vector3.Distance(posToPlace, userConsole.transform.position) > distanceTriggerMovement && !au.inAnimation)
        {
            StartCoroutine(au.MoveToDesination(userConsole.transform, posToPlace, 1f));
        }
        //userConsole.transform.position = posToPlace;


    }
}
