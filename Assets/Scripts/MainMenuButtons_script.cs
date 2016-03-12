using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MainMenuButtons_script : NetworkBehaviour {

	[SerializeField] public GameObject objectNetwork;

	[SerializeField] public GameObject buttonContinue, buttonBack;

	[SerializeField] public GameObject buttonFind,  roomButton;

	[SerializeField] public GameObject buttonCreate, buttonCreateOk, roomCreateNameInput, roomCreatePasswordInput;

	[SerializeField] public GameObject buttonJoinOk, roomJoinName, roomJoinPasswordInput;


	public string roomNameCreate, roomNameJoin;
	public string roomPasswordCreate, roomPasswordJoin;
	private bool lookingForMatches;



	void Start()
	{
		lookingForMatches = false;
		objectNetwork = GameObject.Find ("NetworkManager");
		buttonContinue.SetActive (true); 
		buttonBack.SetActive (false);

		buttonFind.SetActive (false);

		buttonCreate.SetActive (false);
		buttonCreateOk.SetActive (false);
		roomCreateNameInput.SetActive (false);
		roomCreatePasswordInput.SetActive (false);

		buttonJoinOk.SetActive (false);
		roomJoinName.SetActive (false);
		roomJoinPasswordInput.SetActive (false);
	}


	public void OnBack(){
		lookingForMatches = false;

		buttonContinue.SetActive (true); 
		buttonBack.SetActive (false);

		buttonFind.SetActive (false);

		buttonCreate.SetActive (false);
		buttonCreateOk.SetActive (false);
		roomCreateNameInput.SetActive (false);
		roomCreatePasswordInput.SetActive (false);

		buttonJoinOk.SetActive (false);
		roomJoinName.SetActive (false);
		roomJoinPasswordInput.SetActive (false);

		foreach ( GameObject button in GameObject.FindGameObjectsWithTag ("room button")){
			Destroy (button);
		}

	}

	public void OnContinue(){
		
		buttonContinue.SetActive (false);
		buttonBack.SetActive (true);

		buttonFind.SetActive (true);
		buttonCreate.SetActive (true);

		objectNetwork.SendMessage ("OnContinue");
	}

	public void OnStartCreateMatch(){
		
		buttonCreate.SetActive (false);
		buttonFind.SetActive (false);

		roomCreateNameInput.SetActive (true);
		roomCreatePasswordInput.SetActive (true);
		buttonCreateOk.SetActive (true);
	}

	public void OnFinishCreateMatch(){
		roomNameCreate = GameObject.Find ("Create Room Name Text").GetComponent<Text> ().text;
		roomPasswordCreate = GameObject.Find ("Create Room Password").GetComponent<InputField> ().text;
		objectNetwork.SendMessage ("OnFinishCreateMatch", gameObject);
	}


	public void OnFindMatch(){
		
		buttonCreate.SetActive (false);
		buttonFind.SetActive (false);

		lookingForMatches = true;
		StartCoroutine ("FindMatchCRoutine");
	}

	IEnumerator FindMatchCRoutine(){
		objectNetwork.SendMessage ("OnFindMatch");
		yield return new WaitForSeconds (2f);
		while (lookingForMatches) {
			if(lookingForMatches)
				objectNetwork.SendMessage ("OnFindMatch");
			yield return new WaitForSeconds (5f);
		}
	}


	public void CreateMatchButton(string[] match){
		int i = int.Parse(match[1]);
		string matchName = match [0];

		GameObject newButton = Instantiate (roomButton, new Vector3(0f,0f,0f), Quaternion.identity) as GameObject;
		newButton.GetComponentInChildren<Text> ().text = i + ") " + matchName;
		newButton.transform.SetParent (GameObject.Find ("Canvas").transform);
		newButton.GetComponent<RectTransform> ().position = new Vector3(17,6-2*i,0);
			
	}

	// this function is run on the button prefab
	public void OnJoinMatchClick(Text input){
		
		roomNameJoin = input.text.Remove (0,3);
		GameObject canv = GameObject.Find ("Canvas");
		canv.GetComponent<MainMenuButtons_script> ().lookingForMatches = false;
		canv.GetComponent<MainMenuButtons_script> ().roomNameJoin = roomNameJoin;
		foreach (Transform obj in canv.GetComponentsInChildren<Transform> (true)) {
			
			if (obj.gameObject.name == "Join Room Name Text") {
				obj.gameObject.SetActive (true);
				obj.gameObject.GetComponent<Text> ().text = roomNameJoin;
			}
			if (obj.name == "Join Room Password") {
				obj.gameObject.SetActive (true);
			}
			if (obj.gameObject.name == "Join Ok") {
				obj.gameObject.SetActive (true);
			}
		}



		foreach (GameObject room in GameObject.FindGameObjectsWithTag ("room button")) {
			Destroy (room);
		}
	}


	public void OnJoinMatchOk(GameObject pass){
		roomNameJoin = GameObject.Find ("Canvas").GetComponent<MainMenuButtons_script> ().roomNameJoin;
		roomPasswordJoin = pass.GetComponent<InputField>().text; Debug.Log ("Pass: " + roomPasswordJoin);
		objectNetwork.SendMessage ("OnJoinMatch",gameObject);
	}



}
