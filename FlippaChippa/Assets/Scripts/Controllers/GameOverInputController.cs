using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverInputController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public void GoToMenu() {
		SceneManager.LoadScene (0, LoadSceneMode.Single);
	}
}
