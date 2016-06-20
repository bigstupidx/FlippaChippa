using System;
using System.Collections.Generic;
using UnityEngine;

public class FCObservable
{
	Dictionary<FCEvent, List<FCEventListener>> map;

	public FCObservable ()
	{
		map = new Dictionary<FCEvent,List<FCEventListener>> ();
		map [FCEvent.BEGIN] = new List<FCEventListener> ();
		map [FCEvent.MIDDLE] = new List<FCEventListener> ();
		map [FCEvent.END] = new List<FCEventListener> ();
	}

	public void AddListener(FCEventListener listener, FCEvent fcEvent) {
		if (!map [fcEvent].Contains (listener)) {
			map [fcEvent].Add (listener);
		}
	}

	public void RemoveListener(FCEventListener listener, FCEvent fcEvent) {
		map [fcEvent].Remove (listener);
	}

	public void NotifyListeners(FCEvent fcEvent, GameObject gameObject) {
		List<FCEventListener> listeners = map [fcEvent];
		foreach (FCEventListener listener in listeners) {
			listener.OnEvent (fcEvent, gameObject);
		}
	}
}

