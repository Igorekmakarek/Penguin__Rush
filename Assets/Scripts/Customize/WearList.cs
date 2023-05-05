using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WearList : MonoBehaviour, IPunObservable
{
    public static WearList instance;
    private PhotonView view;

    public SpriteRenderer Hat;
    public SpriteRenderer Neck;
    public SpriteRenderer Face;
    public Clothes[] clothes;

    private string currenthat;
    private string currentneck;
    private string currentface;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currenthat);
            stream.SendNext(currentneck);
            stream.SendNext(currentface);
        }
        else
        {
            currenthat = (string)stream.ReceiveNext();             
            currentneck = (string)stream.ReceiveNext();
            currentface = (string)stream.ReceiveNext();
        }
    }

    private void Start()
    {
        instance = this;
        currenthat = PlayerPrefs.GetString("currenthat");
        currentneck = PlayerPrefs.GetString("currentneck");
        currentface = PlayerPrefs.GetString("currentface");
        view = GetComponent<PhotonView>();
        WearClothes();
    }

    private void Update()
    {
        if (!view.IsMine)
        {
            for (int i = 0; i < clothes.Length; i++)
            {
                if (clothes[i].ID == currenthat)
                    Hat.sprite = clothes[i].sprite;
                if (clothes[i].ID == currentneck)
                    Neck.sprite = clothes[i].sprite;
                if (clothes[i].ID == currentface)
                    Face.sprite = clothes[i].sprite;
            }
        }
    }

    private void WearClothes()
    {
        if (!view.IsMine)
            return;

            for (int i = 0; i < clothes.Length; i++)
            {

                if (clothes[i].ID == currenthat)
                    Hat.sprite = clothes[i].sprite;

                if (clothes[i].ID == currentneck)
                    Neck.sprite = clothes[i].sprite;

                if (clothes[i].ID == currentface)
                    Face.sprite = clothes[i].sprite;
            }
    }

    

}
