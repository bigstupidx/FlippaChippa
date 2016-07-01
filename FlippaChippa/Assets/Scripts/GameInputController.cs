using UnityEngine;
using System.Collections;

public class GameInputController : MonoBehaviour {

	private Camera mainCamera;

	private Chip downChip = null;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {

		bool upPhase = false, downPhase = false, dragPhase = false;
		Vector3 inputPosition;

		#if UNITY_ANDROID
		Debug.Log("Looking for android touch input");
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
		Debug.Log("Looking for mouse input");
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
}
