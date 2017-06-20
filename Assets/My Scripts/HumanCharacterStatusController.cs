using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HumanAnimStatus
{
    Idle,
    RUN
}
public class HumanCharacterStatusController : MonoBehaviour {

    //Test
    public HumanIntake humanIntake;

    public HumanAnimStatus status;
    public Color bodyColor;
    public float speed;
    public Animator animator;

    //Reference to model properties.
    public Material suitMaterial;
    public Material helmetMaterial;
    public PanelTextmanager forcastPanel;
    public PanelTextmanager intakePanel;

    private Vector3 initialScale;
    public ScrollingTexture planeScroll;

	// Use this for initialization
	void Awake () {
        initialScale = this.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        SetCharacterStatus();

        //float sign = 1.0f;
        //if (Input.GetKey(KeyCode.LeftControl))
        //{
        //    sign = -1.0f;
        //}

        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    humanIntake.fat += sign*100.0f;
        //}
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    humanIntake.excercise += sign*0.5f;
        //}
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
    }

    public void SetCharacterStatus()
    {
        //Fat
        var fatIntake = humanIntake.fat;
        var unhealthyFatRate = HealthData.GetFatPercentile(fatIntake);
        bodyColor = HealthData.getColorBetweenRedAndGreen(unhealthyFatRate);

        //Excercise
        var excerciseTime = humanIntake.excercise;
        var runSpeedPercentile = HealthData.GetSpeedPercentile(excerciseTime);
        speed = runSpeedPercentile;



        applyVisualProperties();
    }

    private void applyVisualProperties()
    {
        ForecastScore fs = HealthData.ForecastHealthScore(humanIntake);
        forcastPanel.UpdatePanelWithForecastScore(fs);
        intakePanel.UpdatePanelWithHumanIntake(humanIntake);


        suitMaterial.SetColor("_Color", bodyColor);
        helmetMaterial.SetColor("_Color", bodyColor);

        animator.SetFloat("Speed", speed);

        //Adjust fatness when body score is lower than 0.5.
        //if(fs.lipid <=0.5f)
        changeFatnessDirect((1.0f - fs.lipid)*(1-speed));
        planeScroll.scrollSpeed = speed;
    }

    private void changeFatnessDirect(float factor)
    {
        this.transform.localScale = new Vector3(initialScale.x + 5.0f * (factor), initialScale.y, initialScale.z);
    }
    //public void reduceFatBy(float factor)
    //{
    //    var localScale = this.transform.localScale;
    //    this.transform.localScale = new Vector3(localScale.x*(1f-factor), localScale.y,localScale.z);
    //}



    public void Reset()
    {
        humanIntake = new HumanIntake(0.0f,0.0f);
    }
}
