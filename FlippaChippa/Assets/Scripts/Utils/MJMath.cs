using System;
using UnityEngine;


public class MJMath
{
	public static float LerpPyramid(float lerpAmount) {
		if (lerpAmount < 0.5f) {
			//y = a + 2x -> a=0
			//y = 2x -> x^2 -> 0.5*0.5 = 0.25
			return (lerpAmount * lerpAmount) / 0.5f;
		} else {
			//y = 1 - 2x -> 2x = 1 - y -> x = (1-y)/2 -> 0.5 - 0.5*y -> 0.5y - 0.25 * y^2
			float firstHalf = 0.25f;
			lerpAmount -= 0.5f;
			float progress = lerpAmount - lerpAmount * lerpAmount;
			return (firstHalf + progress) / 0.5f;
		}
	}

	public static float LerpExp(float amount) {
		return 1f - Mathf.Cos(amount * Mathf.PI * 0.5f);
	}
}


