using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonReaction : MonoBehaviour {
    public Material mat;
    private Material savedMat;

	// Use this for initialization
	void Start () {
        savedMat = this.gameObject.GetComponent<Renderer>().material;	
	}
	
    public void CursorHover()
    {
        this.gameObject.GetComponent<Renderer>().material = mat;
    }

    public void CursorExit()
    {
        this.gameObject.GetComponent<Renderer>().material = savedMat;
    }
}
