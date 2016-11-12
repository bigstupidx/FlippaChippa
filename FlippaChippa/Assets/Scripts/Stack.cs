using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Stack : MonoBehaviour, FCEventListener
{

	private StackMeta meta;
	public StackMeta Meta  { get { return meta; } }
	public Flipper flipper;
	public Faller faller;
	public bool IsTargetStack;
	public MJParticleExplosion chipParticlesPrefab;
	List<MJParticleExplosion> explosions;

	private CrushChipsMeta crushChipsMeta;
	List<Chip> crushedChips = new List<Chip> ();
	List<Chip> fallingChips = new List<Chip> ();

	void Awake() {
		meta = new StackMeta ();
		explosions = new List<MJParticleExplosion> ();
	}

	// Use this for initialization
	void Start ()
	{
		flipper.AddListener(this, FCEvent.END);
		faller.AddListener (this, FCEvent.END);
		faller.targetTransform = transform;
		crushChipsMeta = Meta.CleanupStackForCrushedChips (0);
		if (crushChipsMeta.HasCrushedChips) {
			Debug.Log ("The stack has [" + crushChipsMeta.crushedChips.Count + "] chips that should be crushed. ");
			StartFalling ();
		}
	}
	
	public void FlipAt(GameObject gameObject) {
		Chip chipSelected = gameObject.GetComponent<Chip> ();
		if (chipSelected == null) {
			Debug.Log ("<color=red>The selected gameobject isn't a chip. Will therefore not flip anything it.</color>");
			return;
		}

		FlipAt (chipSelected.chipMeta.stackPos);
	}

	public void FlipAt(int position) {
		if (flipper.IsFlipping || faller.IsFalling) {	//Extra insurance that the meta stack won't be flipped when the flipper isn't finished
			return;
		}

		List<GameObject> chipsToFlip = new List<GameObject> ();
		for (int i = position; i < transform.childCount; i++) {
			chipsToFlip.Add (transform.GetChild (i).gameObject);
		}

		crushChipsMeta = meta.FlipStackAt (position);
		flipper.Flip (chipsToFlip, transform);

		Debug.LogFormat ("<color=yellow>Flipping at {0}.</color> Updated stack: {1})", position, meta.ToStringShort ());
	}

	public void AddListener(FCEventListener listener) {
		flipper.AddListener (listener, FCEvent.END);
		flipper.AddListener (listener, FCEvent.BEGIN);
		flipper.AddListener (listener, FCEvent.MIDDLE);
	}

	public void AddLandingListener(FCEventListener listener, FCEvent fcEvent) {
		flipper.AddLandingListener (listener, fcEvent);
	}

	public bool Matches(Stack otherStack) {
		return meta.Matches (otherStack.meta);
	}

	public void Add(Chip chip) {
		Meta.Add (chip.chipMeta);
		chip.transform.SetParent (transform);
		float yPos = GetCurrentMaxHeight () + chip.chipMeta.Height / 2f;
		chip.transform.localPosition = new Vector3 (0f, yPos, 0f);
	}

	private float GetCurrentMaxHeight() {
		float sum = 0f;
		for (int i = 0; i < Meta.ChipCount (); i++) {
			sum += Meta.GetChipMetaAt (i).Height;
		}
		return sum;
	}

	public Chip getChipAt(int stackPosition) {
		for (int i = 0; i < transform.childCount; i++) {
			Chip chip = transform.GetChild(i).GetComponent<Chip>();
			if (chip != null && chip.chipMeta.stackPos == stackPosition) {
				return chip;
			}
		}
		return null;
	}

    public void OnEvent(FCEvent fcEvent, GameObject gameObject)
    {
        if (fcEvent == FCEvent.END) {
			if (gameObject.GetComponent<Flipper>() != null) {
				if (crushChipsMeta.HasCrushedChips) {
					StartFalling ();
				}
			}
		}
    }

	public void StartFalling ()
	{
		crushedChips = GetChipsForChipMeta (crushChipsMeta.crushedChips);
		fallingChips = GetChipsForChipMeta (crushChipsMeta.fallingChips);
		faller.transform.parent = transform;
		faller.transform.localPosition = Vector3.zero;
		foreach (Chip chip in fallingChips) {
			float toReplace = crushedChips [0].transform.localPosition.y;
			int indexOffset = chip.chipMeta.stackPos - crushedChips [0].chipMeta.stackPos;
			float relativeY = indexOffset * chip.chipMeta.Height;
			float newY = relativeY + toReplace;
			faller.CreateMoverFor (faller.transform, chip.transform, chip.transform.localPosition, new Vector3 (chip.transform.localPosition.x, newY, chip.transform.localPosition.z));
		}
		while (crushedChips.Count > 0) {
			Chip chipToCrush = crushedChips [0];
			chipToCrush.transform.parent = null;
			//Create and start a particle system
			crushedChips.RemoveAt (0);
			Vector3 chipLocation = chipToCrush.transform.position;
			MJParticleExplosion explosion = GetParticleExplosion ();
			Color chipColor = chipToCrush.GetMainColor ();
			explosion.Explode (chipLocation, chipColor, chipColor);
			chipToCrush.gameObject.SetActive (false);
			Destroy (chipToCrush.gameObject);
		}
		crushChipsMeta = new CrushChipsMeta ();
		crushedChips.Clear ();
		fallingChips.Clear ();
		faller.StartFalling ();
	}

	private MJParticleExplosion GetParticleExplosion() {
		foreach (MJParticleExplosion explosion in explosions) {
			if (!explosion.gameObject.activeSelf) {
				explosion.gameObject.SetActive (true);
				return explosion;
			}
		}

		MJParticleExplosion expl = Instantiate<MJParticleExplosion>(chipParticlesPrefab);
		explosions.Add (expl);
		return expl;
	}

	private float GetCorrectVerticalPosition(Chip chip) {
		float diffY = chip.chipMeta.stackPos * chip.chipMeta.Height - chip.transform.localPosition.y;	//Assumes all chips have the same height
		return chip.transform.position.y + diffY;
	}

	private List<Chip> GetChipsForChipMeta(List<ChipMeta> chipMetas) {
		List<Chip> chips = new List<Chip> ();
		foreach (ChipMeta chipMeta in chipMetas) {
			chips.Add(GetChipForChipMeta(chipMeta));
		}
		return chips;
	}

	private Chip GetChipForChipMeta(ChipMeta chipMeta) {
		for (int i = 0; i < transform.childCount; i++) {
			Chip chip = transform.GetChild (i).GetComponent<Chip> ();
			if (chip != null && chip.chipMeta == chipMeta) {
				return chip;
			}
		}
		return null;

	}


}

