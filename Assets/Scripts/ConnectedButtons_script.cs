using UnityEngine;
using System.Collections;

public class ConnectedButtons_script : MonoBehaviour {

	[SerializeField] public GameObject objectNetwork;

	void Start(){
		objectNetwork = GameObject.Find ("NetworkManager");
	}

	public void OnRagequit(){
		objectNetwork.SendMessage ("rageQuit");
	}

}
