using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMaster_script :NetworkBehaviour {


	// GAMEMASTER WILL ONLY BE ON SERVER
	public int testSelecter;
	public GameObject[] obj;
	private float itemSpawnTime;
	private float itemSpawnCountdown;

	// Use this for initialization

	void Start () {
		if (!isServer)
			return;
		itemSpawnTime = 0.75f;
		itemSpawnCountdown = itemSpawnTime;
		
		for (int i = -20; i<=20; i++){
			for (int j = -10; j <= 10; j++) {
				Cmd_spawnBomb (new Vector2(i,j));
			}
		}



	}
	
	// Update is called once per frame
	void Update () {
		if (!isServer)
			return;
		switch (testSelecter) {
		case 1:
			if (checkSpawnTime ()) {
				//Cmd_spawnBomb (new Vector2(Random.Range (-20,20),Random.Range (-10,10)));
				Cmd_spawnBomb (new Vector2(0,0));
			}
			return;
		case 2:

			return;
		default:

			return;
		}

	}

	bool checkSpawnTime(){
		if (itemSpawnCountdown > 0) {
			itemSpawnCountdown -= Time.deltaTime;
			return false;
		} else {
			itemSpawnCountdown = itemSpawnTime;
			return true;
		}
	}
    
    
	[Command]
	void Cmd_spawnBomb(Vector2 trans){
		//GameObject newObj = (GameObject)Instantiate (obj [1], trans, Quaternion.identity);
        /*
		newObj.GetComponent<bomb_script> ().damage = 1;
		newObj.GetComponent<bomb_script> ().radius = 3.5f;
		newObj.GetComponent<bomb_script> ().knockback = 1000;
		newObj.GetComponent<bomb_script> ().timer = 0.5f;*/
		//NetworkServer.Spawn (newObj);
	}
}
