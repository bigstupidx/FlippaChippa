using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public PrefabsManager manager;
	Course[] courses;
	Course highlightedCourse;

	void Awake () {
		manager = GameObject.FindGameObjectWithTag (Tags.PREFABS_MANAGER).GetComponent<PrefabsManager> ();
	}

	// Use this for initialization
	void Start () {
		//courses = manager.GetCourseSummaries ();
		//highlightedCourse = courses [0];
	}

	public void StartGame(string game) {	//game will can an identifier for the gametype or specific course. Most likely a json obejct
		CourseMeta meta = CourseMetaGenerator.Generate (manager);
		ApplicationModel.courseMeta = meta;

		SceneManager.LoadScene (1, LoadSceneMode.Single);
	}

	public void QuitGame() {
		Application.Quit ();
	}
}
