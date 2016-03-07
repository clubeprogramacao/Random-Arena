using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;


public class playerMovement_script : NetworkBehaviour
{
	

	// movement variables
	public Rigidbody2D rb2d; // link to player physics. Recieves forces, has velocity
	/*
	// hp related stuff
	[SyncVar (hook ="updateHealth")]
	public int health;
	void updateHealth(int newHealth)    {    health = newHealth;    }

	[SyncVar (hook ="updateLastHitter")]
	public string lastHitter;
	void updateLastHitter(string newLasthitter)		{    lastHitter = newLasthitter;    }
	*/

	// make private when not debugging
	private int maxSpeed; // max velocity player can move (external forces included)
	public int playerSpeed; // acceleration increments when player starts to walk


	public float Speed_X; // last frames' player horizontal speed
	public float Speed_Y; // last frames' player vertical speed

	[SyncVar (hook = "updateH")]
	public float h; // horizontal inputs < -1 | 0 | +1 > // vertical inputs 	 < -1 | 0 | +1 >
	void updateH(float newH)	{	h = newH;	}
	[SyncVar (hook = "updateV")]
	public float v; // horizontal inputs < -1 | 0 | +1 > // vertical inputs 	 < -1 | 0 | +1 >
	void updateV(float newV)	{	v = newV;	}

    




	void Start () 
	{
        gameObject.name = "[Player]" + GetInstanceID().ToString();
		rb2d = GetComponent<Rigidbody2D>();
		//healthText = GameObject.Find("Text");

		maxSpeed = 100;
		/* Uncomment to override default values
		playerSpeed = 30;
        health = 100;
        */
	}
	
	// Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

       
    }

	void FixedUpdate () 
	{
        if (!isLocalPlayer)
            return;

		Cmd_getMovementInput ();
		Cmd_move ();
        limitSpeed(maxSpeed);
	}

	//[Command]
	void Cmd_getMovementInput(){
		h = Input.GetAxisRaw ("Horizontal");
		v = Input.GetAxisRaw ("Vertical");
		if (Mathf.Abs ((int)h) == 1 && Mathf.Abs ((int)v) == 1) {
			h *= Mathf.Cos (Mathf.PI / 4);
			v *= Mathf.Cos (Mathf.PI / 4);
		}
	}

	[Command]
	void Cmd_move()
	{
		rb2d.AddForce(new Vector2(h * playerSpeed, v * playerSpeed),ForceMode2D.Impulse);
		Speed_X = rb2d.velocity.x;
		Speed_Y = rb2d.velocity.y;
		Rpc_move ();
	}

	[ClientRpc]
    void Rpc_move()
    {
		rb2d.AddForce(new Vector2(h * playerSpeed, v * playerSpeed),ForceMode2D.Impulse);
        Speed_X = rb2d.velocity.x;
        Speed_Y = rb2d.velocity.y;

		
	}

	void limitSpeed(int speedMax)
	{
		// normalize
		if (Speed_X != 0 || Speed_Y != 0) {
			float length = Mathf.Sqrt (Speed_X*Speed_X + Speed_Y*Speed_Y);
			if(length > speedMax){
				Speed_X = speedMax * Speed_X / length;
				Speed_Y = speedMax * Speed_Y / length;
			}
		}
	}

    


}






















