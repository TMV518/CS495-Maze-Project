using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomSensor : MonoBehaviour
{
    /* This script is used for the bottom sensor.
     * It allows the player to know the coordinates
     * of the tile that it is on, and whether it
     * has touched the goal*/

    public static (char, int) currentPosition;
    public static (char, int) lastPosition;

    public GameObject goalPanel;
    public GameObject menuPanel;
    public GameObject gamePanel;

    
    [SerializeField]public static bool playing = true;

    private bool n;

    public float menuCountdown = 3f;
    private bool lastMenu;

    // Start is called before the first frame update
    void Start()
    {
        n = false;
        playing = true;
        
        menuCountdown = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playing && !n)
        {
            menuCountdown -= Time.deltaTime;
            gamePanel.SetActive(false);
            goalPanel.SetActive(true);

            //GameManager.firstPlay = false;

            if (menuCountdown <= 0f)
            {
                goalPanel.SetActive(false);
                menuPanel.SetActive(true);
                n = true;
            }
        }
        

    }

    /*public static int LastPosition
    {
        get
        {
            return lastPosition;
        }
        set
        {
            this.lastPosition = value;
            this.OnVarChange();
        }
    }*/

    void OnTriggerEnter(Collider other)
    {
        currentPosition = (other.gameObject.name[0], (int)char.GetNumericValue(other.gameObject.name[1]));
        //Debug.Log(currentPosition);

        if (other.tag == "Goal")
        {
            playing = false;
            
        }
    }

    

}
