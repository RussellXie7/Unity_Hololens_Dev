using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGlobalTotals
{
    public Dictionary<string, int> totals;

    public DataGlobalTotals()
    {
        totals = new Dictionary<string, int>();
    }

    public int GetTotal(int year, string name)
    {
        return totals[year + name];
    }

    public void AddData(BarData data)
    {
        //Generate key first.
        string key = data.year + data.categoryName;

        //If totals does not contain current key, add it in without comparison.
        if (!totals.ContainsKey(key))
        {
            totals.Add(key, data.value);
        }
        else
        {
            totals[key] += data.value;
        }
    }
}

public class DataSetConstraints
{
    public int minYear;
    public int maxYear;
    public int minValue;
    public int maxValue;
    public List<string> valueTypeName;

    public DataSetConstraints(int miny, int maxy, int minVal, int maxVal, List<string> areanames)
    {
        minYear = miny;
        maxYear = maxy;
        minValue = minVal;
        maxValue = maxVal;
        valueTypeName = new List<string>(areanames);
    }

    public DataSetConstraints()
    {
        minYear = Int32.MaxValue;
        maxYear = Int32.MinValue;
        minValue = Int32.MaxValue;
        maxValue = Int32.MinValue;
        valueTypeName = new List<string>();
    }
}



public class BarChartBuilder : MonoBehaviour
{
    public Transform topLeftPosition;
    [SerializeField]
    private GameObject barObjectToInit;
    [SerializeField]
    private Transform barsParent;
    public GameObject planeModel;
    public Transform labelsParent;
    public GameObject labelObject;
    public Label ZipCodeLabel;



    public int currentZipCode;

    //Bar mode.
    public bool isBarMode;

    //[REWORK]
    //Manually enter data for now.
    public Dictionary<string, BarData> dataList;
    public string fileNameToLoad;
    public string causeOfDeathFile;
    private Dictionary<int, DataSetConstraints> dataConstraints;
    private List<int> zipcodeList;
    private int zipcodeIndex = 0;

    //Stores the total number for each causeOfDeath per year.
    private DataGlobalTotals dgt;

    //Offset between each bar.
    public float offset;
    public float barHeightScale = 0.1f;

    //Rendering properties.
    public Color[] colorLevels;


    //[REWORK]Temporary Solution for line drawing.
    public LinePiece lineDrawer;

    private Vector3 initScale;
    private Quaternion initRotation;

    void Awake()
    {

        initScale = this.transform.localScale;
        initRotation = this.transform.localRotation;
        BuildBarChart();

    }


    //Update for testing
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            SwitchDisplayMode();

        if (Input.GetKeyDown(KeyCode.I))
            AdjustZipCode(1);
    }



    //Build a chart with x rows and y colums.
    public void BuildBarChart()
    {
        //Record the local scale and build the chart at identity scale matrix.
        this.transform.localScale = Vector3.one;

        Quaternion prevRotation = this.transform.localRotation;
        this.transform.localRotation = Quaternion.identity;

        if (fileNameToLoad != null)
        {
            dataList = new Dictionary<string, BarData>();
            zipcodeList = new List<int>();
            dataConstraints = new Dictionary<int, DataSetConstraints>();
            CommonUtils.load(fileNameToLoad, causeOfDeathFile, ref dataList, ref dataConstraints, ref zipcodeList, ref dgt);
        }
        else
        {
            //Debug.LogError("Data file name is empty!");
        }

        if (currentZipCode == -1)
        {
            currentZipCode = zipcodeList[zipcodeIndex];
        }

        DataSetConstraints dataConstraint = dataConstraints[currentZipCode];

        buildChartWithData(dataConstraint);

        //Set back the local scale
        this.transform.localScale = initScale;
        this.transform.localRotation = prevRotation;
    }

    private void buildChartWithData(DataSetConstraints dataConstraint)
    {
        //Record the local scale and build the chart at identity scale matrix.
        this.transform.localScale = Vector3.one;

        Quaternion prevRotation = this.transform.localRotation;
        this.transform.localRotation = Quaternion.identity;

        ClearAllbars();

        barHeightScale = 1.0f / ((dataConstraint.maxValue - dataConstraint.minValue) / 8.0f);
        ZipCodeLabel.setText(currentZipCode+"");

        //Initialize the size of the chart with constraints.
        int sizex = dataConstraint.valueTypeName.Count;
        int sizey = dataConstraint.maxYear - dataConstraint.minYear + 1;

        //Update the size of the base plane.
        float xSpan = sizex + (sizex - 1) * (offset - 1.0f);
        float zSpan = sizey + (sizey) * (offset - 1.0f);
        planeModel.transform.localScale = new Vector3(xSpan / 10.0f, 1.0f, zSpan / 10.0f);

        //Create labels.



        //Initialzie data on a grid.
        for (int x = 0; x < sizex; x++)
        {
            //Get current area
            string currArea = dataConstraint.valueTypeName[x];

            //Get label position
            if (true)
            {
                var labelPosition = new Vector3(topLeftPosition.position.x + x * offset, topLeftPosition.position.y, topLeftPosition.position.z + offset);
                createLabel(labelPosition, currArea);

                labelPosition = new Vector3(topLeftPosition.position.x + x * offset, topLeftPosition.position.y, topLeftPosition.position.z - sizey * offset);
                createLabel(labelPosition, currArea);
            }


            //[REWORK]
            Bar prevBar = null;

            for (int z = 0; z < sizey; z++)
            {
                //Get current year
                string currYear = dataConstraint.minYear + z + "";

                //Create label for columns.
                if (x == 0)
                {
                    var labelPosition = new Vector3(topLeftPosition.position.x - offset, topLeftPosition.transform.position.y, topLeftPosition.position.z - z * offset);
                    createLabel(labelPosition, currYear);
                }
                if (x == sizex - 1)
                {
                    var labelPosition = new Vector3(topLeftPosition.position.x + (x + 1) * offset, topLeftPosition.transform.position.y, topLeftPosition.position.z - z * offset);
                    createLabel(labelPosition, currYear);
                }

                //Get position to place the current bar to instantiate.
                Vector3 positionToInit = new Vector3(topLeftPosition.position.x + x * offset, this.transform.position.y, topLeftPosition.position.z - z * offset);
                //Instantiate bar.
                GameObject bar = Instantiate(barObjectToInit, barsParent);
                bar.transform.position = positionToInit;


                //Setup bar according to data.
                //Data to use.
                if (!dataList.ContainsKey(currArea + currYear + currentZipCode)) continue;
                BarData dataToUse = dataList[currArea + currYear + currentZipCode];
                if (dataToUse == null)
                {
                    ////[DELETE]
                    //Debug.Log("Current year is " + currYear);
                    //Debug.Log("Current area is " + currArea);
                    //Debug.LogError("Not enough data to use for size x " + sizex + " y " + sizey);
                }

                //Get bar componnet for setup.
                Bar barComponent = bar.GetComponent<Bar>();
                //if (barComponent == null) //Debug.LogError("Bar does not have Bar component!");


                //First set universal scale factor.
                barComponent.universalHeightScaleFactor = barHeightScale;
                //Initailize bar with data.
                barComponent.InitBarWithData(dataToUse);

                //Set bar's color basing on its data.
                float barVal = barComponent.barData.value;
                float percentile = (barVal - dataConstraint.minValue) / (dataConstraint.maxValue - dataConstraint.minValue);
                //Debug.Log("Current percentile is " + percentile);
                //int indexOfColor = (int)(colorLevels.Length * percentile)+1;
                int indexOfColor = Mathf.RoundToInt(colorLevels.Length * percentile);
                //Deal with the case when data is max.
                if (indexOfColor == colorLevels.Length) indexOfColor--;
                //Debug.Log("Current index of Color is " + indexOfColor);
                //Debug.Log("Color levels size is " + colorLevels.Length);
                barComponent.SetColor(colorLevels[indexOfColor]);
                barComponent.SetValueText((int)barVal, colorLevels[indexOfColor]);


                //Draw line.
                if (z > 0)
                {
                    lineDrawer.InstaniateLine(new Vector3[] { prevBar.barTipTrans.position, barComponent.barTipTrans.position });
                }
                prevBar = barComponent;


            }
        }

        lineDrawer.lineSegsParent.gameObject.SetActive(false);

        //Set back the local scale
        this.transform.localScale = initScale;
        this.transform.localRotation = prevRotation;
    }

    public void AdjustZipCode(int i)
    {
        zipcodeIndex += i;
        if (zipcodeIndex < 0)
        {
            zipcodeIndex = zipcodeList.Count - 1;
        }
        if (zipcodeIndex > zipcodeList.Count - 1)
        {
            zipcodeIndex = 0;
        }

        currentZipCode = zipcodeList[zipcodeIndex];
        DataSetConstraints dsc = dataConstraints[currentZipCode];
        buildChartWithData(dsc);
    }


    //Helper function to create labels.
    private void createLabel(Vector3 position, string labelName)
    {
        //Initialize the label at the beginning of each row.
        GameObject labelRow = Instantiate(labelObject, labelsParent);
        labelRow.transform.position = position;
        //Set label's text.
        labelRow.GetComponent<Label>().setText(labelName);
    }

    //Clear all bars.
    public void ClearAllbars()
    {
        planeModel.transform.localScale = Vector3.one;
        CommonUtils.DestroyImmediateAllChildrenOf(lineDrawer.lineSegsParent.transform);     
        CommonUtils.DestroyImmediateAllChildrenOf(labelsParent);

        var children = new List<GameObject>();
        foreach (Transform child in barsParent.transform) children.Add(child.gameObject);
        foreach (var child in children)
            if (child.GetComponent<Bar>())
                CommonUtils.DestroyImmediate(child);
    }


    //Resize BarChart

    //Switch between display mode.
    public void SwitchDisplayMode()
    {
        if (isBarMode)
        {
            isBarMode = false;
            barsParent.gameObject.SetActive(false);
            lineDrawer.lineSegsParent.gameObject.SetActive(true);
        }
        else
        {
            isBarMode = true;
            lineDrawer.lineSegsParent.gameObject.SetActive(false);
            barsParent.gameObject.SetActive(true);
        }
    }


    private void outputDataToFile()
    {

    }
}
