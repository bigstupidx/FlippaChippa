using System;

public class ArrayUtils
{

	public const int DEFAULT_ARRAY_INCREASE = 20;
	public static int[] IncreaseSize(int[] array, int nExtraElements) {
		int[] newArray = new int[array.Length + nExtraElements];
		for (int i = 0; i < array.Length; i++) {
			newArray [i] = array [i];
		}
		return newArray;
	}

	public static int[] Add(int value, int[] array) {
		if (array [array.Length - 1] == 0) {	//There is still space
			for (int i = 0; i < array.Length; i++) { 
				if (array [i] == 0) {
					array [i] = value;
					break;
				}
			}
			return array;
		} else {
			int[] newArray = IncreaseSize (array, DEFAULT_ARRAY_INCREASE);
			return Add (value, newArray);
		}
	}

	public static int TotalNonZeroValues(int[] array) {
		int count = 0;
		for (int i = 0; i < array.Length; i++) {
			if (array [i] != 0) {
				count++;
			} else {
				break;
			}
		}
		return count;
	}

	public static int Sum(int[] array) {
		int sum = 0;
		for (int i = 0; i < array.Length; i++) {
			if (array [i] != 0) {
				sum += array [i];
			} else {
				break;
			}
		}
		return sum;

	}

	public static string Stringify(int[] array) {
		string output = "[" + array[0];
		for (int i = 1; i < array.Length; i++) {
			output += ", " + array [i];
		}
		output += "]";
		return output;
	}

	public static float[] AddFloat(float value, float[] array) {
		if (array [array.Length - 1] == 0) {	//There is still space
			for (int i = 0; i < array.Length; i++) { 
				if (array [i] == 0) {
					array [i] = value;
					break;
				}
			}
			return array;
		} else {
			float[] newArray = IncreaseSizeFloat (array, DEFAULT_ARRAY_INCREASE);
			return AddFloat (value, newArray);
		}
	}

	public static float[] IncreaseSizeFloat(float[] array, int nExtraElements) {
		float[] newArray = new float[array.Length + nExtraElements];
		for (int i = 0; i < array.Length; i++) {
			newArray [i] = array [i];
		}
		return newArray;
	}

	public static int TotalNonZeroValuesFloat(float[] array) {
		int count = 0;
		for (int i = 0; i < array.Length; i++) {
			if (array [i] != 0) {
				count++;
			} else {
				break;
			}
		}
		return count;
	}

	public static float SumFloat(float[] array) {
		float sum = 0;
		for (int i = 0; i < array.Length; i++) {
			if (array [i] != 0) {
				sum += array [i];
			} else {
				break;
			}
		}
		return sum;

	}

	public static string StringifyFloat(float[] array) {
		string output = "[" + array[0];
		for (int i = 1; i < array.Length; i++) {
			output += "; " + array [i];
		}
		output += "]";
		return output;
	}

}
