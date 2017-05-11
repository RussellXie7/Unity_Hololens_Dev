using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePiece : MonoBehaviour
{
    public GameObject objectToShow;

    public Transform lineSegsParent;

    [Range(10, 50)]
    public float lineDotsDensity;


    //public Vector3[] positions;

    // Use this for initialization
    void Start()
    {
        //InstaniateLine(positions);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InstaniateLine(Vector3[] positions)
    {
        for (int i = 0; i < positions.Length - 1; i++)
        {
            //Get current position.
            Vector3 currPivot = positions[i];
            Vector3 nextPivot = positions[i + 1];
            var length = Vector3.Distance(currPivot, nextPivot);
            var dir = Vector3.Normalize(nextPivot - currPivot);

            var offset = length / lineDotsDensity * dir;

            for (int count = 0; count < lineDotsDensity; count++)
            {
                Vector3 posToInit = offset * count + currPivot;

                Instantiate(objectToShow, lineSegsParent);
                objectToShow.transform.position = posToInit;
            }
        }
    }


}
