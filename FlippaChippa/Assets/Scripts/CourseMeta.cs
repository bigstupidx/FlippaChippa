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

	private int[] crushWeights;
	public int[] CrushWeights { get { return crushWeights; } }

	public CourseMeta(int[] chipIDs, bool[] initFlips, int[] flips) : this(chipIDs, new int[chipIDs.Length], initFlips, flips) {
	}

	public CourseMeta(int[] chipIDs, int[] crushWeights, bool[] initFlips, int[] flips) {
		if (chipIDs == null || crushWeights == null || initFlips == null || flips == null) {
			throw new Exception ("None of the input fields for CourseMeta can be null");
		}

		if (crushWeights.Length != chipIDs.Length)   {
			throw new Exception (string.Format("The number of crushweights ({0}) must equal the number of chips ({1}) ", crushWeights.Length, chipIDs.Length));
		} else if (initFlips.Length != chipIDs.Length) {
			throw new Exception (string.Format("The number of initFlips ({0}) must equal the number of chips ({1}) ", initFlips.Length, chipIDs.Length));
		}

		this.chipIDs = chipIDs;
		this.crushWeights = crushWeights;
		this.initFlips = initFlips;
		this.flips = flips;
	}

	public static CourseMeta Default() {
		return courseMeta;
	}
}