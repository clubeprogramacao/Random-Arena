﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;


public class playerMovement_script : NetworkBehaviour
{



	//    ====================    Variables    ====================    //



	public Rigidbody2D rb2d; // link to player physics. Recieves forces, has velocity
    [SyncVar]
    public Vector2 room;

	[SyncVar]
	public int playerSpeed; // acceleration increments when player starts to walk

	[SyncVar]
	public float h; // horizontal inputs < -1 | 0 | +1 > // vertical inputs 	 < -1 | 0 | +1 >

	[SyncVar]
	public float v; // horizontal inputs < -1 | 0 | +1 > // vertical inputs 	 < -1 | 0 | +1 >

    

	//    ====================    Local Commands    ====================    //



	void Start () 
	{
        gameObject.name = "[Player]" + GetInstanceID().ToString();
		rb2d = GetComponent<Rigidbody2D>();
		playerSpeed = 33;
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

		getMovementInput ();
		//predictMovement ();
		Cmd_move ();
	}

	// checks for movement commands on keyboard
	void getMovementInput(){
		h = Input.GetAxisRaw ("Horizontal");
		v = Input.GetAxisRaw ("Vertical");
		if (Mathf.Abs ((int)h) == 1 && Mathf.Abs ((int)v) == 1) {
			h *= Mathf.Cos (Mathf.PI / 4);
			v *= Mathf.Cos (Mathf.PI / 4);
		}
		Cmd_setInputs (h, v);
	}

	// add forces to player right away
	void predictMovement(){
		if(!isServer)
		rb2d.AddForce(new Vector2(h * playerSpeed, v * playerSpeed),ForceMode2D.Impulse);
	}

    [Server]
    void OnTriggerStay2D(Collider2D col)
    {
        if(col.name == "Minimap")
        {
            room = col.transform.position+Vector3.up*4.5f;
            Rpc_changeCamera(room);

        }
        
    }

    [Server]
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "Minimap")
        {
            col.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);

        }
    }

    [Server]
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.name == "Minimap")
        {
            col.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 100);

        }

    }

    //    ====================    Server Only Commands    ====================    //


    // updates the movement variables on server
    [Command]
	void Cmd_setInputs(float newH, float newV){
		h = newH;
		v = newV;
	}

	// applies the force on the server gameobject, and messages all clients of its new position+velocity
	[Command]
	void Cmd_move()
	{
		rb2d.AddForce(new Vector2(h * playerSpeed, v * playerSpeed),ForceMode2D.Impulse);
		Rpc_move (rb2d.position, rb2d.velocity);
	}



	//    ====================    Client Only Commands    ====================    //


	// updates its movement 
	[ClientRpc]
	void Rpc_move(Vector2 newRb2dPos, Vector2 newvelocity)
    {
		if (isServer)
			return;
		//rb2d.position = Vector2.Lerp (rb2d.position, newRb2dPos, 0.01f);
		//rb2d.velocity = Vector2.Lerp (rb2d.velocity, newvelocity, 0.01f);
		//rb2d.AddForce(new Vector2(h * playerSpeed, v * playerSpeed),ForceMode2D.Impulse);

		rb2d.position = newRb2dPos;
		rb2d.velocity = newvelocity;
		
	}

    [ClientRpc]
    void Rpc_changeCamera(Vector2 pos)
    {
        if (!isLocalPlayer)
            return;
        Debug.Log(pos);
        GameObject cam = GameObject.Find("Main Camera");
        Vector3 camPos = cam.transform.position;
        Vector3 newPos = new Vector3(pos.x, pos.y, camPos.z);
        if (Mathf.Abs(Vector3.Magnitude(camPos - newPos)) <= 2) {
            camPos = newPos;
            
        }
        else {
            camPos = new Vector3(Mathf.Lerp(camPos.x, room.x, 0.3f), Mathf.Lerp(camPos.y, room.y, 0.5f), camPos.z);
        }
        cam.transform.position = camPos;
    }



}






















