using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class RageQuit_script : NetworkBehaviour {

    public NetworkManager manager;

    // Use this for initialization
    void Start () {
        manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
	}

    public void rageQuit()
    {
        manager.StopClient();
    }

}
