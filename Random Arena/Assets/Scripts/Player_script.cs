using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;


public class Player_script : MonoBehaviour {

	// TODO: check wether to change some variables to private
	public GameObject gameMaster;

	// animations
	private Animator anim;    // controls variables of the sprite animations (idle / walk)

	// movement variables
	private Rigidbody2D rb2d; // link to player physics. Recieves forces, has velocity
	public int maxSpeed; // max velocity player can move (external forces included)
	public int walkSpeed; // max speed player can make himself walk
	public int startWalkSpeed; // acceleration increments when player starts to walk
	public float Speed_X; // last frames' player horizontal speed
	public float Speed_Y; // last frames' player vertical speed
	public float h; // horizontal inputs < -1 | 0 | +1 >
	public float v; // vertical inputs 	 < -1 | 0 | +1 >
	public bool paralyzed; // makes player unable to walk when true

	// hp related stuff
	public int HP;    // player health points
	public bool takingDamage; // to create a red flash animation
	public GameObject hp_bar; // green  hp bar over the player sprite
	public GameObject GameOverUI; // enables / disables the gameover screen when player dies (0 HP)

	// Texts
	public Text text_HP; /// current HP displayed on canvas
	public string lastHitter;

	void Start () 
	{
		GameOverUI.SetActive (false);
		rb2d = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator> ();

		maxSpeed = 500;
		walkSpeed = 300;
		startWalkSpeed = 50;
		paralyzed = false;
		
		HP = 100;
		takingDamage = false;

		updateText ();
	}
	
	// Update is called once per frame

	void FixedUpdate () 
	{
		h = Input.GetAxisRaw ("p1h");
		v = Input.GetAxisRaw ("p1v");
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
		// speed_east
		// speed_south
		// speed_west
		// speed_north
		// hurt trigger
		if (Speed_X >= 0) {
			anim.SetFloat ("speed_east", (float)Speed_X);
			anim.SetFloat ("speed_west", (float)0);
		}
		if (Speed_X < 0) {
			anim.SetFloat ("speed_east", (float)0);
			anim.SetFloat ("speed_west", (float)-Speed_X);
		}
		if (Speed_Y >= 0) {
			anim.SetFloat ("speed_north", (float)Speed_Y);
			anim.SetFloat ("speed_south", (float)0);
		}
		if (Speed_Y < 0) {
			anim.SetFloat ("speed_north", (float)0);
			anim.SetFloat ("speed_south", (float)-Speed_Y);
		}
		if (takingDamage)
			anim.SetTrigger ("hurt");
		takingDamage = false;
	}

	void updateText()
	{
		text_HP.text = "HP: " + HP.ToString();
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
	void changeHP(int change)
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

	void redFlames()  {changeHP (-10);}
	void greenFlames() {changeHP (10);}

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

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Red_Flair")
			InvokeRepeating("redFlames",0.01f,0.5f);

		if(other.gameObject.tag == "Green_Flair")
			InvokeRepeating("greenFlames",0.01f,0.25f);

	}
	
	void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.tag == "Red_Flair")
			CancelInvoke(methodName:"redFlames");

		if(other.gameObject.tag == "Green_Flair")
			CancelInvoke(methodName:"greenFlames");
	}

	void gameover()
	{
		gameMaster.SendMessage ("playerKilled", lastHitter);
		Destroy (gameObject);
		//GameOverUI.SetActive (true);
	}




}






















