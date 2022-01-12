using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Thomas Von Dollen
 * This script controls the player's movement and keeps 
 * track of their checkpoints. It also lets the player
 * know whether or not it's their first time playing*/

public class Movement : MonoBehaviour
{
    public float stepTimer = 1f;
    public float timer;

    public int previousMove;

    public Rigidbody rb;

    private int lastCheckpointMove;

    public Vector3 currentPos;

    private (char, int) currentCheckpoint; //stores active checkpoint and direction that has been moved in after
    private (char, int) lastPos;

    public List<(char, int)> checkpoints;
    public List<(char, int)> deadEnds;

    public List<int> movements;

    public List<string> checksWithMoves;

    

    public Sensor[] sensors;

    public Sensor upSensor;
    public Sensor downSensor;
    public Sensor rightSensor;
    public Sensor leftSensor;
    public GameObject bottomSensor;

    public int moveCount = 0;

    bool endLoop;

    string[] moveArray;
    int moveDecision;

    public string checkMovesStr = "";

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("PlayedBefore") == 1)
        {
            moveDecision = 0;
            moveArray = SplitMoveString(PlayerPrefs.GetString("Moves"));
        }

        timer = stepTimer;

        currentPos = transform.position;

        checkpoints = new List<(char, int)>();
        deadEnds = new List<(char, int)>();

        movements = new List<int>();
        rb = GetComponent<Rigidbody>();

        checksWithMoves = new List<string>();

        

        endLoop = true;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("PlayedBefore") == 0) //if it's the first time playing...
        {
            if (BottomSensor.playing)
            {
                timer -= Time.deltaTime;

                if (timer <= 0f)
                {

                    SensorDecision();
                    CheckSensors();

                    timer = stepTimer;
                    moveCount++;
                }
            }
            else
            {
                

                if (endLoop)
                {
                    for (int i = 0; i < checksWithMoves.Count; i++)
                    {
                        checkMovesStr += checksWithMoves[i] + "$";
                        
                    }
                    
                    PlayerPrefs.SetString("Moves", checkMovesStr);

                    endLoop = false;
                }

            }
        }
        else  //if not first time playing
        {
            timer -= Time.deltaTime;
            
            if (timer <= 0f)
            {

                //SensorDecision();
                NotFirstTimePlay();

                timer = stepTimer;
                moveCount++;
            }
        }
        

        
    }

    string[] SplitMoveString(string moveStr)
    {
        string[] moveArr = moveStr.Split('$');

        return moveArr;
    }

    void NotFirstTimePlay()
    {
        if (moveCount == 0) //IF AT START
        {
            if (!downSensor.touching)
            {
                MoveDown();
            }
            else if (!upSensor.touching)
            {
                MoveUp();
            }
            else if (!leftSensor.touching)
            {
                MoveLeft();
            }
            else if (!rightSensor.touching)
            {
                MoveRight();
            }
        } else if (Sensor.wallAmount == 2)
        {
            SensorDecision();
        }
        else if (Sensor.wallAmount == 1) //if at intersection and not first time playing
        {
            char currentMove = moveArray[moveDecision][6];  //getting direction to move in
            
            if (currentMove == '1')
            {
                MoveUp();
            } else if (currentMove == '2')
            {
                MoveDown();
            }
            else if (currentMove == '3')
            {
                MoveLeft();
            }
            else if (currentMove == '4')
            {
                MoveRight();
            }

            moveDecision++;
        }
    }

    void CheckSensors() //creates checkpoints, dead ends, calls ReverseMoves()
    {
        
        if (Sensor.wallAmount == 1 && !checkpoints.Contains(BottomSensor.currentPosition)) /* if at intersection and if
                                                                                            * it didn't already touch the
                                                                                            * particular checkpoint*/
        {
            

            Debug.Log(BottomSensor.currentPosition + " Added");
            checkpoints.Add(BottomSensor.currentPosition);
            currentCheckpoint = BottomSensor.currentPosition;
            Debug.Log("Current checkpoint: " + currentCheckpoint);

            //clear current list of movements to build new one
            movements.Clear();
            

        } else if (Sensor.wallAmount == 2)
        {
            //keep going
            

        } else if (Sensor.wallAmount == 3 && !deadEnds.Contains(BottomSensor.currentPosition))
        {
            
            //Debug.Log("DEAD END");

            Debug.Log(BottomSensor.currentPosition + " Added to dead ends");
            deadEnds.Add(BottomSensor.currentPosition);

            

        }
    }

    void CheckDirection()   //checks direction that agent moves in and adds them to array
    {
        /* 1 = up
         * 2 = down
         * 3 = left
         * 4 = right
        */

        if (currentPos.x > transform.position.x) //moved up
        {
            //Debug.Log("You moved up");
            currentPos = transform.position;
            movements.Add(1);

            previousMove = 1;
        }
        else if (currentPos.x < transform.position.x) //moved down
        {
            //Debug.Log("You moved down");
            currentPos = transform.position;
            movements.Add(2);

            previousMove = 2;
        }

        if (currentPos.z > transform.position.z) //moved left
        {
            //Debug.Log("You moved left");
            currentPos = transform.position;
            movements.Add(3);

            previousMove = 3;
        }
        else if (currentPos.z < transform.position.z) //moved right
        {
            //Debug.Log("You moved right");
            currentPos = transform.position;
            movements.Add(4);

            previousMove = 4;
        }

        
    }

    void SensorDecision() //makes decision about what to do at certain points
    {
        
        if (moveCount == 0) //IF AT START
        {
            if (!downSensor.touching)
            {
                MoveDown();
            } else if (!upSensor.touching)
            {
                MoveUp();
            } else if (!leftSensor.touching)
            {
                MoveLeft();
            } else if (!rightSensor.touching)
            {
                MoveRight();
            }
        }
        else if (((upSensor.touching && downSensor.touching) || (leftSensor.touching && rightSensor.touching)) && Sensor.wallAmount == 2) //if it's in a corridor
        {
            if (upSensor.touching && downSensor.touching)
            {
                if (previousMove == 3)
                {
                    //Debug.Log("MOVE LEFT");
                    MoveLeft();
                }
                else if (previousMove == 4)
                {
                    //Debug.Log("MOVE RIGHT");
                    MoveRight();
                }
            }
            else if (leftSensor.touching && rightSensor.touching)
            {
                if (previousMove == 1)
                {
                    //Debug.Log("MOVE UP");
                    MoveUp();
                }
                else if (previousMove == 2)
                {
                    //Debug.Log("MOVE DOWN");
                    MoveDown();
                }
            }
        }
        else if ((!(upSensor.touching && downSensor.touching) || !(leftSensor.touching && rightSensor.touching)) && Sensor.wallAmount == 2) //If it's at a CORNER
        {
            if (((leftSensor.touching || rightSensor.touching) && downSensor.touching) && (previousMove == 3 || previousMove == 4)) //move up
            {
                //Debug.Log("MOVE UP");
                MoveUp();
            }
            else if (((leftSensor.touching || rightSensor.touching) && upSensor.touching) && (previousMove == 3 || previousMove == 4)) //move down
            {
                //Debug.Log("MOVE DOWN");
                MoveDown();
            }
            else if (((upSensor.touching || downSensor.touching) && rightSensor.touching) && (previousMove == 1 || previousMove == 2)) //move left
            {
                //Debug.Log("MOVE LEFT");
                MoveLeft();
            }
            else if (((upSensor.touching || downSensor.touching) && leftSensor.touching) && (previousMove == 1 || previousMove == 2))
            {
                //Debug.Log("MOVE RIGHT");
                MoveRight();
            }
        } else if (Sensor.wallAmount == 3)  //if it's at a dead end
        {
            if (upSensor.touching && leftSensor.touching && rightSensor.touching)
            {
                MoveDown();
            } else if (upSensor.touching && leftSensor.touching && downSensor.touching)
            {
                MoveRight();
            } else if (upSensor.touching && rightSensor.touching && downSensor.touching)
            {
                MoveLeft();
            } else if (downSensor.touching && leftSensor.touching && rightSensor.touching)
            {
                MoveUp();
            }
        }
        else if (Sensor.wallAmount == 1) //if at intersection, pick random direction
        {
            //Debug.Log("INTERSECTION");
            Intersection();
            
            
        }
        
    }


    void Intersection() //decides what to do at intersection
    {
        System.Random rnd = new System.Random();
        int rand = rnd.Next(1, 3);

        currentCheckpoint = BottomSensor.currentPosition;
        //if first time at intersection
        if(!checkpoints.Contains(BottomSensor.currentPosition))
        {
            
            if (upSensor.touching || downSensor.touching)
            {
                if (rand == 1)
                {
                    MoveLeft();
                    lastCheckpointMove = 3;
                    Debug.Log("LAST MOVE AT " + currentCheckpoint + " INTERSECTION: LEFT");
                }
                else
                {
                    MoveRight();
                    lastCheckpointMove = 4;
                    Debug.Log("LAST MOVE AT " + currentCheckpoint + " INTERSECTION: RIGHT");
                }

            }
            else
            {
                
                if (rand == 1)
                {
                    MoveUp();
                    lastCheckpointMove = 1;
                    Debug.Log("LAST MOVE AT " + currentCheckpoint + " INTERSECTION: UP");
                }
                else
                {
                    MoveDown();
                    lastCheckpointMove = 2;
                    Debug.Log("LAST MOVE AT " + currentCheckpoint + " INTERSECTION: DOWN");
                }
            }

            checksWithMoves.Add(BottomSensor.currentPosition.ToString() + lastCheckpointMove.ToString());

        } else  //if not first time at intersection
        {
            if (lastCheckpointMove == 3) //if you went left, go right
            {
                MoveRight();
                Debug.Log("LAST MOVE AT " + currentCheckpoint + " INTERSECTION: RIGHT");
                lastCheckpointMove = 4;
            } else if (lastCheckpointMove == 4)
            {
                MoveLeft();
                Debug.Log("LAST MOVE AT " + currentCheckpoint + " INTERSECTION: LEFT");
                lastCheckpointMove = 3;
            } else if (lastCheckpointMove == 2)
            {
                MoveUp();
                Debug.Log("LAST MOVE AT " + currentCheckpoint + " INTERSECTION: UP");
                lastCheckpointMove = 1;
            } else if (lastCheckpointMove == 1)
            {
                MoveDown();
                Debug.Log("LAST MOVE AT " + currentCheckpoint + " INTERSECTION: DOWN");
                lastCheckpointMove = 2;
            }

            
        }

        checksWithMoves.RemoveAt(checksWithMoves.Count - 1);
        checksWithMoves.Add(BottomSensor.currentPosition.ToString() + lastCheckpointMove.ToString());

    }

    void MoveUp()
    {
        transform.position = new Vector3(transform.position.x - 2f, transform.position.y, transform.position.z);
        CheckDirection();
    }

    void MoveDown()
    {
        transform.position = new Vector3(transform.position.x + 2f, transform.position.y, transform.position.z);
        CheckDirection();
    }

    void MoveLeft()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2f);
        CheckDirection();
    }

    void MoveRight()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2f);
        CheckDirection();
    }
}
