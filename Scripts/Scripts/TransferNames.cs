using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransferNames : MonoBehaviour {
	public Text fromfield;
	public void Transferpl(){
		gameObject.GetComponent<Text> ().text = fromfield.text;
	}
}
