using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof (Rotator)), RequireComponent(typeof(Translator)), RequireComponent(typeof(Scaler))]
public class Flipper : MonoBehaviour, FCEventListener
{
	public float flipHeight = 0.75f;
	public float flipTime = 0.25f;

	private Rotator rotator;
	private Translator translator;
	private Scaler scaler;
	private int nFinished = 0;	//Number of finished transformations, ie Rotator, Translator and Scaler
	private bool isFlipping = false;
	public bool IsFlipping { get {return isFlipping; } }

	private FCObservable observable;
	public Transform targetTransform;

	void Awake() {
		observable = new FCObservable ();
	}

	// Use this for initialization
	void Start ()
	{
		rotator = GetComponent<Rotator> ();
		translator = GetComponent<Translator> ();
		rotator.AddListener (this, FCEvent.END);
		translator.AddListener (this, FCEvent.END);
		scaler = GetComponent<Scaler> ();
		scaler.AddListener (this, FCEvent.END);

		rotator.rotationTime = flipTime;
		translator.translateTime = flipTime;
		translator.translateHeight = flipHeight;

		rotator.CalculateValues ();
		translator.CalculateValues ();
	}

	public void Flip(List<GameObject> chips, Transform stackTransform) {
		if (IsFlipping) {	//Can't start a new flip during a flip
			return;
		}

		float sumLocalYPos = 0f;
		foreach (GameObject chip in chips) {
			sumLocalYPos += chip.transform.localPosition.y;
		}

		transform.parent = stackTransform;
		transform.localPosition = new Vector3 (0f, sumLocalYPos / chips.Count, 0f);

		rotator.StartRotation ();
		translator.StartTranslation();

		foreach (GameObject chip in chips) {
			chip.transform.parent = transform;
		}

		isFlipping = true;
		targetTransform = stackTransform;
		observable.NotifyListeners (FCEvent.BEGIN, gameObject);
	}

	private void ResetChipsParent() {
		while (transform.childCount > 0) {
			Transform chipTransform = transform.GetChild (transform.childCount - 1).transform;
			chipTransform.parent = targetTransform;
		}
		transform.parent = null;
	}

	public void AddListener(FCEventListener listener, FCEvent fcEvent) {
		observable.AddListener (listener, fcEvent);
	}

	#region FCEventListener implementation

	public void OnEvent (FCEvent fcEvent, GameObject gameObject)
	{
		if (fcEvent == FCEvent.END) {
			nFinished++;
		}

		if (nFinished == 2) {	//Finished rotation and translationg, must now perform scaling
			List<Transform> flipChildren = GetChildren();
			ResetChipsParent();
			Chip lowestChip = GetLowestChip (flipChildren);
			float newY = lowestChip.transform.localPosition.y - lowestChip.chipMeta.Height / 2f;
			transform.parent = targetTransform;
			transform.localPosition = new Vector3 (0, newY, 0);
			foreach (Transform t in flipChildren) {
				t.SetParent (transform);
			}
			scaler.StartScaling ();

		} else if (nFinished == 3) {
			nFinished = 0;
			isFlipping = false;
			observable.NotifyListeners (FCEvent.END, gameObject);
			ResetChipsParent ();
		}
	}

	#endregion

	private Chip GetLowestChip(List<Transform> chipTransforms) {
		Transform lowest = chipTransforms [0];
		foreach (Transform chipTransform in chipTransforms) {
			if (chipTransform.transform.localPosition.y < lowest.localPosition.y) {
				lowest = chipTransform;
			}
		}

		Chip chip = lowest.GetComponent<Chip> ();
		return chip;
	}

	private List<Transform> GetChildren() {
		List<Transform> children = new List<Transform>();
		for (int i = 0; i < transform.childCount; i++) {
			Transform child = transform.GetChild (i);
			children.Add(child);
		}
		return children;
	}
}

