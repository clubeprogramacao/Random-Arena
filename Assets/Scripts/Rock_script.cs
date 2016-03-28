using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Rock_script : NetworkBehaviour {

    private Color floor, rock, rockExploded, holeFilled;

	// Use this for initialization
	void Start () {
        floor = new Color(205f / 255f, 114f / 255f, 93f / 255f, 1);
        rock = new Color(139f / 255f, 86f / 255f, 80f / 255f, 1);
        rockExploded = new Color(184f / 255f, 113f / 255f, 105f / 255f, 1);
        holeFilled = new Color(184f / 255f, 113f / 255f, 105f / 255f, 1);
    }
	
	// Update is called once per frame
	void Update () {
        if (!isServer)
            return;


	}

    [Server]
    void OnBombHit(Vector2 displacement)
    {
        if (Mathf.Sqrt(Mathf.Pow(displacement.x, 2) + Mathf.Pow(displacement.y, 2)) <= 4)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().color = new Color(1,0,0,1);
            GetComponent<SpriteRenderer>().sortingOrder = -11;
        }
        else
        {
            Debug.Log(displacement);
            float ang = Vector2.Angle(Vector2.right, displacement); Debug.Log("Angle = " + ang);
            Vector2 v1 = transform.position;
            Vector2 v2 = v1;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().color = holeFilled;
            GetComponent<SpriteRenderer>().sortingOrder = -11;
            if (ang < 45) // right
            {
                Debug.Log("Right");
                v1 = new Vector2(transform.position.x, transform.position.y - 1);
                v2 = new Vector2(transform.position.x + 3, transform.position.y+1);
            }
            if (ang > 45 && ang < 135 && displacement.y > 0) // up
            {
                Debug.Log("Up");
                v1 = new Vector2(transform.position.x - 1, transform.position.y);
                v2 = new Vector2(transform.position.x + 1, transform.position.y + 3);
            }
            if (ang > 45 && ang < 135 && displacement.y < 0) // down
            {
                Debug.Log("Down");
                v1 = new Vector2(transform.position.x - 1, transform.position.y);
                v2 = new Vector2(transform.position.x + 1, transform.position.y - 3);
            }
            if (ang > 135) // left
            {
                Debug.Log("Left");
                v1 = new Vector2(transform.position.x, transform.position.y - 1);
                v2 = new Vector2(transform.position.x - 3, transform.position.y + 1);
            }
            Collider2D[] colliders = Physics2D.OverlapAreaAll(v1, v2);
            foreach(Collider2D col in colliders)
            {
                if(col.gameObject.tag == "Hole")
                {
                    col.GetComponent<SpriteRenderer>().color = holeFilled;
                    col.GetComponent<BoxCollider2D>().isTrigger = true;
                }
                   

            }
            
        }
    }

}
