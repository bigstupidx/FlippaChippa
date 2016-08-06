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
		
	public void AddFlips(int flips) {
		bool addedFlip = false;
		for (int i = 0; i < flipsStats.Length; i++) {
			if (flipsStats [i] == 0) {
				flipsStats [i] = flips;
				addedFlip = true;
				break;
			}
		}

		if (!addedFlip) {	//Happens if the array is full, 
			int[] old = flipsStats;
			flipsStats = new int[old.Length + 20];
			for (int i = 0; i < old.Length; i++) {
				flipsStats [i] = old [i];
			}
			flipsStats [old.Length] = flips;
		}
	}

	public string ToString() {
		string output = "[" + flipsStats[0];
		for (int i = 1; i < flipsStats.Length; i++) {
			output += ", " + flipsStats [i];
		}
		output += "]";
		return output;
	}

	public int TotalGamesPlayed() {
		int sum = 0;
		for (int i = 0; i < flipsStats.Length; i++) {
			if (flipsStats [i] != 0) {
				sum++;
			} else {
				break;
			}
		}
		return sum;
	}

	public int TotalFlips() {
		int sum = 0;
		for (int i = 0; i < flipsStats.Length; i++) {
			if (flipsStats [i] != 0) {
				sum += flipsStats [i];
			} else {
				break;
			}
		}
		return sum;
	}

	public float CalcAverageFlipsPerGame() {
		int totalGames = TotalGamesPlayed ();
		int totalFlips = TotalFlips ();
		return 1f * totalFlips / totalGames;
	}
}