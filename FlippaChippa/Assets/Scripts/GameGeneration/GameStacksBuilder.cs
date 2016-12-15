using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameStacksBuilder
{
	public GameStacks BuildGameStacks(GameStacksMeta metaPair, PrefabsManager prefabsManager) {
		Stack startStack = BuildStackFromStackMeta (metaPair.start, prefabsManager);
		startStack.gameObject.tag = Tags.STACK;
		startStack.transform.position = Vector3.right * 2f;

		Stack targetStack = BuildStackFromStackMeta (metaPair.target, prefabsManager);
		targetStack.gameObject.tag = Tags.STACK_TARGET;
		targetStack.IsTargetStack = true;
		targetStack.Meta.isTargetStack = true;
		targetStack.transform.position = Vector3.zero;

		GameObject targetIndicator = prefabsManager.CreateTargetIndicator ();
		targetIndicator.transform.SetParent (targetStack.transform);
		targetIndicator.transform.localRotation = Quaternion.Euler(new Vector3 (-134.1f, -90f, 90f));
		targetIndicator.transform.localPosition = new Vector3 (0, 0, 0);

		return new GameStacks (targetStack, startStack, metaPair.nFlips);
	}

	private Stack BuildStackFromStackMeta(StackMeta stackMeta, PrefabsManager prefabsManager) {
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
}

