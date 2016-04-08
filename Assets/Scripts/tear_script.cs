using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class tear_script : NetworkBehaviour {


    public string shooter;
	public float tearStrength;
	public Vector2 direction;

	// Use this for initialization
	void Start () {
        gameObject.name = "Bullet: " + GetInstanceID().ToString();

    }

    void OnTearHit()
    {
        Destroy(gameObject);
    }

    

	[Server]
    void OnTriggerEnter2D(Collider2D other) { 
		if (other.gameObject.tag != "Tear" && other.name != shooter && other.name != "Bomb Pick" && other.tag != "Web")
		{
			if (other.gameObject.tag == "Player") {
				other.gameObject.SendMessage ("OnTearHit", gameObject);
			}
			Destroy(gameObject);
		}
        if(other.tag == "Web")
        {
            GetComponent<Rigidbody2D>().AddForce(direction * -other.GetComponent<web_script>().drag, ForceMode2D.Impulse);
        }
	}

}
