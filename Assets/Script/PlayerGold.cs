using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    [SerializeField] private int currentGold = 100;

    public int CurrentGold
    {
        get => currentGold;
        set => currentGold = Mathf.Max(0, value); //굳이 0으로 초기화안하고 이런식으로 프로퍼티 하는 방법도 있구나
    }
}
