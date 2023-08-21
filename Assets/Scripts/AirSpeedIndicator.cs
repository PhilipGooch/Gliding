using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirSpeedIndicator : MonoBehaviour
{
    [SerializeField]
    Transform needlePivotTransform;

    [SerializeField]
    Transform labelPivotTransform;

    [SerializeField]
    GameObject labelPrefab;

    [SerializeField]
    Glider glider;

    [SerializeField]
    float maxSpeed;

    float speed;

    [SerializeField]
    float minAngle, maxAngle;

    [SerializeField]
    int interval;

    private void Awake()
    {
        CreateLabels();
    }

    private void Update()
    {
        speed = Mathf.Clamp(glider.speed, 0, maxSpeed);

        needlePivotTransform.eulerAngles = new Vector3(0, 0, Map(speed, 0, maxSpeed, minAngle, maxAngle));
    }

    static float Map(float value, float istart, float istop, float ostart, float ostop)
    {
        return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
    }

    void CreateLabels()
    {
        for (int i = 0; i < maxSpeed / (interval - 1); i++)
        {
            GameObject label = Instantiate(labelPrefab, transform);
            label.transform.eulerAngles = new Vector3(0, 0, Map(interval * i, 0, maxSpeed, minAngle, maxAngle));
            Text text = label.GetComponentInChildren<Text>();
            text.text = (interval * i).ToString().ToString();
            text.transform.eulerAngles = Vector3.zero;
        }
    }
}
