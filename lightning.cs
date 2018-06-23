using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightning : MonoBehaviour {

    // will need to change code for Unity 2018, Line Renderer is different

    // get the GO and LR
    GameObject lightningObject;
    LineRenderer lr;
    int segments;
    float xPosStart;
    float xPosEnd;
    float yPosStart;
    float yPosEnd;
    float zPosStart;
    float zPosEnd = -1.5f;

    float xPosLast;
    float yPosLast;
    float zPosLast;

    float xDiff;
    float yDiff;
    float zDiff;

    float distance;

    public bool rayHit;

    void Start()
    {
        xPosLast = 0;
        yPosLast = 0;
        zPosLast = 0;

        lightningObject = this.gameObject;
        lr = lightningObject.GetComponent<LineRenderer>();        
    }

    void Update()
    {
        raycastCheck();
        setLightning();        
    }

    // create lightning graphics procedurally
    void setLightning()
    {

        xPosStart = lightningObject.transform.position.x;
        yPosStart = lightningObject.transform.position.y;
        zPosStart = lightningObject.transform.position.z;        

        //Debug.Log(xPosStart + " " + xPosEnd);

        lr.positionCount = segments;

        // set position of the line points
        for (int i = 0; i < segments; i++)
        {

            float wDiff;

            if (segments == 15)
            {
                wDiff = 7.15f;
            }
            else if(segments == 20)
            {
                wDiff = 9.25f;
            }
            else if (segments == 30)
            {
                wDiff = 10.15f;
            }
            else
            {
                wDiff = 19.1f;
            }
            float xPos = Random.Range(xPosLast, xPosLast + xDiff * i / wDiff);
            float yPos = Random.Range(yPosLast, yPosLast + yDiff * i / wDiff);
            float zPos = Random.Range(zPosLast, zPosLast - zDiff * i / wDiff);

            if(i == 0)
            {
                xPos = xPosStart;
                yPos = yPosStart;
                zPos = zPosStart;
            } else if(i == segments - 1)
            {
                xPos = xPosEnd;
                yPos = yPosEnd;
                zPos = -1.5f;
            }

            lr.SetPosition(i, new Vector3(xPos, yPos, zPos));

            xPosLast = xPos;
            yPosLast = yPos;
            zPosLast = zPos;
        }
    }

    // raycast stuff
    void raycastCheck()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 10;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        //layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            rayHit = true;
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            // Debug.Log("Did Hit");
            // Debug.Log(Vector3.Distance(hit.point, lightningObject.transform.position));

            if (Vector3.Distance(new Vector3(hit.point.x, hit.point.y, -1.5f), lightningObject.transform.position) <= 1.5)
            {
                segments = 15;
            }

            if (Vector3.Distance(new Vector3(hit.point.x, hit.point.y, -1.5f), lightningObject.transform.position) <= 3)
            {
                segments = 20;
            }

            if(Vector3.Distance(new Vector3(hit.point.x, hit.point.y, -1.5f), lightningObject.transform.position) > 3)
            {
                segments = 30;
            }

            if (Vector3.Distance(new Vector3(hit.point.x, hit.point.y, -1.5f), lightningObject.transform.position) > 4.5)
            {
                segments = 65;
            }

            distance = Vector3.Distance(new Vector3(hit.point.x, hit.point.y, -1.5f), lightningObject.transform.position);

            xPosEnd = hit.point.x;
            yPosEnd = hit.point.y;
            zPosEnd = hit.point.z;

            xDiff = hit.point.x - lightningObject.transform.position.x;
            yDiff = hit.point.y - lightningObject.transform.position.y;

            xDiff = xDiff / segments;
            yDiff = yDiff / segments;
            zDiff = 3.3f / segments;            
        }
        else
        {
            rayHit = false;
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            segments = 1;
            xPosEnd = 0;
            yPosEnd = 0;
            zPosEnd = 0;
            // Debug.Log("Did not Hit");            
        }
    }

}
