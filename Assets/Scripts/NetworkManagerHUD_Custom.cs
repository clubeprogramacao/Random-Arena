#if ENABLE_UNET
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using System.Collections.Generic;

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
		MatchInfo m_matchInfo;

		void Awake()
		{
			manager = GetComponent<NetworkManager>();
			canv = GameObject.Find ("Canvas");

		}

		public void OnBack(){
			manager.StopHost ();

		}
		public void OnContinue(){
			manager.StartMatchMaker();
		}

		public void OnStartCreateMatch(){
		}


		public void OnFinishCreateMatch(GameObject script){
			string server_Name = script.GetComponent<MainMenuButtons_script> ().roomNameCreate;
			string server_Password = script.GetComponent<MainMenuButtons_script> ().roomPasswordCreate;
			manager.matchMaker.CreateMatch (server_Name, manager.matchSize, true, server_Password, manager.OnMatchCreate);
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
				}
			}
		}
	}
};
#endif //ENABLE_UNET