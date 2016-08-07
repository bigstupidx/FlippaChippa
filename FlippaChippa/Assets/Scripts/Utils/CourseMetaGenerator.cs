using UnityEngine;

public class CourseMetaGenerator
{
	public static CourseMeta Generate(PrefabsManager manager) {
		int size = Random.Range (4, 15);
		int flips = Random.Range (size, (int) (size * 1.5f));
		int flips2 = Random.Range (size, (int) (size * 1.5f));
		return Generate (size, flips, flips2, manager);
	}

	public static CourseMeta Generate(int size, int flips, int flips2, PrefabsManager manager) {
		int[] chipIds = GenerateNonIdenticalChipIDs (size, manager);
		int[] startFlips = new int[flips];
		for (int i = 0; i < size; i++) {
			startFlips [i] = Random.Range (0, size);
		}
		int[] targetFlips = new int[flips2];
		for (int i = 0; i < size; i++) {
			targetFlips [i] = Random.Range (0, size);
		}
		CourseMeta meta = new CourseMeta (chipIds, startFlips, targetFlips);
		return meta;
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
}