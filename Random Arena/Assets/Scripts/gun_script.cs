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
			if (Input.GetKey("mouse 0")) { //if (Input.GetKeyDown ("mouse 0")) {
				switch (player.GetComponent<Player_script> ().direction){
				case 2:
					clone = Instantiate(bullet,new Vector3(player.GetComponent<Transform>().position.x,player.GetComponent<Transform>().position.y-22f,0f) ,Quaternion.identity) as GameObject;
					clone.SetActive(true);
					clone.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Player_script>().Speed_X,-speed);
					break;
				case 4:
					clone = Instantiate(bullet,new Vector3(player.GetComponent<Transform>().position.x-20f,player.GetComponent<Transform>().position.y-9.2f,0f) ,Quaternion.identity) as GameObject;
					clone.SetActive(true);
					clone.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed,player.GetComponent<Player_script>().Speed_Y);
					break;
				case 6:
					clone = Instantiate(bullet,new Vector3(player.GetComponent<Transform>().position.x+20f,player.GetComponent<Transform>().position.y-9.2f,0f) ,Quaternion.identity) as GameObject;
					clone.SetActive(true);
					clone.GetComponent<Rigidbody2D>().velocity = new Vector2(speed,player.GetComponent<Player_script>().Speed_Y);
					break;
				case 8:
					clone = Instantiate(bullet,new Vector3(player.GetComponent<Transform>().position.x,player.GetComponent<Transform>().position.y+2f,0f) ,Quaternion.identity) as GameObject;
					clone.SetActive(true);
					clone.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Player_script>().Speed_X,speed);
					break;
				default:
					break;
				}
				clone.GetComponent<bullet_script>().shooter = player.name;
			}

		}
	}
}
