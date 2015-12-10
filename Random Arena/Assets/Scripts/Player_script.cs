using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;


public class Player_script : MonoBehaviour {

	// TODO: check wether to change some variables to private
	public GameObject gameMaster;

	// animations
	//private Animator anim;    // controls variables of the sprite animations (idle / walk)
	private Animator anim;
	// movement variables
	private Rigidbody2D rb2d; // link to player physics. Recieves forces, has velocity
	public int maxSpeed; // max velocity player can move (external forces included)
	public int walkSpeed; // max speed player can make himself walk
	public int startWalkSpeed; // acceleration increments when player starts to walk
	public float Speed_X; // last frames' player horizontal speed
	public float Speed_Y; // last frames' player vertical speed
	public float h; // horizontal inputs < -1 | 0 | +1 >
	public float v; // vertical inputs 	 < -1 | 0 | +1 >
	public int direction; // direction player is facing (1,2,3,4,6,7,8 or 9)
	public bool paralyzed; // makes player unable to walk when true

	// hp related stuff
	public float HP;    // player health points
	public bool takingDamage; // to create a red flash animation
	public GameObject hp_bar; // green  hp bar over the player sprite
	public string lastHitter; // last player to hit this player

	// Texts
	public Text text_HP; /// current HP displayed on canvas

	public float pointerposx,pointerposy,playerposx,playerposy;

	void Start () 
	{
		rb2d = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator> ();

		maxSpeed = 500;
		walkSpeed = 300;
		startWalkSpeed = 50;
		direction = 2;
		paralyzed = false;
		
		HP = 100;
		takingDamage = false;

		updateText ();
	}
	
	// Update is called once per frame

	void FixedUpdate () 
	{
		if (gameObject.name == "Player") {
			h = Input.GetAxisRaw ("p1h");
			v = Input.GetAxisRaw ("p1v");
		}
		if (gameObject.name == "Player (2)") {
			h = Input.GetAxisRaw ("p2h");
			v = Input.GetAxisRaw ("p2v");
		}

		move ();
	}

	void Update () 
	{
		changeHP (0);
		updateAnim ();
		updateText ();
	}
	void updateAnim()
	{
		direction = 2;
		Vector3 pointerPos = Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
		pointerposx = pointerPos.x;
		pointerposy = pointerPos.y;

		Vector2 playerPos = new Vector2 (transform.position.x, transform.position.y);


		playerposx = playerPos.x;
		playerposy = playerPos.y;

		if (Mathf.Abs (pointerposy - playerPos.y) <= Mathf.Abs (pointerposx - playerPos.x)) {
			if (pointerposx > playerPos.x) {
				direction = 6;
			}
			if (pointerposx < playerPos.x) {
				direction = 4;
			}
		}
		if (Mathf.Abs (pointerposy - playerPos.y) > Mathf.Abs (pointerposx - playerPos.x)) {
			if (pointerposy > playerPos.y) {
				direction = 8;
			}
			if (pointerposy < playerPos.y) {
				direction = 2;
			}
		}
		anim.SetInteger ("Direction", direction);
		anim.SetBool("Moving",!(Mathf.Abs(rb2d.velocity.x) <= 80 && Mathf.Abs(rb2d.velocity.y) <= 80));
	}

	void updateText()
	{
		text_HP.text = "HP: " + ((int)HP).ToString();
	}

	void move()
	{
		Speed_X = (int)rb2d.velocity.x;
		Speed_Y = (int)rb2d.velocity.y;
		limitSpeed (maxSpeed);
		// external forces paralyze, stopping un-paralyzes
		if(paralyzed == true){
			if(Mathf.Abs(rb2d.velocity.x) <= 80 && Mathf.Abs(rb2d.velocity.y) <= 80)
				paralyzed = false;
		}
		else {
			Speed_X += (float)startWalkSpeed * h;
			Speed_Y += (float)startWalkSpeed * v;
			limitSpeed(walkSpeed);
			rb2d.velocity = new Vector2 (Speed_X , Speed_Y);
		}
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

	// calls updateAnim() + gameover()
	void changeHP(float change)
	{
		if (change > 0) {
			// add interaction when healed
			takingDamage = false;
		}
		if (change == 0) {
			takingDamage = false;
		}
		if (change < 0) {
			// add interaction when damaged
			takingDamage = true;
		}
		HP += change;
		
		if (HP >= 100) {
			// when fully healed
			HP = 100;
		}
		if (HP <= 0) {
			// when health fully depletes
			HP = 0;
			gameover();
		}
		// changes size of hp bar sprite
		hp_bar.transform.localScale = new Vector2 ((float)(HP/100f),hp_bar.transform.localScale.y);
		updateAnim ();
	}


	public void lastHit(string killer){
		lastHitter = killer;
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if(other.gameObject.tag == "Arena_Wall")
		{
			changeHP(-10);
			if(other.gameObject.name == "North Wall")
			{
				Speed_Y = -maxSpeed;
				limitSpeed(maxSpeed);
				rb2d.velocity = new Vector2(Speed_X,Speed_Y);
				paralyzed = true;
			}
		}
		if (other.gameObject.tag == "Enemy") {
			changeHP (-5);
		}
	}


	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag == "Red_Flair")
			changeHP (-1f);
		if (other.gameObject.tag == "Green_Flair")
			changeHP (1f);

	}



	void gameover()
	{
		gameMaster.SendMessage ("playerKilled", lastHitter);
		Destroy (gameObject);
	}




}






















