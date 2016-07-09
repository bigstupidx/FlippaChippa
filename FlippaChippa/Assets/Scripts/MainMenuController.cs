using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

	public PrefabsManager manager;

	// Use this for initialization
	void Start () {
		GameObject prefabGameObject = manager.GetPrefabWithId (ApplicationModel.courseLoadedId);
		Course course = prefabGameObject.GetComponent<Course>();
		course.transform.position = Vector3.zero;
		/*course.targetStack.transform.localPosition = Vector3.zero;
		for (int i = 0; i < course.stacks.Count; i++) {
			course.stacks[i].transform.localPosition = new Vector3 ((i + 1) * 1.5f, 0f, 0f);
		}*/
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonUp (0)) {
			ApplicationModel.completedCourse = true;
			SceneManager.LoadScene (0);
		}
	
	}

	void LoadCourse(int courseId) {
		

	}

}
