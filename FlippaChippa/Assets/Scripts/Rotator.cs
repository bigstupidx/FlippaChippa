using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{

	public float rotationTime = 1f;
	private float rotSpeed;

	private float totalRotation = 180f, halfRotation;

	private bool isRotating = false, hasNotifiedHalfWay = false;
	FCObservable observable;

	void Awake() {
		observable = new FCObservable ();
	}

	// Use this for initialization
	void Start ()
	{
		CalculateValues ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (isRotating) {
			float dRot = rotSpeed * Time.deltaTime;
			transform.Rotate (Vector3.right, dRot);
			if (!hasNotifiedHalfWay && (transform.rotation.eulerAngles.x > halfRotation || transform.rotation.eulerAngles.x < -halfRotation)) {
				hasNotifiedHalfWay = true;
				observable.NotifyListeners (FCEvent.MIDDLE, gameObject);
			} else if (transform.rotation.eulerAngles.x > totalRotation || transform.rotation.eulerAngles.x < -totalRotation) {
				//Ferdig med rotatsjon
				isRotating = false;
				transform.rotation = Quaternion.Euler (totalRotation, 0f, 0f);
				observable.NotifyListeners (FCEvent.END, gameObject);
			}
		}
	}

	public void CalculateValues() {
		rotSpeed = totalRotation / rotationTime;
		halfRotation = totalRotation / 2f;
	}

	public void AddListener(FCEventListener listener, FCEvent fcEvent) {
		observable.AddListener (listener, fcEvent);
	}

	public void RemoveListener(FCEventListener listener, FCEvent fcEvent) {
		observable.RemoveListener (listener, fcEvent);
	}

	public void StartRotation() {
		isRotating = true;
		hasNotifiedHalfWay = false;
		transform.rotation = Quaternion.Euler (Vector3.zero);
		observable.NotifyListeners (FCEvent.BEGIN, gameObject);
	}
}

