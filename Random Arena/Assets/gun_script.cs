using UnityEngine;
using System.Collections;

public class gun_script : MonoBehaviour {

	public bool hasGun;
	public GameObject player;
	public Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		hasGun = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (player.GetComponent<Player_script> ().weapon == "gun")
			hasGun = true;
		else
			hasGun = false;

		if (hasGun) {
			if (Input.GetKeyDown ("space") && player.name == "Player") {
				rb2d.AddForce (Vector2.right * 1009, ForceMode2D.Impulse);
			}

		}
	}
}
