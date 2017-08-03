using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquatCounter : MonoBehaviour {


    public GameObject player;
    public GameObject GymUI;
    public float heightOffset = 0.42f;

    private GameObject displayText;
    private GameObject LoadingBar;
    private float count;
    private float initialHeight;
    private bool squatted;
    private string previewText;


	// Use this for initialization
	void Start () {
        count = 0;
        initialHeight = player.gameObject.transform.position.y;
        squatted = false;

        GymUI.SetActive(true);
        displayText = GymUI.transform.Search("CustomDebugDisplay").gameObject;
        LoadingBar = GymUI.transform.Search("Bar").gameObject;
        previewText = displayText.GetComponent<TextMesh>().text;
        GymUI.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

        if(player.transform.position.y < (initialHeight - heightOffset))
        {
            if (!squatted)
            {
                squatted = true;
                count++;
                updateText();
                LoadingBar.SendMessageUpwards("UpdateLoadingBar");
            }
        }

        if(player.transform.position.y > (initialHeight - heightOffset))
        {
            if (squatted)
            {
                squatted = false;
            }
        }
		
	}

    public void StartCounting()
    {
        GymUI.SetActive(true);
        ResetData();
    }

    public void StopCounting()
    {
        ResetData();
        GymUI.SetActive(false);
    }

    public void ResetData()
    {
        displayText.GetComponent<TextMesh>().text = previewText;
        count = 0;
        LoadingBar.SendMessageUpwards("ResetBar");
        squatted = false;
    }

    private void updateText()
    {
        displayText.GetComponent<TextMesh>().text = string.Format("You have done {0} squats", count);
    }

    
}
