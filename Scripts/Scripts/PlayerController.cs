using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/*GENERAL INFO 
  We want to check every frame for player input and then apply that input
 to the player game object every frame as movement
 
 void Update() is called before rendering a frame 
 void FixedUpdate() is called before performing any physics calculations
 we are moving the ball by applying forces to the rigidbody (physics) so 
 we are using FixedUpdate()  
 
 
 FixedUpdate(): shortcut at monodevelop that searches the unity API
 ctrl+'

 
 
 
 
 */

public class PlayerController : MonoBehaviour {
	public float speed;
	private Rigidbody rb;
	public Camera Camera1;
	public Camera Camera2;
	public Camera Camera3;
	public bool strike;
	public bool spare;
	public Transform ball;
	public int NumofTry,i=0;
	private bool resetflag;
	public GameObject prefab;
	private Vector3[] position = new Vector3[10];
	private Quaternion[] q= new Quaternion[10];
	public static Vector3 ballVelocity;
	public int Notfallen;
	private int last;
	public bool player2;
	//Scoring
	public int m;
	public BowlingFrame[] bf = new BowlingFrame[11];
	public int Curscore;
	//printing score
	public GameObject scoreboard;
	//printing strike
	public GameObject Striker;
	public GameObject forspare;
	//changing player 
	public bool changeplayer;
	//flag to end game
	public bool itsend;

	private bool combotry;


	void Start()
	{
		combotry = false;
		itsend = false;
		forspare.GetComponent<SpriteRenderer> ().enabled = false;
		Striker.GetComponent<SpriteRenderer> ().enabled = false;
		gameObject.GetComponent<Rigidbody> ().isKinematic = true;//to start with the start button 
		Curscore = 0;
		InitializeFrames (bf);
		changeplayer = false;
		player2 = false;
		m = 0;
		ballVelocity = Vector3.zero;
		Camera1.enabled = false;
		Camera2.enabled = false;
		Camera3.enabled = true;
		resetflag = false;
		rb=GetComponent<Rigidbody>();
		prefab = GameObject.Find ("prefab");
		NumofTry = 0;
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("pins")) {
			if (g != prefab) {
				position [i] = g.transform.position;
				q [i] = g.transform.rotation;
				i++;
			}
		}

	}



	void FixedUpdate() 
	{
		//KEYBOARD MOVEMENT
		
		float moveHorizontal=Input.GetAxis("Horizontal");
		float moveVertical=Input.GetAxis("Vertical");

		Vector3 movement =new Vector3(moveHorizontal,0,Mathf.Abs(moveVertical));

		//adding forces to the rigidbody
		rb.AddForce(movement * speed*3000);   

		//GetComponent<Rigidbody>().AddForce(Physics.gravity * 2f, ForceMode.Acceleration); 
		GetComponent<Rigidbody> ().AddRelativeTorque (ballVelocity*2 );
		if (strike) {
			Debug.Log ("STRIKE !!!!! ");
			Striker.GetComponent<SpriteRenderer> ().enabled = true;
		} else
			Striker.GetComponent<SpriteRenderer> ().enabled = false;

		if (spare) {
			forspare.GetComponent<SpriteRenderer> ().enabled = true;
		} else
			forspare.GetComponent<SpriteRenderer> ().enabled = false;




		Curscore=CalculateScore();
		scoreboard.transform.GetChild (13).GetComponent<Text> ().text = Curscore.ToString(); 
		BallVelocityMovement(ballVelocity);


	}

	void BallVelocityMovement(Vector3 velocity){
		rb.velocity=velocity*speed;



	}
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Camera2"))
		{
			Camera3.enabled = false;
			Camera1.enabled = false;
			Camera2.enabled = true;
		}

		else if (other.gameObject.CompareTag("Camera3"))
		{
			Camera1.enabled = false;
			Camera2.enabled = false;
			Camera3.enabled = true;
		}


	}

	/*With this function we check the pins that have fallen from their position. Every time we find one pin 
	 * we increase the variable last so in the end we will have the sum of fallen pins*/
	public void Checkfall(){ // called in Reset
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("pins")) {
			//The rotation as Euler angles in degrees relative to the parent transform's rotation.
			if ((g.transform.localEulerAngles.x > 45 && g.transform.localEulerAngles.x<359 )|| (g.transform.localEulerAngles.z > 45  && g.transform.localEulerAngles.z<359))  {
 
				last++;
			}
			 
		}

		Notfallen = last;
		if (last == 10) { 
			if (m <10) {
				resetflag = true;
			}
		}


			newScore (Notfallen, NumofTry, bf, m);
		if (m == 10) {
			Combonewscore (Notfallen, bf, m);
		}
		Debug.Log (Notfallen);
		last = 0;
	}

	/*Resetting ball's position. Additionally we disable pin objects that have fallen on each try
	 * When player reaches the second try or strikes we reset the whole set of pins */
	public void ResetBall( ) //called in Reset
	{
		
		 if (m == 10) {
			m++;
		}
		this.transform.position = ball.position; //we have created an object which inherits the ball's initial position(ball2)
		this.transform.rotation = ball.rotation;
		ballVelocity = Vector3.zero;

		foreach (GameObject g in GameObject.FindGameObjectsWithTag("pins")) {
			g.GetComponent<MyPins> ().DisappearPin (); //any pin that fell down 
		}

		if (resetflag == true) {//valid only for second try or strike
			ResetPins ();

		
			resetflag = false;
			if (m != 9) {
				m++;//next frame
				NumofTry = 0;
			} else if (m == 9) { // ean kanoume strike sto 10o frame tote sinexizoume sti deuteri prospathia , ean erxomaste apo ti deuteri prospathia tote proxorame frame 
				if (NumofTry == 0) {
					NumofTry++;
					resetflag = true;
				}
				else
					m++;
			} 
		} else {
			NumofTry++;
			resetflag = true;
		}

		Camera1.enabled = true;
		Camera2.enabled = false;
		Camera3.enabled = false;
		strike = false;
		spare = false;


		if(m==11){

				gameObject.GetComponent<Rigidbody> ().isKinematic = true; // don't movw player 1
				itsend = true;
				Debug.Log ("END OF GAME");
			}

		

	}


		


	/*Destroy any pin except the prefab and create a new set of pins. The new set will have the 
	previous positions of the pins as we saved them in two arrays on Start()*/
	public void ResetPins()//called in Resetball
	{

		foreach (GameObject g in GameObject.FindGameObjectsWithTag("pins")) {
			if (g != prefab) {
				Destroy (g); //destroy any other pins that are  standing 
			}
		}

		for (i = 0; i < 10; i++) {
			Instantiate (prefab, position [i], q [i]); // create ten new ones 
		}

	}





	public struct BowlingFrame 
	{
	public int[] score;
	public int curScore;
	public bool strike;
	public bool spare;
	

	}

	public void InitializeFrames(BowlingFrame[] bf){
	
		for (int i = 0; i < 11; i++) {
			bf [i].score = new int[2];
			bf [i].score [0] = 0;
			bf [i].score [1] = 0;
			bf [i].strike = false;
			bf [i].spare = false;
			bf [i].curScore = 0;


		}
	
	}

	public void newScore(int fallen,int NumofTry,BowlingFrame[] bf ,int m)
	{
		int i = NumofTry % 2;
		bf [m].score [i] = fallen; // arxika prosthetw tis korines pou epesan

		if(i==0 && fallen==10) // strike
		{
			bf [m].strike = true; // it's been a strike
			strike=true;
			if (m == 9) {
				combotry = true;
			}
			if (m <9) {
				changeplayer = true;
			} 
				
			Debug.Log("Strike");
			//strike++;
		}
		else if(i==1 ) // spare anyways
		{
			if (fallen == 10 || bf [m].score [0] + bf [m].score [1] == 10) {
				bf [m].spare = true;
				spare = true;
				if (m == 9) {
					combotry = true;
				}
				if (m < 9) {
					changeplayer = true;
				} 
				Debug.Log ("Spare");
			}
		}


		//check the previous shots
		//den exw kanei strike twra kai exw kanei strike prin dio frames
		if (i == 0) { // proto try 
			if (m >= 2) {

				if (bf [m].strike) {// ean exw kanei strike i oxi 
					if (bf [m - 2].strike && bf [m - 1].strike) { // kai exw kanei strike kai stis dio alles 
						bf [m - 2].curScore = 30;
					} else if (bf [m - 2].strike == false && bf [m - 1].strike) {//ekana strike mono sto proigoumeno frame
						bf [m - 1].curScore = 0;
					} 

				
				} else if (m >= 2) { //den ekana strike se auto to frame
					if (bf [m - 2].strike && bf [m - 1].strike) { // kai exw kanei strike kai stis dio alles 
						bf [m - 2].curScore = 20 + bf [m].score [0];
					} else if (bf [m - 2].strike == false && bf [m - 1].strike) {//ekana strike mono sto proigoumeno frame
						//do nothing 
					}


				} 
			}
			if (m >= 1) {

				if (bf [m - 1].spare) {
					bf [m - 1].curScore = 10 + bf [m].score [0];
				}
			}
			
		
		} else if (i == 1) {//deutero try
			if (m != 9) {
				changeplayer = true;
			}
			if (m >= 2) {
				if ( bf [m - 1].strike) {//ean ekana strike sto proigoumeno frame kai oxi tora
					bf [m - 1].curScore = 10 + bf [m].score [0] + bf [m].score [1]; // update previous frame's score
				}
		
			}

			if (m >= 1) {
				if ((m-1)==0 && bf [m - 1].strike) {//ean ekana strike sto proigoumeno frame kai oxi tora
					bf [m - 1].curScore = 10 + bf [m].score [0] + bf [m].score [1]; // update previous frame's score
				}
			}
		
		}

		if (bf[m].strike==false && bf[m].spare==false) {
			bf [m].curScore = bf [m].score [0] + bf [m].score [1]; // bazw sto current score tis korines pou petixa
			Debug.Log("Curscorefunc " + bf[m].curScore);	
			}




	}


	public int CalculateScore(){
		int cur = 0;
		for (int i = 0; i < 11; i++) {
			cur += bf [i].curScore;
		}

		return cur;
	}

	public void PrintScore(){

		for (int i = 0; i <= m; i++) {
			if (bf [i].strike) {
				if (i != 9) {
					scoreboard.transform.GetChild (i).GetChild (0).GetComponent<Text> ().text = "X";
					scoreboard.transform.GetChild (i).GetChild (1).GetComponent<Text> ().text = " ";
				} else {
					scoreboard.transform.GetChild (i).GetChild (0).GetComponent<Text> ().text = "X";
					scoreboard.transform.GetChild (i).GetChild (1).GetComponent<Text> ().text = bf [i].score [1].ToString ();
				}
			} else if (bf [i].spare) {
				if (bf [i].score [0] != 0) {
					scoreboard.transform.GetChild (i).GetChild (0).GetComponent<Text> ().text = bf [i].score [0].ToString ();
				} else
					scoreboard.transform.GetChild (i).GetChild (0).GetComponent<Text> ().text = "-";
				
				scoreboard.transform.GetChild (i).GetChild (1).GetComponent<Text> ().text = "/";
			} else {
				if (bf [i].score [0] != 0) {
					scoreboard.transform.GetChild (i).GetChild (0).GetComponent<Text> ().text = bf [i].score [0].ToString ();
				} else
					scoreboard.transform.GetChild (i).GetChild (0).GetComponent<Text> ().text = "-";
				if (bf [i].score [1] != 0) {
					scoreboard.transform.GetChild (i).GetChild (1).GetComponent<Text> ().text = bf [i].score [1].ToString ();
				} else
					scoreboard.transform.GetChild (i).GetChild (1).GetComponent<Text> ().text = "-";
			}
			//if (i != m)
			//	scoreboard.transform.GetChild (i).GetChild (2).GetComponent<Text> ().text = bf [i].curScore.ToString ();
			

		}
		int score2=0;
		for (int p = 0; p < m; p++) {
			for (int k = 0; k <=p; k++) {

				score2+=bf[k].curScore;
			}
			scoreboard.transform.GetChild (p).GetChild (2).GetComponent<Text> ().text = score2.ToString();

			score2 = 0;
		}




	}

	public void ChecknumPlayers(){ // if the player fills the name for second player we play for twosdfd
		if(player2==false)
		player2=true;
	}

	public void Startover(){
		combotry = false;
		itsend = false;
		//ResetBall ();
		this.transform.position = ball.position; //we have created an object which inherits the ball's initial position(ball2)
		this.transform.rotation = ball.rotation;
		ballVelocity = Vector3.zero;
		InitializeFrames(bf);
		ResetPins ();
		MouseMovement.done = false;
		m = 0;
		NumofTry = 0;
		strike = false;
		spare = false;
		gameObject.GetComponent<Rigidbody> ().isKinematic = false;
		Curscore = 0;
		InitializeScoreboard ();
	}

	public void InitializeScoreboard(){
		for (int i = 0; i < 10; i++) {
			scoreboard.transform.GetChild (i).GetChild (0).GetComponent<Text> ().text = " ";
			scoreboard.transform.GetChild (i).GetChild (1).GetComponent<Text> ().text = " ";
			scoreboard.transform.GetChild (i).GetChild (2).GetComponent<Text> ().text = " ";

		}
		scoreboard.transform.GetChild (10).GetComponent<Text> ().text = " ";
	}

	//-------------------------------------------------------------------------------------------------
	//EXTRA FUNCTION FOR 10TH FRAME!! 

	public void Combonewscore(int fallen,BowlingFrame[] bf ,int m){
		Debug.Log ("MPIKE COMBO");
		if (combotry) {
			Debug.Log ("combotry true");
			if (fallen == 10) { // strike
				bf [m].strike = true; // it's been a strike
				strike = true;
				changeplayer = true;
				Debug.Log ("Strike");
				//strike++;
			}

			bf [m].score [0] = fallen;
			bf [m].curScore = bf [m].score [0];
			if (bf [m].strike) {
				scoreboard.transform.GetChild (m).GetComponent<Text> ().text = "X";
			} else {
				scoreboard.transform.GetChild (m).GetComponent<Text> ().text = fallen.ToString ();
				Curscore = CalculateScore ();
	
			}
		} else {
			rb.isKinematic = true;
			MouseMovement.done = true;
			Curscore = CalculateScore ();
			//m++;
			//ResetBall ();
		}
	
	}


}


