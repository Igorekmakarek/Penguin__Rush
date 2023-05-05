using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance = null;

    public List<Player> players;

    public Results Table; 

    public GameObject PlayerPrefab;

    public bool chatOpened;
    public ChatManager chat;

    public bool canShoot;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        SpawnPenguin();

        chat = GameObject.Find("Chat").GetComponent<ChatManager>();
        chat.EnterServerChat(null);
        canShoot = true;
    }

    public void SpawnPenguin()
    {
        Vector3 pos = new Vector3(Random.Range(-3f, 3f), Random.Range(7f, 12f), 0f);
        Player player = PhotonNetwork.Instantiate(PlayerPrefab.name, pos, Quaternion.identity).GetComponent<Player>();
        players.Add(player);

    }

    [PunRPC]
    public void AddToTable(string NickName)
    {
        Table.NewPlayerJoined(NickName);
    }



    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveLobby();
        SceneManager.LoadScene("Menu");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.LogFormat("Player entered the room");
    }
}
