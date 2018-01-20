﻿using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
public class PlayerController : MonoBehaviour {

	[SerializeField] private float speed = 5f;
	[SerializeField] private float lookSensitivity = 3f;

	private PlayerManager manager;

	void Start ()
	{
		manager = GetComponent<PlayerManager>();
	}

	void Update ()
	{

		manager.Move(Vector3.zero);
		manager.Rotate(Vector3.zero);
		manager.RotateCamera(0f);

		Cursor.lockState = CursorLockMode.Locked;

		//Calculate movement velocity as a 3D vector
		float _xMov = Input.GetAxis("Horizontal");
		float _zMov = Input.GetAxis("Vertical");

		Vector3 _movHorizontal = transform.right * _xMov;
		Vector3 _movVertical = transform.forward * _zMov;

		// Final movement vector
		Vector3 _velocity = (_movHorizontal + _movVertical) * speed;

		// Animate movement
//		animator.SetFloat("ForwardVelocity", _zMov);

		//Apply movement
		manager.Move(_velocity);

		//Calculate rotation as a 3D vector (turning around)
		float _yRot = Input.GetAxisRaw("Mouse X");

		Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

		//Apply rotation
		manager.Rotate(_rotation);

		//Calculate camera rotation as a 3D vector (turning around)
		float _xRot = Input.GetAxisRaw("Mouse Y");

		float _cameraRotationX = _xRot * lookSensitivity;

		//Apply camera rotation
		manager.RotateCamera(_cameraRotationX);
	}
}