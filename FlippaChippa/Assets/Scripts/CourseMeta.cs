using System;

public struct CourseMeta
{
	private static CourseMeta courseMeta = new CourseMeta (new int[]{ 0, 0, 0 }, new int[]{ 0, 0, 0 }, new int[]{ 0, 0, 0 });

	private int[] chipIDs;
	public int[] ChipIDs { get { return chipIDs; } }

	private int[] startFlips;
	public int[] StartFlips { get { return startFlips; } }

	private int[] targetFlips;
	public int[] TargetFlips { get { return targetFlips; } }

	public CourseMeta(int[] chipIDs, int[] startFlips, int[] targetFlips) {
		this.chipIDs = chipIDs;
		this.startFlips = startFlips;
		this.targetFlips = targetFlips;
	}

	public static CourseMeta Default() {
		return courseMeta;
	}
}