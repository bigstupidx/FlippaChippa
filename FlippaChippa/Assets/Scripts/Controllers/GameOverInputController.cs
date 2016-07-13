using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameOverInputController : MonoBehaviour {

	public Text NFlipsText;

	// Use this for initialization
	void Start () {
	
	}

	public void GoToMenu() {
		SceneManager.LoadScene (0, LoadSceneMode.Single);
	}

	public void SetNFlips(int flips) {
		if (flips == 1) {
			NFlipsText.text = "1 Flip";
		} else {
			NFlipsText.text = "" + flips  + " Flips";
		}
	}
}
