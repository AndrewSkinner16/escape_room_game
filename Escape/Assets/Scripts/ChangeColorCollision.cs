using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorCollision : MonoBehaviour {
	
	private Renderer rend;

	// Use this for initialization
	void Start () {
		//Set variable rend = the Renderer that's attached to this object
        rend = GetComponent<Renderer>();
		//Initally set the material to red (optional)
		rend.material.SetColor("_Color", Color.red);
	}

	//When a collision first occurs...
	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "Player"){
			rend.material.SetColor("_Color", Color.green);
		}
	}

	//When a collision ends...
	void OnCollisionExit(Collision other){
		rend.material.SetColor("_Color", Color.red);
	}
}
