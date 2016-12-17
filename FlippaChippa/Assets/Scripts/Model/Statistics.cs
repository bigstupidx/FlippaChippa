using System;
using UnityEngine;

[Serializable]
public class Statistics
{
	//To be stored between app-session
	public int totalFlips;
	public int totalTargetChecks;
	public float totalTime;
	public int totalSuccessfullGames;
	public int totalFailedGames;
	public int bestStreakCount;

	//Per app-session
	private int currentStreakCount;

	public Statistics ()
	{
	}
		
	public override string ToString() {
		//return "Total games: " + TotalGames() + ", Total flips: " + totalFlips + ", Total Target Checks: " + totalTargetChecks + ", Total Time: " + totalTime + ", Best streak: " + bestStreakCount;
		return string.Format("Total games: {0}, Total flips: {1}, Total Target Checks: {2}, Total Time: {3}, Best Streak: {4}"
			, TotalGames()
			, totalFlips
			, totalTargetChecks
			, totalTime
			, bestStreakCount
		);
	}

	public int CurrentStreakCount { get { return currentStreakCount; } }

	public void AbortStreak() {
		Debug.LogFormat ("<color=green>Aborting the streak that was {0}</color>", currentStreakCount);
		currentStreakCount = 0;
	}

	public int IncreaseStreakCount() {
		currentStreakCount++;
		Debug.LogFormat ("<color=green>Current streak is {0}</color>", currentStreakCount);
		if (currentStreakCount > bestStreakCount) {
			bestStreakCount = currentStreakCount;
			Debug.LogFormat ("<color=yellow>New best streak count record is {0}</color>", bestStreakCount);
		}
		return currentStreakCount;
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

	public int BestStreakCount() { 
		return bestStreakCount; 
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