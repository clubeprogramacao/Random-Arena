using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class FakeHeart_script : NetworkBehaviour {



	// Use this for initialization
	void Start () {
		if (!isServer)
			return;
			
		gameObject.name = "Bomb";

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
			other.gameObject.SendMessage ("OnBombHit");
			Destroy(gameObject);
		}
	}

}