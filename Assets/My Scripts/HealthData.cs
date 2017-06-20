using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[System.Serializable]
public class HumanIntake 
{
    public float fat;
    //public float carb;
    public float excercise;
    //public float sleep;

    public HumanIntake(float f, float e)
    {
        fat = f;
        excercise = e;
    }

    public HumanIntake Add(HumanIntake hi)
    {
        return new HumanIntake(this.fat+hi.fat, this.excercise+hi.excercise);
    }

}

[System.Serializable]
public class ForecastScore
{
    public float lipid;
    public float body;
}



public static class HealthData
{
    //public static float HEALTHY_SLEEP_HOURS = 8.0f;
    //public static float HEALTHY_CARB_INTAKE = 900f;
    public static float MIN_FAT_INTAKE = 0f;
    public static float MAX_FAT_INTAKE = 800f;

    public static float MIN_EXCERCISE_HOURS = 0f;
    public static float MAX_EXCERCISE_HOURS = 3f;

    //Return the unhealth rate of current fat intake.
    public static float GetFatPercentile(float fatintake)
    {
        if (fatintake < MIN_FAT_INTAKE) return 0;
        if (fatintake > MAX_FAT_INTAKE) return 1.0f;

        return (fatintake - MIN_FAT_INTAKE) / (MAX_FAT_INTAKE - MIN_FAT_INTAKE);
    }

    //Return the healthy rate of current excercise time per day.
    public static float GetSpeedPercentile(float excerciseTime)
    {
        if (excerciseTime < MIN_EXCERCISE_HOURS) return 0;
        if (excerciseTime > MAX_EXCERCISE_HOURS) return 1.0f;

        return (excerciseTime - MIN_EXCERCISE_HOURS) / (MAX_EXCERCISE_HOURS - MIN_EXCERCISE_HOURS);
    }

    public static UnityEngine.Color getColorBetweenRedAndGreen(float n)
    {
        //float R = (255 * n) / 100;
        //float G = (255 * (100 - n)) / 100;
        //float B = 0;
        //float R = n;
        //float G = 1 - n;
        //float B = 0;
        float H = (1 - n) * 0.4f; // Hue (note 0.4 = Green, see huge chart below)
        float S = 0.9f; // Saturation
        float B = 0.9f; // Brightness


        return UnityEngine.Color.HSVToRGB(H, S, B);
    }

    public static ForecastScore ForecastHealthScore(HumanIntake hi)
    {
        ForecastScore fs = new ForecastScore();
        fs.body = 1.0f - GetSpeedPercentile(hi.excercise);
        fs.lipid =1.0f -  GetFatPercentile(hi.fat);

        return fs;
    }
}

