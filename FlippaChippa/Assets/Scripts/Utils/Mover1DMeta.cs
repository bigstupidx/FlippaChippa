using System;
using UnityEngine;

public class Mover1DMeta
{
	private float startPos, endPos;

	private float animationTime = 1f, halfTime;
	private float elapsedTime;

	private bool isFinished = false;
	public bool IsFinished { get { return isFinished; } }

	private Transform transform;

	public Mover1DMeta(float startPos, float endPos, float animationTime) {
		this.startPos = startPos;
		this.endPos = endPos;
		this.animationTime = animationTime;
	}

	/**
	 * Returns the expected position after a specific time without actually altering the position
	 */
	public float UpdatePos(float dt) {
		elapsedTime += dt;
		if (elapsedTime >= animationTime) {
			isFinished = true;
			return endPos;
		}
		float relativeDistance = MJMath.LerpPyramid(Mathf.Clamp01(elapsedTime / animationTime));
		return startPos + relativeDistance * (endPos - startPos);
	}

}

