using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public static GameMenu instance = null;

    public Button ContinueButton;
    public Button OptionsButton;
    public Button ToMenuButton;
    public Button QuitButton;

    bool Opened;

    private void Start()
    {
        instance = this;
        ContinueButton.onClick.AddListener(Close);
        OptionsButton.onClick.AddListener(Options);
        ToMenuButton.onClick.AddListener(GameManager.instance.LeaveRoom);
        QuitButton.onClick.AddListener(QuitGame);
        
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Opened)
                Open();
            else
                Close();
        }
    }

    void Open()
    {
        GameManager.instance.canShoot = false;
        LeanTween.scale(gameObject, Vector3.one, 0.1f);
        Opened = true;
    }

    void Close()
    {
        GameManager.instance.canShoot = true;
        LeanTween.scale(gameObject, Vector3.zero, 0.1f);
        Opened = false;
    }



    public void Options()
    {
        //Опции
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
