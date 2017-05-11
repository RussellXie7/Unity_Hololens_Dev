using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour {

    //Reference to the rendering model.
    [SerializeField] GameObject barModel;

    public Transform barTipTrans;

    public BarData barData;

    public Color color;

    public float universalHeightScaleFactor;
    public Text valueLabel;
    public RectTransform valueCanvas;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    //Public functions for Bar.
    //Change the height of the bar.
    public void SetHeight(float size)
    {
        var originalLocalScale = this.transform.localScale;
        this.transform.localScale = new Vector3(originalLocalScale.x, originalLocalScale.y * size, originalLocalScale.z);
    }

    //Initialize a bar with data.
    public void InitBarWithData(BarData data)
    {
        this.barData = data;
        var originalLocalScale = this.transform.localScale;
        //Set the height of the bar using the hospitalization number.
        this.transform.localScale = new Vector3(originalLocalScale.x, originalLocalScale.y * (data.HospitalizationNum+0.0001f)*1.0f * universalHeightScaleFactor, originalLocalScale.z);

        var yScale = (0.5f / this.transform.localScale.y);
        yScale = yScale > 100.0f ? 1.0f : yScale;
        valueCanvas.localScale = new Vector3(0.5f,yScale , 1.0f);
    }

    //Set a bar's color.
    public void SetColor(Color c)
    {
        //Edit color.
        CommonUtils.EditAlbedoColor(barModel.gameObject, c);
    }

    public void SetValueText(int val, Color color)
    {
        valueLabel.color = color;
        valueLabel.text = val + "";
    }
}
