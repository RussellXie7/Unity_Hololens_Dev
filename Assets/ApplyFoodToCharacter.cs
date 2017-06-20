using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyFoodToCharacter : MonoBehaviour {

    private Transform character;
    public HumanCharacterStatusController hcsc;

    private Animator anim;

    public IntakeProperty cakeIntake;
    public IntakeProperty runningManIntake;

    private void Awake()
    {
        character = GameObject.FindGameObjectWithTag("Main Character").transform;
        anim = this.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ApplyFood("Fat");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            ApplyFood("Excercise");
        }
    }

    bool animInProgress = false;
    public void ApplyFood(string type)
    {
        HumanIntake hiToAdd = null;

        switch (type)
        {
            case "Fat":
                anim.SetTrigger("FlyCake");
                hiToAdd = cakeIntake.intake;
                break;
            case "Excercise":
                anim.SetTrigger("FlyRunningMan");
                hiToAdd = runningManIntake.intake;
                break;
        }

        if(!animInProgress)
            StartCoroutine(addHumanIntake(hiToAdd));
        
    }


    private IEnumerator addHumanIntake(HumanIntake hiToAdd)
    {
        animInProgress = true;
        yield return new WaitForSeconds(1.0f);
        hcsc.humanIntake = hcsc.humanIntake.Add(hiToAdd);
        animInProgress = false;
    }

    //private IEnumerator moveObjectToDest(GameObject objectToMove, Vector3 dest)
    //{
    //    float timer = 0;
    //    var initialPos = objectToMove.transform.position;
    //    var direction = dest - initialPos;
        
    //    while (timer <= 1.0f)
    //    {
    //        float factor = timer / 1.0f;
    //        objectToMove.transform.position = initialPos + direction * factor;

    //        timer += Time.deltaTime;
    //        yield return null;
    //    }

    //    Destroy(objectToMove);
    //}
}
