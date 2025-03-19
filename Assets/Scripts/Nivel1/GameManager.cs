using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public TMP_Text collectiblesNumbersText;
    
    public int collectiblesNumber;


    public void addCollectible()
    {
        collectiblesNumber++;
        collectiblesNumbersText.text = collectiblesNumber.ToString();
    }

    public int CollectiblesNumber => collectiblesNumber;
}
