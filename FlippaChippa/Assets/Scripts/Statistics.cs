﻿using System;
using UnityEngine;

[Serializable]
public class Statistics
{
	public int[] flipsStats;
	public int[] targetChecks;
	public float[] times;

	public Statistics ()
	{
		flipsStats = new int[20];
		targetChecks = new int[20];
		times = new float[20];
	}
		
	public string ToString() {
		return "flips: " + ArrayUtils.Stringify (flipsStats) + "\n"
		+ "target checks: " + ArrayUtils.Stringify (targetChecks) + "\n"
		+ "time: " + ArrayUtils.StringifyFloat (times);
	}

	public void AddTargetChecks(int checks) {
		targetChecks = ArrayUtils.Add (checks, targetChecks);
	}

	public int TotalTargetChecks() {
		return ArrayUtils.Sum (targetChecks);
	}

	public int TargetChecksPerGame() {
		int totalNonZeroValues = ArrayUtils.TotalNonZeroValues (targetChecks);
		if (totalNonZeroValues == 0) {
			return 0;
		}
		return ArrayUtils.Sum (targetChecks) / totalNonZeroValues;
	}

	public void AddFlips(int flips) {
		flipsStats = ArrayUtils.Add (flips, flipsStats);
	}

	public int TotalGamesPlayed() {
		return ArrayUtils.TotalNonZeroValues (flipsStats);
	}

	public void AddTime(float time) {
		times = ArrayUtils.AddFloat (time, times);
	}

	public float TotalTime() {
		return ArrayUtils.SumFloat (times);
	}

	public float TimePerGame() {
		int totalNonZeroValues = ArrayUtils.TotalNonZeroValuesFloat (times);
		if (totalNonZeroValues == 0) {
			return 0;
		}
		return ArrayUtils.SumFloat (times) / totalNonZeroValues;
	}

	public int TotalFlips() {
		return ArrayUtils.Sum (flipsStats);
	}

	public float CalcAverageFlipsPerGame() {
		int totalFlips = TotalFlips ();
		if (totalFlips == 0) {
			return 0;
		}
		int totalGames = TotalGamesPlayed ();
		return 1f * totalFlips / totalGames;
	}
}