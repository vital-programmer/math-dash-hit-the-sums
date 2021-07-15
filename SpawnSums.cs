using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpawnSums : MonoBehaviour
{
    public GameObject Sum, Answer1, Answer2, Answer3, Result, AdsInspector, GPGSLeaderboards,
        PlayButton, HowToPlayButton, Language, Sound, Leaderboard, Instagram;
    public Camera MainCamera;
    public Text Score, GameTitle, BestScore;
    private List<GameObject> SumInst = new List<GameObject>();
    private int potentialResult;
    private Vector3 SumPos;
    private bool next = true;
    private int maximumValue = 20;
    private float speed = 2f;
    private float pause = 3f;
    private char[] Operations = { '+', '-', 'x', '/' };
    private IEnumerator coroutine;

    void OnEnable()
    {
        SumPos = new Vector3(0f, -2f, 0f);
        Result.GetComponentInChildren<Text>().text = "0";
        potentialResult = 0;
        pause = 3f;
        coroutine = Spawn();
        StartCoroutine(coroutine);
    }

    void Update()
    {
        for (int i = 0; i < SumInst.Count; i++)
        {
            if (SumInst[i].transform.position != SumPos)
            {
                SumInst[i].transform.position = Vector3.MoveTowards(SumInst[i].transform.position, SumPos,
                    Time.deltaTime * speed);
            }
            else
            {
                AdsInspector.GetComponent<AdsInspecting>().IncreaseLossAmount();
                string mode = "";
                GameData current = new GameData(CloudVariables.Highscore);
                bool needToUpdate = false;
                switch (maximumValue)
                {
                    case 20:
                        needToUpdate = Convert.ToInt32(Score.text) > current.Easy;
                        if (needToUpdate)
                        {
                            current.Easy = Convert.ToInt32(Score.text);
                        }
                        mode = "Easy";
                        break;
                    case 100:
                        needToUpdate = Convert.ToInt32(Score.text) > current.Medium;
                        if (needToUpdate)
                        {
                            current.Medium = Convert.ToInt32(Score.text);
                        }
                        mode = "Medium";
                        break;
                    case 1000:
                        needToUpdate = Convert.ToInt32(Score.text) > current.Hard;
                        if (needToUpdate)
                        {
                            current.Hard = Convert.ToInt32(Score.text);
                        }
                        mode = "Hard";
                        break;
                }
                //if (PlayerPrefs.GetInt(mode + "BestScore") < Convert.ToInt32(Score.text))
                //{
                //    PlayerPrefs.SetInt(mode + "BestScore", Convert.ToInt32(Score.text));
                //    GPGS.UpdateLeaderboard(mode);
                //}
                if (needToUpdate)
                {
                    CloudVariables.Highscore = current.ToString();
                    GPGS.Instance.SaveData();
                    GPGS.UpdateLeaderboard(mode);
                    Manager.Instance.SwitchLanguage();
                }
                MainCamera.backgroundColor = new Color32(255, 148, 156, 255);
                PlayButton.gameObject.SetActive(true);
                HowToPlayButton.gameObject.SetActive(true);
                Language.gameObject.SetActive(true);
                Sound.gameObject.SetActive(true);
                Leaderboard.gameObject.SetActive(true);
                Instagram.gameObject.SetActive(true);
                GameTitle.gameObject.SetActive(true);
                Score.gameObject.SetActive(false);
                BestScore.gameObject.SetActive(false);
                Answer1.gameObject.SetActive(false);
                Answer2.gameObject.SetActive(false);
                Answer3.gameObject.SetActive(false);
                Result.gameObject.SetActive(false);
                this.enabled = false;
                this.ClearList();
            }
        }
        if (SumInst.Count > 0 && next)
        {
            string sum = GetCurrentSum();
            if (sum != "")
            {
                char operation = sum[0];
                int operand = Convert.ToInt32(sum.Substring(2));
                int correctAnswer = 0;
                int currentResult = Convert.ToInt32(Result.GetComponentInChildren<Text>().text);
                switch (operation)
                {
                    case '+':
                        correctAnswer = currentResult + operand;
                        break;
                    case '-':
                        correctAnswer = currentResult - operand;
                        break;
                    case 'x':
                        correctAnswer = currentResult * operand;
                        break;
                    case '/':
                        correctAnswer = currentResult / operand;
                        break;
                }
                System.Random random = new System.Random();
                int pos = random.Next(3);
                int randomAnswer1 = random.Next(maximumValue);
                while (randomAnswer1 == correctAnswer)
                {
                    randomAnswer1 = random.Next(maximumValue);
                }
                int randomAnswer2 = random.Next(maximumValue);
                while (randomAnswer2 == correctAnswer || randomAnswer2 == randomAnswer1)
                {
                    randomAnswer2 = random.Next(maximumValue);
                }
                switch (pos)
                {
                    case 0:
                        Answer1.GetComponentInChildren<Text>().text = Convert.ToString(correctAnswer);
                        Answer2.GetComponentInChildren<Text>().text = Convert.ToString(randomAnswer1);
                        Answer3.GetComponentInChildren<Text>().text = Convert.ToString(randomAnswer2);
                        break;
                    case 1:
                        Answer1.GetComponentInChildren<Text>().text = Convert.ToString(randomAnswer1);
                        Answer2.GetComponentInChildren<Text>().text = Convert.ToString(correctAnswer);
                        Answer3.GetComponentInChildren<Text>().text = Convert.ToString(randomAnswer2);
                        break;
                    case 2:
                        Answer1.GetComponentInChildren<Text>().text = Convert.ToString(randomAnswer1);
                        Answer2.GetComponentInChildren<Text>().text = Convert.ToString(randomAnswer2);
                        Answer3.GetComponentInChildren<Text>().text = Convert.ToString(correctAnswer);
                        break;
                }
                next = false;
            }
        }
    }

    private void OnDisable()
    {
        StopCoroutine(coroutine);
    }

    public void SetMode(int maximum)
    {
        PlayerPrefs.SetInt("Mode", maximum);
        maximumValue = maximum;
    }

    public int GetMode()
    {
        return maximumValue;
    }

    public void DecreasePause()
    {
        if (pause > 0.5f)
        {
            pause -= 0.1f;
        }
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            GameObject sum = Instantiate(Sum, new Vector3(0f, 6f, 0f), Quaternion.identity,
                GameObject.FindGameObjectWithTag("Canvas").transform);
            sum.GetComponentInChildren<Text>().text = GetNewSum(maximumValue);
            SumInst.Add(sum);
            yield return new WaitForSeconds(pause);
        }
    }

    public string GetNewSum(int maxValue)
    {
        System.Random random = new System.Random();
        char operation = Operations[random.Next(0, 4)];
        int value = 0;
        int result = potentialResult;
        if (result == maxValue && operation == '+')
        {
            operation = '-';
        }
        if (result == 0 && (operation == '-' || operation == '/'))
        {
            operation = '+';
        }
        switch (operation)
        {
            case '+':
                value = random.Next(0, maxValue - result + 1);
                potentialResult += value;
                break;
            case '-':
                value = random.Next(0, result + 1);
                potentialResult -= value;
                break;
            case 'x':
                int maxMultiplier;
                if (result != 0)
                {
                    maxMultiplier = maxValue / result;
                }
                else
                {
                    maxMultiplier = maxValue;
                }
                value = random.Next(0, maxMultiplier + 1);
                potentialResult *= value;
                break;
            case '/':
                List<int> divisors = GetDivisors(result);
                value = divisors[random.Next(0, divisors.Count)];
                potentialResult /= value;
                break;
        }
        return $"{operation} {value}";
    }

    public string GetCurrentSum()
    {
        if (SumInst.Count > 0)
        {
            return SumInst[0].GetComponentInChildren<Text>().text;
        }
        return "";
    }

    public List<int> GetDivisors(int result)
    {
        List<int> resList = new List<int>();
        int i = 1;
        while (i * i <= result)
        {
            if (result % i == 0)
            {
                resList.Add(i);
                resList.Add(result / i);
            }
            i++;
        }
        i--;
        if (i * i == result)
        {
            resList.Remove(i);
        }
        return resList;
    }

    public void DestroySum()
    {
        if (SumInst.Count > 0)
        {
            Destroy(SumInst[0]);
            SumInst.RemoveAt(0);
            next = true;
        }
    }

    public void ClearList()
    {
        while (SumInst.Count > 0)
        {
            Destroy(SumInst[0]);
            SumInst.RemoveAt(0);
        }
        next = true;
    }
}
