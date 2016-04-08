using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TheBelt_script : NetworkBehaviour
{
    

    [Server]
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<playerMovement_script>().playerSpeed += 6;
            Destroy(gameObject);
        }
    }
    

}