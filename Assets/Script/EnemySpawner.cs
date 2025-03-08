using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyHPSliderPrefab;
    [SerializeField] private Transform canvasTransform;
    
    //[SerializeField] private float spawnTime;
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private PlayerHP playerHP;
    [SerializeField] private PlayerGold playerGold;
    private Wave currentWave;
    private int currentEnemyCount;
    private List<Enemy> enemyList;
    public List<Enemy> EnemyList => enemyList;//적의 생성과 삭제는 EnemySpawner에서 하기 때문에 Set은 필요없다.
    public int CurrentEnemyCount => currentEnemyCount;
    public int MaxEnemyCount => currentWave.maxEnemyCount;
    
    void Awake()
    {
        enemyList = new List<Enemy>();
        //StartCoroutine("SpawnEnemy");
    }

    public void StartWave(Wave wave)
    {
        currentWave = wave;
        currentEnemyCount = currentWave.maxEnemyCount;
        StartCoroutine("SpawnEnemy");
    }
    private IEnumerator SpawnEnemy()
    {
        int currentSpawnEnemyCount = 0;

        while (currentSpawnEnemyCount < currentWave.maxEnemyCount)
        {
            int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject enemyObject = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            
            enemy.Setup(this, wayPoints);
            enemyList.Add(enemy);
            SpawnEnemyHPSlider(enemy);

            currentSpawnEnemyCount++;
            yield return new WaitForSeconds(currentWave.spawnRate);
        }
    }

    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int gold)
    {
        if (EnemyDestroyType.Arrive == type)
        {
            playerHP.TakeDamage(1);
        }
        else if (type == EnemyDestroyType.Kill)
        {
            playerGold.CurrentGold += gold;
        }

        currentEnemyCount--;
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    private void SpawnEnemyHPSlider(Enemy enemy)
    {
        GameObject sliderObject = Instantiate(enemyHPSliderPrefab, canvasTransform); //ui는 캔버스의 자식으로 설정되어있어야보인다.
        sliderObject.transform.localScale = Vector3.one;//Enemy는 MonoBehaviour를 상속받아서 자동으로 transform을 가짐
        sliderObject.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        sliderObject.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    }
}
