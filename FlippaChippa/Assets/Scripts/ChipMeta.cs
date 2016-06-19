﻿using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ChipMeta {

	public int prefabId;	//ID for a type of chip, this is not necessarily unique in a stack
	public ChipOrientation orientation;
	public bool isOrientationImportant;
	public int stackPos;	//Position from the bottom of stack. Pos 0 is the bottom chip
	public Stack stack;	//the stack this chip belongs to

	public ChipMeta(int prefabId, ChipOrientation orientation, bool isOrientationImportant, int stackPos, Stack stack) {
		this.prefabId = prefabId;
		this.orientation = orientation;
		this.isOrientationImportant = isOrientationImportant;
		this.stackPos = stackPos;
		this.stack = stack;
	}

	public void Flip() {	//Doesn't actually flip the chip, only "flips" the meta chip
		if (orientation == ChipOrientation.UP) {
			orientation = ChipOrientation.DOWN;
		} else {
			orientation = ChipOrientation.UP;
		}
	}
}
