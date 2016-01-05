using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Game_Master_script : MonoBehaviour {

	public int noPlayers, noEnemies;

	public float p1Score;
	public Text p1scoret;
	public int p1score;

	public GameObject player1 , player2;

	public GameObject zombie1, zombie2, zombie3, zombie4, zombie5, zombie6;

	public GameObject gameoverUI;
	

	private int zombieKillScore;
	private int playerKillScore;
	// Use this for initialization
	void Start () {
		p1Score = 0;

		noPlayers = 1;
		noEnemies = 6;
		zombieKillScore = 100;
		playerKillScore = 500;

		gameoverUI.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		p1scoret.text = "Score: " + ((int)p1Score).ToString();
//S		p2scoret.text = "Score: " + p2Score;
		if (noEnemies == 0) {
			//gameoverUI.SetActive (true);

		}
	}

	public void zombieKilled(string killer){
		noEnemies--;
		if (player1.name == killer)
			p1Score += zombieKillScore;
	}

	public void playerKilled(string killer){
		noPlayers--;
		if (player1.name == killer)
			p1Score += playerKillScore;
	}

}
