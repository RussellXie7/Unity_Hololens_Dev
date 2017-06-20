using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationUtils : MonoBehaviour {

    public bool inAnimation = false;

    public IEnumerator MoveToDesination(Transform trans, Vector3 dest, float seconds)
    {
        inAnimation = true;
        float timer = 0;

        //Record initial position.
        Vector3 initialPos = trans.position;

        while (timer < seconds)
        {
            //Update timer.
            timer += Time.deltaTime;

            //Get current position base on time.
            Vector3 currPos = initialPos + (dest - initialPos) * timer / seconds;

            trans.position = currPos;
            yield return null;
        }

        inAnimation = false;
    }

    public IEnumerator ScaleLocalFromTo(Transform trans, Vector3 initialScale, Vector3 destScale, float seconds)
    {
        inAnimation = true;
        float timer = 0;

        //Record initial position.
        trans.localScale = initialScale;

        while (timer < seconds)
        {
            //Update timer.
            timer += Time.deltaTime;

            //Get current scale base on time.
            Vector3 currScale = initialScale + (destScale - initialScale) * timer / seconds;

            trans.localScale = currScale;
            yield return null;
        }

        inAnimation = false;
    }

    public IEnumerator StartScaleAfter(float sec, Transform trans, Vector3 initialScale, Vector3 destScale, float seconds)
    {
        yield return new WaitForSeconds(sec);
        StartCoroutine(ScaleLocalFromTo(trans, initialScale, destScale, seconds));
    }
}
