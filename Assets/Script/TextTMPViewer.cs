using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextTMPViewer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textPlayerHP;
    [SerializeField] PlayerHP playerHP;
    [SerializeField] TextMeshProUGUI textPlayerGold;
    [SerializeField] TextMeshProUGUI textWave;
    [SerializeField] WaveSystem waveSystem;
    [SerializeField] PlayerGold playerGold;
    [SerializeField] TextMeshProUGUI textEnemyCount;
    [SerializeField] EnemySpawner enemySpawner;
    
    void Update()
    {
        textPlayerHP.text = playerHP.CurrentHP + "/" + playerHP.MaxHP;
        textPlayerGold.text = playerGold.CurrentGold.ToString();
        textWave.text = waveSystem.CurrentWave + "/" + waveSystem.MaxWave;
        textEnemyCount.text = enemySpawner.CurrentEnemyCount + "/" + enemySpawner.MaxEnemyCount;
    }
}
