﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.IO;

public class MenuController : MonoBehaviour {

	public PrefabsManager manager;
	Course[] courses;
	Course highlightedCourse;

	void Awake () {
		manager = GameObject.FindGameObjectWithTag (Tags.PREFABS_MANAGER).GetComponent<PrefabsManager> ();
		PlayGamesClientConfiguration configuration = new PlayGamesClientConfiguration.Builder ().Build ();
		PlayGamesPlatform.InitializeInstance (configuration);
		PlayGamesPlatform.Activate ();
	}

	// Use this for initialization
	void Start () {
		//courses = manager.GetCourseSummaries ();
		//highlightedCourse = courses [0];
		string filePath = Application.persistentDataPath + "/" + Tags.STATISTICS_NAME;
		if (File.Exists (filePath)) {
			StreamReader reader = File.OpenText (filePath);
			string json = reader.ReadToEnd ();
			ApplicationModel.statistics = JsonUtility.FromJson<Statistics> (json);
			Debug.Log("statistics: " + ApplicationModel.statistics.ToString());
		} else {
			ApplicationModel.statistics = new Statistics ();
		}

		Social.localUser.Authenticate((bool success) => {
			if (success) {
				PlayGamesPlatform.Instance.LoadAchievements((IAchievement[] achievements) => {
					ApplicationModel.achievements = achievements;
				});
			}
			Debug.Log("Authenticating user: " + success);
		});
	}

	public void StartGame(string game) {	//game will can an identifier for the gametype or specific course. Most likely a json obejct
		CourseMeta meta = CourseMetaGenerator.Generate (manager);
		ApplicationModel.courseMeta = meta;

		SceneManager.LoadScene (1, LoadSceneMode.Single);
	}

	public void QuitGame() {
		Application.Quit ();
	}

	public void Statistics() {
		SceneManager.LoadScene (2, LoadSceneMode.Single);
	}

	public void Help() {
		SceneManager.LoadScene (3, LoadSceneMode.Single);
	}
}
