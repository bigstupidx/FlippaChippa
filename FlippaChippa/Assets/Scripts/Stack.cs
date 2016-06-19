using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stack : MonoBehaviour {

	ChipMeta[] chips;
	public bool isTargetStack;

	// Use this for initialization
	void Start () {
		chips = GetComponentsInChildren<ChipMeta>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
