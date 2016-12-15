using UnityEngine;
using System.Collections.Generic;

public class CourseMetaGenerator
{

	public static StackMetaPair GenerateStackMetaPair(PrefabsManager manager, Difficulty difficulty) {
		Debug.Log(string.Format("<color=green>{0}</color>", difficulty));
		StackDifficulty diff = StackDifficulty.Get (difficulty);
		int size = Random.Range (diff.MinCips, diff.MaxChips + 1);
		int initFlips = Random.Range (size, (int)(size * 1.5f));
		int flips = Random.Range (size, (int)(size * 1.5f));
		CourseMeta meta =  GenerateCourseMeta (size, initFlips, flips, diff.AllowCrushable, manager);
		StackMetaPair pair = GenerateStackMetaPair (meta, manager);
		while (pair.start.Matches(pair.target)) {
			Debug.Log ("<color=red>Need to generate another stack since the target matches the start.</color>");
			meta = GenerateCourseMeta (size, initFlips, flips, diff.AllowCrushable, manager);
			pair = GenerateStackMetaPair (meta, manager);
		}

		int nFlipsAlternative = FlipsCalulator.CalculateMinFlips (pair.start, pair.target, 100, pair.nFlips);
		Debug.Log (string.Format("Calculated flips ({0}). Generator flips ({1})", nFlipsAlternative, pair.nFlips));
		if (nFlipsAlternative < pair.nFlips) {
			pair.nFlips = nFlipsAlternative;
		}

		return pair;
	}

	public static StackMetaPair CreateFromCourseMeta(CourseMeta courseMeta, PrefabsManager prefabsManager) {
		return GenerateStackMetaPair(courseMeta, prefabsManager);
	}

	public static CourseMeta GenerateCourseMeta(int size, int nInitFlips, int nFlips, bool isCrushable, PrefabsManager manager) {
		int[] chipIds = GenerateNonIdenticalChipIDs (size, manager);
		int[] crushWeights = new int[size];
		if (isCrushable) {
			for (int i = 0; i < size; i++) {
				crushWeights [i] = GenerateCrushWeight (size, size - i, 0.8f);
			}
		}
		bool[] initFlips = new bool[nInitFlips];
		for (int i = 0; i < initFlips.Length; i++) {
			initFlips [i] = Random.value < 0.5f ? true : false;
		}
		int[] flips = new int[nFlips];
		for (int i = 0; i < size; i++) {
			flips [i] = Random.Range (0, size);
		}
		return new CourseMeta (chipIds, crushWeights, initFlips, flips);
	}

	/**
	 * Generates a list of IDs guaranteeing that at least two different IDs exist as long
	 * as the prefabs manager contains at least two different IDs
	 */ 
	private static int[] GenerateNonIdenticalChipIDs(int size, PrefabsManager manager) {
		if (manager.chipPrefabs.Length < 2) {
			throw new UnityException ("Not enough chip prefabs to generate a non-identical stack");
		}

		int[] chipIDs = GenerateChipIDs (size, manager);
		while (AllValuesAreTheSame (chipIDs)) {
			chipIDs [0] = manager.GetRandomChipId ();
		}
		return chipIDs;
	}

	private static int[] GenerateChipIDs(int size, PrefabsManager manager) {
		int[] chipIDs = new int[size];
		for (int i = 0; i < size; i++) {
			chipIDs [i] = manager.GetRandomChipId ();
		}
		return chipIDs;
	}

	/**
	 * Returns false if the list doesn't have any elements or is null
	 */ 
	private static bool AllValuesAreTheSame(int[] array) {
		if (array == null || array.Length == 0) {
			return false;
		}

		int value = array [0];
		for (int i = 0; i < array.Length; i++) {
			if (value != array [i]) {
				return false;
			}
		}
		return true;
	}

	private static int GenerateCrushWeight(int maxCrushWeight, int minCrushWeight, float crushWeightThreshold) {
		float randomValue = Random.value;
		if (randomValue > crushWeightThreshold) {
			return Random.Range (minCrushWeight, maxCrushWeight);
		}
		return 0;
	}

	private static StackMetaPair GenerateStackMetaPair(CourseMeta meta, PrefabsManager manager) {
		StackMeta startStack = new StackMeta ();
		for (int i = 0; i < meta.ChipIDs.Length; i++) {
			ChipMeta chipMeta = manager.GetChipMeta (meta.ChipIDs [i]);
			chipMeta.CrushWeight = meta.CrushWeights [i];
			if (meta.InitFlips [i]) {
				chipMeta.Flip ();
			}
			startStack.Add (chipMeta);
		}
		startStack.CleanupStackForCrushedChips (0);

		StackMeta targetStack = startStack.Copy ();
		int flips = Permute (targetStack, meta.Flips);

		return new StackMetaPair (meta, startStack, targetStack, flips);
	}

	private static int Permute(StackMeta stackMeta, int[] flips) {
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

	private static bool PermutationExists(StackMeta newPermutation, List<StackMeta> existingPermutations) {
		foreach (StackMeta permutation in existingPermutations) {
			if (newPermutation.Matches(permutation)) {
				return true;
			}
		}
		return false;
	}


}