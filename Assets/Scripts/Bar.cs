using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BarData
{
    public int year;
    public string categoryName;
    public int value;



    public BarData(int year, string name, int num)
    {
        this.year = year;
        this.value = num;
        this.categoryName = name;
    }
}
public class Bar : MonoBehaviour {

    //Reference to the rendering model.
    [SerializeField] GameObject barModel;

    public Transform barTipTrans;

    public BarData barData;

    public Color color;

    public float universalHeightScaleFactor;
    public Text valueLabel;
    public RectTransform valueCanvas;


    //Animation for building the bar.
    AnimationUtils au;

    private void Awake()
    {
        au = this.gameObject.AddComponent<AnimationUtils>();
    }

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
        Vector3 initialScale = new Vector3(originalLocalScale.x, 0, originalLocalScale.z); 
        Vector3 finalScale = new Vector3(originalLocalScale.x, originalLocalScale.y * (data.value+1f)*1.0f * universalHeightScaleFactor, originalLocalScale.z);

        var yScale = (0.5f / finalScale.y);
        yScale = yScale > 100.0f ? 1.0f : yScale;
        //valueCanvas.localScale = new Vector3(0.5f,yScale , 1.0f);
        var canvasInitScale = new Vector3(0, 0, 0);
        var canvasFinalScale = new Vector3(0.5f, yScale, 1.0f);
        valueCanvas.localScale = canvasInitScale;

        //Animate the process for building bar.
        StartCoroutine(au.StartScaleAfter(2f, valueCanvas.transform, canvasInitScale, canvasFinalScale, 0.5f));
        StartCoroutine(au.ScaleLocalFromTo(this.transform, initialScale, finalScale, 2f));
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
