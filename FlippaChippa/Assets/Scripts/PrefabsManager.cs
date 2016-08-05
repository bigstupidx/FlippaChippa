using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefabsManager : MonoBehaviour {

	public Chip[] chipPrefabs;
	public Stack stackPrefab;
	public GameObject targetStackIndicator;
	public Flipper flipper;

	// Use this for initialization
	void Start () {
		// Give each chip an id based on its order in the chipprefab list
		for (int i = 0; i < chipPrefabs.Length; i++) {
			chipPrefabs [i].chipMeta.prefabId = i;
		}
	}

	public Chip GetChip(int id) {
		foreach (Chip chip in chipPrefabs) {
			if (chip.chipMeta.prefabId == id) {
				return Instantiate(chip.gameObject).GetComponent<Chip>();
			}
		}
		return null;
	}

	public Chip GetRandomChip() {
		Chip chip = GetChip (GetRandomChipId ());
		if (chip == null) {
			chip = Instantiate (chipPrefabs [0]).GetComponent<Chip> ();
		}
		return chip;
	}

	public int GetRandomChipId() {
		return Random.Range (0, chipPrefabs.Length - 1);
	}

	public Stack CreateStack() {
		Stack stack = Instantiate (stackPrefab.gameObject).GetComponent<Stack> ();
		Flipper flipper = Instantiate (this.flipper.gameObject).GetComponent<Flipper> ();
		stack.flipper = flipper;
		flipper.targetTransform = stack.transform;
		return stack;
	}

	public GameObject CreateTargetIndicator() {
		return Instantiate (targetStackIndicator.gameObject).gameObject;
	}
}
