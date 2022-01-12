using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public Movement agent;
    public Text speedNum;

    public void SpeedUp()
    {
        agent.stepTimer *= .5f;
        
    }

    public void SlowDown()
    {
        agent.stepTimer *= 1.5f;
    }
}
