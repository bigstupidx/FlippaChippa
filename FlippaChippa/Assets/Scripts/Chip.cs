using System;
using UnityEngine;

public class Chip : MonoBehaviour {

	public ChipMeta chipMeta;

	private GameObject chipGameObject;
	private Transform initialTransform;

	void Awake() {
		chipGameObject = transform.FindChild ("chip").gameObject;
	}

	void Start() {
		initialTransform = transform;
	}
}

