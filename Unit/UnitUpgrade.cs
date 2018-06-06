using UnityEngine;
using System.Collections;

public class UnitUpgrade : MonoBehaviour
{
    public event System.Action OnPopDone;
    Coroutine popCoroutine;

    public void popSprite_overTime(float popTime)
    {
        popCoroutine = StartCoroutine(popSpriteTo_overTime(popTime));
    }
    IEnumerator popSpriteTo_overTime(float duration)
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