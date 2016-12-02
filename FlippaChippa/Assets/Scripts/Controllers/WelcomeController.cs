using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WelcomeController : MonoBehaviour {


	public Button menuButton;
	public Text loadingText;

	void Awake() {
		menuButton.gameObject.SetActive (false);

		#if UNITY_ANDROID || UNITY_IOS

		PlayGamesClientConfiguration configuration = new PlayGamesClientConfiguration.Builder ().Build ();
		PlayGamesPlatform.InitializeInstance (configuration);
		PlayGamesPlatform.Activate ();

		#endif
	}

	// Use this for initialization
	void Start () {
		StartCoroutine (Loading());
		Social.localUser.Authenticate((bool success) => {
			if (success) {
				Debug.Log("User has been authenticated", this);
			} else {
				Debug.Log("Failed to authenticate user", this);
			}
		});
	}

	IEnumerator Loading() {
		yield return new WaitForSeconds (1f);
		menuButton.gameObject.SetActive(true);
		loadingText.enabled = false;
	}

	public void Play() {
		SceneManager.LoadScene (Scenes.MAIN_MENU, LoadSceneMode.Single);
	}
}
