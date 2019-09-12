using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainShield : MonoBehaviour {

	private float countdown =3;
	private bool isMoving = false;

	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			if(!isMoving){
				isMoving = true;
			}
		}
		if (isMoving && countdown >0){
			countdown -= 1 * Time.deltaTime;
			transform.Translate(Vector3.up * Time.deltaTime, Space.World);
		}
	}
}
