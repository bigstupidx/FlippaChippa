using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;

public class AchievementRules
{
	public static bool IsLuckyDuckAccomplished(bool isSuccessfull, int flips) {
		return isSuccessfull && flips == 1;
	}

	public static bool IsSpeedsterAccomplished(bool isSuccessfull, float time) {
		return isSuccessfull && time < 10.0f;
	}

	public static bool IsBrainiacAccomplished(bool isSuccessfull, int stackSize) {
		return isSuccessfull && stackSize >= 13;
	}

	public static bool IsEideticMemoryAccomplished(bool successfull, int checks, int stackSize) {
		return successfull && checks == 1 && stackSize >= 8;
	}

	public static bool IsCloseCallAccomplished(bool successfull, int flipsLeft) {
		return successfull && flipsLeft == 0;
	}

	public static bool IsCompletionStreakAccomplished(int streak) {
		return 5 <= streak;
	}

	public static void HandleAchievements(SingleGameStatsMeta statsMeta, StackMeta stackMeta) {
		if (ApplicationModel.achievements == null) {
			Debug.Log ("<color=blue>The achievements hasn't been retrieved. This indicated that the player hasn't signed in using google play games services</color>");
			return;
		}
		#if UNITY_ANDROID || UNITY_IOS
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
		if (!luckyDuck.completed && AchievementRules.IsLuckyDuckAccomplished (statsMeta.SuccessfullGame, statsMeta.NFlips)) {
			Social.ReportProgress (GPGSIds.achievement_lucky_duck, 100.0f, (bool success) => {
				if (success) {
					Debug.Log ("Successfully set the lucky duck achievement");
				} else {
					//Store locally
				}
			});
		}

		IAchievement speedster = ApplicationModel.GetAchievement (GPGSIds.achievement_speedster);
		if (!speedster.completed && AchievementRules.IsSpeedsterAccomplished (statsMeta.SuccessfullGame, statsMeta.Time)) {
			Social.ReportProgress (GPGSIds.achievement_speedster, 100.0f, (bool success) => {
				if (success) {
					Debug.Log("Successfully set the lucky duck achievement");
				} else {
					//Store locally
				}
			});
		}

		IAchievement brainiac = ApplicationModel.GetAchievement (GPGSIds.achievement_brainiac);
		if (!brainiac.completed && AchievementRules.IsBrainiacAccomplished(statsMeta.SuccessfullGame, stackMeta.ChipCount())) {
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

		IAchievement closeCall = ApplicationModel.GetAchievement(GPGSIds.achievement_close_call);
		if (!closeCall.completed && IsCloseCallAccomplished(statsMeta.SuccessfullGame, statsMeta.MaxFlips -  statsMeta.NFlips)) {
			Social.ReportProgress (GPGSIds.achievement_close_call, 100f, (bool success) => {
				if (success) {
					Debug.Log("Successfully set the close call achievement");
				}
			});
		}

		IAchievement completionStreak = ApplicationModel.GetAchievement(GPGSIds.achievement_completion_streak);
		if (!completionStreak.completed && IsCompletionStreakAccomplished(ApplicationModel.statistics.CurrentStreakCount)) {
			Social.ReportProgress(GPGSIds.achievement_completion_streak, 100f, (bool success) => {
				if (success) {
					Debug.Log("Successfully set the completion streak achievement");
				}
			});
		}
		#endif
	}
}

