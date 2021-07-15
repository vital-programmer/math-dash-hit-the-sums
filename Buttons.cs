using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    public static Buttons Instance { get; private set; }

    void Start()
    {
        Instance = this;
    }

    void OnMouseUpAsButton()
    {
        Manager.Instance.MakeSound();
        transform.localScale = new Vector3(1f, 1f, 1f);
        switch (gameObject.name)
        {
            case "PlayButton":
                Manager.Instance.ModeBackgroundOn();
                break;
            case "ModeCloseButton":
                Manager.Instance.ModeBackgroundOff();
                break;
            case "HowToPlayButton":
                Manager.Instance.InstructionBackgroundOn();
                break;
            case "InstructionCloseButton":
                Manager.Instance.InstructionBackgroundOff();
                break;
            case "Language":
                Manager.Instance.LanguageBackgroundOn();
                break;
            case "LanguageCloseButton":
                Manager.Instance.LanguageBackgroundOff();
                break;
            case "ExitCloseButton":
            case "No":
                Manager.Instance.ExitBackgroundOff();
                break;
            case "Yes":
                Application.Quit();
                break;
            case "Sound":
                Manager.Instance.ManageSound();
                break;
            case "Leaderboard":
                GPGS.ShowLeaderboardUI();
                break;
            case "Instagram":
                Application.OpenURL("https://www.instagram.com/_.math_game._/");
                break;
            case "DE":
            case "EN":
            case "RU":
            case "UA":
                PlayerPrefs.SetString("Language", gameObject.name);
                Manager.Instance.SwitchLanguage();
                Manager.Instance.LanguageBackgroundOff();
                break;
            case "EasyMode":
                Manager.Instance.StartEasyMode();
                break;
            case "MediumMode":
                Manager.Instance.StartMediumMode();
                break;
            case "HardMode":
                Manager.Instance.StartHardMode();
                break;

        }
    }
}
