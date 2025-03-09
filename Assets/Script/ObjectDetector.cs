using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField] private TowerSpawner towerSpawner;
    [SerializeField] private TowerDataViewer towerDataViewer;
    
    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    private Transform hitTransform = null;
    private void Awake()
    {
        mainCamera = Camera.main; // Camera.main - GameObject.FindWithTag("MainCamera") 와 같은 역할을 하지만 최적화되어 있음
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() == true) //마우스(또는 터치 입력)가 UI 요소 위에 있는지를 확인하는 함수
        {
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hitTransform = hit.transform;
                
                if (hit.transform.CompareTag("Tile"))
                {
                    towerSpawner.SpawnTower(hit.transform);
                }
                else if (hit.transform.CompareTag("Tower")) //아 raycast로 얘가 먼저 더 인식이 될 수가 있나? 신기하네..
                {
                    towerDataViewer.OnPanel(hit.transform);
                }   
            }
            
            if (hitTransform == null || hitTransform.CompareTag("Tower") == false)
            {
                towerDataViewer.OffPanel();
            }
            
            hitTransform = null;
        }
    }
}
