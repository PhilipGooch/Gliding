using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ArtificialHorizon : MonoBehaviour
{
    [SerializeField]
    Glider glider;

    [SerializeField]
    RectTransform horizonTransform;

    int precis_g = 180;

    float ROLL = 0;
    float PITCH = 0;

    [SerializeField]
    RectTransform horizonLeftPoint, horizonRightPoint;

    public Line line;

    void Awake()
    {
        //horizonLeftPoint.localPosition = new Vector3(-10, 300, -0.01f);
        //horizonRightPoint.localPosition = new Vector3(10, 300, -0.01f);

        //Vector3[] corners = new Vector3[4];
        //horizonLeftPoint.GetWorldCorners(corners);
        //Vector3 startWorldPosition = corners[1];
        //
        //horizonRightPoint.GetWorldCorners(corners);
        //Vector3 endWorldPosition = corners[1];

        //Vector3 startPosition = transform.InverseTransformPoint(startWorldPosition);
        //Vector3 endPosition = transform.InverseTransformPoint(endWorldPosition);

        Vector3 startPosition = horizonLeftPoint.position;
        Vector3 endPosition = horizonRightPoint.position;

        Vector3 midpoint = (startPosition + endPosition) / 2f;

        horizonTransform.pivot = new Vector2(0.5f, 0.5f);
        horizonTransform.localPosition = midpoint;
        horizonTransform.sizeDelta = new Vector2(Vector3.Distance(startPosition, endPosition), horizonTransform.sizeDelta.y);

        Vector3 direction = (endPosition - startPosition).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        horizonTransform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void Update()
    {
        //float roll = glider.transform.rotation.z;
        //float pitch = glider.transform.rotation.x;
        //float yaw = glider.transform.rotation.y;
        //
        //float cos_pitch = Mathf.Cos(pitch * Mathf.PI / 180);
        //float cos_yaw = Mathf.Cos(yaw * Mathf.PI / 180);
        //float cos_roll = Mathf.Cos(roll * Mathf.PI / 180);
        //float sin_pitch = Mathf.Sin(pitch * Mathf.PI / 180);
        //float sin_yaw = Mathf.Sin(yaw * Mathf.PI / 180);
        //float sin_roll = Mathf.Sin(roll * Mathf.PI / 180);
        //
        //m_forward.x = sin_yaw * cos_pitch;
        //m_forward.y = sin_pitch;
        //m_forward.z = cos_pitch * -cos_yaw;
        //
        //m_up.x = -cos_yaw * sin_roll - sin_yaw * sin_pitch * cos_roll;
        //m_up.y = cos_pitch * cos_roll;
        //m_up.z = -sin_yaw * sin_roll - sin_pitch * cos_roll * -cos_yaw;

        //float roll = glider.transform.eulerAngles.z;
        //float pitch = glider.transform.eulerAngles.x;
        //
        //float maxPitch = 45;
        //float alpha = Mathf.Acos(-pitch / maxPitch);
        //float beta = roll * Mathf.Deg2Rad;
        //float x1 = Mathf.Cos(alpha + beta);
        //float y1 = Mathf.Sin(alpha + beta);
        //float x2 = Mathf.Cos(beta - alpha);
        //float y2 = Mathf.Sin(beta - alpha);
        //Vector2 a = new Vector2(x1, y1);
        //Vector2 b = new Vector2(x2, y2);
        //Vector2 horizon = b - a;
        //float angle = Vector2.Angle(a, b);
        //
        //Debug.Log(angle);




        //Vector3 perpendicularToHorizon = Vector3.Cross(horizonTransform.right, horizonTransform.forward);
        //horizonTransform.localPosition = perpendicularToHorizon * Mathf.Sin(glider.transform.localEulerAngles.x) * 100;// glider.transform.localEulerAngles.x, 360);
        //Debug.Log(glider.transform.localEulerAngles.x);

        Quaternion swing;
        Quaternion twist;
        SwingTwistDecomposition(glider.transform.rotation, glider.transform.forward, out twist, out swing);
        horizonTransform.eulerAngles = new Vector3(0, 0, -twist.eulerAngles.z);

        // possibly use swing twist here...
        float roll = -twist.eulerAngles.z * Mathf.Deg2Rad;

        SwingTwistDecomposition(glider.transform.rotation, glider.transform.right, out twist, out swing);

        float pitch = 0;// twist.eulerAngles.z * Mathf.Deg2Rad;


        //float roll = (-ROLL * Mathf.PI) / 180f;      // convert roll to radians
        //float pitch = (PITCH * Mathf.PI) / 180f;      // convert pitch to radians.
        float cosroll = Mathf.Cos(roll);
        float sinroll = Mathf.Sin(roll);
        bool isUp = (PITCH < 90) && (PITCH > -90); // upright ?


        //
        //  Draw the horizon and ground lines leading toward it rotated and shifted according to pitch and 
        //  roll. We treat the inverted situation differently to simplifly the math and avoid 3d projectsion
        //  etc.
        //
        if (isUp)
        {
            // Upright ground image 
            int croll = (int)(cosroll * precis_g);                                          // precompute some trig.
            int sroll = (int)(sinroll * precis_g);
            //
            int ypitch = (int)((pitch * croll) / (Mathf.PI / 4.0));
            int xpitch = (int)(-(pitch * sroll) / (Mathf.PI / 2.0));
            //        
            drawRotateShiftedLine(-250, 0, 250, 0, croll, sroll, xpitch, ypitch);  // Draw Horizon                
                                                                                   //drawRotateShiftedLine(-60, 250, -25, 0, croll, sroll, xpitch, ypitch);  // Draw lines orthogonal(ish)
                                                                                   //drawRotateShiftedLine(60, 250, 25, 0, croll, sroll, xpitch, ypitch);  // to horizon                        
        }
        else
        {
            // Inverted ground image.
            int croll = (int)(cosroll * precis_g);         // display rolls is backwards when inverted.
            int sroll = (int)(-sinroll * precis_g);
            if (pitch > 0)
                pitch = -Mathf.PI + pitch;
            else
                pitch = Mathf.PI + pitch;
            //

            int ypitch = (int)((pitch * croll) / (Mathf.PI / 4.0));
            int xpitch = (int)(-(pitch * sroll) / (Mathf.PI / 2.0));
            //                                        
            drawRotateShiftedLine(-250, 0, 250, 0, croll, sroll, xpitch, ypitch);   // Draw Horizon
                                                                                    //drawRotateShiftedLine(-60, -250, -25, 0, croll, sroll, xpitch, ypitch);   // Draw lines orthogonal(ish)
                                                                                    //drawRotateShiftedLine(60, -250, 25, 0, croll, sroll, xpitch, ypitch);   // to horizo (but inverted view) 
        }

    }

    void SwingTwistDecomposition(Quaternion rotation, Vector3 twistAxis, out Quaternion twist, out Quaternion swing)
    {
        // Normalize the twist axis.
        twistAxis.Normalize();

        // Calculate the projection of the rotation onto the twist axis.
        float projection = Vector3.Dot(ToAngleAxisVector(rotation), twistAxis) * Mathf.Deg2Rad;
        Vector3 twistRotation = projection * twistAxis;

        // Calculate the twist quaternion.
        twist = new Quaternion(twistRotation.x, twistRotation.y, twistRotation.z, Mathf.Cos(projection / 2));

        // Calculate the swing quaternion.
        swing = rotation * Quaternion.Inverse(twist);
    }

    Vector3 ToAngleAxisVector(Quaternion rotation)
    {
        float angle;
        Vector3 axis;
        rotation.ToAngleAxis(out angle, out axis);
        return axis * angle;
    }

    //void CalculateHorizonPositionAndRotation(float roll, float pitch)
    //{
    //    float maxPitch = 45;
    //    float alpha = Mathf.Acos(-pitch/maxPitch);
    //    float beta = roll * Mathf.Deg2Rad;
    //    float x1 = Mathf.Cos(alpha + beta);
    //    float y1 = Mathf.Sin(alpha + beta);
    //    float x2 = Mathf.Cos(beta - alpha);
    //    float y2 = Mathf.Sin(beta - alpha);
    //    Vector2 a = new Vector2(x1, y1);
    //    Vector2 b = new Vector2(x2, y2);
    //    Vector2 horizon = b - a;
    //    float angle = Vector2.Angle(a, b);
    //
    //}

    static float Map(float value, float istart, float istop, float ostart, float ostop)
    {
        return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
    }


    void drawRotateShiftedLine(int x0, int y0, int x1, int y1, int croll, int sroll, int xshift, int yshift)
    {
        int x2 = (x0 * croll - y0 * sroll) / precis_g;
        int y2 = (x0 * sroll + y0 * croll) / precis_g;
        int X0 = x2 + xshift;
        int Y0 = y2 + yshift;
        int x3 = (x1 * croll - y1 * sroll) / precis_g;
        int y3 = (x1 * sroll + y1 * croll) / precis_g;
        int X1 = x3 + xshift;
        int Y1 = y3 + yshift;


        line.a = new Vector2(X0, Y0);
        line.b = new Vector2(X1, Y1);

        //line(X0, Y0, X1, Y1);
    }
}
