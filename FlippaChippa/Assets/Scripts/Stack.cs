using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stack : MonoBehaviour
{

	public StackMeta meta;
	public Flipper flipper;

	// Use this for initialization
	void Start ()
	{
		for (int i = 0; i < transform.childCount; i++) {
			GameObject child = transform.GetChild (i).gameObject;
			Chip chip = child.GetComponent<Chip> ();
			meta.Add (chip.chipMeta);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButton (0)) {
			List<GameObject> chipsToFlip = new List<GameObject> ();
			for (int i = 2; i < transform.childCount; i++) {
				chipsToFlip.Add (transform.GetChild (i).gameObject);
			}
			flipper.Flip (chipsToFlip, transform);
		}
	}

}

