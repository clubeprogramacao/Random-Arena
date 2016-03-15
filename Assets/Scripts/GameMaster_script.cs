using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameMaster_script :NetworkBehaviour {


	// GAMEMASTER WILL ONLY BE ON SERVER

	public GameObject[] obj;
	private int itemSpawnTime;
	private float itemSpawnCountdown;


	// Use this for initialization

	void Start () {
		if (!isServer)
			return;
		itemSpawnTime = 1;
		itemSpawnCountdown = itemSpawnTime;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isServer)
			return;

		if (checkSpawnTime ()) {
			Cmd_spawnBomb (new Vector2(Random.Range (-20,20),Random.Range (-10,10)));

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
		GameObject newObj = (GameObject)Instantiate (obj[1], trans, Quaternion.identity);
		NetworkServer.Spawn (newObj);
	}
}
