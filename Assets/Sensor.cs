using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Thomas Von Dollen
 * This script is used on each of the 4 side sensors.
 * It lets the player know how many walls it's touching.*/

public class Sensor : MonoBehaviour
{
    public string sensorName;

    public static int wallAmount;

    

    public bool touching = false;

    // Start is called before the first frame update
    void Start()
    {
        wallAmount = 0;
        sensorName = this.gameObject.name;

        
    }

    void OnTriggerEnter(Collider other) //Checks if sensors are touching walls, adds wall amount
    {
        if (other.gameObject.tag == "Wall")
        {
            //Debug.Log(sensorName + " is touching a wall");

            touching = true;
            wallAmount += 1;    //adding to wall amount

            //Debug.Log("Amount of walls: " + wallAmount);

        }
    }

    void OnTriggerExit(Collider other) //Checks if sensors leave walls, subtracts wall amount
    {
        if (other.gameObject.tag == "Wall")
        {
            //Debug.Log(sensorName + " is no longer touching a wall");
            touching = false;
            wallAmount -= 1;
            //Debug.Log(wallAmount);

            
        }
    }



}
