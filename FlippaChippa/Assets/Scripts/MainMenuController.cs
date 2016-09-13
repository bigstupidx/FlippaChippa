using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonUp (0)) {
			ApplicationModel.completedCourse = true;
			SceneManager.LoadScene (Scenes.MAIN_MENU);
		}
	
	}

}
