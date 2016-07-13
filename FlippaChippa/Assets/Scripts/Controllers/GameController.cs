using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour, FCEventListener {

	private Stack targetStack;
	private List<Stack> stacks;

	public GameInputController gameInputController;
	public Canvas gameOverCanvas;

	private HUDController hud;
	private PauseMenuController pauseMenu;

	private SingleGameStatsMeta statsMeta;

	void Awake() {
		statsMeta = new SingleGameStatsMeta ();
	}

	// Use this for initialization
	void Start () {
		targetStack = GameObject.FindGameObjectWithTag (Tags.STACK_TARGET).GetComponent<Stack>();
		targetStack.AddListener (this);
		gameOverCanvas.gameObject.SetActive (false);

		hud = GameObject.FindGameObjectWithTag (Tags.HUD).GetComponent<HUDController>();
		hud.SetNFlips (0);

		pauseMenu = GameObject.FindGameObjectWithTag (Tags.PAUSE_MENU).GetComponent<PauseMenuController> ();
		hud.SetNFlips (0);

		stacks = new List<Stack> ();
		GameObject[] stackGameObjects = GameObject.FindGameObjectsWithTag (Tags.STACK);
		foreach (GameObject stackGameObject in stackGameObjects) {
			Stack stack = stackGameObject.GetComponent<Stack> ();
			stack.AddListener (this);
			stacks.Add (stack);
		}
		Debug.Log ("stacks: " + stacks);
		Debug.Log ("Target stack: " + targetStack);
	}

	public void PauseGame() {
		Debug.Log ("Pausing game");
		Time.timeScale = 0;
		pauseMenu.gameObject.SetActive (true);
		hud.gameObject.SetActive (false);
		gameInputController.enabled = false;
	}

	public void ResumeGame() {
		Debug.Log ("Resuming game");
		Time.timeScale = 1;
		hud.gameObject.SetActive (true);
		pauseMenu.gameObject.SetActive (false);
		gameInputController.enabled = true;
	}

	public void RestartGame() {
		Time.timeScale = 1;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	public void GoToMenu() {
		Time.timeScale = 1;
		SceneManager.LoadScene (0);
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
					gameInputController.enabled = false;
					hud.gameObject.SetActive (false);
					pauseMenu.gameObject.SetActive (false);
					gameOverCanvas.gameObject.SetActive (true);
				}
			}
		}
	}

	#endregion
}
