using UnityEngine;
using System.Collections;



public class gun_script : MonoBehaviour {

	public bool hasGun;
	public GameObject player;
	public GameObject bullet;
	private GameObject clone;
	private Rigidbody2D rb2db;
	private int speed;

	// Use this for initialization
	void Start () {
		hasGun = true;
		speed = 1000;
	}
	
	// Update is called once per frame
	void Update () {
		if (player.GetComponent<Player_script> ().weapon == "gun")
			hasGun = true;
		else
			hasGun = false;

		if (hasGun) {
			if (Input.GetKeyDown ("mouse 0")) {
				GameObject clone = Instantiate(bullet,player.transform.position,Quaternion.identity) as GameObject;
				clone.SetActive(true);
				rb2db =  clone.GetComponent<Rigidbody2D>();//player.GetComponent<Transform>().position, Quaternion.identity
				switch (player.GetComponent<Player_script> ().direction){
				case 2:
					rb2db.position = new Vector2 (player.GetComponent<Transform>().position.x,player.GetComponent<Transform>().position.y-20.7f);
					rb2db.velocity = new Vector2(player.GetComponent<Player_script>().Speed_X,-speed);
					break;
				case 4:
					rb2db.position = new Vector2 (player.GetComponent<Transform>().position.x-16.9f,player.GetComponent<Transform>().position.y-9.2f);
					rb2db.velocity = new Vector2(-speed,player.GetComponent<Player_script>().Speed_Y);
					break;
				case 6:
					rb2db.position = new Vector2 (player.GetComponent<Transform>().position.x+16.9f,player.GetComponent<Transform>().position.y-9.2f);
					rb2db.velocity = new Vector2(speed,player.GetComponent<Player_script>().Speed_Y);
					break;
				case 8:
					rb2db.position = new Vector2 (player.GetComponent<Transform>().position.x,player.GetComponent<Transform>().position.y+1.9f);
					rb2db.velocity = new Vector2(player.GetComponent<Player_script>().Speed_X,speed);
					break;
				default:
					break;
				}
			}

		}
	}
}
