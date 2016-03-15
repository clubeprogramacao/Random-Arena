using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class bomb_script : NetworkBehaviour {

	[SyncVar]
	public float timer;

	[SyncVar]
	public int damage;

	// Use this for initialization
	void Start () {
		gameObject.name = "Bomb";
	}

	[Server]
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			other.SendMessage ("OnBombExplosion",-damage);
		}
	}

	void Update(){
		if (timer > 0) {
			timer -= Time.deltaTime;
		} else {
			gameObject.GetComponents <CircleCollider2D> ()[1].enabled = true;
			Destroy (gameObject,0.3f);
		}
	}

}