using System;
using UnityEngine;

[Serializable]
public class Statistics
{
	public int[] flipsStats;

	public Statistics ()
	{
		flipsStats = new int[20];
	}
		
	public string ToString() {
		return ArrayUtils.Stringify (flipsStats);
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