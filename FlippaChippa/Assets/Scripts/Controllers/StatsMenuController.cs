using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StatsMenuController : MonoBehaviour
{

	public Text totalGamesPlayed, totalFlips, flipsPerGame;

	void Start() {
		totalGamesPlayed.text = "" + ApplicationModel.statistics.TotalGamesPlayed ();
		totalFlips.text = "" + ApplicationModel.statistics.TotalFlips ();
		flipsPerGame.text = "" + (int)(ApplicationModel.statistics.CalcAverageFlipsPerGame ());
	}

	public void Menu() {
		SceneManager.LoadScene (0, LoadSceneMode.Single);
	}

}

