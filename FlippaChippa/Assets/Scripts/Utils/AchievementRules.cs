using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;

public class AchievementRules
{
	public static bool IsLuckyDuckAccomplished(int flips) {
		return flips == 1;
	}

	public static bool IsSpeedsterAccomplished(float time) {
		return time < 10.0f;
	}

	public static bool IsBrainiacAccomplished(int stackSize) {
		return stackSize >= 13;
	}

	public static bool IsEideticMemoryAccomplished(int checks, int stackSize) {
		return checks == 1 && stackSize >= 8;
	}

	public static void HandleAchievements(SingleGameStatsMeta statsMeta, StackMeta stackMeta) {

		PlayGamesPlatform.Instance.IncrementAchievement (GPGSIds.achievement_newbie, 1, (bool success) => {
			if (success) {
				Debug.Log("Successfully updated the newbie-achievement");
			} else {
				//Store locally
			}
		});

		PlayGamesPlatform.Instance.IncrementAchievement (GPGSIds.achievement_fanish, 1, (bool success) => {
			if (success) {
				Debug.Log("Successfully updated the fanish-achievement");
			} else {
				//Store locally
			}
		});

		PlayGamesPlatform.Instance.IncrementAchievement (GPGSIds.achievement_mega_fan, 1, (bool success) => {
			if (success) {
				Debug.Log("Successfully updated the mega fan-achievement");
			} else {
				//Store locally
			}
		});

		if (AchievementRules.IsLuckyDuckAccomplished (statsMeta.NFlips)) {
			Social.ReportProgress (GPGSIds.achievement_lucky_duck, 100.0f, (bool success) => {
				if (success) {
					Debug.Log("Successfully set the lucky duck achievement");
				} else {
					//Store locally
				}
			});
		}

		if (AchievementRules.IsSpeedsterAccomplished (statsMeta.Time)) {
			Social.ReportProgress (GPGSIds.achievement_speedster, 100.0f, (bool success) => {
				if (success) {
					Debug.Log("Successfully set the lucky duck achievement");
				} else {
					//Store locally
				}
			});
		}

		if (AchievementRules.IsBrainiacAccomplished(stackMeta.ChipCount())) {
			Social.ReportProgress (GPGSIds.achievement_brainiac, 100.0f, (bool success) => {
				if (success) {
					Debug.Log("Successfully set the brainiac achievement");
				} else {
					//Store locally
				}
			});
		}

		if (AchievementRules.IsEideticMemoryAccomplished (statsMeta.NTargetChecks, stackMeta.ChipCount ())) {
			Social.ReportProgress (GPGSIds.achievement_eidetic_memory, 100.0f, (bool success) =>{
				if (success) {
					Debug.Log("Successfully sett the eidetic memory achievement"); 
				} else {
					//Store locally
				}
			});
		}
	}
}

