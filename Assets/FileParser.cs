using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileParser : MonoBehaviour {

    public string inFileName;
    public string causeOfDeathFileName;

    private void Start()
    {
    }

    public static void load(string inFileName,string causeOfDeathFileName, Dictionary<string, BarData> list, ref DataSetConstraints dataConstraints)
    {
        //Load Cause of Death first.
        string[] causesOfDeathList = File.ReadAllLines(causeOfDeathFileName);

        string[] stringList = File.ReadAllLines(inFileName);

        //Initialize min max vals with opposite values.
        int minYear = Int32.MaxValue;
        int maxYear = Int32.MinValue;
        int minVal = Int32.MaxValue;
        int maxVal = Int32.MinValue;
        List<string> existingAreaNames = new List<string>();
        foreach (string name in causesOfDeathList)
        {
            existingAreaNames.Add(name);
        }

        foreach (string line in stringList)
        {
            //Parse the file.
            char[] delimiterChars = { ',', '"', '(', ')' };
            string[] words = line.Split(delimiterChars);

            //0 is year, 2 -> 15 are values. so i-2 for i in [2-15] are names.
            string currYear = words[0];
            int year = Convert.ToInt32(currYear);

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

                //Update min max vals.
                if (year < minYear) minYear = year;
                if (year > maxYear) maxYear = year;
                if (numOfPeep < minVal) minVal = numOfPeep;
                if (numOfPeep > maxVal) maxVal = numOfPeep;

                BarData data = new BarData(year, currDisease, numOfPeep);

                list.Add(currDisease + year, data);
            }
            //Set the updated data constraint.
            dataConstraints = new DataSetConstraints(minYear, maxYear, minVal, maxVal, existingAreaNames);

        }

    }
    
}


//[0]	"1999"	System.String
//[1]	"90010"	System.String
//[2]	"8"	System.String
//[3]	"3"	System.String
//[4]	"1"	System.String
//[5]	"1"	System.String
//[6]	""	System.String
//[7]	""	System.String
//[8]	"1"	System.String
//[9]	""	System.String
//[10]	"1"	System.String
//[11]	""	System.String
//[12]	""	System.String
//[13]	""	System.String
//[14]	"5"	System.String
//[15]	""	System.String
//[16]	"37.853695"	System.String
//[17]	"-122.022868"	System.String
//[18]	""	System.String
//[19]	""	System.String
//[20]	"37.853695"	System.String
//[21]	" -122.022868"	System.String
//[22]	""	System.String
//[23]	""	System.String