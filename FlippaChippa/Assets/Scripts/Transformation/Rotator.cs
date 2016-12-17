using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{

	public float rotationTime = 1f;

	private float totalRotation = 180f;
	private float elapsedTime = 0f;

	private bool isRotating = false, hasNotifiedHalfWay = false;
	FCObservable observable;
	private Vector3 startEuler = Vector3.zero, endEuler;

	void Awake() {
		observable = new FCObservable ();
	}

	// Use this for initialization
	void Start ()
	{
		endEuler = new Vector3 (totalRotation, 0f, 0f);
	}

	// Update is called once per frame
	void Update ()
	{
		if (isRotating) {
			elapsedTime += Time.deltaTime;
			float progress = elapsedTime / rotationTime;
			Vector3 rotationProgress = Vector3.Lerp (startEuler, endEuler, progress);
			Quaternion lerp = Quaternion.Euler (rotationProgress);
			transform.rotation = lerp;
			if (!hasNotifiedHalfWay && progress >= 0.5f) {
				hasNotifiedHalfWay = true;
				observable.NotifyListeners (FCEvent.MIDDLE, gameObject);
			} else if (progress >= 1f) {
				isRotating = false;
				transform.rotation = Quaternion.Euler (endEuler);
				observable.NotifyListeners (FCEvent.END, gameObject);
			}
		}
	}

	public void AddListener(FCEventListener listener, FCEvent fcEvent) {
		observable.AddListener (listener, fcEvent);
	}

	public void RemoveListener(FCEventListener listener, FCEvent fcEvent) {
		observable.RemoveListener (listener, fcEvent);
	}

	public void StartRotation() {
		isRotating = true;
		elapsedTime = 0f;
		hasNotifiedHalfWay = false;
		transform.rotation = Quaternion.Euler (startEuler);
		observable.NotifyListeners (FCEvent.BEGIN, gameObject);
	}
}

