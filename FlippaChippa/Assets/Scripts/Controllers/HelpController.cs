using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HelpController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Menu() {
		SceneManager.LoadScene (Scenes.MAIN_MENU, LoadSceneMode.Single);
	}
}
