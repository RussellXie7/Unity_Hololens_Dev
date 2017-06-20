using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelTextmanager : MonoBehaviour {

    public Image[] backgrounds;
    public Text[] labelTexts;
    public Text[] labelValues;

    public string[] labelTitles;

    private Dictionary<string, int> labelTitleToIndexDic = new Dictionary<string, int>();

    private void Awake()
    {
        for (int i = 0; i < labelTitles.Length; i++)
        {
            labelTexts[i].text = labelTitles[i];
            labelTitleToIndexDic.Add(labelTitles[i], i);
        }
    }

    public void UpdatePanelValueWithPercentage(string title, float value)
    {
        int index = labelTitleToIndexDic[title];

        labelValues[index].text = (100f*value).ToString("0")+"";

        updateBackGroundColor(index, 1-value);
    }

    public void UpdatePanelValue(string title, float value)
    {
        int index = labelTitleToIndexDic[title];

        if(title.CompareTo("Fat") == 0)
            labelValues[index].text = value.ToString("0") + " grams";
        else if (title.CompareTo("Excercise") == 0)
            labelValues[index].text = value.ToString("0.0") + " hours";

        //updateBackGroundColor(index, 1 - value);
    }

    private void updateBackGroundColor(int i, float value)
    {
        backgrounds[i].color = HealthData.getColorBetweenRedAndGreen(value);
        labelValues[i].color = HealthData.getColorBetweenRedAndGreen(value);
    }

    public void UpdatePanelWithForecastScore(ForecastScore fs)
    {
        UpdatePanelValueWithPercentage("Lipid", fs.lipid);
        UpdatePanelValueWithPercentage("Body", 1 - fs.body);
    }

    public void UpdatePanelWithHumanIntake(HumanIntake mi)
    {
        UpdatePanelValue("Fat", mi.fat);
        UpdatePanelValue("Excercise", mi.excercise);
    }

}
