using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public int[] etalonNumber;
    private int[] itemNumber;

    private int cow, bull;
    private int tryItem;
    private int positionNumber;

    [SerializeField] Button StartButton;
    [SerializeField] Image[] TrueNumberImages;
    [SerializeField] Image[] InputNumberImages;

    [SerializeField] GameObject GameName;
    [SerializeField] GameObject GeneratedNumber, TrueNumber;
    [SerializeField] GameObject ControlPanel, InputNumberField, HelpPanel , LanguagePanel;
    [SerializeField] GameObject LogView, ContentView;
    [SerializeField] GameObject LogItemPrefab;
    [SerializeField] GameObject WinPanel;

    [SerializeField] ScrollRect scrollRect;

    private Sprite[] NumbersAll;
    private Sprite emptyCell;

    void Start()
    {
        NumbersAll = Resources.LoadAll<Sprite>("Numbers");
        emptyCell = Resources.Load<Sprite>("EmptyCell");
        NewMove();
    }

    public void StartGameButton()
    {

        GenerateNumber();

        if (etalonNumber.Length > 0)
        {
            StartButton.gameObject.SetActive(false);
            TrueNumber.SetActive(false);
            GameName.SetActive(false);
            WinPanel.SetActive(false);
            LanguagePanel.SetActive(false);

            HelpPanel.SetActive(true);
            ControlPanel.SetActive(true);
            InputNumberField.SetActive(true);
            LogView.SetActive(true);
        }
        GenerateTrueNumber();

        foreach (var item in ContentView.GetComponentsInChildren<RectTransform>().Where(n => n.name == LogItemPrefab.name))
        {
            Destroy(item.gameObject);
        }
        tryItem = 0;
    }

    void GenerateNumber()
    {
        HashSet<int> uniqueNumbers = new();
        while (uniqueNumbers.Count < 4)
        {
            int random = UnityEngine.Random.Range(0, 10);
            uniqueNumbers.Add(random);
        }
        etalonNumber = uniqueNumbers.ToArray();
    }
    void GenerateTrueNumber()
    {
        for (int i = 0; i < etalonNumber.Length; i++)
        {
            string numberString = "Numbers_" + etalonNumber[i];
            TrueNumberImages[i].sprite = NumbersAll.Where(n => n.name == numberString).First();
        }
    }

    void WinScreen()
    {
        if (bull == 4)
        {

            WinPanel.SetActive(true);
            StartButton.gameObject.SetActive(true);
            TrueNumber.SetActive(true);


            HelpPanel.SetActive(false);
            ControlPanel.SetActive(false);
            InputNumberField.SetActive(false);
            LogView.SetActive(false);

            WinPanel.GetComponentsInChildren<TMP_Text>().Where(n => n.name == "Score").First().text = tryItem.ToString();
        }
    }

    public void NumButtonClick(int number)
    {
        if (positionNumber < 4 && !itemNumber.Contains(number))
        {
            string numberString = "Numbers_" + number;
            InputNumberImages[positionNumber].sprite = NumbersAll.Where(n => n.name == numberString).First();
            itemNumber[positionNumber] = number;
            positionNumber++;
        }
    }

    public void NewMove()
    {
        cow = 0;
        bull = 0;
        itemNumber = new int[4] { 100, 100, 100, 100 };
        positionNumber = 0;

        for (int i = 0; i < InputNumberImages.Length; i++)
        {
            InputNumberImages[i].sprite = emptyCell;
        }
    }

    public void BackMove()
    {
        if (positionNumber > 0)
        {
            positionNumber--;
            InputNumberImages[positionNumber].sprite = emptyCell;
            itemNumber[positionNumber] = 100;
        }
    }

    public void CheckMove()
    {
        if (positionNumber == 4)
        {
            for (int i = 0; i < itemNumber.Length; i++)
            {
                if (itemNumber[i] == etalonNumber[i])
                {
                    bull++;
                }
                else if (etalonNumber.Contains(itemNumber[i]))
                {
                    cow++;
                }
            }
            CreateLogItem();

            WinScreen();
            NewMove();
        }
    }

    void CreateLogItem()
    {
        tryItem++;
        int finalScore = 0;
        for (int i = 0; i < itemNumber.Length; i++)
        {
            finalScore += itemNumber[i] * Convert.ToInt32(Math.Pow(10, itemNumber.Length - i - 1));
        }

        GameObject newItem = Instantiate(LogItemPrefab);
        newItem.transform.SetParent(ContentView.transform);
        newItem.name = LogItemPrefab.name;
        newItem.transform.localScale = Vector3.one;

        TMP_Text[] texts = newItem.GetComponentsInChildren<TMP_Text>();
        foreach (TMP_Text text in texts)
        {
            if (text.name == "ItemNumber")
            {
                text.text = tryItem.ToString();
            }
            else if (text.name == "Cows")
            {
                text.text = cow.ToString();
            }
            else if (text.name == "Bulls")
            {
                text.text = bull.ToString();
            }
            else if (text.name == "Number")
            {
                text.text = finalScore.ToString("D4");
            }
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0;
            Canvas.ForceUpdateCanvases();
        }
    }

    public void SetLanguage(string language)
    { 
        PlayerPrefs.SetString("Lang", language);
        SceneManager.LoadScene(0);
    }

}
