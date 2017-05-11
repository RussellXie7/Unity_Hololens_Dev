using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CommonUtils : MonoBehaviour {

    public static void DestroyImmediateAllChildrenOf(Transform t)
    {
        var children = new List<GameObject>();
        foreach (Transform child in t) children.Add(child.gameObject);
        children.ForEach(child => DestroyImmediate(child));
    }

    public static void DestroImmediateyAllChildrenOf(GameObject g)
    {
        DestroyImmediateAllChildrenOf(g.transform);
    }

    public static void DestroyAllChildrenOf(Transform t)
    {
        var children = new List<GameObject>();
        foreach (Transform child in t) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
    }

    public static void DestroyAllChildrenOf(GameObject g)
    {
        DestroyAllChildrenOf(g.transform);
    }

    public static bool EditEmissionColor(GameObject gameObject, Color color)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material mat = renderer.material;
            mat.SetColor("_EmissionColor", color);
            return true;
        }
        else
        {
            Debug.LogWarning("Gameobject " + gameObject + "\nCannont find renderer to emit color.");
            return false;
        }
    }

    public static bool EditAlbedoColor(GameObject gameObject, Color color)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            var tempMaterial = new Material(renderer.sharedMaterial);
            tempMaterial.color = Color.red;
            tempMaterial.SetColor("_Color", color);
            renderer.sharedMaterial = tempMaterial;
            return true;
        }
        else
        {
            Debug.LogWarning("Gameobject " + gameObject + "\nCannont find renderer to edit color.");
            return false;
        }
    }




    public static void save(Dictionary<string, BarData> list)
    {

        File.WriteAllText("data.txt", string.Empty);

        StreamWriter sw = new StreamWriter("data.txt");
        foreach (var data in list)
        {
            writeToFile(sw, data.Value);
        }


        sw.Close();
    }

    public static void load(string filename, Dictionary<string,BarData> list, ref DataSetConstraints dataConstraints)
    {
        string[] stringList = File.ReadAllLines(filename);

        //Initialize min max vals with opposite values.
        int minYear = Int32.MaxValue;
        int maxYear = Int32.MinValue;
        int minVal = Int32.MaxValue;
        int maxVal = Int32.MinValue;
        List<string> existingAreaNames = new List<string>();


        foreach (string json in stringList)
        {
            BarData data = JsonUtility.FromJson<BarData>(json);

            //Add existing area to area name list if is not already contained.
            if (!existingAreaNames.Contains(data.HospitalName))
            {
                existingAreaNames.Add(data.HospitalName);
            }
            //Update min max vals.
            if (data.year < minYear) minYear = data.year;
            if (data.year > maxYear) maxYear = data.year;
            if (data.HospitalizationNum < minVal) minVal = data.HospitalizationNum;
            if (data.HospitalizationNum > maxVal) maxVal = data.HospitalizationNum;



            list.Add(data.HospitalName+data.year,data);
        }
        //Set the updated data constraint.
        dataConstraints = new DataSetConstraints(minYear, maxYear, minVal, maxVal, existingAreaNames);

    }



    private static void writeToFile(StreamWriter sw, BarData data)
    {
        string json = JsonUtility.ToJson(data);

        sw.WriteLine(json);
    }


    public static void load(string inFileName, string causeOfDeathFileName, Dictionary<string, BarData> list, ref Dictionary<int, DataSetConstraints> dataConstraints, ref List<int> zipCodeList)
    {
        //Load Cause of Death first.
        string[] causesOfDeathList = File.ReadAllLines(causeOfDeathFileName);

        string[] stringList = File.ReadAllLines(inFileName);




        foreach (string line in stringList)
        {
            //Parse the file.
            char[] delimiterChars = { ',', '"', '(', ')' };
            string[] words = line.Split(delimiterChars);

            //0 is year,1 is zipcode, 2 -> 15 are values. so i-2 for i in [2-15] are names.
            string currYear = words[0];
            string zipCode = words[1];

            int zipCodeInt = Convert.ToInt32(zipCode);
            int year = Convert.ToInt32(currYear);

            if (!zipCodeList.Contains(zipCodeInt))
            {
                zipCodeList.Add(zipCodeInt);
            }

            if (!dataConstraints.ContainsKey(zipCodeInt))
            {
                dataConstraints.Add(zipCodeInt, new DataSetConstraints());
            }

            DataSetConstraints dsc = dataConstraints[zipCodeInt];

            foreach (string name in causesOfDeathList)
            {
                if(!dsc.areaNames.Contains(name))
                dsc.areaNames.Add(name);
            }

            for (int i = 2; i <= 15; i++)
            {
                string currDisease = causesOfDeathList[i - 2];
                string numOfPeepString = words[i];
                
                int numOfPeep = 0;

                if (numOfPeepString.CompareTo("") == 0) numOfPeep = 0;
                else
                {
                    numOfPeep = Convert.ToInt32(numOfPeepString);
                }
                if (list.ContainsKey(currDisease + year)) continue;

                //Update min max vals.
                if (year < dsc.minYear) dsc.minYear = year;
                if (year > dsc.maxYear) dsc.maxYear = year;
                if (numOfPeep < dsc.minValue) dsc.minValue = numOfPeep;
                if (numOfPeep > dsc.maxValue) dsc.maxValue = numOfPeep;

                BarData data = new BarData(year, currDisease, numOfPeep);



                list.Add(currDisease + year + zipCodeInt, data);
            }

        }


    }

}
