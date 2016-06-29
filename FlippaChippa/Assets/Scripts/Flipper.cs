using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof (Rotator)), RequireComponent(typeof(Translator))]
public class Flipper : MonoBehaviour, FCEventListener
{
	public float flipHeight = 0.75f;
	public float flipTime = 0.25f;

	private Rotator rotator;
	private Translator translator;
	private int nFinished = 0;	//Number of finished transformations, ie Rotator and Translator
	private bool isFlipping = false;
	public bool IsFlipping { get {return isFlipping; } }

	private FCObservable observable;
	public Transform targetTransform;

	// Use this for initialization
	void Start ()
	{
		observable = new FCObservable ();

		rotator = GetComponent<Rotator> ();
		translator = GetComponent<Translator> ();
		rotator.AddListener (this, FCEvent.END);
		translator.AddListener (this, FCEvent.END);

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
	}

	private void ResetChipsParent() {
		while (transform.childCount > 0) {
			Transform chipTransform = transform.GetChild (transform.childCount - 1).transform;
			chipTransform.parent = targetTransform;
		}
		transform.parent = null;
	}

	#region FCEventListener implementation

	public void OnEvent (FCEvent fcEvent, GameObject gameObject)
	{
		if (fcEvent == FCEvent.END) {
			nFinished++;
		}

		if (nFinished == 2) {
			nFinished = 0;
			isFlipping = false;
			observable.NotifyListeners (FCEvent.END, gameObject);
			ResetChipsParent ();
		}
	}

	#endregion
}

