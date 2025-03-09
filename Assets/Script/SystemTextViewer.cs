using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum SystemType
{
    Money = 0,
    Build
}

public class SystemTextViewer : MonoBehaviour
{
    private TextMeshProUGUI textSystem;
    private TextAlpha tmpAlpha;

    private void Awake()
    {
        textSystem = GetComponent<TextMeshProUGUI>();
        tmpAlpha = GetComponent<TextAlpha>();
    }

    public void PrintText(SystemType type)
    {
        switch (type)
        {
            case SystemType.Money:
                textSystem.text = "System : Not enough money...";
                break;
            case SystemType.Build:
                textSystem.text = "System : Invalid Building...";
                break;
        }
        
        tmpAlpha.FadeOut();
    }
}
