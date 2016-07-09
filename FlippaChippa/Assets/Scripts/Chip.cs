using System;
using UnityEngine;

public class Chip : MonoBehaviour {

	public ChipMeta chipMeta;

	private Transform initialTransform;

	void Awake() {
	}

	void Start() {
		initialTransform = transform;
	}
}

