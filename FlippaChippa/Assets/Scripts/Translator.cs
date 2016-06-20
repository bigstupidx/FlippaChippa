using UnityEngine;
using System.Collections;

public class Translator : MonoBehaviour
{

	public float translateHeight = 0.5f;
	public float translateTime = 1f;

	private float startY, halfTranslateTime;

	private bool isTranslating = false, hasNotifiedHalfWay = false;
	private float a, b;
	private float elapsedTime;

	private FCObservable observable;

	void Awake() {
		observable = new FCObservable ();
	}

	// Use this for initialization
	void Start ()
	{
		CalculateValues();
	}

	// Update is called once per frame
	void Update ()
	{
		if (isTranslating) {
			elapsedTime += Time.deltaTime;
			float y = startY + a * elapsedTime * elapsedTime + b * elapsedTime;
			if (y < startY) {	//has finished the rotation
				isTranslating = false;
				transform.localPosition = new Vector3 (transform.localPosition.x, startY, transform.localPosition.z);
				observable.NotifyListeners (FCEvent.END, gameObject);
			} else {
				transform.localPosition = new Vector3 (transform.localPosition.x, y, transform.localPosition.z);
				if (!hasNotifiedHalfWay && elapsedTime > halfTranslateTime) {
					hasNotifiedHalfWay = true;
					observable.NotifyListeners (FCEvent.MIDDLE, gameObject);
				}
			}
		}
	}

	public void StartTranslation() {
		isTranslating = true;
		hasNotifiedHalfWay = false;
		startY = transform.localPosition.y;
		elapsedTime = 0f;
		observable.NotifyListeners (FCEvent.BEGIN, gameObject);
	}

	public void AddListener(FCEventListener listener, FCEvent fcEvent) {
		observable.AddListener (listener, fcEvent);
	}

	public void RemoveListener(FCEventListener listener, FCEvent fcEvent) {
		observable.RemoveListener (listener, fcEvent);
	}

	public void CalculateValues() {
		//0 = height + a*t*t --> a = -height / ( (t/2)*(t/2) )
		a = -translateHeight / (translateTime * translateTime / 4);

		//dy/dt = 2*a*t + b -> b = -2*a*t
		b = -a * translateTime;

		halfTranslateTime = translateTime / 2f;
	}

	public void FinalizeTranslation() {
		transform.localPosition = new Vector3 (transform.localPosition.x, startY, transform.localPosition.z);
	}
}

