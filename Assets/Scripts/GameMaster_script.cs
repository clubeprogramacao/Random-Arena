using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameMaster_script :NetworkBehaviour {


	// GAMEMASTER WILL ONLY BE ON SERVER

	// Use this for initialization
	void Start () {
		if (!isServer)
			return;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isServer)
			return;
	}
}
