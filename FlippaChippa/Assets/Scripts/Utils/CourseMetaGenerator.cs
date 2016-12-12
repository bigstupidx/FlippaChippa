using UnityEngine;

public class CourseMetaGenerator
{
	
//	public static CourseMeta Generate(PrefabsManager manager) {
//		return Generate (manager, Difficulty.HARD);
//	}

	public static CourseMeta Generate(PrefabsManager manager, Difficulty difficulty) {
		Debug.Log(string.Format("<color=green>{0}</color>", difficulty));
		StackDifficulty diff = StackDifficulty.Get (difficulty);
		int size = Random.Range (diff.MinCips, diff.MaxChips + 1);
		int initFlips = Random.Range (size, (int)(size * 1.5f));
		int flips = Random.Range (size, (int)(size * 1.5f));
		return Generate (size, initFlips, flips, diff.AllowCrushable, manager);
	}

	public static CourseMeta Generate(int size, int nInitFlips, int nFlips, bool isCrushable, PrefabsManager manager) {
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
			chipIDs = GenerateChipIDs (size, manager);
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
}