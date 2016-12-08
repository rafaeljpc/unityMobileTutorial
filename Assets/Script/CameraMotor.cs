using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour {

	public Transform lookAt;

	private Vector3 desiredPosition;
	private Vector3 offset;

	private Vector2 touchPosition;
	private float swipeResistance = 200.0f;

	private float smoothSpeed = 7.5f;
	private float distance = 150.0f;
	private float yOffset = 135.0f;

	void Start () {
		offset = new Vector3 (0, yOffset, -1f * distance);
	}
	
	void Update () {
		if (Input.GetKeyDown (KeyCode.LeftArrow))
			SlideCamera (true);
		else if (Input.GetKeyDown (KeyCode.RightArrow))
			SlideCamera (false);

		if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown(1)) {
			touchPosition = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp (0) || Input.GetMouseButtonUp(1)) {
			float swipeForce = touchPosition.x - Input.mousePosition.x;
			if (Mathf.Abs (swipeForce) > swipeResistance) {
				SlideCamera (swipeForce < 0);
			}				
		}
	}

	private void FixedUpdate() {
		desiredPosition = lookAt.position + offset;
		transform.position = Vector3.Lerp (transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
		transform.LookAt (lookAt.position + Vector3.up * 50);
	}

	public void SlideCamera(bool left) {
		if (left) {
			offset = Quaternion.Euler (0, 90, 0) * offset;
		} else {
			offset = Quaternion.Euler (0, -90,0) * offset;
		}
	}
}
