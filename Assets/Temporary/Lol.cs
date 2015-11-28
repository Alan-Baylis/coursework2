using UnityEngine;
using System.Collections;

public class Lol : MonoBehaviour {
	[Range(100, 1000)]
	public int force = 500;
	public Transform cameraObject;
	public KeyCode key;
	
	// Update is called once per frame
	void Update () {
		if (cameraObject != null) {
			cameraObject.LookAt (transform);
			cameraObject.position.Set (cameraObject.position.x, transform.position.y, cameraObject.position.z);
		}

		if (Input.GetKeyDown (key)) {
			GetComponent<Rigidbody>().AddForce(Vector3.up * force);
			GetComponent<Rigidbody>().AddTorque(Vector3.back * 30);
		}
	}
}
