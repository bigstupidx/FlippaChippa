﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StackMetaTester : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		TestAddChip ();
		TestRemoveChip ();
		TestDemoStack ();
		TestFlipStack ();
		TestFlipStackJustTop ();
		TestFlipMiddle ();
		TestFlipAt3 ();
		TestMatchesOrientationNotImportant ();
		TestMatchesOrientationIsImportant ();
		Debug.Log ("All tests finished");
		Debug.Log("nSuccesses: " + TestManager.nSuccess + ", nFailed: " + TestManager.nFailed);
	}

	public void TestAddChip() {
		StackMeta stack = new StackMeta ();
		TestManager.AssertEquals (stack.ChipCount (), 0, "TestAddChip");
		stack.Add(new ChipMeta(11, ChipOrientation.UP, false, 12, 0.2f));
		TestManager.AssertEquals (stack.ChipCount (), 1, "TestAddChip");
		TestManager.AssertEquals (stack.GetChipMetaAt (0).stackPos, 0, "TestAddChip");
		stack.Add(new ChipMeta(21, ChipOrientation.UP, false, 122, 0.2f));
		TestManager.AssertEquals (stack.GetChipMetaAt (1).stackPos, 1, "TestAddChip");
	}

	public void TestRemoveChip() {
		StackMeta stack = GetStackMetaOf5 ();
		Debug.Log ("stack: " + stack.ToStringShort ());
		int initalCount = 5;
		ChipMeta chip = stack.GetChipMetaAt (initalCount - 1);
		stack.RemoveAt (stack.ChipCount () - 1);
		TestManager.AssertEquals (stack.ChipCount(), initalCount - 1, "TestRemoveChip");
		chip = stack.GetChipMetaAt(2);
		stack.RemoveAt (2);
		for (int i = 0; i < stack.ChipCount(); i++) {
			ChipMeta chipI = stack.GetChipMetaAt(i);
			stack.RemoveAt (i);
			TestManager.AssertEquals (i, chipI.stackPos, "TestRemoveChip stack pos is not the same");
		}
	}

	public void TestFlipStack() {
		Debug.Log ("Test flip stack start");
		StackMeta stack = GetStackMetaOf5();
		Debug.Log ("Before flip at 0: " + stack.ToStringShort ());
		stack.FlipStackAt (0);
		Debug.Log ("After flip at 0: " + stack.ToStringShort ());
		TestManager.AssertEquals (stack.GetChipMetaAt (0).prefabId, 15, "");
		TestManager.AssertEquals (stack.GetChipMetaAt (1).prefabId, 14, "");
		TestManager.AssertEquals (stack.GetChipMetaAt (2).prefabId, 13, "");
		TestManager.AssertEquals (stack.GetChipMetaAt (3).prefabId, 12, "");
		TestManager.AssertEquals (stack.GetChipMetaAt (4).prefabId, 11, "");

		TestManager.AssertTrue (stack.GetChipMetaAt (0).orientation == ChipOrientation.DOWN, "");
		TestManager.AssertTrue (stack.GetChipMetaAt (1).orientation == ChipOrientation.DOWN, "");
		TestManager.AssertTrue (stack.GetChipMetaAt (2).orientation == ChipOrientation.DOWN, "");
		TestManager.AssertTrue (stack.GetChipMetaAt (3).orientation == ChipOrientation.DOWN, "");
		TestManager.AssertTrue (stack.GetChipMetaAt (4).orientation == ChipOrientation.DOWN, "");

		TestManager.AssertEquals (stack.GetChipMetaAt (0).stackPos, 0, "");
		TestManager.AssertEquals (stack.GetChipMetaAt (1).stackPos, 1, "");
		TestManager.AssertEquals (stack.GetChipMetaAt (2).stackPos, 2, "");
		TestManager.AssertEquals (stack.GetChipMetaAt (3).stackPos, 3, "");
		TestManager.AssertEquals (stack.GetChipMetaAt (4).stackPos, 4, "");
	}

	public void TestFlipStackJustTop() {
		Debug.Log ("Test flip stack just top");
		StackMeta stack = GetStackMetaOf5 ();
		Debug.Log ("Before flip at top: " + stack.ToStringShort ());
		stack.FlipStackAt (stack.ChipCount () - 1);
		Debug.Log ("After flip at top: " + stack.ToStringShort ());
		TestManager.AssertTrue (stack.GetChipMetaAt (0).orientation == ChipOrientation.UP, "Wrong orientation for chip");
		TestManager.AssertTrue (stack.GetChipMetaAt (1).orientation == ChipOrientation.UP, "Wrong orientation for chip");
		TestManager.AssertTrue (stack.GetChipMetaAt (2).orientation == ChipOrientation.UP, "Wrong orientation for chip");
		TestManager.AssertTrue (stack.GetChipMetaAt (3).orientation == ChipOrientation.UP, "Wrong orientation for chip");

		TestManager.AssertTrue (stack.GetChipMetaAt (4).orientation == ChipOrientation.DOWN, "Wrong orientation for chip");
		TestManager.AssertEquals (stack.GetChipMetaAt (4).stackPos, 4, "Wrong stackpos for chip");
		TestManager.AssertEquals (stack.GetChipMetaAt (4).prefabId, 15, "Wrong prefabid");
	}

	public void TestFlipMiddle() {
		Debug.Log ("Test flip middle");
		StackMeta stack = GetStackMetaOf5 ();

		Debug.Log ("Before flip at top: " + stack.ToStringShort ());
		stack.FlipStackAt (1);
		Debug.Log ("After flip at top: " + stack.ToStringShort ());

		TestManager.AssertTrue (stack.GetChipMetaAt (0).prefabId == 11, "Wrong prefabid");
		TestManager.AssertTrue (stack.GetChipMetaAt (0).orientation == ChipOrientation.UP, "Wrong orientation");
		TestManager.AssertTrue (stack.GetChipMetaAt (0).isOrientationImportant == false, "Orientation shouldn't be important");
		TestManager.AssertTrue (stack.GetChipMetaAt (0).stackPos == 0, "Wrong stackpos");

		TestManager.AssertTrue (stack.GetChipMetaAt (1).prefabId == 15, "Wrong prefabid");
		TestManager.AssertTrue (stack.GetChipMetaAt (1).orientation == ChipOrientation.DOWN, "Wrong orientation");
		TestManager.AssertTrue (stack.GetChipMetaAt (1).isOrientationImportant == false, "Orientation shouldn't be important");
		TestManager.AssertTrue (stack.GetChipMetaAt (1).stackPos == 1, "Wrong stackpos");

		TestManager.AssertTrue (stack.GetChipMetaAt (2).prefabId == 14, "Wrong prefabid");
		TestManager.AssertTrue (stack.GetChipMetaAt (2).orientation == ChipOrientation.DOWN, "Wrong orientation");
		TestManager.AssertTrue (stack.GetChipMetaAt (2).isOrientationImportant == false, "Orientation shouldn't be important");
		TestManager.AssertTrue (stack.GetChipMetaAt (2).stackPos == 2, "Wrong stackpos");

		TestManager.AssertTrue (stack.GetChipMetaAt (3).prefabId == 13, "Wrong prefabid");
		TestManager.AssertTrue (stack.GetChipMetaAt (3).orientation == ChipOrientation.DOWN, "Wrong orientation");
		TestManager.AssertTrue (stack.GetChipMetaAt (3).isOrientationImportant == false, "Orientation shouldn't be important");
		TestManager.AssertTrue (stack.GetChipMetaAt (3).stackPos == 3, "Wrong stackpos");

		TestManager.AssertTrue (stack.GetChipMetaAt (4).prefabId == 12, "Wrong prefabid");
		TestManager.AssertTrue (stack.GetChipMetaAt (4).orientation == ChipOrientation.DOWN, "Wrong orientation");
		TestManager.AssertTrue (stack.GetChipMetaAt (4).isOrientationImportant == false, "Orientation shouldn't be important");
		TestManager.AssertTrue (stack.GetChipMetaAt (4).stackPos == 4, "Wrong stackpos");

	}

	public void TestFlipAt3() {
		Debug.Log ("Test Flip at 3");
		StackMeta stack = GetStackMetaOf5 ();

		Debug.Log ("Before flip at top: " + stack.ToStringShort ());
		stack.FlipStackAt (3);
		Debug.Log ("After flip at top: " + stack.ToStringShort ());

		TestManager.AssertTrue (stack.GetChipMetaAt (0).prefabId == 11, "Wrong prefabid");
		TestManager.AssertTrue (stack.GetChipMetaAt (0).orientation == ChipOrientation.UP, "Wrong orientation");
		TestManager.AssertTrue (stack.GetChipMetaAt (0).isOrientationImportant == false, "Orientation shouldn't be important");
		TestManager.AssertTrue (stack.GetChipMetaAt (0).stackPos == 0, "Wrong stackpos");

		TestManager.AssertTrue (stack.GetChipMetaAt (1).prefabId == 12, "Wrong prefabid");
		TestManager.AssertTrue (stack.GetChipMetaAt (1).orientation == ChipOrientation.UP, "Wrong orientation");
		TestManager.AssertTrue (stack.GetChipMetaAt (1).isOrientationImportant == false, "Orientation shouldn't be important");
		TestManager.AssertTrue (stack.GetChipMetaAt (1).stackPos == 1, "Wrong stackpos");

		TestManager.AssertTrue (stack.GetChipMetaAt (2).prefabId == 13, "Wrong prefabid");
		TestManager.AssertTrue (stack.GetChipMetaAt (2).orientation == ChipOrientation.UP, "Wrong orientation");
		TestManager.AssertTrue (stack.GetChipMetaAt (2).isOrientationImportant == false, "Orientation shouldn't be important");
		TestManager.AssertTrue (stack.GetChipMetaAt (2).stackPos == 2, "Wrong stackpos");

		TestManager.AssertTrue (stack.GetChipMetaAt (3).prefabId == 15, "Wrong prefabid");
		TestManager.AssertTrue (stack.GetChipMetaAt (3).orientation == ChipOrientation.DOWN, "Wrong orientation");
		TestManager.AssertTrue (stack.GetChipMetaAt (3).isOrientationImportant == false, "Orientation shouldn't be important");
		TestManager.AssertTrue (stack.GetChipMetaAt (3).stackPos == 3, "Wrong stackpos");

		TestManager.AssertTrue (stack.GetChipMetaAt (4).prefabId == 14, "Wrong prefabid");
		TestManager.AssertTrue (stack.GetChipMetaAt (4).orientation == ChipOrientation.DOWN, "Wrong orientation");
		TestManager.AssertTrue (stack.GetChipMetaAt (4).isOrientationImportant == false, "Orientation shouldn't be important");
		TestManager.AssertTrue (stack.GetChipMetaAt (4).stackPos == 4, "Wrong stackpos");

	}

	private StackMeta GetStackMetaOf5() {
		StackMeta stack = new StackMeta ();
		stack.Add (new ChipMeta (11, ChipOrientation.UP, false, 11, 0.2f));
		stack.Add (new ChipMeta (12, ChipOrientation.UP, false, 11, 0.2f));
		stack.Add (new ChipMeta (13, ChipOrientation.UP, false, 11, 0.2f));
		stack.Add (new ChipMeta (14, ChipOrientation.UP, false, 11, 0.2f));
		stack.Add (new ChipMeta (15, ChipOrientation.UP, false, 11, 0.2f));
		return stack;
	}

	private StackMeta GetStackMetaOf5WithOrientation() {
		StackMeta stack = new StackMeta ();
		stack.Add (new ChipMeta (11, ChipOrientation.UP, true, 11, 0.2f));
		stack.Add (new ChipMeta (12, ChipOrientation.UP, true, 11, 0.2f));
		stack.Add (new ChipMeta (13, ChipOrientation.UP, true, 11, 0.2f));
		stack.Add (new ChipMeta (14, ChipOrientation.UP, true, 11, 0.2f));
		stack.Add (new ChipMeta (15, ChipOrientation.UP, true, 11, 0.2f));
		return stack;
	}

	public void TestDemoStack() {
		StackMeta stack = GetStackMetaOf5 ();
		TestManager.AssertTrue (stack.GetChipMetaAt (0).prefabId == 11, "Wrong prefabid");
		TestManager.AssertTrue (stack.GetChipMetaAt (0).orientation == ChipOrientation.UP, "Wrong orientation");
		TestManager.AssertTrue (stack.GetChipMetaAt (0).isOrientationImportant == false, "Orientation shouldn't be important");
		TestManager.AssertTrue (stack.GetChipMetaAt (0).stackPos == 0, "Wrong stackpos");

		TestManager.AssertTrue (stack.GetChipMetaAt (1).prefabId == 12, "Wrong prefabid");
		TestManager.AssertTrue (stack.GetChipMetaAt (1).orientation == ChipOrientation.UP, "Wrong orientation");
		TestManager.AssertTrue (stack.GetChipMetaAt (1).isOrientationImportant == false, "Orientation shouldn't be important");
		TestManager.AssertTrue (stack.GetChipMetaAt (1).stackPos == 1, "Wrong stackpos");

		TestManager.AssertTrue (stack.GetChipMetaAt (2).prefabId == 13, "Wrong prefabid");
		TestManager.AssertTrue (stack.GetChipMetaAt (2).orientation == ChipOrientation.UP, "Wrong orientation");
		TestManager.AssertTrue (stack.GetChipMetaAt (2).isOrientationImportant == false, "Orientation shouldn't be important");
		TestManager.AssertTrue (stack.GetChipMetaAt (2).stackPos == 2, "Wrong stackpos");

		TestManager.AssertTrue (stack.GetChipMetaAt (3).prefabId == 14, "Wrong prefabid");
		TestManager.AssertTrue (stack.GetChipMetaAt (3).orientation == ChipOrientation.UP, "Wrong orientation");
		TestManager.AssertTrue (stack.GetChipMetaAt (3).isOrientationImportant == false, "Orientation shouldn't be important");
		TestManager.AssertTrue (stack.GetChipMetaAt (3).stackPos == 3, "Wrong stackpos");

		TestManager.AssertTrue (stack.GetChipMetaAt (4).prefabId == 15, "Wrong prefabid");
		TestManager.AssertTrue (stack.GetChipMetaAt (4).orientation == ChipOrientation.UP, "Wrong orientation");
		TestManager.AssertTrue (stack.GetChipMetaAt (4).isOrientationImportant == false, "Orientation shouldn't be important");
		TestManager.AssertTrue (stack.GetChipMetaAt (4).stackPos == 4, "Wrong stackpos");
	}

	public void TestMatchesOrientationNotImportant() {
		Debug.Log ("Test Matches Orientation Not Important");
		StackMeta stack1 = GetStackMetaOf5 ();
		StackMeta stack2 = GetStackMetaOf5 ();
		TestManager.AssertTrue(stack1.Matches(stack2), "Stack1 and Stack2 should match");
		TestManager.AssertTrue(stack1.Matches(stack1), "Stack1 and Stack2 should match");
		TestManager.AssertTrue(stack1.Matches(stack1), "Stack1 and Stack2 should match");

		stack2.FlipStackAt (3);
		TestManager.AssertFalse (stack1.Matches (stack2), "Stack1 and stack2 shouldn't match");
		TestManager.AssertFalse (stack2.Matches (stack1), "Stack1 and stack2 shouldn't match");

		stack2.FlipStackAt (3);
		TestManager.AssertTrue(stack1.Matches(stack2), "Stack1 and Stack2 should match");
		TestManager.AssertTrue(stack1.Matches(stack1), "Stack1 and Stack2 should match");
	}

	public void TestMatchesOrientationIsImportant() {
		Debug.Log ("Test Matches Orientation Is Important");
		StackMeta stack1 = GetStackMetaOf5WithOrientation ();
		StackMeta stack2 = GetStackMetaOf5WithOrientation ();
		TestManager.AssertTrue (stack1.Matches (stack2), "Should match");
		TestManager.AssertTrue (stack2.Matches (stack1), "Should match");
		TestManager.AssertTrue (stack1.Matches (stack1), "Should match");

		stack2.GetChipMetaAt (0).Flip ();
		TestManager.AssertFalse (stack1.Matches (stack2), "Shouldn't match");
		TestManager.AssertFalse (stack2.Matches (stack1), "Shouldn't match");
	}

}

