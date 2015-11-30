using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class pause_menu : MonoBehaviour {

	public GameObject pauseUI;
	private bool paused = false;

	void start(){
		pauseUI.SetActive (false);
	}
	void Update(){
		if (Input.GetButtonDown ("Pause")) {
			paused = !paused;
		}
		if (paused) {
			pauseUI.SetActive (true);
			Time.timeScale = 0;
		} else {
			pauseUI.SetActive (false);
			Time.timeScale = 1;
		}
	}
	public void resume(){
		paused = false;
	}

	public void restart (){
		Application.LoadLevel (Application.loadedLevel);
	}

	public void exit(){
		Application.Quit ();
	}
	public void mainMenu(){
		Application.LoadLevel ("Main Menu");
	}
}
