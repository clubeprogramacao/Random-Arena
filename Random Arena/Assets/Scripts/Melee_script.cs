using UnityEngine;
using System.Collections;

public class Melee_script : MonoBehaviour {

	public GameObject Player;
	public GameObject knife;
	bool attack;
	int damage;
	int delay;
	int period;



	void Start()
	{
		attack = false;
		damage = 30;
		delay = 5;
		period = 50;
	}
	// Update is called once per frame

	void FixedUpdate()
	{
		knife.transform.position = Player.transform.position;

	}

	void Update () 
	{
		attack = (bool)Input.GetKey ("space");
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (attack) {
			if (other.gameObject.tag == "Enemy") {
				InvokeRepeating("other.GetComponent<Enemy_script_1>().knifeDamage()",delay/1000f,period/1000f);
			}
		}
	}

	void OnTriggerExit(Collider2D other)
	{
		if (other.gameObject.tag == "Enemy") {
			CancelInvoke("other.GetComponent<Enemy_script_1>().knifeDamage()");
		}
	}
}
