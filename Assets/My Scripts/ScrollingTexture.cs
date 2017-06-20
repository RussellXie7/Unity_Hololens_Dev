using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTexture : MonoBehaviour {

    public float scrollSpeed = 0.90f;
    public float scrollSpeed2 = 0.90f;

    private void FixedUpdate()
    {

        var offset = Time.time * scrollSpeed;
        var offset2 = Time.time * scrollSpeed2;
        this.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(offset2, -offset);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
