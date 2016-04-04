using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class bombPick_script : NetworkBehaviour {



	// Use this for initialization
	void Start () {
		if (!isServer)
			return;
			
		gameObject.name = "Bomb Pick";

	}


	void OnCollisionEnter2D(Collision2D other)
	{
		if(isServer)
			Cmd_fakeHeartCollision (other);
	}

	[Server]
	void Cmd_fakeHeartCollision(Collision2D other){
		if (other.gameObject.tag == "Player")
		{
			other.gameObject.SendMessage ("OnBombPickHit");
			Destroy(gameObject);
		}
	}

}