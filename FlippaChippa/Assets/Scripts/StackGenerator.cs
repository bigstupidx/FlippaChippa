using UnityEngine;
using System.Collections;

public class StackGenerator
{
	public PrefabsManager prefabsManager;

	public void SetPrefabsManager (PrefabsManager manager) {
		prefabsManager = manager;
	}

	public GameStacks GenerateStacks(int[] chipPrefabIds, int[] startFlips, int[] targetFlips) {
		Chip[] allChips = GetChips (chipPrefabIds);

		StackMeta startStackMeta = CreateStackMeta (allChips);
		startStackMeta.Permute (startFlips);
		Stack startStack = CreateStackFrom (startStackMeta);
		startStack.gameObject.tag = Tags.STACK;
		startStack.transform.position = Vector3.right * 2f;

		StackMeta targetStackMeta = startStackMeta.Copy ();
		targetStackMeta.Permute (targetFlips);
		Stack targetStack = CreateStackFrom (targetStackMeta);
		targetStack.gameObject.tag = Tags.STACK_TARGET;
		targetStack.IsTargetStack = true;
		targetStack.Meta.isTargetStack = true;
		targetStack.transform.position = Vector3.zero;

		GameObject targetIndicator = prefabsManager.CreateTargetIndicator ();
		targetIndicator.transform.SetParent (targetStack.transform);
		targetIndicator.transform.localPosition = new Vector3 (0, -0.15f, 0);


		foreach (Chip chip in allChips) {
			GameObject.Destroy (chip.gameObject);
		}

		return new GameStacks (targetStack, startStack);
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

