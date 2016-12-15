using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefabsManager : MonoBehaviour {

	public Chip[] chipPrefabs;
	public Stack stackPrefab;
	public GameObject targetStackIndicator;
	public Flipper flipper;
	public Faller faller;
	public Mover mover;
	private Mover[] moverPool;

	void Awake () {
		// Give each chip an id based on its order in the chipprefab list
		for (int i = 0; i < chipPrefabs.Length; i++) {
			chipPrefabs [i].chipMeta.prefabId = i;
		}
		moverPool = new Mover[15];
		for (int i = 0; i < moverPool.Length; i++) {
			moverPool [i] = Instantiate (mover).GetComponent<Mover>();
			moverPool [i].gameObject.SetActive (false);
			moverPool [i].transform.parent = transform.parent;
		}
	}

	// Use this for initialization
	void Start () {
		
	}

	public Chip GetChip(int id) {
		foreach (Chip chip in chipPrefabs) {
			if (chip.chipMeta.prefabId == id) {
				return Instantiate(chip.gameObject).GetComponent<Chip>();
			}
		}
		return null;
	}

	public ChipMeta GetChipMeta(int id) {
		foreach (Chip chip in chipPrefabs) {
			if (chip.chipMeta.prefabId == id) {
				return chip.chipMeta.Copy();
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
		return Random.Range (0, chipPrefabs.Length);
	}

	public Stack CreateStack() {
		Stack stack = Instantiate (stackPrefab.gameObject).GetComponent<Stack> ();
		Flipper flipper = Instantiate (this.flipper.gameObject).GetComponent<Flipper> ();
		Faller faller = Instantiate (this.faller.gameObject).GetComponent<Faller> ();
		stack.flipper = flipper;
		stack.faller = faller;
		flipper.targetTransform = stack.transform;
		faller.targetTransform = stack.transform;
		return stack;
	}

	public GameObject CreateTargetIndicator() {
		return Instantiate (targetStackIndicator.gameObject).gameObject;
	}

	public Mover GetMover() {
		for (int i = 0; i < moverPool.Length; i++) {
			if (!moverPool [i].gameObject.activeSelf) {
				moverPool [i].gameObject.SetActive (true);
				return moverPool [i];
			}
		}
		throw new UnityException ("No free movers!");
	}
}
