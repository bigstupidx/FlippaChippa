using System;
using UnityEngine;

public class TestManager
{
	public static int nSuccess = 0, nFailed = 0;


	public static void AssertEquals(int a, int b, string message) {
		if (a == b) {
			nSuccess++;
		} else {
			nFailed++;
			Debug.Log ("Failed! " + message);
		}
	}

	public static void AssertEquals(object a, object b, string message) {
		if (a == b) {
			nSuccess++;
		} else {
			nFailed++;
			Debug.Log ("Failed! " + message);
		}
	}

	public static void AssertTrue(bool b, string message) {
		if (b) {
			nSuccess++;
		} else {
			nFailed++;
			Debug.Log ("Failed! " + message);
		}
	}

	public static void AssertFalse(bool b, string message) {
		AssertTrue (!b, message);
	}
}

