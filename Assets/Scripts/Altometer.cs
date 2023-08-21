using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Altometer : MonoBehaviour
{
    [SerializeField]
    Glider glider;

    [SerializeField]
    Transform outerNeedlePivotTransform;

    [SerializeField]
    Transform innerNeedlePivotTransform;

    [SerializeField]
    Transform labelPivotTransform;

    [SerializeField]
    GameObject labelPrefab;

    private void Awake()
    {
        CreateLabels();
    }

    private void Update()
    {
        float height = Mathf.Clamp(glider.transform.position.y, 0, 10000);
        outerNeedlePivotTransform.eulerAngles = new Vector3(0, 0, Map(height, 0, 1000, 0, -360));
        innerNeedlePivotTransform.eulerAngles = new Vector3(0, 0, Map(height, 0, 10000, 0, -360));
    }

    void CreateLabels()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject label = Instantiate(labelPrefab, transform);
            label.transform.eulerAngles = new Vector3(0, 0, -360 / 10 * i);
            Text text = label.GetComponentInChildren<Text>();
            text.text = i.ToString();
            text.transform.eulerAngles = Vector3.zero;
        }
    }

    static float Map(float value, float istart, float istop, float ostart, float ostop)
    {
        return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
    }
}

