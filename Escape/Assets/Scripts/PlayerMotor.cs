using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

	[SerializeField]
	private Camera cam;

	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private float cameraRotationX = 0f;
	private float currentCameraRotationX = 0f;

	[SerializeField]
	private float cameraRotationLimit = 85f;

	private Rigidbody rb;

	public bool invertCamera;

	private PlayerController pc;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		pc = GetComponent<PlayerController>();
		if (pc.isLocalPlayer){
			return;
		}
		cam.enabled = false;
	}
	
	//Gets a movement vector
	public void Move (Vector3 _veclocity){
		velocity = _veclocity;
	}

	//Gets a rotational vector
	public void Rotate (Vector3 _rotation){
		rotation = _rotation;
	}

	//Gets a rotational vector for the camera
	public void RotateCamera (float _cameraRotationX){
		cameraRotationX = _cameraRotationX;
	}

	//Run every physics iteration
	void FixedUpdate (){
		PerformMovement ();
		PerformRotation ();
	}

	// Perform movmement based on velocity variable
	void PerformMovement (){
		if (velocity != Vector3.zero){
			rb.MovePosition (rb.position + velocity * Time.fixedDeltaTime);
		} else {
			rb.velocity = Vector3.zero;
		}
	}

	void PerformRotation (){
		rb.MoveRotation (rb.rotation * Quaternion.Euler(rotation));
		if (cam != null) {
			if (invertCamera) {
				//Set our rotation and clamp it
				currentCameraRotationX += cameraRotationX;
				currentCameraRotationX = Mathf.Clamp (currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

				//Apply our rotation to the transform of our camera
				cam.transform.localEulerAngles = new Vector3 (currentCameraRotationX, 0f, 0f);
			} else {
				currentCameraRotationX -= cameraRotationX;
				currentCameraRotationX = Mathf.Clamp (currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

				cam.transform.localEulerAngles = new Vector3 (currentCameraRotationX, 0f, 0f);
			}
		}
	}
}
