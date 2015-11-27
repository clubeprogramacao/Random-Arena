using UnityEngine;
using System.Collections;

public class PlayerwKnife_script : MonoBehaviour {
	
	public Collider2D knife_east;
	public GameObject player;

	private float attackCooldown;
	private float attackTimer;
	public bool hasKnife;
	public bool attacking;
	public int facing; // up 8 | down 2 | left 4 | right 6 //

	void Awake()
	{
		hasKnife = true;
		attacking = false;
		knife_east.enabled = false; //
		attackTimer = 0f;
		attackCooldown = 0.3f;
		facing = 6;
	}

	void Update()
	{
		if(Input.GetKeyDown("space") && attackTimer == 0 && facing == 6){
			attacking = true;
			knife_east.enabled = true;  //
			attackTimer = attackCooldown;
		}
		if (attacking & attackTimer > 0) {
			attackTimer -= Time.deltaTime;
			if (attackTimer <= 0)
				attackTimer = 0;
		}
		else{
			attacking = false;
			knife_east.enabled = false; //
		}
	}
	/*
	int getDirection(){
		float x = player.GetComponent<Player_script> ().Speed_X;
		float y = player.GetComponent<Player_script> ().Speed_Y;

		if( x > y && x > 0){

		}
		return 6;
	}*/


}
