using System;
using UnityEngine;
using UnityEngine.UI;

public class Chip : MonoBehaviour {

	public ChipMeta chipMeta;

	private bool doFade;	//any kind of fading is in progress
	private bool continueFadeOnCompletion;
	private Color fromColor, toColor;
	private float duration, elapsedTime;	//current fade duration and elapsed time

	public Color highlightColor;	//Input in editor
	private Color standardColor;

	private Material material;

	private Transform initialTransform;
	private GameObject crushIndicator;

	void Awake() {
	}

	void Start() {
		crushIndicator = transform.GetChild (0).gameObject;
		SetCrushIndicatorVisibiltyAndValue ();
		initialTransform = transform;
		material = GetComponent<Renderer> ().material;
		material.EnableKeyword ("_EMISSION");
		standardColor = material.GetColor ("_EmissionColor");
	}

	void SetCrushIndicatorVisibiltyAndValue () {
		if (chipMeta.IsCrushable) {
			for (int i = 0; i < crushIndicator.transform.childCount; i++) {
				Text crushText = crushIndicator.transform.GetChild (i).GetComponentInChildren<Text> ();
				crushText.text = "" + chipMeta.CrushWeight;
			}
		} else {
			crushIndicator.SetActive (false);
		}
	}

	void Update() {
		if (doFade) {
			elapsedTime += Time.deltaTime;
			if (elapsedTime > duration) {
				if (continueFadeOnCompletion) {
					StartNextFade ();
				} else {
					doFade = false;
					material.SetColor ("_EmissionColor", standardColor);
				}
			} else {
				float lerpProgress = Mathf.Clamp01(elapsedTime / duration);
				material.SetColor ("_EmissionColor", Color.Lerp (fromColor, toColor, lerpProgress));
			}
		}
	}

	private void StartNextFade() {
		Color temp = toColor;
		toColor = fromColor;
		fromColor = temp;

		StartFade ();
	}

	private void StartFade() {
		elapsedTime = 0f;
		doFade = true;
	}

	public void Highlight(float fadeDuration) {
		duration = fadeDuration;
		fromColor = standardColor;
		toColor = highlightColor;
		continueFadeOnCompletion = true;
		StartFade ();
	}

	public void UnHighlight(float fadeDuration) {
		float timeLeft = duration - elapsedTime;
		if (timeLeft > fadeDuration) {
			duration = fadeDuration;
		} else {
			duration = timeLeft;
		}

		fromColor = material.GetColor ("_EmissionColor");
		toColor = standardColor;

		continueFadeOnCompletion = false;
		StartFade ();
	}
}

