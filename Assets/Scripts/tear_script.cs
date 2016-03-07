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

    void OnTriggerEnter2D(Collider2D other)
    {
		if(isServer)
			Cmd_tearCollision (other);
    }

	[Server]
	void Cmd_tearCollision(Collider2D other){
		if (other.gameObject.tag != "Tear" && other.name != shooter)
		{
			if (other.gameObject.tag == "Player") {
				other.gameObject.SendMessage ("OnTearHit", gameObject);
			}
			Destroy(gameObject);
		}
	}

}
