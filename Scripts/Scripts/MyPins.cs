using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyPins : MonoBehaviour {




	void FixedUpdate(){
		GetComponent<Rigidbody>().AddForce(Physics.gravity * 3.5f, ForceMode.Acceleration); 
	}

	public void DisappearPin()
	{
		if ((gameObject.transform.localEulerAngles.x > 45 && gameObject.transform.localEulerAngles.x<359 )|| (gameObject.transform.localEulerAngles.z > 45  && gameObject.transform.localEulerAngles.z<359)) 
		{
			Debug.Log ("gameObject.transform.localEulerAngles.x" + gameObject.transform.localEulerAngles.x);
			Debug.Log ("gameObject.transform.localEulerAngles.z " + gameObject.transform.localEulerAngles.z);
			Destroy (gameObject);
		
			Debug.Log ("DESTROY");
		}
	}




}
	
	

