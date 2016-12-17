using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class StackMeta {

	private List<ChipMeta> chips;
	public bool isTargetStack;

	public StackMeta() {
		chips = new List<ChipMeta> ();
	}

	public void Add(ChipMeta chipMeta) {
		chips.Add (chipMeta);
		chipMeta.stackPos = chips.Count - 1;
	}

	public ChipMeta RemoveAt(int position) {
		if (0 <= position && position < chips.Count) {
			ChipMeta toRemove = chips[position];
			chips.RemoveAt (position);
			for (int i = position; i < chips.Count; i++) {
				ChipMeta chip = chips [i];
				chip.stackPos = i;
			}
			return toRemove;
		}
		return null;
	}

	public ChipMeta GetChipMetaAt(int position) {
		return chips [position];
	}

	public int ChipCount() {
		return chips.Count;
	}

	public CrushChipsMeta FlipStackAt(int position) {	//Doesn't actually flip the UI stack, only flips the the chips meta
		ReverseOrderAt(position, chips);
		for (int i = position; i < chips.Count; i++) {
			ChipMeta chip = chips [i];
			chip.Flip ();
			chip.stackPos = i;
		}

		CrushChipsMeta crushingChips = CheckForCrushingChips (position);
		foreach (ChipMeta meta in crushingChips.crushedChips) {
			RemoveAt (meta.stackPos);
		}
		return crushingChips;
	}

	public CrushChipsMeta CleanupStackForCrushedChips(int position) {
		CrushChipsMeta crushChipsMeta = CheckForCrushingChips (position);
		foreach (ChipMeta chipMeta in crushChipsMeta.crushedChips) {
			RemoveAt (chipMeta.stackPos);
		}
		return crushChipsMeta;
	}

	public CrushChipsMeta CheckForCrushingChips(int position) {
		List<ChipMeta> crushing = new List<ChipMeta>();
		List<ChipMeta> falling = new List<ChipMeta> ();
		int stackPosOfCrushedChip = -1;
		for (int i = position; i < chips.Count; i++) {	//No point in checking the chips that are below the flipped chip index
			if (ShouldCrushChip (i)) {
				crushing.Add (chips[i]);
				stackPosOfCrushedChip = i;
			} else if (stackPosOfCrushedChip != -1) {
				falling.Add (chips [i]);
			}
		}
		return new CrushChipsMeta (crushing, falling);
	}

	public static List<ChipMeta> ReverseOrderAt(int position, List<ChipMeta> list) {
		if (position == list.Count - 1) {
			return list;
		}

		int end = ((list.Count - position) / 2) + position;
		int counter = 0;
		for (int pos1 = position; pos1 < end; pos1++) {
			int pos2 = list.Count - 1 - counter++;
			ChipMeta chip1 = list [pos1];
			ChipMeta chip2 = list [pos2];
			list [pos1] = chip2;
			list [pos2] = chip1;
		}
		return list;
	}

	public bool CanMatch(StackMeta other) {
		StackMeta targetStack = isTargetStack ? this : other;
		StackMeta flippableStack = isTargetStack ? other : this;
		
		if (targetStack.ChipCount () > flippableStack.ChipCount ()) {
			return false;
		}

		Dictionary<string, int> target = targetStack.MapByIdAndCrushWeight();
		Dictionary<string, int> flippable = flippableStack.MapByIdAndCrushWeight();
		if (AnyTargetChipsMissingInFlippable(target, flippable)) {
			return false;
		}

		if (flippableStack.ChipCount() > targetStack.ChipCount()) {
			for (int i = 0; i < flippableStack.ChipCount(); i++) {
				ChipMeta chipMeta = flippableStack.GetChipMetaAt(i);
				if (chipMeta.IsCrushable && flippableStack.ChipCount() <= chipMeta.CrushWeight 
						&& !target.ContainsKey(StringifyChipMeta(chipMeta))) {
					return false;
				}
			}
		}

		return true;
	}

	private bool AnyTargetChipsMissingInFlippable(Dictionary<string, int> target, Dictionary<string, int> flippable) {
		foreach (KeyValuePair<string, int> prefabPair in target) {
			if (!flippable.ContainsKey(prefabPair.Key)) {
				return true;
			} else if (flippable[prefabPair.Key] < prefabPair.Value) {
				return true;
			}
		}
		return false;
	}

	private bool CanChipsThatShouldBeCrushedBeCrushedWithTheRemainingStack(StackMeta flippableStack, StackMeta targetStack, Dictionary<string, int> target) {
		for (int i = 0; i < flippableStack.ChipCount(); i++) {
			ChipMeta chipMeta = flippableStack.GetChipMetaAt(i);
			if (chipMeta.IsCrushable 
					&& !CanBeCrushed(chipMeta, flippableStack.ChipCount()) 
					&& !target.ContainsKey(StringifyChipMeta(chipMeta))) {
				return false;
			}
		}
		return true;
	}

	private bool CanBeCrushed(ChipMeta chipMeta, int totalStackWeight) {
		if (chipMeta.IsCrushable) {
			return chipMeta.CrushWeight <= (totalStackWeight - chipMeta.Weight);
		}
		return false;
	}

	int CountChipsWithCrushWeight(int crushWeight, List<ChipMeta> chips) {
		int sum = 0; 
		foreach (ChipMeta meta in chips) {
			if (meta.CrushWeight == crushWeight) {
				sum++;
			}
		}
		return sum;
	}

	Dictionary<string, int> MapByIdAndCrushWeight() {
		Dictionary<string, int> dict = new Dictionary<string, int>();
		for (int i = 0; i < ChipCount(); i++) {
			string key = StringifyChipMeta(GetChipMetaAt(i));
			if (!dict.ContainsKey(key)) {
				dict.Add(key, 0);
			}
			dict[key]++;
		}
		return dict;
	}

	string StringifyChipMeta(ChipMeta meta) {
		return string.Format("{0}#{1}", meta.prefabId, meta.CrushWeight);
	}

	Dictionary<int, int> GetPrefabIdCount() {
		Dictionary<int, int> dict = new Dictionary<int, int> ();
		for (int i = 0; i < ChipCount (); i++) {
			int prefabId = GetChipMetaAt (i).prefabId;
			if (!dict.ContainsKey(prefabId)) {
				dict.Add (prefabId, 0);
			}
			dict [prefabId] = dict[prefabId] + 1;
		}
		return dict;
	}
		
	public bool Matches(StackMeta other) {
		if (other.chips.Count != chips.Count) {
			return false;
		}

		for (int i = 0; i < chips.Count; i++) {
			ChipMeta thisChip = chips [i];
			ChipMeta otherChip = other.chips [i];
			if (thisChip.prefabId != otherChip.prefabId) {
				return false;
			}
			if (thisChip.isOrientationImportant && thisChip.orientation != otherChip.orientation) {
				return false;
			}
			if (thisChip.CrushWeight != otherChip.CrushWeight) {
				return false;
			}
		}

		return true;
	}

	public string ToStringShort() {
		string output = "size: " + ChipCount() + ", isTarget: " + isTargetStack + ", chips: [";
		for (int i = 0; i < chips.Count; i++) {
			ChipMeta chip = chips [i];
			output += "\n" + chip.ToStringShort ();
			if (i != chips.Count - 1) {
				output += ", ";
			}
		}
		output += "]";
		return output;
	}

	//Create a deep copy
	public StackMeta Copy() {
		StackMeta copy = new StackMeta ();
		copy.isTargetStack = isTargetStack;
		foreach (ChipMeta chipmeta in chips) {
			copy.chips.Add (chipmeta.Copy ());
		}
		return copy;
	}

	public void Permute(int[] flipPositions) {
		for (int i = 0; i < flipPositions.Length; i++) {
			FlipStackAt (flipPositions [i]);
		}
	}

	private bool ShouldCrushChip(int position) {
		if (position < 0 || chips.Count - 1 < position || !chips[position].IsCrushable) {
			return false;
		}
		
		int sumWeightAbove = 0;
		for (int i = chips.Count - 1; i > position; i--) {
			sumWeightAbove += chips[i].Weight;
		}

		return chips[position].CrushWeight <= sumWeightAbove;
	}

}
