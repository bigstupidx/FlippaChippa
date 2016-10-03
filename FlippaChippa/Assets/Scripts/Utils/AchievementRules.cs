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

	public static bool IsEideticMemoryAccomplished(bool successfull, int checks, int stackSize) {
		return successfull && checks == 1 && stackSize >= 8;
	}

	public static void HandleAchievements(SingleGameStatsMeta statsMeta, StackMeta stackMeta) {
		if (ApplicationModel.achievements == null) {
			Debug.Log ("The achievements hasn't been retrieved. This indicated that the player hasn't signed in using google play games services");
			return;
		}

		IAchievement newbie = ApplicationModel.GetAchievement (GPGSIds.achievement_newbie);
		if (!newbie.completed) {
			PlayGamesPlatform.Instance.IncrementAchievement (GPGSIds.achievement_newbie, 1, (bool success) => {
				if (success) {
					Debug.Log ("Successfully updated the newbie-achievement");
				} else {
					//Store locally
				}
			});
		}

		IAchievement fanish = ApplicationModel.GetAchievement (GPGSIds.achievement_fanish);
		if (!fanish.completed) {
			PlayGamesPlatform.Instance.IncrementAchievement (GPGSIds.achievement_fanish, 1, (bool success) => {
				if (success) {
					Debug.Log ("Successfully updated the fanish-achievement");
				} else {
					//Store locally
				}
			});
		}

		IAchievement megaFan = ApplicationModel.GetAchievement (GPGSIds.achievement_mega_fan);
		if (!megaFan.completed) {
			PlayGamesPlatform.Instance.IncrementAchievement (GPGSIds.achievement_mega_fan, 1, (bool success) => {
				if (success) {
					Debug.Log ("Successfully updated the mega fan-achievement");
				} else {
					//Store locally
				}
			});
		}

		IAchievement luckyDuck = ApplicationModel.GetAchievement (GPGSIds.achievement_lucky_duck);
		if (!luckyDuck.completed && AchievementRules.IsLuckyDuckAccomplished (statsMeta.NFlips)) {
			Social.ReportProgress (GPGSIds.achievement_lucky_duck, 100.0f, (bool success) => {
				if (success) {
					Debug.Log ("Successfully set the lucky duck achievement");
				} else {
					//Store locally
				}
			});
		}

		IAchievement speedster = ApplicationModel.GetAchievement (GPGSIds.achievement_speedster);
		if (!speedster.completed && AchievementRules.IsSpeedsterAccomplished (statsMeta.Time)) {
			Social.ReportProgress (GPGSIds.achievement_speedster, 100.0f, (bool success) => {
				if (success) {
					Debug.Log("Successfully set the lucky duck achievement");
				} else {
					//Store locally
				}
			});
		}

		IAchievement brainiac = ApplicationModel.GetAchievement (GPGSIds.achievement_brainiac);
		if (!brainiac.completed && AchievementRules.IsBrainiacAccomplished(stackMeta.ChipCount())) {
			Social.ReportProgress (GPGSIds.achievement_brainiac, 100.0f, (bool success) => {
				if (success) {
					Debug.Log("Successfully set the brainiac achievement");
				} else {
					//Store locally
				}
			});
		}

		IAchievement eideticMemory = ApplicationModel.GetAchievement (GPGSIds.achievement_eidetic_memory);
		if (!eideticMemory.completed && IsEideticMemoryAccomplished (statsMeta.SuccessfullGame, statsMeta.NTargetChecks, stackMeta.ChipCount ())) {
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

