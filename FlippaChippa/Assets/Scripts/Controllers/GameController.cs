using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour, FCEventListener {

	private Stack targetStack;
	private List<Stack> stacks;

	public GameInputController gameInputController;
	public Canvas gameOverCanvas;

	// Use this for initialization
	void Start () {
		targetStack = GameObject.FindGameObjectWithTag (Tags.STACK_TARGET).GetComponent<Stack>();
		gameOverCanvas.gameObject.SetActive (false);

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

	#region FCEventListener implementation

	public void OnEvent (FCEvent fcEvent, GameObject gameObject)
	{
		if (stacks.Count == 1) {	//Makes no sense to compare the target stack with multiple stacks
			Debug.Log ("targetStack: " + targetStack.Meta.ToStringShort ());
			Debug.Log ("clickable stack: " + stacks [0].Meta.ToStringShort ());
			if (targetStack.Matches (stacks [0])) {
				gameInputController.gameObject.SetActive (false);
				gameOverCanvas.gameObject.SetActive (true);
			}
		}
	}

	#endregion
}
