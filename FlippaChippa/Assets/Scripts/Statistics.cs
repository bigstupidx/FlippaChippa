using System;
using UnityEngine;

[Serializable]
public class Statistics
{
	public int totalFlips;
	public int totalTargetChecks;
	public float totalTime;
	public int totalSuccessfullGames;
	public int totalFailedGames;

	public Statistics ()
	{
	}
		
	public string ToString() {
		return "Total games: " + TotalGames() + ", Total flips: " + totalFlips + ", Total Target Checks: " + totalTargetChecks + ", Total Time: " + totalTime;
	}

	public void RegisterCompletedGame(SingleGameStatsMeta stats) {
		totalFlips += stats.NFlips;
		totalTargetChecks += stats.NTargetChecks;
		totalTime += stats.Time;
		if (stats.SuccessfullGame) {
			totalSuccessfullGames++;
		} else {
			totalFailedGames++;
		}
	}

	public int TotalGames() { return totalSuccessfullGames + totalFailedGames; }

	public int TotalSuccessfullGames() {
		return totalSuccessfullGames;
	}

	public int TotalFailedGames() {
		return totalFailedGames;
	}

	public int TotalChecks() { return totalTargetChecks; }

	public int TotalFlips() { return totalFlips; }

	public float TotalTime() { return totalTime; }

	public int AverageFlips() {
		if (TotalGames() == 0) {
			return 0;
		}
		return totalFlips / TotalGames();
	}

	public int AverageTargetChecks() {
		if (TotalGames() == 0) {
			return 0;
		}
		return totalTargetChecks / TotalGames();
	}

	public float AverageTime() {
		if (TotalGames() == 0) {
			return 0;
		}
		return totalTime / TotalGames();
	}
		
}