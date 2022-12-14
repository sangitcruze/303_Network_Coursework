using TMPro;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject startMenu;
    public TMP_InputField usernameField;
    public TMP_InputField IPInputField;
    public TMP_Text Victory;


    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);

        }
    }


    public void ConnectToServer()
    {
        //hides the starts menu
        startMenu.SetActive(false);

        usernameField.interactable = false;
        IPInputField.interactable = false;

        Client.instance.ConnectToServer();


    }

}


