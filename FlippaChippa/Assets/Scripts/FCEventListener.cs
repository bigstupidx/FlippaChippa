using System;
using UnityEngine;

public interface FCEventListener
{
	void OnEvent(FCEvent fcEvent, GameObject gameObject);
}

