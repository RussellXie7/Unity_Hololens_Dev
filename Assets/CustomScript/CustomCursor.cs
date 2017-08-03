using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour{

    private GameObject focusedObj;

    public void UpdateFocusedObject(GameObject obj = null) {
        //Debug.Log("******Received new object, which is " + obj.name);

        if (focusedObj != null)
        {
            if (focusedObj.name == "HideMenuButton" || focusedObj.name == "HideMenuButtonText" ||
                focusedObj.name == "BedRoomOption" || focusedObj.name == "OfficeOption" ||
                focusedObj.name == "KitchenOption" || focusedObj.name == "GymOption")
            {
                focusedObj.SendMessageUpwards("OnHoverExit");
            }
        }


        if (obj != null)
        {
            if (obj.name == "HideMenuButton" || obj.name == "HideMenuButtonText" ||
                obj.name == "BedRoomOption" || obj.name == "OfficeOption" ||
                obj.name == "KitchenOption" || obj.name == "GymOption")
            {
                obj.SendMessageUpwards("OnHover");
            }
        }


        focusedObj = obj;

    }


    public void OnCursorClick()
    {
        if (focusedObj)
        {
            //Debug.Log("!!!! the "+focusedObj.name + " is clicked");
            focusedObj.SendMessageUpwards("OnClicked", focusedObj.name);
        }
    }

	// Use this for initialization
	void Start () {
        focusedObj = null;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
