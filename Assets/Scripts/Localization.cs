using TMPro;
using UnityEngine;

public class Localization : MonoBehaviour
{
    private string localizationLanguage;
    public string localizationRu;
    public string localizationEn;
    public string localizationDe;

    private TMP_Text text;
    void Start()
    {
        localizationLanguage = PlayerPrefs.GetString("Lang");
        text = GetComponent<TMP_Text>();
        text.text = localizationLanguage switch
        {
            "Ru-ru" => localizationRu,
            "En-en" => localizationEn,
            "De-de" => localizationDe,
            _ => localizationEn,
        };
    }
}
