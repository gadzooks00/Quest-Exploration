using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugWrite : MonoBehaviour
{
    public OVRCameraRig vRCameraRig;
    public GameObject goalObject;
    Vector3 userPos, userRot;
    Vector3 goalPos, goalAngle;
    double xPosDiff, zPosDiff, yPosDiff, desX, desY;
    double xAngleDiff, yAngleDiff; 
    float avgAngleDiff, vibrationAmplitude, totalPosDiff;
    double findAngleDifference(double ang1, double ang2)
    {
        double diff = ang2 - ang1; //example case, 30, 360. diff = 330
        if(diff >= 180) //330 > 180
        {
            return diff - 360; //returns -30
        }
        else if(diff <= -180) //360, 30. diff = -330
        {
            return 360 + diff;
        }
        else
        {
            return diff;
        }
    }
    double relativeToAbsolute(double relAngle)
    {
        if (relAngle < 0)
            return -relAngle;
        else
            return 360 - relAngle;
    }
    void Update()
    {
        userPos = vRCameraRig.centerEyeAnchor.position;
        userRot = vRCameraRig.centerEyeAnchor.eulerAngles;
        goalPos = goalObject.transform.position;
        xPosDiff = goalPos.x - userPos.x;
        yPosDiff = goalPos.y - userPos.y;
        zPosDiff = goalPos.z - userPos.z;
        totalPosDiff = (float)System.Math.Sqrt(Mathf.Pow((float)xPosDiff, 2) + Mathf.Pow((float)yPosDiff, 2) + Mathf.Pow((float)zPosDiff, 2));
        desX = relativeToAbsolute(System.Math.Atan(yPosDiff / zPosDiff) * (180 / System.Math.PI));
        desY = System.Math.Atan(zPosDiff / xPosDiff) * (180 / System.Math.PI);
        xAngleDiff = findAngleDifference(userRot.x, desX);
        yAngleDiff = findAngleDifference(userRot.y, desY);
        avgAngleDiff = (float)((xAngleDiff + yAngleDiff) / 2);
        Debug.DrawLine(userPos, goalPos, Color.blue);
        Debug.Log("avgDis: " + totalPosDiff);
        vibrationAmplitude = (1- (totalPosDiff / 10)) < 0.1f ? 0.1f : (1 - (totalPosDiff / 10));
        if(System.Math.Abs(yAngleDiff) < 15)
        {
            OVRInput.SetControllerVibration(1f, vibrationAmplitude, OVRInput.Controller.RTouch);
            OVRInput.SetControllerVibration(1f, vibrationAmplitude, OVRInput.Controller.LTouch);
        }
        else if(System.Math.Sign(yAngleDiff) == 1)
        {
            OVRInput.SetControllerVibration(0.1f, vibrationAmplitude, OVRInput.Controller.RTouch);
            OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.LTouch);
        }
        else
        {
            OVRInput.SetControllerVibration(0.1f, vibrationAmplitude, OVRInput.Controller.LTouch);
            OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);
        }
        
    }
}
