using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FlipsCalulator
{
	class PointClass : IComparable<PointClass> {
		public int point, index;
		public PointClass(int index, int points) {
			this.point = points;
			this.index = index;
		}

		#region IComparable implementation
		public int CompareTo (PointClass other)
		{
			return other.point.CompareTo (point);
		}
		#endregion
	}

	public static int CalculateMinFlips(StackMeta start, StackMeta target, int maxToKeep, int maxFlips) 
	{
		int permutationCounter = 1;
		List<StackMeta> toKeep = new List<StackMeta> ();
		toKeep.Add (start);
		while (permutationCounter < maxFlips) {
			List<StackMeta> permuteAll = PermuteStacks (toKeep);
			List<PointClass> points = AwardPoints (permuteAll, target);
			toKeep = FilterOut (permuteAll, points, maxToKeep);
			for (int i = 0; i < toKeep.Count; i++) {
				if (toKeep [i].Matches (target)) {
					return permutationCounter;
				}
			}
			permutationCounter++;
		}
		return permutationCounter;
	}

	private static List<StackMeta> PermuteStacks(List<StackMeta> stacks) {
		List<StackMeta> output = new List<StackMeta> ();
		foreach (StackMeta meta in stacks) {
			output.AddRange (PermuteStack (meta));
		}
		return output;
	}

	private static List<StackMeta> FilterOut(List<StackMeta> permutations, List<PointClass> points, int maxToKeep) {
		points.Sort ();
		List<StackMeta> toKeep = new List<StackMeta> ();
		for (int i = 0; i < Math.Min(maxToKeep, permutations.Count); i++) {
			toKeep.Add (permutations [points [i].index]);
		}
		return toKeep;
	}

	private static List<StackMeta> PermuteStack(StackMeta stack) {
		List<StackMeta> permutations = new List<StackMeta> ();
		for (int i = 0; i < stack.ChipCount(); i++) {
			StackMeta copy = stack.Copy ();
			copy.FlipStackAt (i);
			permutations.Add (copy);
		}
		return permutations;
	}

	private static List<PointClass> AwardPoints(List<StackMeta> permutations, StackMeta target) {
		List<PointClass> points = new List<PointClass> ();
		for (int i = 0; i < permutations.Count; i++) {
			StackMeta permutation = permutations [i];
			points.Add (new PointClass(i, AwardPoints (permutation, target)));
		}
		return points;
	}

	private static int AwardPoints(StackMeta permutation, StackMeta target) {
		int pointCompletelyCorrect = 3;
		int pointCorrectPos = 1;
		int pointsAwarded = 0;
		if (permutation.ChipCount() < target.ChipCount()) {
			return pointsAwarded;
		}
		for (int i = 0; i < target.ChipCount(); i++) {
			ChipMeta targetMeta = target.GetChipMetaAt (i);
			ChipMeta permutationMeta = permutation.GetChipMetaAt (i);
			if (targetMeta.prefabId == permutationMeta.prefabId) {
				if (targetMeta.orientation == permutationMeta.orientation) {
					pointsAwarded += pointCompletelyCorrect;
				} else {
					pointsAwarded += pointCorrectPos;
				}
			}
		}
		return pointsAwarded;
	}

}

