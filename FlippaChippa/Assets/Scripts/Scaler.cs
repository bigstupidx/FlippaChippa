using UnityEngine;
using System.Collections;

public class Scaler : MonoBehaviour
{

	public float scaleDiff = 0.2f;
	public float scaleTime = 0.5f;
	public int nTops = 2;

	private Vector3 initalScale;

	private float elapsedTime = 0f;
	private float maxtime = 2 * Mathf.PI;

	private bool isScaling = false;
	public bool IsScaling { get { return isScaling; } }

	private FCObservable observable;

	void Awake() {
		observable = new FCObservable ();
		maxtime = 2 * Mathf.PI * nTops;
	}

	// Use this for initialization
	void Start ()
	{
		initalScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isScaling) {
			elapsedTime += Time.deltaTime;
			if (elapsedTime > scaleTime) {
				isScaling = false;
				transform.localScale = initalScale;
				observable.NotifyListeners (FCEvent.END, gameObject);
			} else {
				float timeAmount = (scaleTime - elapsedTime) / scaleTime;
				float sinTime = timeAmount * maxtime;	//sin(ax), where a = nScalingPoints, x= timeAmount * maxTime
				float scale = scaleDiff * timeAmount * Mathf.Sin(sinTime);
				float finalScale = initalScale.y + scale;
				transform.localScale = new Vector3 (initalScale.x, finalScale, initalScale.z);
			}
		}
	}

	public void StartScaling() {
		maxtime = 2 * Mathf.PI * nTops;
		isScaling = true;
		elapsedTime = 0;
	}

	public void AddListener(FCEventListener listener, FCEvent fcEvent) {
		observable.AddListener (listener, fcEvent);
	}
}

