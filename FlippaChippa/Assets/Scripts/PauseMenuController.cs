using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenuController : MonoBehaviour
{
	public Text NFlipsText;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void SetNFlips(int flips) {
		if (flips == 1) {
			NFlipsText.text = "1 Flip";
		} else {
			NFlipsText.text = "" + flips  + " Flips";
		}
	}
}

