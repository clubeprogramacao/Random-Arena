using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Heart_script : NetworkBehaviour {


	public int health;

	// Use this for initialization
	void Start () {
		if (!isServer)
			return;
		gameObject.name = "Heart ";
		health = 1;
	}


	void OnCollisionEnter2D(Collision2D other)
	{
		if(isServer)
			Cmd_heartCollision (other);
	}

	[Server]
	void Cmd_heartCollision(Collision2D other){
		if (other.gameObject.tag == "Player")
		{
			if (other.gameObject.GetComponent<combat_script> ().health < other.gameObject.GetComponent<combat_script> ().maxHealth) {
				other.gameObject.SendMessage ("OnHeartHit", gameObject);
				Destroy(gameObject);
			}
		}
	}

}