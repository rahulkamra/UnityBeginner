using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BootScriptPlayerPref : MonoBehaviour
{

    private Button btnSaveText;
    private InputField inputField;
    private Button btnGetText;
    private Text outputField;

    // Use this for initialization
    void Start ()
    {

        btnSaveText = GameObject.Find("btnSaveText").GetComponent<Button>(); ;
        btnSaveText.onClick.AddListener(OnClick);

        btnGetText = GameObject.Find("btnGetText").GetComponent<Button>();
        btnGetText.onClick.AddListener(OnReadText);

        inputField = GameObject.Find("txtInput").GetComponent<InputField>();
        outputField = GameObject.Find("output").GetComponent<Text>();
        OnReadText();

    }

    public void OnReadText()
    {
        string value = PlayerPrefs.GetString("key");
        Debug.Log(outputField);
        if(value != null)
            outputField.text = value;
    }

    public void OnClick()
    {
        string currentText = inputField.text;
        PlayerPrefs.SetString("key", currentText);
        PlayerPrefs.Save();
        //save is important to avoid crash by default unity save on exit
    }
	
	
}
