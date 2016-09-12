using System;
using UnityEngine;

[Serializable]
public class Statistics
{
	public int totalGames;
	public int totalFlips;
	public int totalTargetChecks;
	public float totalTime;

	public Statistics ()
	{
	}
		
	public string ToString() {
		return "Total games: " + totalGames + ", Total flips: " + totalFlips + ", Total Target Checks: " + totalTargetChecks + ", Total Time: " + totalTime;
	}

	public void RegisterCompletedGame(SingleGameStatsMeta stats) {
		totalGames++;
		totalFlips += stats.NFlips;
		totalTargetChecks += stats.NTargetChecks;
		totalTime += stats.Time;
	}

	public int TotalGames() { return totalGames; }

	public int TotalChecks() { return totalGames; }

	public int TotalFlips() { return totalFlips; }

	public float TotalTime() { return totalTime; }

	public int AverageFlips() {
		if (totalGames == 0) {
			return 0;
		}
		return totalFlips / totalGames;
	}

	public int AverageTargetChecks() {
		if (totalGames == 0) {
			return 0;
		}
		return totalTargetChecks / totalGames;
	}

	public float AverageTime() {
		if (totalGames == 0) {
			return 0;
		}
		return totalTime / totalGames;
	}
		
}