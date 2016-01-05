using UnityEngine;
using System.Collections;

public class PlayerwKnife_script : MonoBehaviour {
	
	public GameObject knife_north;
	public GameObject knife_east;
	public GameObject knife_west;
	public GameObject knife_south;
	public GameObject player;
	public GameObject knife;

	private float attackCooldown;
	private float attackTimer;
	public bool hasKnife;
	public bool attacking;
	public int facing; // up 8 | down 2 | left 4 | right 6 //
	public float x;
	public float y;
	private string fireString;

	void Awake()
	{
		hasKnife = true;
		attacking = false;
		facing = 2;
		updateKnife(facing,false);
		attackTimer = 0f;
		attackCooldown = 0.3f;
		fireString = "mouse 0";
	}

	void Update()
	{
		if(player.GetComponent<Player_script>().weapon == "knife")
			knife.SetActive (hasKnife);
		getDirection ();
		if (Input.GetKeyDown (fireString) && attackTimer == 0 && player.name == "Player") {
				attacking = true;
				updateKnife (facing, true);
				attackTimer = attackCooldown;
			}
			if (attacking & attackTimer > 0) {
				attackTimer -= Time.deltaTime;
				if (attackTimer <= 0)
					attackTimer = 0;
			} else {
				attacking = false;
				updateKnife (facing, false);
			}
	}

	int getDirection(){
		if (player.name == "Player") {
			facing = (player.GetComponent<Player_script> ().direction);
		} else {
			Debug.Log("ERROR: no player found on getDirection on PlayerwKnife_script");
			return 2;
		}
		return facing;
	}

	void updateKnife(int dir, bool permission){
		knife_north.SetActive (false);  //
		knife_east.SetActive (false);
		knife_west.SetActive (false);
		knife_south.SetActive (false);
		switch (dir) {
		case 2:
			knife_south.SetActive (permission);
			return;
		case 4:
			knife_west.SetActive (permission);
			return;
		case 6:
			knife_east.SetActive (permission);
			return;
		case 8:
			knife_north.SetActive (permission);
			return;
		}
	}


}
