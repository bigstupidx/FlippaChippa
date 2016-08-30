using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatisticsController : MonoBehaviour
{

	public Text NFlipsText;
	public Text NTargetChecks;
	public Text Time;

	// Use this for initialization
	void Start ()
	{

	}

	public void SetNFlips(int flips) {
		if (flips == 1) {
			NFlipsText.text = "1 Flip";
		} else {
			NFlipsText.text = "" + flips  + " Flips";
		}
	}

	public void SetNTargetChecks(int checks) {
		if (checks == 1) {
			NTargetChecks.text = "1 Target check";
		} else {
			NTargetChecks.text = "" + checks + " Target cheks";
		}
	}

	public void SetTime(float time) {
		int timeInSeconds = (int)time;
		int seconds = timeInSeconds % 60;
		int minutes = (timeInSeconds - seconds) / 60;
		string secondsString = seconds < 10 ? "0" + seconds : "" + seconds;
		string minutesString = minutes < 10 ? "0" + minutes : "" + minutes;
		Time.text = minutesString + ":" + secondsString;
	}
}

