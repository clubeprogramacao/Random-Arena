using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class playerShoot_script : NetworkBehaviour {

	public GameObject[] tear; // prefab that will become the newTear

	[SyncVar]
	public int tearSelecter;

	[SyncVar]
	public float displacementH;

	[SyncVar]
	public float displacementV; // so it doesnt spawn on top of player

	[SyncVar]
	public int tearSpeed; // unitys per second

	[SyncVar]
	public float tearDistance; // seconds

	[SyncVar]
	public float tearRate;   // tears per second

	[SyncVar]
	public float tearTimer; // cooldown on tear

	[SyncVar]
	public int facing;


	void Start()
	{
		if (!isLocalPlayer || !isServer)
			return;
		/* Uncomment to override Default setup
		tearSpeed = 10;
		tearDistance = 20;
		tearRate = 1.5f;
		*/
		tearSelecter = 1;
		tearTimer = 0;
		facing = 2;
		tearSelecter = 0;
	} // Start

	// Update is called once per frame
	void Update () 
	{

        if(isServer)
            decreaseTimer();

		if (!isLocalPlayer)
			return;

		// check direction (mouse pos)
		facing = getDirection (Input.mousePosition, transform.position);
		Cmd_updateDirection (facing);
		// fire tear
		if (Input.GetKey (KeyCode.Mouse0)) {
			Cmd_shootTear (facing);
		}
        // decrease timers
    }// Update


	// returns direction the player is shooting (2=Down | 4=Left | 6=Right | 8=Up)
	static int getDirection (Vector3 mouse, Vector3 player){
		mouse = Camera.main.ScreenToWorldPoint (new Vector3(mouse.x, mouse.y,mouse.z));
		var diff = new Vector2 (mouse.x - player.x, mouse.y - player.y);
		if (Mathf.Abs (diff.y)  >= Mathf.Abs (diff.x)) { // up or down*
			if (diff.y > 0) { // up
				return 8;
			} else // down
				return 2;
		} 
		else { // left or right* 
			if (diff.x < 0) { // left
				return 4;
			} else // right
				return 6;
		}
	} // getDirection

    //    ====================    Server Functions    ====================    //
    [Server]
    void decreaseTimer()
    {
        if (tearTimer > 0)
        {
            tearTimer -= Time.deltaTime;
        }
            if (tearTimer < 0)
            tearTimer = 0;
        
    }


    // shoots the tear on the server, and the server spawns it on every client (including this one)
    [Command]
	void Cmd_shootTear(int dir){

        if (tearTimer > 0)
            return;
		tearTimer = 1 / tearRate;
		GameObject newTear;
        Vector2 palyerVelocity = GetComponent<Rigidbody2D>().velocity; 
        Vector2 direction2;
		Vector3 direction3;
		float displacement;
		int speed = GetComponent<playerMovement_script> ().playerSpeed;

		switch (dir){
		case 2: // down
			direction2 = new Vector2(palyerVelocity.x/speed, Vector2.down.y);
            if(palyerVelocity.y < -5)
            {
                direction2 += new Vector2(0f, -0.4f);
            }
            direction3 = Vector3.down;
			displacement = displacementV;
			break;
		case 4: // left
			direction2 = new Vector2(Vector2.left.x , palyerVelocity.y/speed);
            if (palyerVelocity.x < -5)
            { 
                direction2 += new Vector2(-0.4f, 0f);
                }
            direction3 = Vector3.left;
			displacement = displacementH;
			break;
		case 6: // right
			direction2 = new Vector2(Vector2.right.x , palyerVelocity.y/speed);
            if (palyerVelocity.x > 5)
            {
                direction2 += new Vector2(0.4f, 0f);
                }
            direction3 = Vector3.right;
			displacement = displacementH;
			break;
		case 8: // up
			direction2 = new Vector2(palyerVelocity.x/speed, Vector2.up.y);
            if (palyerVelocity.y > 5)
            {
                direction2 += new Vector2(0f, 0.4f); 
                }
            direction3 = Vector3.up;
			displacement = displacementV;
			break;
		default:
			direction2 = Vector2.down;
			direction3 = Vector3.down;
			displacement = displacementV;
			break;
			
		} // switch
		newTear = (GameObject)Instantiate (tear[0], transform.position + direction3 * displacement, Quaternion.identity);
		newTear.GetComponent<Rigidbody2D> ().AddForce (direction2 * tearSpeed, ForceMode2D.Impulse);
		newTear.GetComponent<tear_script> ().direction = direction2;

        newTear.GetComponent<tear_script> ().shooter = gameObject.name;
		newTear.GetComponent<tear_script> ().tearStrength = gameObject.GetComponent<combat_script> ().attack;
		Destroy (newTear, tearDistance);
		NetworkServer.Spawn (newTear);
	} // Cmd_shootTear

	[Command]
	void Cmd_updateDirection(int dir){
		facing = dir;
	}


}// playerShoot_script
