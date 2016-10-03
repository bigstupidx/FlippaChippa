using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stack : MonoBehaviour
{

	private StackMeta meta;
	public StackMeta Meta  { get { return meta; } }
	public Flipper flipper;
	public bool IsTargetStack;

	void Awake() {
		meta = new StackMeta ();
	}

	// Use this for initialization
	void Start ()
	{
		/*
		for (int i = 0; i < transform.childCount; i++) {
			GameObject child = transform.GetChild (i).gameObject;
			Chip chip = child.GetComponent<Chip> ();
			meta.Add (chip.chipMeta);
		}*/
	}
	
	public void FlipAt(GameObject gameObject) {
		Chip chipSelected = gameObject.GetComponent<Chip> ();
		if (chipSelected == null) {
			Debug.Log ("The selected gameobject isn't a chip. Will therefore not flip anything it.");
			return;
		}

		FlipAt (chipSelected.chipMeta.stackPos);
	}

	public void FlipAt(int position) {
		if (flipper.IsFlipping) {	//Extra insurance that the meta stack won't be flipped when the flipper isn't finished
			return;
		}

		List<GameObject> chipsToFlip = new List<GameObject> ();
		for (int i = position; i < transform.childCount; i++) {
			chipsToFlip.Add (transform.GetChild (i).gameObject);
		}

		meta.FlipStackAt (position);
		flipper.Flip (chipsToFlip, transform);

		Debug.Log ("Flipping at: " + position);
		Debug.Log (meta.ToStringShort ());
	}

	public void AddListener(FCEventListener listener) {
		flipper.AddListener (listener, FCEvent.END);
		flipper.AddListener (listener, FCEvent.BEGIN);
		flipper.AddListener (listener, FCEvent.MIDDLE);
	}

	public bool Matches(Stack otherStack) {
		return meta.Matches (otherStack.meta);
	}

	public void Add(Chip chip) {
		Meta.Add (chip.chipMeta);
		chip.transform.SetParent (transform);
		float yPos = GetCurrentMaxHeight () + chip.chipMeta.Height / 2f;
		chip.transform.localPosition = new Vector3 (0f, yPos, 0f);
		Debug.Log ("currentMaxHeight: " + GetCurrentMaxHeight());
		Debug.Log ("chip.ChipMeta.Height / 2: " + chip.chipMeta.Height / 2f);
		Debug.Log ("Meta.count: " + Meta.ChipCount ());	
	}

	private float GetCurrentMaxHeight() {
		float sum = 0f;
		for (int i = 0; i < Meta.ChipCount (); i++) {
			sum += Meta.GetChipMetaAt (i).Height;
		}
		return sum;
	}

}

