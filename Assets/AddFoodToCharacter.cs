using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddFoodToCharacter : MonoBehaviour {

    //public bool iAmDrag;

    //public Text dragText;
    //public Text rotateText;
    public ApplyFoodToCharacter aftc;



    void Toggle(GameObject currentObj)
    {
        if (currentObj.name == "Fat")
        {
            //dragText.text = "On";
            //rotateText.text = "Off";
            //Gesture.Instance.GestureTransitionOuterControl(1);
            aftc.ApplyFood("Fat");
        }

        else if (currentObj.name == "Excercise")
        {
            //rotateText.text = "On";
            //dragText.text = "Off";
            // call rotate
            //Gesture.Instance.GestureTransitionOuterControl(0);
            aftc.ApplyFood("Excercise");
        }


        else
        {
            Debug.Log("How come nothing happen!!");
        }
    }
}
