using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public PrefabsManager manager;
	Course[] courses;
	Course highlightedCourse;

	// Use this for initialization
	void Start () {
		courses = manager.GetCourseSummaries ();
		highlightedCourse = courses [0];
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (0)) {
			ApplicationModel.courseLoadedId = highlightedCourse.id;
			SceneManager.LoadScene (1, LoadSceneMode.Single);
		}
	}
}
