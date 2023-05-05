using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenClose : MonoBehaviour
{
    public Button[] CloseButton;
    public Button[] OpenButton;

    public float time;

    private void Start()
    {
        for (int i = 0; i < CloseButton.Length; i++)
        CloseButton[i].onClick.AddListener(Close);
        for (int i = 0; i < OpenButton.Length; i++)
        OpenButton[i].onClick.AddListener(Open);
    }

    public void Open()
    {
        LeanTween.scale(gameObject, Vector2.one, time).setEaseInCirc();
    }

    public void Close()
    {
        LeanTween.scale(gameObject, Vector2.zero, time).setEaseOutCirc();
    }

}
