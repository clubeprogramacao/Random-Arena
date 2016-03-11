#if ENABLE_UNET
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace UnityEngine.Networking
{

	[AddComponentMenu("Network/NetworkManagerHUD")]
	[RequireComponent(typeof(NetworkManager))]
	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public class NetworkManagerHUD_Custom : MonoBehaviour
	{
		public NetworkManager manager;
		[SerializeField] public bool showGUI = true;
		public GameObject canv;

		void Awake()
		{
			manager = GetComponent<NetworkManager>();
			canv = GameObject.Find ("Canvas");

		}
		/*
		void Update()
			{
			if (!showGUI)
				return;

			if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
			{
				if (Input.GetKeyDown(KeyCode.S))
				{
					manager.StartServer();
				}
				if (Input.GetKeyDown(KeyCode.H))
				{
					manager.StartHost();
				}
				if (Input.GetKeyDown(KeyCode.C))
				{
					manager.StartClient();
				}
				if (Input.GetKeyDown(KeyCode.M))
				{
					manager.StartMatchMaker();
				}

			}
			if (NetworkServer.active && NetworkClient.active)
			{
				if (Input.GetKeyDown(KeyCode.X))
				{
					manager.StopHost();
				}
			}
		}*/

		public void OnBack(){
			//manager.StopMatchMaker();
			//buttonContinue.SetActive (true);
			//buttonBack.SetActive (false);
			//buttonFind.SetActive (false);
			//buttonCreate.SetActive (false);
			manager.StopHost ();

		}
		public void OnContinue(){
			//buttonContinue.SetActive (false);
			//buttonBack.SetActive (true);
			//buttonFind.SetActive (true);
			//buttonCreate.SetActive (true);

			manager.StartMatchMaker();
		}

		public void OnStartCreateMatch(){
		}


		public void OnFinishCreateMatch(GameObject script){
			string server_Name = script.GetComponent<MainMenuButtons_script> ().roomNameCreate;
			string server_Password = script.GetComponent<MainMenuButtons_script> ().roomPasswordCreate;
			manager.matchMaker.CreateMatch (server_Name, manager.matchSize, true, server_Password, manager.OnMatchCreate);
			Debug.Log (server_Password);
		}

		public void rageQuit(){
			manager.StopHost();
		}

		public void OnFindMatch(){
			manager.matchMaker.ListMatches(0,20, "", manager.OnMatchList);

			int i = 0;
			if (manager.matches != null) {
				
				foreach (var match in manager.matches) {
					bool alreadyExists = false;
					foreach ( GameObject button in GameObject.FindGameObjectsWithTag ("room button")){
						if (button.name == match.name) {
							alreadyExists = true;
						}
					}
					if (alreadyExists)
						continue;
					string[] thisMatch = new string[] { match.name, i.ToString (), match.networkId.ToString ()};
					i++;
					canv = GameObject.Find ("Canvas");
					canv.SendMessage ("CreateMatchButton", thisMatch);
				}
			}
		}
		public void OnJoinMatch(GameObject script){
			string server_Name = script.GetComponent<MainMenuButtons_script> ().roomNameJoin;
			string server_Password = script.GetComponent<MainMenuButtons_script> ().roomPasswordJoin;
			foreach (var match in manager.matches) {
				if (match.name == server_Name) {
					manager.matchMaker.JoinMatch (match.networkId, server_Password, manager.OnMatchJoined);
					Debug.Log (server_Password);
				}
			}
		}

		/*
		foreach (var match in manager.matches)
		{
			if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Join Match:" + match.name)|| Input.GetKeyDown(KeyCode.K))
			{
				manager.matchName = match.name;
				Debug.Log(match.name);
				manager.matchSize = (uint)match.currentSize;
				manager.matchMaker.JoinMatch(match.networkId, "", manager.OnMatchJoined);
			}
			ypos += spacing;
		*/	
		void OnGUI()
			{
			if (!showGUI)
				return;

			int xpos = 10 ;
			int ypos = 40 ;
			int spacing = 24;


			if (NetworkClient.active && !ClientScene.ready)
			{
				if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Client Ready")||Input.GetKeyDown(KeyCode.R))
				{
					ClientScene.Ready(manager.client.connection);

					if (ClientScene.localPlayers.Count == 0)
					{
						ClientScene.AddPlayer(0);
					}
				}
				ypos += spacing;
			}

			if (NetworkServer.active || NetworkClient.active)
			{
				if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop (X)")|| Input.GetKeyDown(KeyCode.X))
				{
					manager.StopHost();
				}
				ypos += spacing;
			}

			if (!NetworkServer.active && !NetworkClient.active)
			{
				ypos += 10;

				if (manager.matchMaker == null || Input.GetKeyDown(KeyCode.M))
				{
					if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Continue"))
					{
						manager.StartMatchMaker();
					}
					ypos += spacing;
				}
				else
				{
					if (manager.matchInfo == null)
					{
						if (manager.matches == null)
						{
							if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Create Internet Match")|| Input.GetKeyDown(KeyCode.N))
							{
								manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", manager.OnMatchCreate);
							}
							ypos += spacing;

							GUI.Label(new Rect(xpos, ypos, 100, 20), "Room Name:");
							manager.matchName = GUI.TextField(new Rect(xpos+100, ypos, 100, 20), manager.matchName);
							ypos += spacing;

							ypos += 10;

							if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Find Internet Match")|| Input.GetKeyDown(KeyCode.J))
							{
								manager.matchMaker.ListMatches(0,20, "", manager.OnMatchList);
							}
							ypos += spacing;
						}
						else
						{
							foreach (var match in manager.matches)
							{
								if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Join Match:" + match.name)|| Input.GetKeyDown(KeyCode.K))
								{
									manager.matchName = match.name;
									Debug.Log(match.name);
									manager.matchSize = (uint)match.currentSize;
									manager.matchMaker.JoinMatch(match.networkId, "", manager.OnMatchJoined);
								}
								ypos += spacing;
							}
						}
					}

					ypos += spacing;

				}
			}
		}
	}
};
#endif //ENABLE_UNET