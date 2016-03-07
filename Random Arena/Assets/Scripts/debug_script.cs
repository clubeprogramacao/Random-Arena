using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class debug_script : NetworkBehaviour {

	/*
    private GameObject canv;
    public GameObject player;

    private GameObject id1;
    private GameObject id2;
    private GameObject id3;
    private GameObject id4;
    private GameObject id5;
    private GameObject id6;

    public string myName;
    public string myLastHitter;
    public string myHealth;

    public string hisName;
    public string hisLastHitter;
    public string hisHealth;


    // Use this for initialization
    void Start () {


        canv = GameObject.Find("Debug Canvas");

        id1 = GameObject.Find("Text (0)");
        id2 = GameObject.Find("Text (1)");
        id3 = GameObject.Find("Text (2)");
        id4 = GameObject.Find("Text (3)");
        id5 = GameObject.Find("Text (4)");
        id6 = GameObject.Find("Text (5)");

        //canv.SetActive(false);
	}
	// Update is called once per frame
	void Update () {
        if (isLocalPlayer)
        {

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                canv.SetActive(true);
            }
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                canv.SetActive(false);
            }

            myName = player.name.ToString();
			myLastHitter = "LastHitter : " + player.GetComponent<playerMovement_script>().lastHitter;
			myHealth = "Health : " + player.GetComponent<playerMovement_script>().health.ToString();

            id1.GetComponent<Text>().text = myName;
            id2.GetComponent<Text>().text = myLastHitter;
            id3.GetComponent<Text>().text = myHealth;
        }
        else
        {
            hisName = player.name.ToString();
			hisLastHitter = "LastHitter : " + player.GetComponent<playerMovement_script>().lastHitter;
			hisHealth = "Health : " + player.GetComponent<playerMovement_script>().health.ToString();

            id4.GetComponent<Text>().text = hisName;
            id5.GetComponent<Text>().text = hisLastHitter;
            id6.GetComponent<Text>().text = hisHealth;
        }

       

	} */
}
