using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelUIControl : MonoBehaviour {
    public Text panelText;
    public float sizeFactor;

    //Stoer the initial scale.
    private Vector3 initScale;

    private void Awake()
    {
        initScale = this.transform.localScale;
    }

    public void SetPanelText(string s)
    {
        panelText.text = s;
    }

    private void Update()
    {
        //Check for distance to the camera, update the size of current object base on distance.
        float distToCamear = Vector3.Distance(Camera.main.transform.position, this.transform.position);
        this.transform.localScale = initScale * distToCamear * sizeFactor;
        //Set z of local scale back, becasue we only want to scale on x and y directions.
        this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, 1.0f);
        if(this.transform.localScale.x >= 25.0f)
            this.transform.localScale = new Vector3(25.0f, 25.0f, 1.0f);
    }
}
