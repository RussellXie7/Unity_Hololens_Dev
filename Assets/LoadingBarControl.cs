using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBarControl : MonoBehaviour {

    private GameObject loadingBar;
    private Text hourText;
    private Text minText;

    private float minCount = 0;
    private float hourCount = 0;

	// Use this for initialization
	void Start () {
        loadingBar = gameObject.transform.Search("LoadingBar").gameObject;
        hourText = gameObject.transform.Search("hour").gameObject.GetComponent<Text>();
        minText = gameObject.transform.Search("min").gameObject.GetComponent<Text>();
        ResetBar();
    }
	

    public void UpdateLoadingBar() {
        UpdateTime();
        loadingBar.GetComponent<Image>().fillAmount += 0.05f;
    }

    private void UpdateTime()
    {
        if(minCount >= 45)
        {
            hourCount += 1;
            minCount = 0f;

        }
        else
        {
            minCount += 15f;
        }

        hourText.text = string.Format("{0} hour", hourCount);
        minText.text = string.Format("{0} min", minCount);

    }

    public void ResetBar()
    {
        minCount = 0;
        hourCount = 0;
        loadingBar.GetComponent<Image>().fillAmount = 0;
        hourText.text = string.Format("{0} hour", hourCount);
        minText.text = string.Format("{0} min", minCount);
    }
}
