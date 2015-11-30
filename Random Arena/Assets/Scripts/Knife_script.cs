using UnityEngine;
using System.Collections;

public class Knife_script : MonoBehaviour {

	public float damage;
	public string enemy;

	void Awake(){
		damage = 30f;
		enemy = "None";
	}
	void OnTriggerEnter2D(Collider2D other){
		enemy = other.gameObject.tag;
		if (other.gameObject.tag == "Enemy") {
			other.SendMessage("changeHP",-damage);
		}

		if (other.gameObject.tag == "Player") {
			other.SendMessage("changeHP",-damage);
		}
	}
}
