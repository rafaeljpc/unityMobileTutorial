using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Motor : MonoBehaviour {

	public float moveSpeed = 5.0f;
	public float drag = 0.5f;
	public float terminalRotationSpeed = 25.0f;
	public VirtualJoystick moveJoystick;
	public Button boostButton;

	public float boostSpeed = 100.0f;
	public float boostCooldown = 5.0f;
	private float lastBoost;

	private Rigidbody controller;
	private Transform camTransform;

	void Start() {
		lastBoost = Time.time - boostCooldown;

		controller = GetComponent<Rigidbody> ();
		controller.maxAngularVelocity = terminalRotationSpeed;
		controller.drag = drag;

		camTransform = Camera.main.transform;
	}

	void Update() {
		Vector3 dir = Vector3.zero;

		dir.x = Input.GetAxis ("Horizontal");
		dir.z = Input.GetAxis ("Vertical");

		if (dir.magnitude > 1)
			dir.Normalize ();

		if (moveJoystick.InputDirection != Vector3.zero)
			dir = moveJoystick.InputDirection;

		Vector3 rotatedDir = camTransform.TransformDirection (dir);
		rotatedDir = new Vector3 (rotatedDir.x, 0, rotatedDir.z);
		rotatedDir = rotatedDir.normalized * dir.magnitude;

		controller.AddForce (rotatedDir * moveSpeed);
	}

	void LateUpdate() {
		boostButton.interactable = Time.time - lastBoost > boostCooldown;
	}

	public void Boost() {
		Debug.Log ("Boost");
		if (Time.time - lastBoost > boostCooldown) {
			controller.AddForce (controller.velocity.normalized * boostSpeed, ForceMode.VelocityChange);
			lastBoost = Time.time;
		}
	}

}
