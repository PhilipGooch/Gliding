using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    RectTransform rectTransform;

    public Vector2 a;
    public Vector2 b;
    public float width;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        
    }

    private void Update()
    {
        Vector2 midpoint = (a + b) / 2f;


        rectTransform.localPosition = midpoint;
        rectTransform.sizeDelta = new Vector2(Vector2.Distance(a, b), width);

        Vector3 direction = (b - a).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

}
