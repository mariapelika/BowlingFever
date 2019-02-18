using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Reset : MonoBehaviour {
	public GameObject player1;
	public GameObject player2;
	public GameObject startoverbutton;
	public GameObject exitbutton;
	public Text name1;
	public Text name2;
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponent<PlayerController> () != null) {
			
			StartCoroutine (ResetBallposition (other));
		}

	}


	IEnumerator ResetBallposition(Collider other)
	{
		
		other.gameObject.GetComponent<Rigidbody> ().isKinematic = true;
	




		yield return new WaitForSecondsRealtime (1);
		other.gameObject.GetComponent<PlayerController> ().Checkfall ();
		yield return new WaitForSecondsRealtime (2);
		if (other.gameObject.GetComponent<PlayerController> ().m != 10) {
			other.gameObject.GetComponent<PlayerController> ().PrintScore ();
		}
		other.gameObject.GetComponent<PlayerController> ().ResetBall ();
		MouseMovement.done = false;
		Debug.Log ("m=" + other.gameObject.GetComponent<PlayerController> ().m);
		if (player1.GetComponent<PlayerController> ().itsend == true || player2.GetComponent<PlayerController> ().itsend == true) {//end of game
			if (player1.GetComponent<PlayerController> ().player2 == true && player2.GetComponent<PlayerController> ().itsend == true) { // end of game for both players
				startoverbutton.SetActive (true);
				exitbutton.SetActive (true);
			 
				Debug.Log ("PLAYER FROZEN END OF GAME");
			} else if (player1.GetComponent<PlayerController> ().player2 == false) { //end of game for one player
				startoverbutton.SetActive (true);
				exitbutton.SetActive (true);
				Debug.Log ("PLAYER FROZEN END OF GAME");
			
			} 
		}else {



	
	
				other.gameObject.GetComponent<Rigidbody> ().isKinematic = false;
				//gameObject.tag = "Camera1";


				//CHANGE PLAYER ON MULTIPLAYER MODE
				//prepei sto start na balw na min kineitai o player2

				if (other.GetComponent<PlayerController> ().m != 9) {
					if (player1.GetComponent<PlayerController> ().player2 && player1.GetComponent<PlayerController> ().changeplayer) { //if a second player exists and its time for his turn 
						name1.color=Color.black;
					    player1.GetComponent<Rigidbody> ().isKinematic = true; // don't movw player 1
						player1.GetComponent<SphereCollider> ().enabled = false;
						player1.GetComponent<MeshRenderer> ().enabled = false;


						//start moving player 2
					    name2.color=Color.red;
						player2.GetComponent<Rigidbody> ().isKinematic = false; //  movw player 2
						player2.GetComponent<SphereCollider> ().enabled = true;
						player2.GetComponent<MeshRenderer> ().enabled = true;
						Debug.Log ("PLAYER2");
						player1.GetComponent<PlayerController> ().changeplayer = false;
				
					} else if (player2.GetComponent<PlayerController> ().changeplayer) { //if a second player exists and its time for his turn 
					    name2.color=Color.black;
					    player2.GetComponent<Rigidbody> ().isKinematic = true; // don't movw player 2
						player2.GetComponent<SphereCollider> ().enabled = false;
						player2.GetComponent<MeshRenderer> ().enabled = false;


						//start moving player 2
					    name1.color=Color.red;
						player1.GetComponent<Rigidbody> ().isKinematic = false; //  movw player 1
						player1.GetComponent<SphereCollider> ().enabled = true;
						player1.GetComponent<MeshRenderer> ().enabled = true;
						Debug.Log ("PLAYER1 mpike");
						player2.GetComponent<PlayerController> ().changeplayer = false;
		
					}
		
				}


		}
	}

}
