using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GameController : MonoBehaviour, FCEventListener {

	private Stack targetStack;
	private List<Stack> stacks;

	private int indexOfVisibleStack = 0;

	public GameInputController gameInputController;

	private StatisticsController hud, pauseMenu, gameOverMenu;
	private HUDController hudController;

	private SingleGameStatsMeta statsMeta;
	private CameraController cameraController;
	private StackGenerator stackGenerator;
	private PrefabsManager prefabsManager;
	private Blur blur;

	void Awake() {
		statsMeta = new SingleGameStatsMeta ();
		statsMeta.NTargetChecks = 1;
		cameraController = GameObject.FindGameObjectWithTag (Tags.MAIN_CAMERA).GetComponent<CameraController> ();
		stackGenerator = new StackGenerator ();
		prefabsManager = GameObject.FindGameObjectWithTag (Tags.PREFABS_MANAGER).GetComponent<PrefabsManager> ();
		blur = cameraController.GetComponent<Blur> ();
		blur.enabled = false;
	}

	// Use this for initialization
	void Start () {
		stackGenerator.SetPrefabsManager (prefabsManager);
		int[] chipIds = ApplicationModel.courseMeta.ChipIDs;
		int[] startFlips = ApplicationModel.courseMeta.StartFlips;
		int[] targetFlips = ApplicationModel.courseMeta.TargetFlips;
		GameStacks gamestacks = stackGenerator.GenerateStacks (chipIds, startFlips, targetFlips, true);
		targetStack = gamestacks.Target;
		targetStack.AddListener (this);

		hud = GameObject.FindGameObjectWithTag (Tags.HUD).GetComponent<StatisticsController>();
		hud.SetNFlips (0);
		hudController = hud.GetComponent<HUDController> ();

		pauseMenu = GameObject.FindGameObjectWithTag (Tags.PAUSE_MENU).GetComponent<StatisticsController> ();
		pauseMenu.SetNFlips (0);

		gameOverMenu = GameObject.FindGameObjectWithTag (Tags.GAME_OVER_MENU).GetComponent<StatisticsController> ();
		gameOverMenu.SetNFlips (0);
		gameOverMenu.gameObject.SetActive (false);
		ResumeGame ();

		stacks = new List<Stack> ();
		stacks.Add (gamestacks.Player);
		foreach (Stack stack in stacks) {
			stack.AddListener (this);
		}
		Debug.Log ("stacks: " + stacks);
		Debug.Log ("Target stack: " + targetStack);
	}

	void Update() {

	}

	public void PauseGame() {
		Debug.Log ("Pausing game");
		Time.timeScale = 0;
		pauseMenu.gameObject.SetActive (true);
		hud.gameObject.SetActive (false);
		gameInputController.enabled = false;
		blur.enabled = true;
	}

	public void ResumeGame() {
		Debug.Log ("Resuming game");
		Time.timeScale = 1;
		hud.gameObject.SetActive (true);
		pauseMenu.gameObject.SetActive (false);
		gameInputController.enabled = true;
		blur.enabled = false;
	}

	public void RestartGame() {
		Time.timeScale = 1;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	public void GoToMenu() {
		Time.timeScale = 1;
		SceneManager.LoadScene (0);
	}

	public void NextGame() {
		Time.timeScale = 1;
		ApplicationModel.courseMeta = CourseMetaGenerator.Generate (prefabsManager);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	#region FCEventListener implementation

	public void OnEvent (FCEvent fcEvent, GameObject gameObject)
	{
		if (fcEvent == FCEvent.BEGIN) 
		{
			statsMeta.NFlips++;
			hud.SetNFlips (statsMeta.NFlips);
			pauseMenu.SetNFlips (statsMeta.NFlips);
		} 
		else if (fcEvent == FCEvent.END) 
		{
			if (stacks.Count == 1) {	//Makes no sense to compare the target stack with multiple stacks
				Debug.Log ("targetStack: " + targetStack.Meta.ToStringShort ());
				Debug.Log ("clickable stack: " + stacks [0].Meta.ToStringShort ());
				if (targetStack.Matches (stacks [0])) {
					blur.enabled = true;
					gameInputController.enabled = false;
					hud.gameObject.SetActive (false);
					pauseMenu.gameObject.SetActive (false);
					gameOverMenu.gameObject.SetActive (true);
					gameOverMenu.SetNFlips (statsMeta.NFlips);

					ApplicationModel.statistics.AddFlips (statsMeta.NFlips);
					ApplicationModel.statistics.AddTargetChecks (statsMeta.NTargetChecks);
					string filePath = Application.persistentDataPath + "/" + Tags.STATISTICS_NAME;
					string json = JsonUtility.ToJson (ApplicationModel.statistics);
					File.WriteAllText (filePath, json);
				}
			}
		}
	}

	#endregion

	private int TotalNumberOfStacks() {
		return stacks.Count + 1;
	}

	public void ShowNextStack() {
		int intendedNextStack = indexOfVisibleStack + 1;

		if (-1 < intendedNextStack && intendedNextStack < TotalNumberOfStacks ()) {
			indexOfVisibleStack = intendedNextStack;
			cameraController.MoveRight ();
			SetSwipeButtonsEnable ();
		}

	}

	public void ShowPreviousStack() {
		int intendedNextStack = indexOfVisibleStack - 1;

		if (-1 < intendedNextStack && intendedNextStack < TotalNumberOfStacks ()) {
			indexOfVisibleStack = intendedNextStack;
			cameraController.MoveLeft ();
			SetSwipeButtonsEnable ();
		}
	}

	private void SetSwipeButtonsEnable() {
		if (indexOfVisibleStack - 1 < 0) {	//Showing the target stack
			hudController.DisableLeftButton ();
			statsMeta.NTargetChecks++;
		} else {
			hudController.EnableLeftButton ();
		}

		if (indexOfVisibleStack + 1 >= TotalNumberOfStacks ()) {
			hudController.DisableRightButton ();
		} else {
			hudController.EnableRightButton ();
		}
	}
}
