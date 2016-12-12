using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SettingsController : MonoBehaviour
{

	Difficulty[] difficulties;
	public Text easySelectedText, normalSelectedText, hardSelectedText;
	Switcher switcher;

	void Awake() {
		difficulties = new Difficulty[]{ Difficulty.EASY, Difficulty.NORMAL, Difficulty.HARD };
		switcher = GetComponent<Switcher> ();
		switcher.StartIndex = IndexFromDifficulty (ApplicationModel.difficulty);
	}

	int IndexFromDifficulty(Difficulty difficulty) {
		if (difficulty == Difficulty.EASY) {
			return 0;
		} else if (difficulty == Difficulty.NORMAL) {
			return 1;
		} else {
			return 2;
		}
	}

	// Use this for initialization
	void Start ()
	{
		SetCorrectSelected (ApplicationModel.difficulty);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void SetCorrectSelected(Difficulty difficulty) {
		easySelectedText.enabled = false;
		normalSelectedText.enabled = false;
		hardSelectedText.enabled = false;
		if (difficulty == Difficulty.EASY) {
			easySelectedText.enabled = true;
		} else if (difficulty == Difficulty.NORMAL) {
			normalSelectedText.enabled = true;
		} else if (difficulty == Difficulty.HARD) {
			hardSelectedText.enabled = true;
		}
	}

	public void Nay() { 
		SceneManager.LoadScene (Scenes.MAIN_MENU, LoadSceneMode.Single);
	}

	public void Yay() {
		ApplicationModel.difficulty = difficulties [switcher.CurrentlyVisibleIndex];
		SceneManager.LoadScene (Scenes.MAIN_MENU, LoadSceneMode.Single);
	}
}

