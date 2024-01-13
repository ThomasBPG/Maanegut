using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIhandler : MonoBehaviour
{
    public static int collectedStones = 0;

    public TMP_Text stoneText;
    public TMP_Text lifeText;
    
    void Update()
    {
        stoneText.text = $"Samlede Sten {collectedStones}";
        lifeText.text = $"Liv {Player.life}";
    }
}
