using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class combat_script : NetworkBehaviour {



	//    ====================    Sync Variables (updated on Server)    ====================    //



	[SyncVar] 
	public float health; // current health

	[SyncVar] 
	public float maxHealth; // max health

	[SyncVar] 
	public float attack; // how much damage does the player have

	[SyncVar] 
	public float invincTimer; // time left of invincibility (0 if not invincible)

	[SyncVar] 
	public string lasthitter; // time left of invincibility (0 if not invincible)

	[SyncVar] 
	public int bombs;


	//    ====================    UI objects for display on the client    ====================    //



	public Text healthText;



	//    ====================    Client Functions    ====================    //



	// Use this for initialization
	void Start () {

		if (isClient) {

			healthText = GameObject.Find ("Text").GetComponent <Text> ();

		}
		if (!isServer)
			return;
		

		maxHealth = 10;
		health = maxHealth;
		attack = 2;
		invincTimer = 0;
		bombs = 0;

	}

	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer)
			return;
		
		// inviciTimer --
		if(invincTimer > 0){
			invincTimer -= Time.deltaTime;
		}
		healthText.text = "HP: " + health;
	}

	// runs whenever a tear hits this gameobject
	public void OnTearHit(GameObject tearHit){
		if (!isServer)
			return;
		float tearDamage = tearHit.GetComponent<tear_script> ().tearStrength;
		string tearShooter =  tearHit.GetComponent<tear_script> ().shooter;
		Vector2 tearDirection = tearHit.GetComponent<tear_script> ().direction;
		Cmd_knockBack (100f,tearDirection);
		Cmd_changeHealth (-tearDamage);
		lasthitter = tearShooter;
	}

	public void OnHeartHit(GameObject heart){
		if (!isServer)
			return;
		int heal = heart.GetComponent<Heart_script> ().health;
		Cmd_changeHealth (heal);
	}


	public void OnBombHit(){
		if (!isServer)
			return;
		Cmd_changeBombs ();
	}

	//    ====================    Server Functions    ====================    //



	// changes player health (to values between 0 and maxHealth)
	[Command]
	void Cmd_changeHealth(float healthDiff){
		if (health + healthDiff <= 0) {
			// he's dead
			health = maxHealth;
			transform.position = Vector3.zero;
			GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			Rpc_respawn ();
		} else if (health + healthDiff > maxHealth) {
			health = maxHealth;
		} else {
			health += healthDiff;
		}
	}

	// knockback on tear hit
	[Command]
	void Cmd_knockBack(float intensity, Vector2 direction){
		gameObject.GetComponent<Rigidbody2D> ().AddForce (direction*intensity,ForceMode2D.Impulse);
		//Rpc_knockBack (intensity,direction);
	}


	[Command]
	void Cmd_changeBombs(){
		bombs++;
	}


	//    ====================    Client Only Commands    ====================    //


	// sets pos to 0,0
	[ClientRpc]
	void Rpc_respawn(){
		if (isLocalPlayer) {
			transform.position = Vector3.zero;
			GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		}
	}

	// knockback on tear hit
	[ClientRpc]
	void Rpc_knockBack(float intensity, Vector2 direction){
		gameObject.GetComponent<Rigidbody2D> ().AddForce (direction*intensity,ForceMode2D.Impulse);
	} 


}
