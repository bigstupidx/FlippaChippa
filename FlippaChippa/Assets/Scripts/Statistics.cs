using System;
using UnityEngine;

[Serializable]
public class Statistics
{
	public int[] flipsStats;
	public int[] targetChecks;

	public Statistics ()
	{
		flipsStats = new int[20];
		targetChecks = new int[20];
	}
		
	public string ToString() {
		return "flips: " + ArrayUtils.Stringify(flipsStats) + "\n"
		+ "target checks: " + ArrayUtils.Stringify(targetChecks);
	}

	public void AddTargetChecks(int checks) {
		targetChecks = ArrayUtils.Add (checks, targetChecks);
	}

	public int TotalTargetChecks() {
		return ArrayUtils.Sum (targetChecks);
	}

	public void AddFlips(int flips) {
		flipsStats = ArrayUtils.Add (flips, flipsStats);
	}

	public int TotalGamesPlayed() {
		return ArrayUtils.TotalNonZeroValues (flipsStats);
	}

	public int TotalFlips() {
		return ArrayUtils.Sum (flipsStats);
	}

	public float CalcAverageFlipsPerGame() {
		int totalGames = TotalGamesPlayed ();
		int totalFlips = TotalFlips ();
		return 1f * totalFlips / totalGames;
	}
}