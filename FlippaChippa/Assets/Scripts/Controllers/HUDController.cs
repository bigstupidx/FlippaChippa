using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{

	public Button leftArrow, rightArrow;

	// Use this for initialization
	void Start ()
	{
		DisableLeftButton ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void DisableLeftButton() {
		leftArrow.interactable = false;
	}

	public void DisableRightButton() {
		rightArrow.interactable = false;
	}

	public void EnableLeftButton() {
		leftArrow.interactable = true;
	}

	public void EnableRightButton() {
		rightArrow.interactable = true;
	}
}

