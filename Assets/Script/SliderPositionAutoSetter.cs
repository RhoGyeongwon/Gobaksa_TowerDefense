using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPositionAutoSetter : MonoBehaviour
{
    [SerializeField] private Vector3 distance = Vector3.down * 20.0f;
    private Transform targetTransform;
    private RectTransform _rectTransform;

    public void Setup(Transform _transform)
    {
        targetTransform = _transform;
        _rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        if (targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }
        
        Vector3 targetPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        _rectTransform.position = targetPosition + distance; //_rectTransform은 Vector3형식이 아닌데 Vector3로 대입될 경우 과연 어떻게 되는거지..
    }
}
