using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Game_Master_script : MonoBehaviour {

	public int noPlayers, noEnemies;

	public float time;  // untampered time
	public float timeLeft; // time left untill end of round (horde mode)

	public float p1Score;
	public Text p1scoret;
	public Text victoryscore;
	public Text timeText;
	public int p1score;

	public GameObject player1;

	public GameObject zombie;

	public GameObject gameoverUI;
	public GameObject victoryUI;

	private int zombieKillScore;
	private int playerKillScore;
	// Use this for initialization
	void Start () {
		time = 0;
		timeLeft = 5;
		p1Score = 0;
		noPlayers = 1;
		noEnemies = 0;
		zombieKillScore = 100;
		playerKillScore = 500;
		gameoverUI.SetActive (false);
		createWave (8);

	}

	// Update is called once per frame
	void Update () {
		p1scoret.text = "Score: " + ((int)p1Score).ToString();
		timeText.text = "Next wave in: " + ((int)timeLeft).ToString() + " seconds!";
//S		p2scoret.text = "Score: " + p2Score;
		if (noEnemies == 0) {
			victoryscore.text = p1scoret.text;
			victoryUI.SetActive(true);
		}
		if (timeLeft <= 0) {
			createWave(4);
			timeLeft = 5;
		}
		time += Time.deltaTime;
		timeLeft -= Time.deltaTime;
	}

	void createWave(int nen){
		GameObject[] enemy = new GameObject[nen];
		for (int i = 0; i<nen; i+=4) {
			enemy[i] = Instantiate(zombie,new Vector3(150f,150f,0f),Quaternion.identity) as GameObject;enemy[i].SetActive(true);
			enemy[i+1] = Instantiate(zombie,new Vector3(-150f,150f,0f),Quaternion.identity) as GameObject;enemy[i+1].SetActive(true);
			enemy[i+2] = Instantiate(zombie,new Vector3(-150f,-150f,0f),Quaternion.identity) as GameObject;enemy[i+2].SetActive(true);
			enemy[i+3] = Instantiate(zombie,new Vector3(150f,-150f,0f),Quaternion.identity) as GameObject;enemy[i+3].SetActive(true);
		}
		noEnemies += nen;
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
		gameoverUI.SetActive (true);
	}

}
