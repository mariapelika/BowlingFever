using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sidelinesound : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player")) {

			this.GetComponent<AudioSource> ().Play ();
		}

	}




}
