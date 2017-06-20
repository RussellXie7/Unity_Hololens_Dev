using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonState : MonoBehaviour {

    bool isRotate = true;

    public GameObject rotateModel;
    public GameObject translateModel;

    public Text text;
    public Text zipcodeText;

	// Use this for initialization
	void Awake () {
        SetButtonState();

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SwitchButtonState();
        }
	}

    private void SetButtonState()
    {
        rotateModel.SetActive(isRotate);
        translateModel.SetActive(!isRotate);


        if (isRotate)
        {
            text.text = "Switch to Rotate";
            text.color = Color.white;
        }
        else
        {
            text.text = "Switch to Move";
            text.color = Color.white;   
        }
    }

    public void SwitchButtonState()
    {
        isRotate = !isRotate;
        SetButtonState();
    }

    public void SetText(string s)
    {
        zipcodeText.text = s;
    }

}
