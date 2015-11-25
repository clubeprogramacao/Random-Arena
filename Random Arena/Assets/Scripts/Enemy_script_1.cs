using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;


public class Enemy_script_1 : MonoBehaviour {
	
	// TODO: check wether to change some variables to private
	
	// animations
	private Animator anim;    // controls variables of the sprite animations (idle / walk)
	private int redFlashTime; // duration of damage indication (ms)
	
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
	
	// Texts
	public Text text_X; // current horizontal speed displayed on canvas
	public Text text_Y;	// current vertical speed displayed on canvas
	public Text text_HP; /// current HP displayed on canvas
	
	
	void Start () 
	{
		rb2d = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator> ();
		
		redFlashTime = 20; // ms
		maxSpeed = 500;
		walkSpeed = 150;
		startWalkSpeed = 50;
		paralyzed = false;
		
		HP = 100;
		takingDamage = false;

		InvokeRepeating ("getMovement",0.25f, 0.75f);
	}
	
	// Update is called once per frame
	
	void FixedUpdate () 
	{

	}
	
	void Update () 
	{
		changeHP (0);
		updateAnim ();
	}

	void getMovement()
	{
		h = Random.value*2-1;
		v = Random.value*2-1;
		if (h == 0 && v == 0)
			getMovement ();
		move ();
	}

	void updateAnim()
	{
		anim.SetFloat ("Speed_X", (float)Speed_X);
		anim.SetFloat ("Speed_Y", (float)Speed_Y);
		anim.SetBool ("Paralyzed", (bool)paralyzed);
		anim.SetBool ("Damaged", (bool)takingDamage);
	}
	
	void move()
	{
		Speed_X = (int)rb2d.velocity.x;
		Speed_Y = (int)rb2d.velocity.y;
		limitSpeed (maxSpeed);
		// external forces paralyze, stopping un-paralyzes
		if(paralyzed == true){
			if(Mathf.Abs(rb2d.velocity.x) <= 40 && Mathf.Abs(rb2d.velocity.y) <= 40)
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
			anim.SetTrigger("damaged");
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
	void greenFlames() {changeHP (5);}
	
	void OnTriggerStay2D(Collider2D other)
	{
		// add code
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Arena_Wall")
		{
			changeHP(-10);
			if(other.GetComponent<Walls_script>().wall == "North")
			{
				
				anim.SetTrigger("damaged");
				Speed_Y = -maxSpeed;
				limitSpeed(maxSpeed);
				rb2d.velocity = new Vector2(Speed_X,Speed_Y);
				paralyzed = true;
			}
		}
		
		
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
		Destroy (gameObject);
	}
	
	
	
	
}





















