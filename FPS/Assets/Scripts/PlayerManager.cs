﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerManager : MonoBehaviour {
	Rigidbody rb;

	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private float cameraRotationX = 0f;
	private float currentCameraRotationX = 0f;

	[SerializeField] private Camera cam;
	[SerializeField] private Camera weaponCam;
	[SerializeField] private float cameraRotationLimit = 85f;

	public float jumpForce;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
	}

	// Gets a movement vector
	public void Move (Vector3 _velocity)
	{
		velocity = _velocity;
	}

	// Gets a rotational vector
	public void Rotate(Vector3 _rotation)
	{
		rotation = _rotation;
	}

	// Gets a rotational vector for the camera
	public void RotateCamera(float _cameraRotationX)
	{
		cameraRotationX = _cameraRotationX;
	}

	// Run every physics iteration
	void FixedUpdate ()
	{

//        grounded = Physics.Linecast(transform.position, groundCheck.position, 1<< LayerMask.NameToLayer("Ground"));

        if ((Input.GetButtonDown("Jump")))
        {
			Debug.Log("It works!");
            rb.AddForce(transform.up * jumpForce * Time.deltaTime);
		}


		PerformMovement();
		PerformRotation();
	}

	//Perform movement based on velocity variable
	void PerformMovement ()
	{
		if (velocity != Vector3.zero)
		{
			rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
		}

	}
	//Perform rotation
	void PerformRotation ()
	{
		rb.MoveRotation(rb.rotation * Quaternion.Euler (rotation));
		if (cam != null)
		{
			// Set our rotation and clamp it
			currentCameraRotationX -= cameraRotationX;
			currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

			//Apply our rotation to the transform of our camera
			cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
			weaponCam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
		}
	}

	void PerfomJump(){

	}

}