using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;

public class GameController : MonoBehaviour, FCEventListener, LandingListener {

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
	private BlurOptimized blur;

	void Awake() {
		statsMeta = new SingleGameStatsMeta ();
		statsMeta.DifficultyFactor = 0.3f;
		statsMeta.NTargetChecks = 1;
		cameraController = GameObject.FindGameObjectWithTag (Tags.MAIN_CAMERA).GetComponent<CameraController> ();
		stackGenerator = new StackGenerator ();
		prefabsManager = GameObject.FindGameObjectWithTag (Tags.PREFABS_MANAGER).GetComponent<PrefabsManager> ();

		blur = cameraController.GetComponent<BlurOptimized> ();
		blur.enabled = false;

		stackGenerator.SetPrefabsManager (prefabsManager);
		int[] chipIds = ApplicationModel.courseMeta.ChipIDs;
		int[] crushWeight = ApplicationModel.courseMeta.CrushWeights;
		bool[] initFlips = ApplicationModel.courseMeta.InitFlips;
		int[] startFlips = ApplicationModel.courseMeta.Flips;
		GameStacks gamestacks = stackGenerator.GenerateStacks(chipIds, crushWeight, initFlips, startFlips);
		statsMeta.MaxFlips = gamestacks.MaxFlips + (int)(Mathf.Max(gamestacks.Target.Meta.ChipCount() * statsMeta.DifficultyFactor, 1));
		targetStack = gamestacks.Target;
		targetStack.AddListener (this);

		hud = GameObject.FindGameObjectWithTag (Tags.HUD).GetComponent<StatisticsController>();
		hud.SetNFlips (statsMeta.MaxFlips);
		hudController = hud.GetComponent<HUDController> ();

		pauseMenu = GameObject.FindGameObjectWithTag (Tags.PAUSE_MENU).GetComponent<StatisticsController> ();
		pauseMenu.SetNFlipsLeft (statsMeta.MaxFlips);
		pauseMenu.SetNTargetChecks (1);
		pauseMenu.SetTime (0f);

		gameOverMenu = GameObject.FindGameObjectWithTag (Tags.GAME_OVER_MENU).GetComponent<StatisticsController> ();
		gameOverMenu.SetNFlipsLeft (statsMeta.MaxFlips);
		gameOverMenu.gameObject.SetActive (false);
		ResumeGame ();

		stacks = new List<Stack> ();
		stacks.Add (gamestacks.Player);
		foreach (Stack stack in stacks) {
			stack.AddListener (this);
		}
	}

	// Use this for initialization
	void Start () {
	}

	void Update() {
		statsMeta.Time += Time.deltaTime;
	}

	public void PauseGame() {
		Debug.Log ("<color=purple>Pausing game</color>");
		Time.timeScale = 0;
		pauseMenu.SetNTargetChecks (statsMeta.NTargetChecks);
		pauseMenu.SetTime (statsMeta.Time);
		pauseMenu.gameObject.SetActive (true);
		hud.gameObject.SetActive (false);
		gameInputController.enabled = false;
		blur.enabled = true;
	}

	public void ResumeGame() {
		Debug.Log ("<color=purple>Resuming game</color>");
		Time.timeScale = 1;
		hud.gameObject.SetActive (true);
		pauseMenu.gameObject.SetActive (false);
		gameInputController.enabled = true;
		blur.enabled = false;
	}

	public void RestartGame() {
		Debug.Log ("<color=purple>Restarting game</color>");
		Time.timeScale = 1;
		ApplicationModel.statistics.AbortStreak ();
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	public void GoToMenu() {
		Time.timeScale = 1;
		ApplicationModel.statistics.AbortStreak ();
		SceneManager.LoadScene (Scenes.MAIN_MENU);
	}

	public void NextGame() {
		Time.timeScale = 1;
		ApplicationModel.courseMeta = CourseMetaGenerator.Generate (prefabsManager, ApplicationModel.difficulty);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	#region FCEventListener implementation

	public void OnEvent (FCEvent fcEvent, GameObject gameObject)
	{
		if (fcEvent == FCEvent.BEGIN) {
			statsMeta.NFlips++;
		} else if (fcEvent == FCEvent.MIDDLE) {
			hud.SetNFlips (statsMeta.MaxFlips - statsMeta.NFlips);
			pauseMenu.SetNFlipsLeft (statsMeta.MaxFlips - statsMeta.NFlips);
		}
		else if (fcEvent == FCEvent.END) 
		{
			bool outOfFlips = statsMeta.NFlips == statsMeta.MaxFlips;
			if (outOfFlips || stacks.Count == 1 ) {	//Makes no sense to compare the target stack with multiple stacks
				bool canMatch = stacks [0].Meta.CanMatch (targetStack.Meta);
				bool stacksMatch = targetStack.Matches (stacks [0]);
				bool gameIsOver = stacksMatch || outOfFlips || !canMatch;
				if (gameIsOver) {
					blur.enabled = true;
					gameInputController.enabled = false;
					hud.gameObject.SetActive (false);
					pauseMenu.gameObject.SetActive (false);
					gameOverMenu.gameObject.SetActive (true);
					gameOverMenu.SetNTargetChecks (statsMeta.NTargetChecks);
					gameOverMenu.SetTime (statsMeta.Time);
					gameOverMenu.SetNFlipsLeft (statsMeta.MaxFlips - statsMeta.NFlips);
					if (targetStack.Matches (stacks [0])) {
						gameOverMenu.SetTitle ("Yay!!!");
						statsMeta.SuccessfullGame = true;
						ApplicationModel.statistics.IncreaseStreakCount ();
					} else {
						gameOverMenu.SetTitle ("Nay...");
						statsMeta.SuccessfullGame = false;
						ApplicationModel.statistics.AbortStreak ();
					}
					ApplicationModel.statistics.RegisterCompletedGame (statsMeta);
					string filePath = Application.persistentDataPath + "/" + Tags.STATISTICS_NAME;
					string json = JsonUtility.ToJson (ApplicationModel.statistics);
					File.WriteAllText (filePath, json);
					
					AchievementRules.HandleAchievements (statsMeta, targetStack.Meta);
				}
			}
		}
	}

	#endregion

	private int TotalNumberOfStacks() {
		return stacks.Count + 1;
	}

	public void ShowNextStack() {
		if (IsAnimatingStack () || cameraController.IsMoving) {
			return;
		}
		int intendedNextStack = indexOfVisibleStack + 1;

		if (-1 < intendedNextStack && intendedNextStack < TotalNumberOfStacks ()) {
			indexOfVisibleStack = intendedNextStack;
			cameraController.MoveRight ();
			SetSwipeButtonsEnable ();
		}

	}

	public void ShowPreviousStack() {
		if (IsAnimatingStack () || cameraController.IsMoving) {
			return;
		}

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

	private bool IsAnimatingStack () {
		foreach (Stack stack in stacks) {
			if (stack.flipper.IsFlipping || stack.faller.IsFalling) {
				return true;
			}
		}
		return false;
	}

	public void AddLandingListener(FCEventListener listener, FCEvent fcEvent) {
		foreach (Stack stack in stacks) {
			stack.AddLandingListener (listener, fcEvent);
		}
	}
}
