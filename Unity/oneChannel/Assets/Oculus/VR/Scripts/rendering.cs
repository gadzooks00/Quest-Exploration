using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rendering : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject oShelves, guideArrow;
    float timeStart;
    public static GameObject desCube;
    public static bool cubeFound;
    bool isActiveState, changedThisPress;
    void setCube()
    {
        GameObject[] m_TargetsList;
        m_TargetsList = GameObject.FindGameObjectsWithTag("ShelfThing");
        desCube = m_TargetsList[Random.Range(0, m_TargetsList.Length)];
        desCube.tag = "TargetCube";
    }
    void Start()
    {      
        changedThisPress = false;
        oShelves = GameObject.Find("Shelves");
        oShelves.SetActive(false);
        isActiveState = false;        
        guideArrow = GameObject.Find("GuideArrow");
        guideArrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.Get(OVRInput.Button.One) && !changedThisPress)
        {
            if(!isActiveState)
            {
                oShelves.SetActive(true);
                timeStart = Time.fixedTime;
                Debug.Log("!!!Time = " + timeStart);
                setCube();
                Debug.Log("!!!Cube is " + desCube);
                guideArrow.SetActive(true);
                isActiveState = true;
                changedThisPress = true;
                cubeFound = false;
            }
            else
            {
                isActiveState = false;
            }
        }
        else if(!OVRInput.Get(OVRInput.Button.One))
        {
            changedThisPress = false;
        }
        if(cubeFound && isActiveState == true)
        {
            oShelves.SetActive(false);
            guideArrow.SetActive(false);
            Debug.Log("!!!Finished! Time = " + (Time.fixedTime - timeStart) + (desCube == null ? "" : "Wrong Cube"));
            isActiveState = false;
        }
    }
}
