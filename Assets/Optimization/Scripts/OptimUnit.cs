using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Profiling;
using Random = UnityEngine.Random;

public class OptimUnit : MonoBehaviour
{
    private Vector3 currentVelocity;
    private float timeToVelocityChange;
    private float currentAngularVelocity;
    private float timeToAngularVelocityChange;

    private float originalXVelocity;
    private float originalZVelocity;


    private Vector3 areaSize;

    public void SetAreaSize(Vector3 size)
    {
        areaSize = size;
    }

    private void Start()
    {
        PickNewVelocity();
        PickNewVelocityChangeTime();
        PickNewAngularVelocity();
        PickNewAngularVelocityChangeTime();
    }

    // Update is called once per frame
    void Update()
    {
        Profiler.BeginSample("Handling Time");
        HandleTime();
        Profiler.EndSample();

        Profiler.BeginSample("Rotations ");
        //AIs slighly better than my code
        /*//AI suggested this as further simplification of my code and this works as intended, the profiler 
        //sample here reduces usage another ~5% compared to my double teranry 
        var t = transform;
        var signX = Mathf.Sign(t.position.x);
        var signZ = Mathf.Sign(t.position.z);

        t.Rotate(signX * currentAngularVelocity * Time.deltaTime, 0, signZ * currentAngularVelocity * Time.deltaTime);
*/

        //my slighlty better code
        var t = transform;
        //implemented two ternary condiaional statements within the trasnform.rotate reducing total calls and ~5% better perfomance
        transform.Rotate(transform.position.x <= 0 ? currentAngularVelocity * Time.deltaTime : -currentAngularVelocity * Time.deltaTime,
            0,
            transform.position.z >= 0 ? currentAngularVelocity * Time.deltaTime : -currentAngularVelocity * Time.deltaTime);

        //orignal code below 
        /*var t = transform;

        if(transform.position.x <= 0)
            transform.Rotate(currentAngularVelocity * Time.deltaTime, 0, 0);
        else if(transform.position.x > 0)
            transform.Rotate(-currentAngularVelocity * Time.deltaTime, 0 ,0);
        
        //planned ternary
        transform.position.x <= 0 ? currentAngularVelocity * Time.deltaTime 



        if(transform.position.z >= 0)
            transform.Rotate(0,0, currentAngularVelocity * Time.deltaTime);
        else if(transform.position.z < 0)
            transform.Rotate(0,0, -currentAngularVelocity * Time.deltaTime);*/
        Profiler.EndSample();

        Profiler.BeginSample("Movement");
        Move();
        Profiler.EndSample();


        Profiler.BeginSample("Bounding Box");
        originalXVelocity = currentVelocity.x;
        originalZVelocity = currentVelocity.z;
        //made these into ternary operators aswell for a few more frames, and generally less calls and checks per 
        currentVelocity.x = (transform.position.x > areaSize.x && currentVelocity.x > 0) || (transform.position.x < -areaSize.x && currentVelocity.x < 0) ?
            -currentVelocity.x : currentVelocity.x;
        currentVelocity.z = (transform.position.z > areaSize.z && currentVelocity.z > 0) || (transform.position.z < -areaSize.z && currentVelocity.z < 0) ? 
            -currentVelocity.z : currentVelocity.z;
        //needed origaln vel stored in order to cut down number of times new velocity function called. 
        if (currentVelocity.x != originalXVelocity || currentVelocity.z != originalZVelocity)
        {
            PickNewVelocityChangeTime();
        }


        /* //check if we are moving away from the zone and invert velocity if this is the case
         if (transform.position.x > areaSize.x && currentVelocity.x > 0)
         {
             currentVelocity.x *= -1;
             PickNewVelocityChangeTime(); //we pick a new change time as we changed velocity
         }
         else if (transform.position.x < -areaSize.x && currentVelocity.x < 0)
         {
             currentVelocity.x *= -1;
             PickNewVelocityChangeTime();
         }

         if (transform.position.z > areaSize.z && currentVelocity.z > 0)
         {
             currentVelocity.z *= -1;
             PickNewVelocityChangeTime(); //we pick a new change time as we changed velocity
         }
         else if (transform.position.z < -areaSize.z && currentVelocity.z < 0)
         {
             currentVelocity.z *= -1;
             PickNewVelocityChangeTime();
         }*/
        Profiler.EndSample();
    }


    private void PickNewVelocity()
    {
        currentVelocity = Random.insideUnitSphere;
        currentVelocity.y = 0;
        currentVelocity *= 10.0f;
    }

    private void PickNewAngularVelocity()
    {
        currentAngularVelocity = Random.Range(-180.0f, 180.0f);
    }

    private void PickNewVelocityChangeTime()
    {
        timeToVelocityChange = Random.Range(2.0f, 5.0f);
    }

    private void PickNewAngularVelocityChangeTime()
    {
        timeToAngularVelocityChange = Random.Range(2.0f, 5.0f);
    }

    void Move()
    {
        transform.position += currentVelocity * Time.deltaTime;
        /*Vector3 position = transform.position;
        
        float distanceToCenter = Vector3.Distance(Vector3.zero, position);
        float speed = 0.5f + distanceToCenter / areaSize.magnitude;
        
        int steps = Random.Range(1000, 2000);
        float increment = Time.deltaTime / steps;
        for (int i = 0; i < steps; ++i)
        {
            position += currentVelocity * increment * speed;
        }
        
        transform.position = position;*/
    }

    private void HandleTime()
    {
        timeToVelocityChange -= Time.deltaTime;
        if (timeToVelocityChange < 0)
        {
            PickNewVelocity();
            PickNewVelocityChangeTime();
        }

        timeToAngularVelocityChange -= Time.deltaTime;
        if (timeToAngularVelocityChange < 0)
        {
            PickNewAngularVelocity();
            PickNewAngularVelocityChangeTime();
        }
    }
}
