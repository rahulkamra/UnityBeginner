using UnityEngine;
using System.Collections;

public class SplineDecorator : MonoBehaviour
{
    public BezierSpline Spline;
    public Transform[] Items;
    public int frequency;

    void Awake()
    {
        if (frequency < 0 || Items == null || Items.Length == 0)
            return;

        float stepSize = 1f / (frequency * Items.Length);
        int count = 0;
        for (int fdx = 0; fdx < frequency; fdx++)
        {
            for (int idx = 0; idx < Items.Length; idx++)
            {

                Transform item = Instantiate(Items[idx]);
                Vector3 position = Spline.GetPoint(count * stepSize);
                item.transform.localPosition = position;
                item.transform.parent = this.transform;
                count++;
            }
        }
    }
}
