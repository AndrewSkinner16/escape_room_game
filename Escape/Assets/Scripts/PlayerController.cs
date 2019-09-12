using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float speed = 7f;
	[SerializeField]
	private float lookSensitivity = 3;

	[SerializeField]
	private GameObject bulletPrefab;
	[SerializeField]
	private Transform bulletSpawn;

	private PlayerMotor motor;

	private GameObject touchingObject;
	private bool holdingObject = false;

	private Transform self;

	Vector3 oldPosition;
	Vector3 currentPosition;
	Quaternion oldRotation;
	Quaternion currentRotation;

	public bool isLocalPlayer = false;

	void Start (){
		motor = GetComponent<PlayerMotor> ();
		self = gameObject.transform;
	}

	void Update (){
		if(!isLocalPlayer){
			return;
		}

		Cursor.lockState = CursorLockMode.Locked;

		//calculate movement velocity as a 3D vector
		float _xMov = Input.GetAxis ("Horizontal");
		float _zMov = Input.GetAxis ("Vertical");

		Vector3 moveHorizontal = transform.right * _xMov; 
		Vector3 moveVertical = transform.forward * _zMov;

		//final movement vector
		Vector3 velocity = (moveHorizontal + moveVertical) * speed;

		//Apply movement
		motor.Move (velocity);
		currentPosition = transform.position;

		//Calculate rotation as a 3D vector (turning around)
		float _yRot = Input.GetAxisRaw ("Mouse X");

		Vector3 rotation = new Vector3 (0f, _yRot, 0f) * lookSensitivity;

		//Apply rotation
		motor.Rotate (rotation);
		currentRotation = transform.rotation;

		//Calculate camera rotation as a 3D vector (turning around)
		float _xRot = Input.GetAxisRaw ("Mouse Y");

		float cameraRotationX = _xRot * lookSensitivity;

		//Apply camera rotation
		motor.RotateCamera (cameraRotationX);

	if (currentPosition != oldPosition){
		NetworkManager.instance.GetComponent<NetworkManager>().CommandMove(transform.position);
		oldPosition = currentPosition;

	}
	if (currentRotation != oldRotation){
		NetworkManager.instance.GetComponent<NetworkManager>().CommandTurn(transform.rotation);
		oldRotation = currentRotation;
	}


		//Check for firing
		// if (Input.GetKeyDown(KeyCode.Space)){
		// 	Fire();
		// }

		if (Input.GetKeyDown(KeyCode.P)){
			PickUpItem();
		}
		if (Input.GetKeyDown(KeyCode.F)){
			DropItem();
		}
	}

	void PickUpItem(){
		if (touchingObject == null && holdingObject == false){
			return;
		} else {
			holdingObject = true;
			touchingObject.transform.parent = self;
		}
	}

	void DropItem(){
		if (touchingObject == null && holdingObject == false){
			return;
		} else {
			touchingObject.transform.parent = null;
		}
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "personal_shield"){
			Debug.Log("colliding with Color");
			touchingObject = other.gameObject;
			Debug.Log(touchingObject);
		}
	}

	void OnCollisionExit(Collision other){
		if (holdingObject == false){
			if (other.gameObject.tag == "color"){
				Debug.Log("Stopped colliding with color");
				touchingObject = null;
				Debug.Log(touchingObject);
			}
		}
	}

	void Fire(){
    	var bullet = (GameObject)Instantiate (bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
    	bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;
		Physics.IgnoreCollision(bullet.GetComponent<SphereCollider>(), gameObject.GetComponent<BoxCollider>());
    	Destroy(bullet, 2.0f);
	}
}
