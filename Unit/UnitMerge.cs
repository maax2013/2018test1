using UnityEngine;
using System.Collections;

public class UnitMerge : MonoBehaviour
{
    public event System.Action OnMergeDone;
    public event System.Action OnPopDone;

    Coroutine moveCoroutine;
    Coroutine popCoroutine;

    public void mergeTo_overTime(Vector3 target, float mergeTime)
    {
        //      if (moveCoroutine) {
        //          StopCoroutine (moveCoroutine);
        //      }
        //      moveCoroutine = StartCoroutine (moveSpriteTo_overTime (target, mergeTime));
        moveCoroutine = StartCoroutine(mergeToOverTime(target, mergeTime));
    }

    public void popSprite_overTime(float popTime)
    {
        popCoroutine = StartCoroutine(popSpriteOverTime(popTime));
    }

    IEnumerator mergeToOverTime(Vector3 target, float duration)
    {
        float elapsedTime = 0;
        Vector3 startingPos = transform.localPosition;
        while (elapsedTime < duration)
        {
            transform.localPosition = Vector3.Lerp(startingPos, target, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.localPosition = startingPos;

        if (OnMergeDone != null)
        {
            OnMergeDone();
            OnMergeDone = null;
        }
    }

    IEnumerator popSpriteOverTime(float duration)
    {
        Vector3 target = new Vector3(1.3f, 1.3f, 1f);//~~~~~~~~~~~~~~~~~~~~~~~~~~~
        float elapsedTime = 0;

        float popUpTime = duration * 0.2f;
        float pauseTime = duration * 0.3f;
        float shrinkTime = duration * 0.5f;

        Vector3 startingScale = transform.localScale;

        while (elapsedTime < popUpTime)
        {
            transform.localScale = Vector3.Lerp(startingScale, target, (elapsedTime / popUpTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.localScale = target;

        yield return new WaitForSeconds(pauseTime);

        elapsedTime = 0;
        while (elapsedTime < shrinkTime)
        {
            transform.localScale = Vector3.Lerp(target, startingScale, (elapsedTime / shrinkTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.localScale = startingScale;

        if (OnPopDone != null)
        {
            OnPopDone();
            OnPopDone = null;
        }
    }
}
