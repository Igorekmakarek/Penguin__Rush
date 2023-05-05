using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccesorieSlot : MonoBehaviour
{
    public Sprite PreviewSprite;
    public Clothes info;
    public bool isHat;
    public bool isFace;
    public bool isNeck;

    Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PreviewAccessorie);

        if (PlayerPrefs.GetString("currenthat") == info.ID)
            PreviewAccessorie();
        if (PlayerPrefs.GetString("currentneck") == info.ID)
            PreviewAccessorie();
        if (PlayerPrefs.GetString("currentface") == info.ID)
            PreviewAccessorie();
    }

    private void PreviewAccessorie()
    {
        if (isHat)
        {
            Customize.instance.WearHat(PreviewSprite);
            PlayerPrefs.SetString("currenthat", info.ID);
        }

        if (isFace)
        {
            Customize.instance.WearFace(PreviewSprite);
            PlayerPrefs.SetString("currentface", info.ID);
        }

        if (isNeck)
        {
            Customize.instance.WearNeck(PreviewSprite);
            PlayerPrefs.SetString("currentneck", info.ID);
        }

    }
}
