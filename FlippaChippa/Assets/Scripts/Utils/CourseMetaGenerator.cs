using UnityEngine;

public class CourseMetaGenerator
{
	public static CourseMeta Generate(PrefabsManager manager) {
		int size = Random.Range (3, 15);
		int flips = Random.Range (size, size * 2);
		int flips2 = Random.Range (size, size * 2);
		return Generate (size, flips, flips2, manager);
	}

	public static CourseMeta Generate(int size, int flips, int flips2, PrefabsManager manager) {
		int[] chipIds = new int[size];
		for (int i = 0; i < size; i++) {
			chipIds [i] = manager.GetRandomChipId ();
		}
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
}