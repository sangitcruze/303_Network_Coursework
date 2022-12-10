using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemCollector : MonoBehaviour
{
    int coins = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            coins++;
            Debug.Log("Coins: " + coins);
            PlayerManager  player = gameObject.GetComponent<PlayerManager>();
            ClientSend.CoinCollector(player);
        }
    }
}
