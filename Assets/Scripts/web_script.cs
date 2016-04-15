using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class web_script : NetworkBehaviour {
    public bool destroyed;
    public float drag;
    void Start()
    {
        destroyed = false;
    }

	[Server]
    void OnTriggerEnter2D(Collider2D other)
    {
        if (destroyed)
        {
            return;
        }
        if(other.tag == "Tear")
        {
            Vector2 speed = other.gameObject.GetComponent<Rigidbody2D>().velocity;
            other.GetComponent<Rigidbody2D>().AddForce(speed * (-drag)*15, ForceMode2D.Impulse);
        }
    }

    [Server]
    void OnBombHit()
    {
        destroyed = true;
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
        gameObject.GetComponentInChildren<AreaEffector2D>().drag = 0;
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
