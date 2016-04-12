using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class web_script : NetworkBehaviour {

	[Server]
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Tear")
        {
            Vector2 speed = other.gameObject.GetComponent<Rigidbody2D>().velocity;
            other.GetComponent<Rigidbody2D>().AddForce(speed * (-1.5f/7f)*15, ForceMode2D.Impulse);
            Debug.Log(speed * (-1.5f / 7f) * 15);
        }
    }
    /*
    [Server]
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Vector2 speed = other.gameObject.GetComponent<Rigidbody2D>().velocity;
            other.GetComponent<Rigidbody2D>().AddForce(speed * (-2f / 7f) * 15, ForceMode2D.Impulse);
        }
    }
    */
}
