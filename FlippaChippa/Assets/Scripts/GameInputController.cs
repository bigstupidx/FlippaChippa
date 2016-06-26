using UnityEngine;
using System.Collections;

public class GameInputController : MonoBehaviour {

	private Camera mainCamera;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {

		bool clickedScreen = false;
		Vector3 clickPosition = Vector3.zero;

		#if UNITY_ANDROID
		Debug.Log("Looking for android touch input");
		if (Input.touches.Length > 0) {
			clickedScreen = Input.GetTouch(0).phase == TouchPhase.Ended ? true : false;
			clickPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
		}
		#endif


		#if UNITY_EDITOR
		Debug.Log("Looking for mouse input");
		if (!clickedScreen) {
			clickedScreen = Input.GetMouseButtonUp(0);
			clickPosition = Input.mousePosition;
		}
		#endif

		if (clickedScreen) {
			RaycastHit raycastHit = new RaycastHit ();
			Ray ray = mainCamera.ScreenPointToRay (clickPosition);
			Physics.Raycast (ray, out raycastHit, 20f);

			if (raycastHit.collider != null) {	//Happens if the player clicks in an empty space
				GameObject clickedObject = raycastHit.collider.transform.gameObject;
				Chip chip = clickedObject.GetComponent<Chip> ();
				if (chip != null && chip.transform.parent != null) {	//Check that we are dealing with a chip, and if so, that the chip has a parent
					Stack stack = chip.transform.parent.gameObject.GetComponent<Stack> ();
					if (stack != null) {	//Necessary because a chip can have a flipper as a parent during flips
						stack.FlipAt(chip.chipMeta.stackPos);
					}
				}
			}
		}
	
	}
}
