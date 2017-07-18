using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCheck : MonoBehaviour {

    [HideInInspector]
    public bool isColliding;

	// Use this for initialization
	void Start () {
        isColliding = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        isColliding = true;
    }

    public void DisableAllMesh()
    {
        foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>())
            r.enabled = false;
    }

    public void EnableAllMesh()
    {
        foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>())
            r.enabled = true;
    }
}
