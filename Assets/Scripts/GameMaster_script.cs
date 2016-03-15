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

	[ServerCallback]
	void Start () {
		if (!isServer)
			return;
		itemSpawnTime = 1;
		itemSpawnCountdown = itemSpawnTime;
	}
	
	// Update is called once per frame
	[ServerCallback]
	void Update () {
		if (!isServer)
			return;

		if (checkSpawnTime ()) {
			spawnItem (obj[1],new Vector2(Random.Range (-20,20),Random.Range (-10,10)));

		}
	}

	[ServerCallback]
	bool checkSpawnTime(){
		if (itemSpawnCountdown > 0) {
			itemSpawnCountdown -= Time.deltaTime;
			return false;
		} else {
			itemSpawnCountdown = itemSpawnTime;
			return true;
		}
	}

	[ServerCallback]
	void spawnItem(GameObject spawnObj, Vector2 trans){
		GameObject newOnj = (GameObject)Instantiate (spawnObj, trans, Quaternion.identity);
		NetworkServer.Spawn (newOnj);
	}
}
