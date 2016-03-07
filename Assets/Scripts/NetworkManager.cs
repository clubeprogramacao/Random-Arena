using UnityEngine;
using System.Collections;
using UnityEngine.UI;


// This will handle hosting a server or connecting to an existing server
public class NetworkManager : MonoBehaviour {
    
    // The initialization requires a maximum amount of players (in this case 4) and a port number (25000). 
    // For the server registration the name of the game should be unique, 
    // otherwise you might get in trouble with others projects using the same name. 
    // The room name can be any name

    private const string typeName = "RandomArenaCP";
    private const string gameName = "Sala T4.3";

    private void StartServer()
    {
        // This MasterServer is run by Unity and could be down due to maintenance. You can download and run your own MasterServer locally
        //MasterServer.ipAddress = "193.136.205.252";

        Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
        MasterServer.RegisterHost(typeName, gameName);
    }

    // If the server is successfully initialized, OnServerInitialized() will be called. 
    void OnServerInitialized()
    {
        Debug.Log("Server Initializied");
        SpawnPlayer();
    }

    /*
    We now have the functionality to create a server, but can not yet search for existing servers or join one of them. 
    To achieve this, we need to send a request to the master server to get a list of HostData. 
    This contains all data required to join a server. Once the host list is received, 
    a message is sent to the game which triggers OnMasterServerEvent(). 
    This function is called for several events, so we need to add a check to see if the message equals MasterServerEvent.HostListReceived. 
    If this is the case, we can store the host list.
    */
    private HostData[] hostList;

    private void RefreshHostList()
    {
        MasterServer.RequestHostList(typeName);
    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        Debug.Log("MasterServerEvent");
        if (msEvent == MasterServerEvent.HostListReceived)
            hostList = MasterServer.PollHostList();
    }

    public void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
        SpawnPlayer();
    }

    void OnConnectedToServer()
    {
        Debug.Log("Server Joined");
    }

    void OnGUI()
    {
        if (!Network.isClient && !Network.isServer)
        {
            if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
                StartServer();

            if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
                RefreshHostList();

            if (hostList != null)
            {
                for (int i = 0; i < hostList.Length; i++)
                {
                    if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
                        JoinServer(hostList[i]);
                }
            }
        }
    }
    
    public GameObject playerPrefab;

    private void SpawnPlayer()
    {
        Network.Instantiate(playerPrefab, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
    }
}
