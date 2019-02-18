using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinSound : MonoBehaviour {
	public AudioSource audio1;
	public AudioSource audio2;


	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player")) {
			if (other.gameObject.GetComponent<PlayerController> ().NumofTry == 0) {
				audio1.Play ();
			} else
				audio2.Play ();
		}

	}

}
