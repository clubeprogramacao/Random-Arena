using UnityEngine;
using System.Collections;

public class bullet_script : MonoBehaviour {

	private bool destroy;
	// Use this for initialization
	void Start () {
		destroy = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (destroy) {
			Destroy(gameObject);
		}
	}
	
	void OnCollisionEnter2D(Collision2D other){
		if(other.collider.tag == "Enemy"){
			other.gameObject.GetComponent<Enemy_script_1>().changeHP(-50);
		}
		if(other.collider.name != "Player")
			destroy = true;
	}
}
