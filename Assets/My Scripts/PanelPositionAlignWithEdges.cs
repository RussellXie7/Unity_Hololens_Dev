using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelPositionAlignWithEdges : MonoBehaviour {

    public Transform[] edgeTransforms;


    private void Update()
    {
        Transform closestEdgeTransform = null;
        float minDist = Mathf.Infinity;
        foreach (Transform t in edgeTransforms)
        {
            float currDist = Vector3.Distance(t.position, Camera.main.transform.position);
            if (currDist < minDist)
            {
                closestEdgeTransform = t;
                minDist = currDist;
            }
        }

        MoveToEdge(closestEdgeTransform);
    }


    private void MoveToEdge(Transform destEdgeTrans)
    {
        this.transform.position = destEdgeTrans.position;
        this.transform.forward = -destEdgeTrans.forward;
    }
}
