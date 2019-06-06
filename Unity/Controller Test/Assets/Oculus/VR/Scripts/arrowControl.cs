using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class arrowControl : MonoBehaviour
{
    public Text debugText;
    private GameObject targetCube;
    Vector3 targetPosition;
    const float aThreshold = 0.2f, vertConstant = 0.5f, horiConstant = 0.2f;
    float[] tactorValues = { 0.0f, 0.0f, 0.0f, 0.0f };
    float findAbsoluteAngleDifference(float angleStart, float angleEnd)
    {
        float angle1 = angleEnd - angleStart;
        float angle2 = angleEnd - (angleStart + 360);
        float angle3 = (angleEnd + 360) - angleStart;
        if (Mathf.Abs(angle1) < Mathf.Abs(angle2))
        {
            if (Mathf.Abs(angle1) < Mathf.Abs(angle3))
            {
                return angle1;
            }
            else
            {
                return angle3;
            }
        }
        else
        {
            if (Mathf.Abs(angle2) < Mathf.Abs(angle3))
            {
                return angle2;
            }
            else
            {
                return angle3;
            }
        }
    }
    void Start()
    {
        transform.position = transform.parent.position;
        targetCube = rendering.desCube;
        targetPosition = new Vector3(targetCube.transform.position.x, targetCube.transform.position.y, targetCube.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 diffVector;
        float vecMag, heading, strength;
        //find target cube and find difference
        targetCube = rendering.desCube;
        targetPosition = new Vector3(targetCube.transform.position.x, targetCube.transform.position.y, targetCube.transform.position.z);
        diffVector = targetPosition - transform.parent.position;
        vecMag = diffVector.magnitude;
        strength = (1/vecMag);
        //update position and angle of guiding arrow
        transform.position = transform.parent.position;
        /*transform.rotation = Quaternion.Euler(0,0,0);
        transform.Translate(diffVector / 10);*/
        transform.LookAt(targetCube.transform.position);
        transform.Translate(Vector3.forward / 10);
        //calculate desired tactor values
        heading = transform.localEulerAngles.y;
        if (heading > 355 || heading < 5)
        {
            tactorValues[0] = tactorValues[2] = vertConstant * strength;
        }
        else
        {
            if (heading > 180)
            {
                tactorValues[0] = 0;
                tactorValues[2] = vertConstant*strength;
            }
            else
            {
                tactorValues[0] = vertConstant*strength;
                tactorValues[2] = 0;
            }
        }
        heading = transform.parent.rotation.y;
        if (heading < 45 || heading > 315 || (heading < 225 && heading > 135))
        {
            heading = transform.localEulerAngles.x;
            if (heading > 355 || heading < 5)
            {
                tactorValues[1] = tactorValues[3] = horiConstant * strength;
            }
            else
            {
                if (heading > 180)
                {
                    tactorValues[1] = horiConstant * strength;
                    tactorValues[3] = 0;
                }
                else
                {
                    tactorValues[1] = 0;
                    tactorValues[3] = horiConstant * strength;
                }
            }
        }
        else
        {
            heading = transform.localEulerAngles.z;
            if (heading > 355 || heading < 5)
            {
                tactorValues[1] = tactorValues[3] = horiConstant * strength;
            }
            else
            {
                if (heading > 180)
                {
                    tactorValues[1] = horiConstant * strength;
                    tactorValues[3] = 0;
                }
                else
                {
                    tactorValues[1] = 0;
                    tactorValues[3] = horiConstant * strength;
                }
            }

        }
        //send tactor values to computer
        Debug.Log("***" + tactorValues[0] + "," + tactorValues[1] + "," + tactorValues[2] + "," + tactorValues[3]);
        //debugText.text = tactorValues[0] + "," + tactorValues[1] + "," + tactorValues[2] + "," + tactorValues[3];
    }
}
