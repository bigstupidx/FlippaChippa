using UnityEngine;
using System.Collections;

public class ChipListHighligter : MonoBehaviour {

	public float timeBetweenHighlights = 2f, highlightTime = 1f;
	private float elapsedWaitTime = 1.5f;	//Starts a little ways in the animation
	private bool wait = true;
	private Chip lastHighlightedChip;
	
	// Update is called once per frame
	void Update () {
		if (wait) {
			elapsedWaitTime += Time.deltaTime;
			if (elapsedWaitTime >= timeBetweenHighlights) {
				elapsedWaitTime = 0f;
				Chip[] chips = GetComponentsInChildren<Chip>();
				int chipToHightLight = Random.Range(0, chips.Length);
				if (chips[chipToHightLight] == lastHighlightedChip) {
					chipToHightLight = (chipToHightLight + 1) % chips.Length;
				}
				Chip chip = chips[chipToHightLight];
				chip.HighlightOnce(highlightTime);
				lastHighlightedChip = chip;
				elapsedWaitTime = 0f;
			}
		}
	}

	public void Stop() {
		wait = false;
		if (lastHighlightedChip != null) {
			lastHighlightedChip.UnHighlight(0.15f);
		}
	}
}
