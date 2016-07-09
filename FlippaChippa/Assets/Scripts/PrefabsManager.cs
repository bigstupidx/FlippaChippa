using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefabsManager : MonoBehaviour {

	public Course[] prefabs;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public GameObject GetPrefabWithId(int courseId) {
		foreach (Course prefab in prefabs) {
			if (prefab.id == courseId) {
				return Instantiate (prefab.gameObject);
			}
		}
		return null;
	}

	public Course[] GetCourseSummaries() {
		Course[] courses = new Course[prefabs.Length];
		for (int i = 0; i < prefabs.Length; i++) {
			Course prefab = prefabs[i];	
			GameObject gameObject = Instantiate (prefab.gameObject);
			Course course = gameObject.GetComponent<Course> ();
			course.stacks = new List<StackMeta>();
			courses [i] = course;
		}
		return courses;
	}
}
