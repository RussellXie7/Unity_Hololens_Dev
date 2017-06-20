using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFaceCamera : MonoBehaviour {

    [SerializeField] private Transform cameraTran;

    private void Start()
    {
        if (cameraTran == null) cameraTran = Camera.main.transform;
    }


    // Update is called once per frame
    void Update () {

        var t = this.gameObject.transform;


        t.forward = cameraTran.forward;
	}
}
