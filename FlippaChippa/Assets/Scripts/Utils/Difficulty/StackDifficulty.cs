using System.Collections;

public class StackDifficulty
{
	private int minChips;
	public int MinCips { get { return minChips; } }

	private int maxChips;
	public int MaxChips { get { return maxChips; } }

	private bool allowCrushable;
	public bool AllowCrushable { get { return allowCrushable; } }

	private Difficulty difficulty;
	public Difficulty Difficulty { get { return difficulty; } }

	public StackDifficulty(Difficulty difficulty, int minChips, int maxChips, bool allowCrushable) {
		this.difficulty = difficulty;
		this.minChips = minChips;
		this.maxChips = maxChips;
		this.allowCrushable = allowCrushable;
	}

	private static StackDifficulty easy;

	public static StackDifficulty Easy {
		get {
			if (easy == null) {
				easy = new StackDifficulty (Difficulty.EASY, 4, 6, false);
			}
			return easy;
		}
	}

	private static StackDifficulty normal;
	public static StackDifficulty Normal {
		get {
			if (normal == null) {
				normal = new StackDifficulty (Difficulty.NORMAL, 4, 10, false);
			}
			return normal;
		}
	}

	private static StackDifficulty hard;
	public static StackDifficulty Hard {
		get {
			if (hard == null) {
				hard = new StackDifficulty (Difficulty.HARD, 4, 13, true);
			}
			return hard;
		}
	}

	public static StackDifficulty Get(Difficulty difficulty) {
		if (difficulty == Difficulty.EASY) {
			return Easy;
		} else if (difficulty == Difficulty.NORMAL) {
			return Normal;
		} else {
			return Hard;
		}
	}

}

