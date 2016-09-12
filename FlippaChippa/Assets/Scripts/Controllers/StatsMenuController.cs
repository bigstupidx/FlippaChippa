using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StatsMenuController : MonoBehaviour
{

	public Text totalGamesPlayed, totalFlips, flipsPerGame;
	public Text totalChecks, checksPerGame;
	public Text totalTime, timePerGame;
	public Text gamesPlayed1, gamesPlayed2;
	public Button NextButton, PreviousButton;
	public RectTransform flipsContent, targetContent, timeContent, allContent;
	private int currentVisible = 0;	//0 == FlipsContent
	public RectTransform canvasRect;
	private float startPos = 0f, endPos = 0f;
	private float elapsedTime = 0f;
	public float animationTime = 0.5f;
	private bool doMove = false;

	void Awake() {
		SetSwipeButtonsEnable ();
	}

	void Start() {
		totalFlips.text = "" + ApplicationModel.statistics.TotalFlips ();
		flipsPerGame.text = "" + (int)(ApplicationModel.statistics.AverageFlips ());
		totalChecks.text = "" + ApplicationModel.statistics.TotalChecks ();
		checksPerGame.text = "" + ApplicationModel.statistics.AverageTargetChecks ();
		totalTime.text = getTimeString (ApplicationModel.statistics.TotalTime ());
		timePerGame.text = getTimeString (ApplicationModel.statistics.AverageTime ());
		string totalGamesPlayedString = "" + ApplicationModel.statistics.TotalGames ();
		totalGamesPlayed.text = totalGamesPlayedString;
		gamesPlayed1.text = totalGamesPlayedString;
		gamesPlayed2.text = totalGamesPlayedString;

		Debug.Log ("width: " + canvasRect.rect.width);

		targetContent.anchoredPosition = new Vector2 (canvasRect.rect.width, targetContent.anchoredPosition.y);
		timeContent.anchoredPosition = new Vector2 (canvasRect.rect.width * 2, timeContent.anchoredPosition.y);
	}

	void Update() {
		if (doMove) {
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= animationTime) {
				doMove = false;
				allContent.anchoredPosition = new Vector2 (endPos, allContent.anchoredPosition.y);
			} else {
				float progress = elapsedTime / animationTime;
				float lerp = MJMath.LerpPyramid (progress);
				float newPos = startPos + (endPos - startPos) * lerp;
				allContent.anchoredPosition = new Vector2 (newPos, allContent.anchoredPosition.y);
			}
		}
	}

	public void NextScreen() {
		if (currentVisible == 2) {	//Showing the last screen
			return;
		}
		startPos = -currentVisible * canvasRect.rect.width;
		endPos = startPos - canvasRect.rect.width;
		doMove = true;
		elapsedTime = 0f;
		currentVisible++;
		SetSwipeButtonsEnable ();
	}

	public void PreviousScreen() {
		if (currentVisible == 0) {	//Showing the first screen
			return;
		}

		startPos = -currentVisible * canvasRect.rect.width;
		endPos = startPos + canvasRect.rect.width;
		doMove = true;
		elapsedTime = 0f;
		currentVisible--;
		SetSwipeButtonsEnable ();
	}

	public void Menu() {
		SceneManager.LoadScene (0, LoadSceneMode.Single);
	}

	private void SetSwipeButtonsEnable() {
		if (currentVisible == 0) {	//Showing the target stack
			DisableLeftButton ();
		} else {
			EnableLeftButton ();
		}

		if (currentVisible == 2) {
			DisableRightButton ();
		} else {
			EnableRightButton ();
		}
	}

	public void DisableLeftButton() {
		PreviousButton.interactable = false;
	}

	public void DisableRightButton() {
		NextButton.interactable = false;
	}

	public void EnableLeftButton() {
		PreviousButton.interactable = true;
	}

	public void EnableRightButton() {
		NextButton.interactable = true;
	}

	string getTimeString(float time) {
		int timeInSeconds = (int)time;
		int seconds = timeInSeconds % 60;
		int minutes = (timeInSeconds - seconds) / 60;
		int hours = timeInSeconds / (60 * 60);
		string secondsString = seconds < 10 ? "0" + seconds : "" + seconds;
		string minutesString = minutes < 10 ? "0" + minutes : "" + minutes;
		string hoursString = hours < 10 ? "0" + hours + ":" : "" + hours + ":";
		if (hours == 0) {
			hoursString = "";
		}
		return hoursString + minutesString + ":" + secondsString;
	}

}

