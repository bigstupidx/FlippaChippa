using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
    public Vector3 startPosition, endPosition;
	private Vector3 totalDiff;
    public float moveTime;

	private bool isMoving = false;
	private float elapsedTime;

	private FCObservable observable;

	void Awake() {
		observable = new FCObservable ();
	}

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		if (isMoving) {
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= moveTime) {
				isMoving = false;
 				transform.localPosition = endPosition;
				observable.NotifyListeners(FCEvent.END, gameObject);
			} else {
				Vector3 currentDiff = totalDiff * MJMath.LerpExp(elapsedTime / moveTime);
				transform.localPosition = startPosition + currentDiff;
			}
		}
	}

	public void StartMovement() {
		totalDiff = endPosition - startPosition;
		isMoving = true;
		elapsedTime = 0f;
	}

	public void AddListener(FCEventListener listener, FCEvent fcEvent) {
		observable.AddListener (listener, fcEvent);
	}

	public void RemoveListener(FCEventListener listener, FCEvent fcEvent) {
		observable.RemoveListener (listener, fcEvent);
	}
	
}

