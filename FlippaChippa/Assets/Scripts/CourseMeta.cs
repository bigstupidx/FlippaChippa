using System;

public struct CourseMeta
{
	private static CourseMeta courseMeta = new CourseMeta (new int[]{ 0, 0, 0 }, new bool[]{ false, false, false }, new int[]{ 0, 0, 0 });

	private int[] chipIDs;
	public int[] ChipIDs { get { return chipIDs; } }

	private bool[] initFlips;
	public bool[] InitFlips { get { return initFlips; }}

	private int[] flips;
	public int[] Flips { get { return flips; } }

	public CourseMeta(int[] chipIDs, bool[] initFlips, int[] flips) {
		this.chipIDs = chipIDs;
		this.initFlips = initFlips;
		this.flips = flips;
	}

	public static CourseMeta Default() {
		return courseMeta;
	}
}