using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; private set; }
    public Sprite SpriteSound, SpriteSoundOff;
    public GameObject Scene, ModeBackground, InstructionBackground, LanguageBackground, ExitBackground, PlayButton,
        HowToPlayButton, Language, Sound, Leaderboard, Instagram, Answer1, Answer2, Answer3, Result;
    private TextAsset temp;
    public Text Instruction, PlayText, HowToPlayText, ModeText, LanguageText, InstructionText, EasyModeText,
        MediumModeText, HardModeText, BestScore, ExitText, Yes, No, GameTitle, Score;
    public Camera MainCamera;

    void Start()
    {
        Instance = this;

        #region Sound
        if (!PlayerPrefs.HasKey("Sound"))
        {
            PlayerPrefs.SetInt("Sound", 1);
        }
        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            Sound.GetComponent<Image>().sprite = SpriteSoundOff;
        }
        else
        {
            Sound.GetComponent<Image>().sprite = SpriteSound;
        }
        #endregion /Sound

        #region Language
        if (!PlayerPrefs.HasKey("Language"))
        {
            PlayerPrefs.SetString("Language", "EN");
        }
        SwitchLanguage();
        #endregion /Language
    }

    void Update()
    {
        #region Exit
        if (Input.GetKey(KeyCode.Escape))
        {
            if (Sound.activeSelf && !ModeBackground.activeSelf && !InstructionBackground.activeSelf &&
                !LanguageBackground.activeSelf)
            {
                if (ExitBackground.activeSelf)
                {
                    ExitBackgroundOff();
                }
                else
                {
                    ExitBackgroundOn();
                }
            }
        }
        #endregion /Exit

        SwitchLanguage();
    }

    public void ModeBackgroundOn()
    {
        DisableMainScreen();
        ModeBackground.SetActive(true);
    }

    public void ModeBackgroundOff()
    {
        ModeBackground.SetActive(false);
        EnableMainScreen();
    }

    public void InstructionBackgroundOn()
    {
        DisableMainScreen();
        InstructionBackground.SetActive(true);
    }

    public void InstructionBackgroundOff()
    {
        InstructionBackground.SetActive(false);
        EnableMainScreen();
    }

    public void LanguageBackgroundOn()
    {
        DisableMainScreen();
        LanguageBackground.SetActive(true);
    }

    public void LanguageBackgroundOff()
    {
        LanguageBackground.SetActive(false);
        EnableMainScreen();
    }

    public void ExitBackgroundOn()
    {
        DisableMainScreen();
        ExitBackground.SetActive(true);
    }

    public void ExitBackgroundOff()
    {
        ExitBackground.SetActive(false);
        EnableMainScreen();
    }

    public void ManageSound()
    {
        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            PlayerPrefs.SetInt("Sound", 1);
            Sound.GetComponent<Image>().sprite = SpriteSound;
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 0);
            Sound.GetComponent<Image>().sprite = SpriteSoundOff;
        }
    }

    public void MakeSound()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            GetComponent<AudioSource>().Play();
        }
    }

    private void DisableMainScreen()
    {
        PlayButton.GetComponent<BoxCollider2D>().enabled = false;
        HowToPlayButton.GetComponent<BoxCollider2D>().enabled = false;
        Language.GetComponent<BoxCollider2D>().enabled = false;
        Sound.GetComponent<BoxCollider2D>().enabled = false;
        Leaderboard.GetComponent<BoxCollider2D>().enabled = false;
        Instagram.GetComponent<BoxCollider2D>().enabled = false;
    }

    private void EnableMainScreen()
    {
        PlayButton.GetComponent<BoxCollider2D>().enabled = true;
        HowToPlayButton.GetComponent<BoxCollider2D>().enabled = true;
        Language.GetComponent<BoxCollider2D>().enabled = true;
        Sound.GetComponent<BoxCollider2D>().enabled = true;
        Leaderboard.GetComponent<BoxCollider2D>().enabled = true;
        Instagram.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void StartEasyMode()
    {
        Scene.GetComponent<SpawnSums>().SetMode(20);
        MainCamera.backgroundColor = new Color32(255, 254, 148, 255);
        StartGame();
    }

    public void StartMediumMode()
    {
        Scene.GetComponent<SpawnSums>().SetMode(100);
        StartGame();
    }

    public void StartHardMode()
    {
        Scene.GetComponent<SpawnSums>().SetMode(1000);
        MainCamera.backgroundColor = new Color32(211, 148, 255, 255);
        StartGame();
    }

    private void StartGame()
    {
        ModeBackground.SetActive(false);
        PlayButton.gameObject.SetActive(false);
        HowToPlayButton.gameObject.SetActive(false);
        Language.gameObject.SetActive(false);
        Sound.gameObject.SetActive(false);
        Leaderboard.gameObject.SetActive(false);
        Instagram.gameObject.SetActive(false);
        GameTitle.gameObject.SetActive(false);
        Score.gameObject.SetActive(true);
        BestScore.gameObject.SetActive(true);
        Answer1.gameObject.SetActive(true);
        Answer2.gameObject.SetActive(true);
        Answer3.gameObject.SetActive(true);
        Result.gameObject.SetActive(true);
        Scene.GetComponent<SpawnSums>().enabled = true;
        Score.text = "0";
        EnableMainScreen();
    }

    public void SwitchLanguage()
    {
        temp = Resources.Load(PlayerPrefs.GetString("Language") + "/Instruction") as TextAsset;
        Instruction.text = temp.text;
        temp = Resources.Load(PlayerPrefs.GetString("Language") + "/OtherTexts") as TextAsset;
        string[] lines = temp.text.Split('\n');
        PlayText.text = lines[0];
        HowToPlayText.text = lines[1];
        ModeText.text = lines[2];
        LanguageText.text = lines[3];
        InstructionText.text = lines[4];
        EasyModeText.text = lines[5];
        MediumModeText.text = lines[6];
        HardModeText.text = lines[7];
        GameData current = new GameData(CloudVariables.Highscore);
        int score = 0;
        switch (PlayerPrefs.GetInt("Mode"))
        {
            case 20:
                score = current.Easy;
                break;
            case 100:
                score = current.Medium;
                break;
            case 1000:
                score = current.Hard;
                break;
        }
        BestScore.text = $"{lines[8]}: {score}";
        ExitText.text = lines[9];
        Yes.text = lines[10];
        No.text = lines[11];
    }
}