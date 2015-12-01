using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Game_Master_script : MonoBehaviour {

	public float p1Score;
	public float p2Score;
	public Text p1score;
	public Text p2score;

	public GameObject player1;
	public GameObject player2;

	public GameObject zombie1, zombie2, zombie3, zombie4, zombie5, zombie6;

	public GameObject gameoverUI;
	public int noPlayers, noEnemies;

	private int zombieKillScore;
	private int playerKillScore;
	// Use this for initialization
	void Start () {
		p1Score = 0;
		p2Score = 0;

		noPlayers = 2;
		noEnemies = 6;
		zombieKillScore = 100;
		playerKillScore = 500;

		gameoverUI.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		p1score.text = "Score: " + p1Score;
		p2score.text = "Score: " + p2Score;
		if (noPlayers <= 1 ) {
			gameoverUI.SetActive (true);
		}
	}

	public void zombieKilled(string killer){
		noEnemies--;
		if (player1.name == killer)
			p1Score += zombieKillScore;
		if (player2.name == killer)
			p2Score += zombieKillScore;
	}

	public void playerKilled(string killer){
		noPlayers--;
		if (player1.name == killer)
			p1Score += playerKillScore;
		if (player2.name == killer)
			p2Score += playerKillScore;
	}

}
