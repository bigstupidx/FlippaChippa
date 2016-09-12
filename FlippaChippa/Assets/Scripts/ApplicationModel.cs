using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class ApplicationModel {

	public static int courseLoadedId = 0;
	public static bool completedCourse = false;
	public static CourseMeta courseMeta = CourseMeta.Default();
	public static Statistics statistics;
	public static IAchievement[] achievements = null;

	public static IAchievement GetAchievement(string id) {
		for (int i = 0; i < achievements.Length; i++) {
			if (achievements [i].id.Equals (id)) {
				return achievements [i];
			}
		}
		return null;
	}

}
