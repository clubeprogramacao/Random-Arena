using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class bomb_script : NetworkBehaviour {

	[SyncVar]
	public float timer;

	[SyncVar]
	public int damage;

	[SyncVar]
	public float radius;

	[SyncVar]
	public float knockback;

	// Use this for initialization
	void Start () {
		gameObject.name = "Bomb";
		radius = 5;
		knockback = 1000;
	}
	/*
	[Server]
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			other.SendMessage ("OnBombExplosion",-damage);
		}
	}
*/
	void Update(){
		if (!isServer)
			return;

		if (timer > 0) {
			timer -= Time.deltaTime;
		} else {
			//gameObject.GetComponents <CircleCollider2D> ()[1].enabled = true;
			Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, radius);
			foreach (Collider2D col in colliders) {
				
				Vector2 dist = (col.transform.position - transform.position);
                if (col.tag == "Rock") 
                {
                    col.SendMessage("OnBombHit", dist);
                }
				if (col.attachedRigidbody != null)
				    col.GetComponent<Rigidbody2D> ().AddForce (dist.normalized*knockback,ForceMode2D.Impulse);

				if (col.GetComponent<combat_script> () != null) {
					col.SendMessage ("OnBombExplosion", -damage);
				}
			}
			Destroy (gameObject);
		}
	}

}