using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customize : MonoBehaviour
{
    public static Customize instance;

    public Image Hat;
    public Image Neck;
    public Image Face;

    public Button UnwearHatButton;
    public Button UnwearNeckButton;
    public Button UnwearFaceButton;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Hat.enabled = false;
        Face.enabled = false;
        Neck.enabled = false;

        UnwearHatButton.onClick.AddListener(UnwearHat);
        UnwearFaceButton.onClick.AddListener(UnwearFace);
        UnwearNeckButton.onClick.AddListener(UnwearNeck);

        if (PlayerPrefs.GetString("currenthat") == "none")
            UnwearHat();
        else
            Hat.enabled = true;

        if (PlayerPrefs.GetString("currentface") == "none")
            UnwearFace();
        else
            Face.enabled = true;

        if (PlayerPrefs.GetString("currentneck") == "none")
            UnwearNeck();
        else
            Neck.enabled = true;
    }

    public void WearHat(Sprite Prewiew)
    {
        Hat.sprite = Prewiew;
        Hat.enabled = true;
    }

    public void WearFace(Sprite Prewiew)
    {
        Face.sprite = Prewiew;
        Face.enabled = true;
    }

    public void WearNeck(Sprite Prewiew)
    {
        Neck.sprite = Prewiew;
        Neck.enabled = true;
    }

    private void UnwearHat()
    {
        Hat.enabled = false;
        PlayerPrefs.SetString("currenthat", "none");
    }

    private void UnwearFace()
    {
        Face.enabled = false;
        PlayerPrefs.SetString("currentface", "none");
    }

    private void UnwearNeck()
    {
        Neck.enabled = false;
        PlayerPrefs.SetString("currentneck", "none");
    }



}
