using TMPro;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject startMenu;
    public TMP_InputField usernameField;
    public TMP_InputField IPInputField;


    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance alredy exists, destroying object!");
            Destroy(this);

        }
    }


    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        usernameField.interactable = false;
        IPInputField.interactable = false;
        Client.instance.ConnectToServer();
    }

}


