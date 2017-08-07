using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyBudgetManager : MonoBehaviour {

    /// <summary>
    /// The loading bar has to be the child of this gameobject
    /// </summary>
    private GameObject loadingBar;

    /// <summary>
    /// The remaining money in the budget
    /// </summary>
    private Text budgetText;

    /// <summary>
    /// The current budget
    /// </summary>
    private int currentBudget = 0;


    

    /// <summary>
    /// Initial amount of budget
    /// </summary>
    public int m_initialBudget = 1500;

    // Use this for initialization
    void Start () {
        loadingBar = gameObject.transform.Search("LoadingBar").gameObject;
        budgetText = gameObject.transform.Search("BudgetText").gameObject.GetComponent<Text>();
        ResetBar();
    }
	
    /// <summary>
    /// This function will deduct the corresponding amount of money on the budget. And update bar and texts
    /// return false if there is insufficient money
    /// </summary>
    /// <param name="amount"></param>
    public bool SpendBudget(int amount)
    {
        int targetAmount = currentBudget - amount;

        if (targetAmount < 0)
        {
            return false;
        }

        int previousAmount = currentBudget;
        currentBudget = targetAmount; 

        // animation to deduct the bar
        StartCoroutine(UpdateBarAndText(previousAmount, currentBudget));

        return true;
    }


    IEnumerator UpdateBarAndText(float start, float end)
    {
        float currBudget = start;
        float barPercent = start / m_initialBudget;
        float barPercentTarget = (float)end / m_initialBudget;
        while(currBudget > (end+10))
        {
            currBudget = Mathf.Lerp(currBudget, end, 0.1f);
            barPercent = Mathf.Lerp(barPercent, barPercentTarget, 0.1f);

            Helper_UpdateBarAndText(currBudget, barPercent);
                
            yield return new WaitForSeconds(Time.deltaTime * 0.5f);
        }

        Helper_UpdateBarAndText(end, barPercentTarget);

        yield return null;
    }


    private void Helper_UpdateBarAndText(float budgetAmount, float percent)
    {
        loadingBar.GetComponent<Image>().fillAmount = percent;
        budgetText.text = string.Format("{0}", Mathf.FloorToInt(budgetAmount));
    }
    
    /// <summary>
    /// This function reset the bar to its initial state,
    /// By default, money starts at 1500
    /// </summary>
    public void ResetBar()
    {
        currentBudget = m_initialBudget;
        loadingBar.GetComponent<Image>().fillAmount = 1;
        budgetText.text = string.Format("{0}",currentBudget);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!SpendBudget(200)) {
                Debug.Log("Insufficient Fund");
            }

            Debug.Log("The Current Budget is " + currentBudget);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            if (!SpendBudget(500))
            {
                Debug.Log("Insufficient Fund");
            }

            Debug.Log("The Current Budget is " + currentBudget);
        }
    }
}
