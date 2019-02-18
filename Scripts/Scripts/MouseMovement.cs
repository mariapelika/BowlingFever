using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*We use the mouse movement so we can convert it automatically to mobile game afterwards! */
public class MouseMovement : MonoBehaviour {
	
	private Vector2 InitialPosition;
	private Vector2 FinalPosition;
	private Vector2 distance;
	private float Initialtime;
	private float Finaltime;
	private float CalculatedTime;
	private static float constant=5f;
	public static bool done=false;


	public void OnMouseDown(){ //Starts on touch
		InitialPosition = Input.mousePosition;
		Initialtime = Time.time;
		Debug.Log ("called1");
	}


	public void OnMouseUp(){ // Ends on touch
		if (!done) {
			this.GetComponent<AudioSource> ().Play ();
			Debug.Log ("called2");
			FinalPosition = Input.mousePosition;
			Finaltime = Time.time;

			//Calculations
			distance = FinalPosition - InitialPosition;
			CalculatedTime = Finaltime - Initialtime;

			float position1 = distance.x * Time.deltaTime * CalculatedTime * constant;
			float position2 = distance.y * Time.deltaTime * CalculatedTime * constant;

			Vector3 finalvelocity = new Vector3 (position1, 0f, position2);
			PlayerController.ballVelocity = finalvelocity;

		} 
		done = true;
	}

}
