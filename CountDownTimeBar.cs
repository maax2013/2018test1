using UnityEngine;
using System.Collections;

public class CountDownTimeBar : MonoBehaviour
{
	//	[SerializeField] GameObject cdTimerPrefab;
	[SerializeField] GameObject barFill;
	//	[SerializeField] Transform timerHolder;

	public event System.Action onTimesUp;
	//	public event System.Action onCountDownAlert;

	bool isRuning = false;

	float elapsedTime = 0f;
	float alertTime = 1f;
	bool isAlertState;
	Vector3 startingScale = new Vector3 (1f, 1f, 1f);
	Vector3 targetScale = new Vector3 (0f, 1f, 1f);

	//	public void initTimer ()
	//	{
	//		tempTileBG = Instantiate (cdTimerPrefab, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
	//	}
	//

	public bool IsRuning ()
	{
		return isRuning;
	}

	public void startCountDown (float cdTime)
	{
		isAlertState = false;
		gameObject.SetActive (true);
		isRuning = true;
		StartCoroutine (depleteTimeBar_overTime (cdTime));
	}

	public void stopTimer ()
	{
		gameObject.SetActive (false);
		isRuning = false;
	}

	IEnumerator depleteTimeBar_overTime (float duration)
	{
		elapsedTime = 0f;

		while (elapsedTime < duration) {
			barFill.transform.localScale = Vector3.Lerp (startingScale, targetScale, (elapsedTime / duration));
			if (duration - elapsedTime < alertTime) {
				enterAlertState ();
			}
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		barFill.transform.localScale = targetScale;
		stopTimer ();
		if (onTimesUp != null) {
			onTimesUp ();
		}
	}

	void enterAlertState ()
	{
		if (!isAlertState) {
			//TODO: enter alert state of the count down timer bar
			isAlertState = true;
		}
	}
}

