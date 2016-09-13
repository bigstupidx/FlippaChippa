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
		PlayGamesClientConfiguration configuration = new PlayGamesClientConfiguration.Builder ().Build ();
		PlayGamesPlatform.InitializeInstance (configuration);
		PlayGamesPlatform.Activate ();
	}

	// Use this for initialization
	void Start () {
		Social.localUser.Authenticate((bool success) => {
			menuButton.gameObject.SetActive(true);
			loadingText.enabled = false;

			if (success) {
				Debug.Log("User has been authenticated", this);
			} else {
				Debug.Log("Failed to authenticate user", this);
			}
		});
	
	}

	public void Play() {
		SceneManager.LoadScene (Scenes.MAIN_MENU, LoadSceneMode.Single);
	}
}
