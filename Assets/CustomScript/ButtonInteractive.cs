using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ButtonInteractive : MonoBehaviour {

    private Vector3 savedPos;



	// Use this for initialization
	public void OnHover()
    {
        transform.Search("Overlay").gameObject.SetActive(true);
    }

    public void OnHoverExit()
    {
        transform.Search("Overlay").gameObject.SetActive(false);
    }

    private void Update()
    {

    }

    private void Start()
    {
        savedPos = transform.localPosition;
    }

}
