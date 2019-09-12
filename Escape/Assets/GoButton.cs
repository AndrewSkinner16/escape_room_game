using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoButton : MonoBehaviour {

//add countdown to disable button
	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player"){
			NetworkManager.instance.GetComponent<NetworkManager>().CommandGoButtonPressed();
		}
	}
}
