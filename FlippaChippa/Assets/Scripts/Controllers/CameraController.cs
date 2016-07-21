using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private Mover1DMeta moverMeta;
	public float moveTime = 1f;
	public float deltaMove = 1.5f;

	void Start () {
		moverMeta = new Mover1DMeta (transform.position.x, transform.position.x, 0f);
	}
	
	void Update () {
		if (!moverMeta.IsFinished) {
			transform.position = new Vector3 (moverMeta.UpdatePos (Time.deltaTime), transform.position.y, transform.position.z);
		}
	}

	public void MoveLeft() {
		moverMeta = new Mover1DMeta (transform.position.x, transform.position.x - deltaMove, moveTime);
	}

	public void MoveRight() {
		moverMeta = new Mover1DMeta (transform.position.x, transform.position.x + deltaMove, moveTime);
	}

}
