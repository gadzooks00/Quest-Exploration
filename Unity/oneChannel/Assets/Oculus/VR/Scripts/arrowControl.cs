using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class arrowControl : MonoBehaviour
{
    public Text debugText;
    private GameObject targetCube;
    Vector3 targetPosition;
    const float aThreshold = 0.2f, vertConstant = 0.05f, horiConstant = 0.1f;
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
        if (!Options.enableGuideArrow)
        {
            transform.localScale = new Vector3(0, 0, 0); //make arrow invisible if undesired
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 diffVector;
        float vecMag, heading, strength;
        bool horiReached = false;
        //find target cube and find difference
        targetCube = rendering.desCube;
        targetPosition = new Vector3(targetCube.transform.position.x, targetCube.transform.position.y, targetCube.transform.position.z);
        diffVector = targetPosition - transform.parent.position;

        //set variable determining amplitude to displacement from the desired cube
        vecMag = diffVector.magnitude;
        strength = (1 / vecMag);

        //update position and angle of guiding arrow
        transform.position = transform.parent.position;
        transform.LookAt(targetCube.transform.position);
        transform.Translate(Vector3.forward / 10);

        //calculate desired tactor values
        if (Options.enableVibrotactorFeedback)
        {
            if (!Options.twoChannelMode) // ONE CHANNEL HAPTIC CONDITION
            {
                if (Options.enableActiveButton && OVRInput.Get(OVRInput.Button.Two))
                {
                    heading = transform.localEulerAngles.y;
                    if (heading > 345 || heading < 15)
                    {
                        horiReached = true;
                        tactorValues[0] = tactorValues[2] = 100;
                    }
                    else
                    {
                        horiReached = false;
                        if (heading > 180)
                        {
                            tactorValues[0] = 0;
                            tactorValues[2] = horiConstant * strength;
                        }
                        else
                        {
                            tactorValues[0] = horiConstant * strength;
                            tactorValues[2] = 0;
                        }
                    }
                    if (horiReached)
                    {
                        heading = transform.parent.rotation.y;
                        if (heading < 45 || heading > 315 || (heading < 225 && heading > 135))
                        {
                            heading = transform.localEulerAngles.x;
                            if (heading > 350 || heading < 10)
                            {
                                tactorValues[1] = tactorValues[3] = 100;
                            }
                            else
                            {
                                if (heading > 180)
                                {
                                    tactorValues[1] = 0;
                                    tactorValues[3] = vertConstant * strength;
                                }
                                else
                                {
                                    tactorValues[1] = vertConstant * strength;
                                    tactorValues[3] = 0;
                                }
                            }
                        }
                        else
                        {
                            heading = transform.localEulerAngles.z;
                            if (heading > 350 || heading < 10)
                            {
                                tactorValues[1] = tactorValues[3] = 100;
                            }
                            else
                            {
                                if (heading > 180)
                                {
                                    tactorValues[1] = 0;
                                    tactorValues[3] = vertConstant * strength;
                                }
                                else
                                {
                                    tactorValues[1] = vertConstant * strength;
                                    tactorValues[3] = 0;
                                }
                            }

                        }
                    }
                    else
                    {
                        tactorValues[1] = tactorValues[3] = 0;
                    }
                }
                else
                {
                    if (vecMag < Options.correctDistance)
                    {
                        tactorValues[0] = tactorValues[1] = tactorValues[2] = tactorValues[3] = 1;
                    }
                    else
                    {
                        tactorValues[0] = tactorValues[1] = tactorValues[2] = tactorValues[3] = 0.1f;
                    }

                }
            }
            else // TWO CHANNEL HAPTIC CONDITION
            {
                tactorValues[0] = tactorValues[1] = tactorValues[2] = tactorValues[3] = 0;
                if (OVRInput.Get(OVRInput.Button.Three))
                {
                    heading = transform.localEulerAngles.y;
                    if (heading > 345 || heading < 15)
                    {
                        tactorValues[0] = tactorValues[2] = 0.1f;
                    }
                    else
                    {
                        horiReached = false;
                        if (heading > 180)
                        {
                            tactorValues[0] = 0;
                            tactorValues[2] = horiConstant * strength;
                        }
                        else
                        {
                            tactorValues[0] = horiConstant * strength;
                            tactorValues[2] = 0;
                        }
                    }
                }
                if (OVRInput.Get(OVRInput.Button.Four))
                {
                    heading = transform.parent.rotation.y;
                    if (heading < 45 || heading > 315 || (heading < 225 && heading > 135))
                    {
                        heading = transform.localEulerAngles.x;
                        if (heading > 350 || heading < 10)
                        {
                            tactorValues[1] = tactorValues[3] = 0.1f;
                        }
                        else
                        {
                            if (heading > 180)
                            {
                                tactorValues[1] = 0;
                                tactorValues[3] = vertConstant * strength;
                            }
                            else
                            {
                                tactorValues[1] = vertConstant * strength;
                                tactorValues[3] = 0;
                            }
                        }
                    }
                    else
                    {
                        heading = transform.localEulerAngles.z;
                        if (heading > 350 || heading < 10)
                        {
                            tactorValues[1] = tactorValues[3] = 0.1f;
                        }
                        else
                        {
                            if (heading > 180)
                            {
                                tactorValues[1] = 0;
                                tactorValues[3] = vertConstant * strength;
                            }
                            else
                            {
                                tactorValues[1] = vertConstant * strength;
                                tactorValues[3] = 0;
                            }
                        }

                    }
                }
                //send tactor values to computer
                Debug.Log("***" + tactorValues[0] + "," + tactorValues[1] + "," + tactorValues[2] + "," + tactorValues[3]);
            }
        }
        if (Options.enableGuideArrow)
        {
            if (Options.guideArrowProportionalSize)
            {
                transform.localScale = new Vector3(1, 1, 1) * strength;
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            if (Options.mobileGuideArrow)
            {
                float pHeading = transform.parent.rotation.y;
                heading = transform.localEulerAngles.y;
                if (pHeading < 45 || pHeading > 315 || (pHeading < 225 && pHeading > 135))
                {
                    pHeading = transform.localEulerAngles.x;
                    transform.position = transform.parent.position;
                    if (pHeading > 350 || pHeading < 10)
                    {
                        transform.Translate(Vector3.forward / 5);
                    }
                    else
                    {
                        if (pHeading > 180)
                        {
                            transform.Translate(Vector3.up / 10);
                        }
                        else
                        {
                            transform.Translate(Vector3.down / 10);
                        }
                    }
                }
                else
                {
                    pHeading = transform.localEulerAngles.z;
                    if (pHeading > 350 || pHeading < 10)
                    {
                        transform.Translate(Vector3.forward / 5);
                    }
                    else
                    {
                        if (pHeading > 180)
                        {
                            transform.Translate(Vector3.up / 10);
                        }
                        else
                        {
                            transform.Translate(Vector3.down / 10);
                        }
                    }

                }
                if (heading > 345 || heading < 15)
                {
                    transform.Translate(Vector3.forward / 5);
                }
                else
                {
                    horiReached = false;
                    if (heading > 180)
                    {
                        transform.Translate(Vector3.right / 10);
                    }
                    else
                    {
                        transform.Translate(Vector3.left / 10);
                    }
                }
            }
            else
            {
                transform.position = transform.parent.position;
                transform.rotation = transform.parent.rotation;
                transform.Translate(Vector3.forward / 10);
            }
        }
    }
}
