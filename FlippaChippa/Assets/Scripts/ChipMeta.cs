using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ChipMeta {

	public int prefabId;	//ID for a type of chip, this is not necessarily unique in a stack
	public ChipOrientation orientation;
	public bool isOrientationImportant;
	public int stackPos;	//Position from the bottom of stack. Pos 0 is the bottom chip
	public float Height;

	public ChipMeta(int prefabId, ChipOrientation orientation, bool isOrientationImportant, int stackPos, float height) {
		this.prefabId = prefabId;
		this.orientation = orientation;
		this.isOrientationImportant = isOrientationImportant;
		this.stackPos = stackPos;
		this.Height = height;
	}

	public void Flip() {	//Doesn't actually flip the chip, only "flips" the meta chip
		if (orientation == ChipOrientation.UP) {
			orientation = ChipOrientation.DOWN;
		} else {
			orientation = ChipOrientation.UP;
		}
	}

	public string ToStringShort() {
		return "{prefabId: " + prefabId + ", chipOrientation: " + orientation.ToString() + ", stackPos: " + stackPos + "}";
	}

	//Creates a deep copy
	public ChipMeta Copy ()
	{
		ChipMeta copy = new ChipMeta (prefabId, orientation, isOrientationImportant, stackPos, Height);
		copy.Height = Height;
		return copy;
	}
}
