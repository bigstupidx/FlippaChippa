using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Switcher : MonoBehaviour {

	public RectTransform[] screens;
	public Button NextButton, PreviousButton;
	public RectTransform canvasRect, allContent;

	private int currentVisible = 0, lastVisible = 0;	//0 == FlipsContent
	public int CurrentlyVisibleIndex { get { return currentVisible; } }
	private float startPos = 0f, endPos = 0f;
	private float elapsedTime = 0f;
	public float animationTime = 0.5f;
	private bool doMove = false;

	// Use this for initialization
	void Start () {
		lastVisible = screens.Length - 1;

		for (int i = 0; i < screens.Length; i++) {
			RectTransform rect = screens [i];
			rect.anchoredPosition = new Vector2 (i * canvasRect.rect.width, rect.anchoredPosition.y);
		}

		NextButton.onClick.AddListener(NextScreen);
		PreviousButton.onClick.AddListener (PreviousScreen);

		SetSwipeButtonsEnable ();
	}
	
	// Update is called once per frame
	void Update () {
		if (doMove) {
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= animationTime) {
				doMove = false;
				allContent.anchoredPosition = new Vector2 (endPos, allContent.anchoredPosition.y);
			} else {
				float progress = elapsedTime / animationTime;
				float lerp = MJMath.LerpPyramid (progress);
				float newPos = startPos + (endPos - startPos) * lerp;
				allContent.anchoredPosition = new Vector2 (newPos, allContent.anchoredPosition.y);
			}
		}	
	}


	public void NextScreen() {
		if (currentVisible == lastVisible) {	//Showing the last screen
			return;
		}
		startPos = -currentVisible * canvasRect.rect.width;
		endPos = startPos - canvasRect.rect.width;
		doMove = true;
		elapsedTime = 0f;
		currentVisible++;
		SetSwipeButtonsEnable ();
	}

	public void PreviousScreen() {
		if (currentVisible == 0) {	//Showing the first screen
			return;
		}

		startPos = -currentVisible * canvasRect.rect.width;
		endPos = startPos + canvasRect.rect.width;
		doMove = true;
		elapsedTime = 0f;
		currentVisible--;
		SetSwipeButtonsEnable ();
	}

	private void SetSwipeButtonsEnable() {
		if (currentVisible == 0) {	//Showing the target stack
			DisableLeftButton ();
		} else {
			EnableLeftButton ();
		}

		if (currentVisible == lastVisible) {
			DisableRightButton ();
		} else {
			EnableRightButton ();
		}
	}

	public void DisableLeftButton() {
		PreviousButton.interactable = false;
	}

	public void DisableRightButton() {
		NextButton.interactable = false;
	}

	public void EnableLeftButton() {
		PreviousButton.interactable = true;
	}

	public void EnableRightButton() {
		NextButton.interactable = true;
	}

}
