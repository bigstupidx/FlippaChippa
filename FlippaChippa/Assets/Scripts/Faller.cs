using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof (Scaler))]
public class Faller : MonoBehaviour, FCEventListener 
{
	private PrefabsManager prefabsManager;
	public float fallTime;
	private List<Mover> movers;
	private Scaler scaler;

	private int nFinishedMovements;

	private bool isFalling = false;
	public bool IsFalling { get { return isFalling; } }

	public Transform targetTransform;
	private FCObservable observable;

	void Awake() {
		prefabsManager = GameObject.FindGameObjectWithTag (Tags.PREFABS_MANAGER).GetComponent<PrefabsManager>();
		observable = new FCObservable ();
		movers = new List<Mover> ();
		scaler = GetComponent<Scaler> ();
		scaler.AddListener (this, FCEvent.END);
	}
		
	// Use this for initialization
	void Start ()
	{
	}

	public void CreateMoverFor(Transform moverParent, Transform tTransform, Vector3 startPosition, Vector3 endPosition) {
		Mover mover = prefabsManager.GetMover ();
		mover.transform.parent = moverParent;
		mover.transform.localPosition = startPosition;
		mover.startPosition = startPosition;
		mover.endPosition = endPosition;
		mover.moveTime = fallTime;
		mover.AddListener (this, FCEvent.END);

		tTransform.parent = mover.transform;
		movers.Add (mover);
	}

	public void ClearMovers() {
		foreach (Mover mover in movers) {
			mover.RemoveListener (this, FCEvent.END);
			mover.gameObject.SetActive (false);
			mover.transform.parent = prefabsManager.transform;
		}
	}

	public void StartFalling() {
		nFinishedMovements = 0;
		isFalling = true;
		foreach (Mover mover in movers) {
			mover.StartMovement ();
		}
	}

	public void AddListener(FCEventListener listener, FCEvent fcEvent) {
		observable.AddListener (listener, fcEvent);
	}

	public void RemoveListener(FCEventListener listener, FCEvent fcEvent) {
		observable.RemoveListener (listener, fcEvent);
	}

	#region FCEventListener implementation

	public void OnEvent (FCEvent fcEvent, GameObject gameObject)
	{
		if (fcEvent == FCEvent.END) {
			nFinishedMovements++;
			if (gameObject.GetComponent<Scaler>() != null) {
				observable.NotifyListeners (FCEvent.END, gameObject);
				isFalling = false;
				ClearMovers ();

				List<Chip> childChips = GetAllChips ();
				foreach (Chip chip in childChips) {
					chip.transform.parent = targetTransform;
				}
				transform.parent = null;
			} else if (movers.Count == nFinishedMovements) {
				List<Chip> chips = GetChipsForMovers ();
				foreach (Chip chip in chips) {
					chip.transform.parent = targetTransform;
				}
				Transform lowest = GetLowestChip (chips);
				Vector3 lowestAsFallerLocal = transform.InverseTransformPoint (lowest.position);
				scaler.transform.localPosition = new Vector3 (lowestAsFallerLocal.x, lowestAsFallerLocal.y - chips [0].chipMeta.Height / 2, lowestAsFallerLocal.z);
				foreach (Chip chip in chips) {
					chip.transform.parent = scaler.transform;
				}
				scaler.StartScaling ();
			}
		}
	}

	#endregion

	private Transform GetLowestTransform(List<Transform> chipTransforms) {
		Transform lowest = chipTransforms [0];
		foreach (Transform chipTransform in chipTransforms) {
			if (chipTransform.transform.localPosition.y < lowest.localPosition.y) {
				lowest = chipTransform;
			}
		}
		return lowest;
	}

	private Transform GetLowestChip(List<Chip> chipTransforms) {
		Transform lowest = chipTransforms [0].transform;
		foreach (Chip chip in chipTransforms) {
			if (chip.transform.position.y < lowest.position.y) {
				lowest = chip.transform;
			}
		}
		return lowest;
	}

	private List<Transform> GetAllChildTransforms() {
		List<Transform> allTransforms = new List<Transform> ();
		foreach (Mover mover in movers) {
			for (int i = 0; i < mover.transform.childCount; i++) {
				allTransforms.Add (mover.transform.GetChild (i));
			}
		}
		return allTransforms;
	}

	private List<Chip> GetAllChips() {
		List<Chip> chips = new List<Chip> ();
		for (int i = 0; i < transform.childCount; i++) {
			Chip chip = transform.GetChild (i).GetComponent<Chip> ();
			if (chip != null) {
				chips.Add (chip);
			}
		}
		return chips;
	}

	private List<Chip> GetChipsForMovers() {
		List<Chip> chips = new List<Chip> ();
		for (int i = 0; i < transform.childCount; i++) {
			Mover mover = transform.GetChild (i).GetComponent<Mover> ();
			if (mover != null) {
				Chip chip = mover.gameObject.GetComponentInChildren<Chip> ();
				if (chip != null) {
					chips.Add (chip);
				}
			}
		}
		return chips;
	}
}

