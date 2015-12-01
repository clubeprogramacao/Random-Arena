using UnityEngine;
using System.Collections;

public class Knife_script : MonoBehaviour {

	public GameObject player;
	string playerName;
	public float damage;
	public string enemy;

	void Awake(){
		damage = 30f;
		enemy = "None";
		playerName = player.gameObject.name; 
	}
	void OnTriggerEnter2D(Collider2D other){
		enemy = other.gameObject.tag;
		if (other.gameObject.tag == "Enemy") {
			other.SendMessage("knifeDamage");
			other.SendMessage("lastHit",playerName);
		}

		if (other.gameObject.tag == "Player") {
			other.SendMessage("changeHP",-damage);
			other.SendMessage("lastHit",playerName);
		}
	}
}
