using UnityEngine;

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

	public LayerMask groundLayers;
	public CapsuleCollider col;
	public float jumpForce;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		col = GetComponent<CapsuleCollider>();
	}

	public void Move (Vector3 _velocity)
	{
		velocity = _velocity;
	}

	public void Rotate(Vector3 _rotation)
	{
		rotation = _rotation;
	}

	public void RotateCamera(float _cameraRotationX)
	{
		cameraRotationX = _cameraRotationX;
	}

	void FixedUpdate ()
	{
		PerfomJump();
		PerformMovement();
		PerformRotation();
	}



	void PerformMovement (){
		if (velocity != Vector3.zero)
		{
			rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
		}

	}
	void PerformRotation (){
		rb.MoveRotation(rb.rotation * Quaternion.Euler (rotation));
		if (cam != null)
		{
			currentCameraRotationX -= cameraRotationX;
			currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

			//Apply our rotation to the transform of our camera
			cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
			weaponCam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
		}
	}

	void PerfomJump(){
        if (isGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce * Time.deltaTime, ForceMode.Impulse);
		}
	}

	private bool isGrounded(){
		return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x,
		 col.bounds.min.y, col.bounds.center.z), col.radius * .5f, groundLayers);
	}

	public void TakeDamage(){
		
	}

}