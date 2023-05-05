using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;

    private Button Tutorial;
    private Button Quit;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Tutorial = transform.GetChild(0).GetComponent<Button>();
        Quit = transform.GetChild(3).GetComponent<Button>();

        Tutorial.onClick.AddListener(StartTutorial);
        Quit.onClick.AddListener(ExitGame);

    }

    void StartTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    void ExitGame()
    {
        Application.Quit();
    }

    public void SaveNickName(Text text)
    {
        PlayerPrefs.SetString("Nick", text.text);
        Debug.Log("Your Nick is now " + text.text);
    }

    public string GetNickName()
    {
        return PlayerPrefs.GetString("Nick");
    }
    
}
