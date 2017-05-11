using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Label : MonoBehaviour {

    public Text labelText;
    public RectTransform panelTransform;


    public void setText(string text)
    {
        //Set GUI's text.
        labelText.text = text;
        //Adjust panel size.
        panelTransform.sizeDelta = new Vector2(1.0f / 17.0f * text.Length, 0.1f);
    }
}
