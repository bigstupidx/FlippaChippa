using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stack : MonoBehaviour {

	Chip[] chips;
	public bool isTargetStack;

	// Use this for initialization
	void Start () {
		chips = GetComponentsInChildren<Chip>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
