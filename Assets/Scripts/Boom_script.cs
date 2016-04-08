using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Boom_script : NetworkBehaviour
{


    [Server]
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<combat_script>().numberOfBombs += 10;
            Destroy(gameObject);
        }
    }


}