using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
	public Camera camera3;
	public Camera camera1;
    public GameObject ball;
	
	private Vector3 offset;
	// Use this for initialization
	void Start () {
		offset=transform.position-ball.transform.position;
	}
	
	//it is guaranted to run after all processes are done
	void LateUpdate () {
	
		 
		transform.position=ball.transform.position+offset;
	}

	public void Changefrommenu(){
		camera3.enabled = false;
		camera1.enabled = true;
	}

	public void backtomenu(){
		camera3.enabled = false;
		camera1.enabled = true;
	}
}
