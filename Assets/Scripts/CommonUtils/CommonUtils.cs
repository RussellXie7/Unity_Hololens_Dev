using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;
using System.Linq;
using System.Threading;

#if !UNITY_EDITOR && UNITY_METRO
using Windows.Storage;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using System;
#endif

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

        StreamWriter sw = new StreamWriter(new FileStream(Application.dataPath + "data.txt", FileMode.Open));

        //StreamWriter sw = new StreamWriter("data.txt");
        foreach (var data in list)
        {
            writeToFile(sw, data.Value);
        }


        //sw.Close();
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
            if (!existingAreaNames.Contains(data.categoryName))
            {
                existingAreaNames.Add(data.categoryName);
            }
            //Update min max vals.
            if (data.year < minYear) minYear = data.year;
            if (data.year > maxYear) maxYear = data.year;
            if (data.value < minVal) minVal = data.value;
            if (data.value > maxVal) maxVal = data.value;



            list.Add(data.categoryName+data.year,data);
        }
        //Set the updated data constraint.
        dataConstraints = new DataSetConstraints(minYear, maxYear, minVal, maxVal, existingAreaNames);

    }



    private static void writeToFile(StreamWriter sw, BarData data)
    {
        string json = JsonUtility.ToJson(data);

        sw.WriteLine(json);
    }


    public static void load(string inFileName, string causeOfDeathFileName, ref Dictionary<string, BarData> list, ref Dictionary<int, DataSetConstraints> dataConstraints, ref List<int> zipCodeList, ref DataGlobalTotals dgt)
    {


        string folderName =
#if !UNITY_EDITOR && UNITY_METRO
            ApplicationData.Current.RoamingFolder.Path;
#else
            "Assets";
#endif

        //Load Cause of Death first.
        var stringList =
#if !UNITY_EDITOR && UNITY_METRO
            ReadLines(OpenFileForRead(folderName, inFileName),
              Encoding.UTF8).ToList<string>();
#else
            File.ReadAllLines(inFileName);
#endif


        //List<string> causesOfDeathList = ReadLines(OpenFileForRead(folderName, causeOfDeathFileName),
        //              Encoding.UTF8).ToList<string>();

        List < string> causesOfDeathList = new List<string>();
        for (int i = 0; i < 14; i++)
        {
            causesOfDeathList.Add(stringList[i]);
        }

        //string[] causesOfDeathList = File.ReadAllLines(OpenFileForRead("", causeOfDeathFileName);

        //string[] stringList = File.ReadAllLines(inFile.text);

        //Initialize DGT.
        if (dgt == null) dgt = new DataGlobalTotals();

        int size =
#if !UNITY_EDITOR && UNITY_METRO
            stringList.Count;
#else
            stringList.Length;
#endif


        for (int i = 15; i < size; i++)
        {
            string line = stringList[i];

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
                if (!dsc.valueTypeName.Contains(name))
                    dsc.valueTypeName.Add(name);
            }

            for (int j = 2; j <= 15; j++)
            {
                string currDisease = causesOfDeathList[j - 2];
                string numOfPeepString = words[j];
                
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

                dgt.AddData(data);

                list.Add(currDisease + year + zipCodeInt, data);
            }

        }

        int k = 1;
    }



    private static Stream OpenFileForRead(string folderName, string fileName)
    {
        Stream stream = null;
#if !UNITY_EDITOR && UNITY_METRO
  Task task = new Task(
    async () => {
      StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(folderName);
      StorageFile file = await folder.GetFileAsync(fileName);
      stream = await file.OpenStreamForReadAsync();
    });
  task.Start();
  task.Wait();
#endif
        return stream;
    }


    public static IEnumerable<string> ReadLines(Stream streamProvider,
                                     Encoding encoding)
    {
        using (var reader = new StreamReader(streamProvider, encoding))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }
    }
}


