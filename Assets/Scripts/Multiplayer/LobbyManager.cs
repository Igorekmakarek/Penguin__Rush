
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager instance;

    public Text Log;


    public InputField nickName;
    public InputField numberOfPlayers;
    public InputField createInput;
    public InputField joinInput;


    public void Start()
    {
        if (instance == null)
            instance = this;

        nickName.text = PlayerPrefs.GetString("NickName");
        PhotonNetwork.NickName = nickName.text;



        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";

        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster()
    {
        Log.text += "\nYou succesfully connected to master server!";

    }



    public void CreateRoom()
    {
        PhotonNetwork.NickName = nickName.text;
        PlayerPrefs.SetString("NickName", nickName.text);
        RoomOptions roomOptions = new RoomOptions();


        int number;

        bool success = int.TryParse(numberOfPlayers.text, out number);
        if (success)
        {
            if (number >= 2 && number <= 8)
            {
                roomOptions.MaxPlayers = (byte)number;
                Log.text += "\nYou created a room";
            }
            else
            {

                return;
            }
        }
        else
        {
            return;
        }

        PhotonNetwork.CreateRoom(createInput.text, roomOptions);        //создание комнаты с названием которое выбрал игрок

    }

    public void Test()
    {
        createInput.text = "TestRoom";
        numberOfPlayers.text = "2";
        CreateRoom();
    }

    

    public void JoinRoom()
    {
        PhotonNetwork.NickName = nickName.text;
        PlayerPrefs.SetString("NickName", nickName.text);
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public void JoinRandom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void LeaveRoom()
    {
        
        PhotonNetwork.LeaveRoom();

    }

    public override void OnLeftRoom()
    {
        Log.text += "\nYou left the room";

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Log.text += "\nCouldn't find a room with this name";
    }

    public override void OnJoinedRoom()
    {
        StartGame();
        Log.text += "\nYou joined the room";

        // SceneManager.LoadScene("FeastWorld");

    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Tutorial");
    }


}
