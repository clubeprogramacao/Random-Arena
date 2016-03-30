using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class combat_script : NetworkBehaviour {


	public GameObject bomb;

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
	public string lasthitter; 

	[SyncVar] 
	public int numberOfBombs;

	[SyncVar] 
	public int bombDamage;

	//    ====================    UI objects for display on the client    ====================    //



	public Text healthText;



	//    ====================    Client Functions    ====================    //



	// Use this for initialization
	void Start () {

		if (isClient) {

			healthText = GameObject.Find ("Health").GetComponent <Text> ();

		}
		if (!isServer)
			return;
		

		maxHealth = 10;
		health = maxHealth;
		attack = 2;
		invincTimer = 0;
		numberOfBombs = 10;
		bombDamage = 3;
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

		if (Input.GetKeyDown (KeyCode.Mouse1)) {
			Cmd_spawnBomb ();
		}

		if (isServer && health <= 0) {
			respawn ();
		}
	}

	// runs whenever a tear hits this gameobject
	public void OnTearHit(GameObject tearHit){
		if (!isServer)
			return;
		float tearDamage = tearHit.GetComponent<tear_script> ().tearStrength;
		string tearShooter =  tearHit.GetComponent<tear_script> ().shooter;
		Vector2 tearDirection = tearHit.GetComponent<tear_script> ().direction;
		if(invincTimer <= 0)
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


	public void OnBombPickHit(){
		if (!isServer)
			return;
		Cmd_changeBombs ();
	}

	public void OnBombExplosion(int damage){
		if (!isServer)
			return;
		Cmd_changeHealth (damage);
	}

	//    ====================    Server Functions    ====================    //

	[Server]
	void respawn(){
		
		health = maxHealth;
		transform.position = Vector3.zero;
		GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		//Rpc_respawn ();
	}

	// changes player health (to values between 0 and maxHealth)
	[Command]
	void Cmd_changeHealth(float healthDiff){


		if (healthDiff < 0) { // in case of damage
			if (invincTimer <= 0) { // in case of not invincible
				health += healthDiff;
				invincTimer = 1; 
			}
		} else { // in case of heal
			health += healthDiff;
		}
		// crop health overflow
		if (health > maxHealth)
			health = maxHealth;

		// death
		if (health <= 0) {
			health = 0;
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
		numberOfBombs++;
	}

	[Command]
	void Cmd_spawnBomb(){
		if (numberOfBombs > 0) {
			numberOfBombs--;
			GameObject newBomb = (GameObject)Instantiate (bomb, transform.position, Quaternion.identity);
			newBomb.GetComponent<bomb_script> ().timer = 1;
			newBomb.GetComponent<bomb_script> ().damage = bombDamage;
			NetworkServer.Spawn (newBomb);
		}
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
