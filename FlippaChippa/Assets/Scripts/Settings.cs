using System;

[Serializable]
public class Settings
{
	public Difficulty difficulty;
	public bool hasSetManually;

	public Settings() {
		difficulty = Difficulty.EASY;
		hasSetManually = false;
	}
}

