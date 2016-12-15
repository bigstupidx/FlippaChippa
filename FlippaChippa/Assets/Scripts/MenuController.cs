using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using System.IO;
using GooglePlayGames;
using System;

public class MenuController : MonoBehaviour {

	public PrefabsManager manager;
	Course[] courses;
	Course highlightedCourse;
	public Button achievementsButton;
	private Image achievementIcon;

	void Awake () {
		manager = GameObject.FindGameObjectWithTag (Tags.PREFABS_MANAGER).GetComponent<PrefabsManager> ();
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
			Debug.LogFormat("statistics: {0}", ApplicationModel.statistics.ToString());
		} else {
			ApplicationModel.statistics = new Statistics ();
		}
		string settingsFilePath = Application.persistentDataPath + "/" + Tags.SETTINGS_DATA ;
		Debug.LogFormat ("settings path: {0}", settingsFilePath);
		if (File.Exists (settingsFilePath)) {
			StreamReader reader = File.OpenText (settingsFilePath);
			string json = reader.ReadToEnd ();
			ApplicationModel.settings = JsonUtility.FromJson<Settings> (json);
			Debug.LogFormat ("settings: {0}", ApplicationModel.settings.ToString ());
		} else {
			ApplicationModel.settings = new Settings();
		}

		Transform childTransform = achievementsButton.transform.GetChild (0);
		achievementIcon = childTransform.GetComponent<Image> ();

		#if UNITY_ANDROID || UNITY_IOS
		try {
			PlayGamesPlatform.Instance.LoadAchievements((IAchievement[] achievements) => {
				ApplicationModel.achievements = achievements;
			});
		} catch (NullReferenceException e) {
		}
		#else
		ApplicationModel.achievements = new IAchievement[0];
		#endif
	}

	void Update() {
		#if UNITY_ANDROID || UNITY_IOS
		if (!Social.localUser.authenticated) {
			achievementsButton.interactable = false;
			achievementIcon.color = new Color (achievementIcon.color.r, achievementIcon.color.g, achievementIcon.color.b, 0.5f);
		} else {
			achievementsButton.interactable = true;
			achievementIcon.color = new Color (achievementIcon.color.r, achievementIcon.color.g, achievementIcon.color.b, 1f);
		}
		#else
		achievementsButton.interactable = false;
		achievementIcon.color = new Color (achievementIcon.color.r, achievementIcon.color.g, achievementIcon.color.b, 0.5f);
		#endif
	}

	public void StartGame(string game) {	//game will can an identifier for the gametype or specific course. Most likely a json obejct
		ApplicationModel.stackMetaPair = CourseMetaGenerator.GenerateStackMetaPair (manager, ApplicationModel.settings.difficulty);

		SceneManager.LoadScene (Scenes.GAME, LoadSceneMode.Single);
	}

	public void QuitGame() {
		Application.Quit ();
	}

	public void Statistics() {
		SceneManager.LoadScene (Scenes.STATISTICS, LoadSceneMode.Single);
	}

	public void Help() {
		SceneManager.LoadScene (Scenes.HELP, LoadSceneMode.Single);
	}

	public void Settings() {
		SceneManager.LoadScene (Scenes.SETTINGS, LoadSceneMode.Single);
	}

	public void ShowAchievements() {
		#if UNITY_ANDROID || UNITY_IOS
		PlayGamesPlatform.Instance.ShowAchievementsUI ();
		#endif
	}
}
