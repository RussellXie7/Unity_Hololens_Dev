using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFollowCameraClamped : MonoBehaviour {

    public float maxDegreeOffset;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {

        //Get the angle between user forward and this forward.
        float angleBetween = Mathf.Abs( Vector3.Angle(this.transform.forward, Camera.main.transform.forward));

        if (angleBetween >= maxDegreeOffset)
            this.transform.forward = Camera.main.transform.forward;
	}
}
