using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class main_menu_script : MonoBehaviour {

	public void newGame(){
		Application.LoadLevel ("SinglePlayer");
	}
	
	public void exit(){
		Application.Quit ();
	}
}
