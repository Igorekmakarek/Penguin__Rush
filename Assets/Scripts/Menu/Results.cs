using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Linq;

public class Results : MonoBehaviour
{

    public Transform PlayersTransform;

    int num;        //Количество строк

    private PhotonView view;
    private GameObject[] Players = new GameObject[8];
    private Text[] Names = new Text[8];
    private Text[] Kills = new Text[8];
    private Text[] Deaths = new Text[8];

    private int[] kills = new int[8];
    private int[] deaths = new int[8];

    bool Opened;


    private void Awake()
    {
        num = PlayersTransform.childCount;

        for (int i = 0; i < num; i++)
        {
            Players[i] = PlayersTransform.GetChild(i).gameObject;
            Players[i].LeanScale(Vector3.zero, 0f);

            Names[i] = Players[i].transform.GetChild(0).GetComponent<Text>();
            Kills[i] = Players[i].transform.GetChild(1).GetComponent<Text>();
            Deaths[i] = Players[i].transform.GetChild(2).GetComponent<Text>();
        }

    }

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            if (!Opened)
                Open();
        }
        else
            Close();
    }

    private void Sort()
    {

    }

    void Open()
    {
        LeanTween.scale(gameObject, Vector3.one, 0.1f);
        Opened = true;
    }

    void Close()
    {
        LeanTween.scale(gameObject, Vector3.zero, 0.1f);
        Opened = false;
    }

    public void NewPlayerJoined(string name)
    {

        Debug.Log("NewPlayerJoined! ! ");
        for (int i = 0; i < num; i++)
        {
            if (Players[i].transform.localScale.x == 0)
            {
                Debug.Log($"Name {name} added to the table");
                Players[i].LeanScale(Vector3.one, 0.5f);
                Names[i].text = name;
                Kills[i].text = "0";
                Deaths[i].text = "0";
                return;
            }
        }
    }

    [PunRPC]
    public void PlayerDead(string name)
    {
        for (int i = 0; i < num; i++)
        {
            if (Names[i].text == name)
            {
                deaths[i]++;
                Deaths[i].text = "" + deaths[i];
            }
        }
    }

    public void RPCDead(string Name)
    {
        view.RPC("PlayerDead", RpcTarget.All, Name);
    }

    public void RPCKill(string Name)
    {
        view.RPC("PlayerGotKill", RpcTarget.All, Name);
    }

    [PunRPC]
    public void PlayerGotKill(string name)
    {
        for (int i = 0; i < num; i++)
        {
            if (Names[i].text == name)
            {
                kills[i]++;
                Kills[i].text = "" + kills[i];
            }
        }
    }

}
