using UnityEngine;
using System.Collections;

public class animations : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// up is far, down is near
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1*(int)gameObject.GetComponent<Rigidbody2D>().position.y;
	}
}
