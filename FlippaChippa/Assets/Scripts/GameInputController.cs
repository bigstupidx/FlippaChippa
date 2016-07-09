using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameInputController : MonoBehaviour {

	private Camera mainCamera;

	private Chip downChip = null;

	private Canvas hudCanvas, pauseMenuCanvas;

	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
		hudCanvas = GameObject.FindGameObjectWithTag (Tags.HUD).GetComponent<Canvas> ();
		pauseMenuCanvas = GameObject.FindGameObjectWithTag (Tags.PAUSE_MENU).GetComponent<Canvas> ();

		hudCanvas.gameObject.SetActive (true);
		pauseMenuCanvas.gameObject.SetActive (false);
	}
	
	void Update () {

		bool upPhase = false, downPhase = false, dragPhase = false;
		Vector3 inputPosition = Vector3.zero;

		#if UNITY_ANDROID
		if (Input.touches.Length > 0) {
			TouchPhase phase = Input.GetTouch(0).phase;
			if (phase == TouchPhase.Began) {
				downPhase = true;
			} else if (phase == TouchPhase.Ended) {
				upPhase = true;
			} else {
				dragPhase = true;
			}
			inputPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
		}
		#endif


		#if UNITY_EDITOR
		if (Input.GetMouseButtonDown(0)) {
			downPhase = true;
		} else if (Input.GetMouseButtonUp(0)) {
			upPhase = true;
		} else if (Input.GetMouseButton(0)){
			dragPhase = true;
		}
		inputPosition = Input.mousePosition;
		#endif

		if (downPhase) {
			RaycastHit raycastHit = new RaycastHit ();
			Ray ray = mainCamera.ScreenPointToRay (inputPosition);
			Physics.Raycast (ray, out raycastHit, 20f);

			if (raycastHit.collider != null) {
				GameObject clickedObject = raycastHit.collider.transform.gameObject;
				Chip chip = clickedObject.GetComponent<Chip> ();
				if (chip != null) {
					downChip = chip;
				}
			}
		} 
		else if (upPhase) {
			if (downChip != null) {
				RaycastHit raycastHit = new RaycastHit ();
				Ray ray = mainCamera.ScreenPointToRay (inputPosition);
				Physics.Raycast (ray, out raycastHit, 20f);

				if (raycastHit.collider != null) {	//Happens if the player clicks in an empty space
					GameObject clickedObject = raycastHit.collider.transform.gameObject;
					Chip chip = clickedObject.GetComponent<Chip> ();
					if (chip != null && chip.transform.parent != null && ArePartOfTheSameStack(chip, downChip)) {	//Check that we are dealing with a chip, and if so, that the chip has a parent
						Stack stack = chip.transform.parent.gameObject.GetComponent<Stack> ();
						if (stack != null) {	//Necessary because a chip can have a flipper as a parent during flips
							stack.FlipAt (chip.chipMeta.stackPos);
						}
					}
				}
				downChip = null;
			} else {
				
			}
		}
	
	}

	private bool ArePartOfTheSameStack(Chip chip1, Chip chip2) {
		Transform stack1Transform = chip1.transform.parent;
		Transform stack2Transform = chip2.transform.parent;
		return stack1Transform == stack2Transform;
	}

	public void PauseGame() {
		Debug.Log ("Pausing game");
		Time.timeScale = 0;
		pauseMenuCanvas.gameObject.SetActive (true);
		hudCanvas.gameObject.SetActive (false);
	}

	public void ResumeGame() {
		Debug.Log ("Resuming game");
		Time.timeScale = 1;
		hudCanvas.gameObject.SetActive (true);
		pauseMenuCanvas.gameObject.SetActive (false);
	}

	public void RestartGame() {
		Time.timeScale = 1;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	public void GoToMenu() {
		Time.timeScale = 1;
		SceneManager.LoadScene (0);
	}
}
