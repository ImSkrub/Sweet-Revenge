using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Coins : MonoBehaviour
{
    private float coins;
    private TextMeshProUGUI textMesh;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        //coins += Time.deltaTime;
        textMesh.text = coins.ToString("0");
    }

    public void AddCoins(float entryCoins)
    {
        coins += entryCoins;
    }
}
