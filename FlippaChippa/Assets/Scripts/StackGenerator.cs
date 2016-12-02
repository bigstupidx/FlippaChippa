using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class StackGenerator
{
	public PrefabsManager prefabsManager;

	public void SetPrefabsManager (PrefabsManager manager) {
		prefabsManager = manager;
	}

	public GameStacks GenerateStacks(int[] chipPrefabIds, int[] crushWeights, bool[] initFlips, int[] flips) {
		Chip[] allChips = GetChips (chipPrefabIds);
		for (int i = 0; i < allChips.Length; i++) {
			allChips [i].chipMeta.CrushWeight = crushWeights [i];
		}

		FlipChips (allChips, initFlips);

		StackMeta allChipsStackMeta = CreateStackMeta(allChips);
		allChipsStackMeta.CleanupStackForCrushedChips (0);
		Stack startStack = CreateStackFrom (allChipsStackMeta);
		startStack.gameObject.tag = Tags.STACK;
		startStack.transform.position = Vector3.right * 2f;

		int nFlips = Permute (allChipsStackMeta, flips);
		Stack targetStack = CreateStackFrom (allChipsStackMeta);
		targetStack.gameObject.tag = Tags.STACK_TARGET;
		targetStack.IsTargetStack = true;
		targetStack.Meta.isTargetStack = true;
		targetStack.transform.position = Vector3.zero;

		GameObject targetIndicator = prefabsManager.CreateTargetIndicator ();
		targetIndicator.transform.SetParent (targetStack.transform);
		targetIndicator.transform.localRotation = Quaternion.Euler(new Vector3 (-134.1f, -90f, 90f));
		targetIndicator.transform.localPosition = new Vector3 (0, 0.08f, 0);

		foreach (Chip chip in allChips) {
			GameObject.Destroy (chip.gameObject);
		}
			
		Debug.Log ("<color=green>--- Stack Generation Start ---</color>");
		Debug.LogFormat ("start stack: {0}", startStack.Meta.ToStringShort ());
		Debug.LogFormat ("target stack: {0}", targetStack.Meta.ToStringShort ());
		Debug.Log ("<color=green>--- Stack Generation end ---</color>");

		int nFlipsAlternative = FlipsCalulator.CalculateMinFlips (startStack.Meta, targetStack.Meta, 100, nFlips);
		Debug.Log (string.Format("Calculated flips ({0}). Generator flips ({1})", nFlipsAlternative, nFlips));
		if (nFlipsAlternative < nFlips) {
			nFlips = nFlipsAlternative;
		}
		return new GameStacks (targetStack, startStack, nFlips);
	}

	private int Permute(StackMeta stackMeta, int[] flips) {
		int nFlips = 0;
		List<StackMeta> flipHistory = new List<StackMeta> ();
		for (int i = 0; i < flips.Length; i++) {
			stackMeta.FlipStackAt (flips [i]);
			if (PermutationExists(stackMeta, flipHistory)) {
				//Undo flip since we've been here before
				stackMeta.FlipStackAt (flips [i]);
			} else {
				flipHistory.Add (stackMeta.Copy ());
				nFlips++;
			}
		}

		return nFlips;
	}

	private bool PermutationExists(StackMeta newPermutation, List<StackMeta> existingPermutations) {
		foreach (StackMeta permutation in existingPermutations) {
			if (newPermutation.Matches(permutation)) {
				return true;
			}
		}
		return false;
	}
		

	private void FlipChips(Chip[] chips, bool[] flips) {
		if (flips.Length != chips.Length) {
			Debug.Log ("<color=red>The flips-array has a different length than the chips-array</color>");
		}
		int end = Math.Min (chips.Length, flips.Length);
		for (int i = 0; i < end; i++) {
			bool flip = flips [i];
			if (flip) {
				Chip chip = chips [i];
				chip.chipMeta.Flip ();
				chip.transform.localRotation = Quaternion.Euler (new Vector3 (180f, 0f, 0f));
			}
		}
	}

	private Chip[] GetChips(int[] prefabId) {
		Chip[] chips = new Chip[prefabId.Length];
		for (int i = 0; i < chips.Length; i++) {
			Chip chip = prefabsManager.GetChip (prefabId [i]);
			chips [i] = chip;
		}
		return chips;
	}

	Stack CreateStackFrom(StackMeta stackMeta) {
		Stack stack = prefabsManager.CreateStack ();
		for (int i = 0; i < stackMeta.ChipCount (); i++) {
			int prefabId = stackMeta.GetChipMetaAt (i).prefabId;
			Chip chip = prefabsManager.GetChip (prefabId);
			stack.Add (chip);
			chip.chipMeta.CrushWeight = stackMeta.GetChipMetaAt (i).CrushWeight;
			if (chip.chipMeta.orientation != stackMeta.GetChipMetaAt (i).orientation) {
				chip.chipMeta.Flip ();
				chip.transform.localRotation = Quaternion.Euler (180f, 0f, 0f);
			}
		}
		return stack;
	}

	private StackMeta CreateStackMeta(Chip[] chips) {
		StackMeta stackMeta = new StackMeta ();
		foreach (Chip chip in chips) {
			stackMeta.Add (chip.chipMeta);
		}
		return stackMeta;
	}
}

