using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

	public void Pausegame(){
		Time.timeScale = 0;
	}

	public void Unpausegame(){
		Time.timeScale=1;
	}


}
