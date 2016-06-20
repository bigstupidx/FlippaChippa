using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class StackMeta {

	public List<ChipMeta> chips;
	public bool isTargetStack;

	public StackMeta() {
		chips = new List<ChipMeta> ();
	}

	public void Add(ChipMeta chipMeta) {
		chips.Add (chipMeta);
		chipMeta.stackPos = chips.Count - 1;
	}

	public void Remove(int position) {
		chips.RemoveAt (position);

		if (position != chips.Count) {
			for (int i = position; i < chips.Count; i++) {
				ChipMeta chip = chips [i];
				chip.stackPos = i;
			}
		}
	}

	public void FlipStackAt(int position) {	//Doesn't actually flip the UI stack, only flips the the chips meta
		ReverseOrderAt(position, chips);
		for (int i = position; i < chips.Count; i++) {
			ChipMeta chip = chips [i];
			chip.Flip ();
			chip.stackPos = i;
		}
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
		}

		return true;
	}

	public string ToStringShort() {
		string output = "[";
		for (int i = 0; i < chips.Count; i++) {
			ChipMeta chip = chips [i];
			output += chip.ToStringShort ();
			if (i != chips.Count - 1) {
				output += ", ";
			}
		}
		output += "]";
		return output;
	}
	
}
