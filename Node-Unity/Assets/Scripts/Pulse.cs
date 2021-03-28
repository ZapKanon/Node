using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    public float EndXPos { get; set; }
    public float DistanceTraveled { get; set; }
    //The node grid column that currently matches this pulse's position. Used for creating energy objects.
    [field:SerializeField] public int CurrentGridColumn { get; set; }

    private int previousGridColumn;

    public List<GameObject> activePulses;

    public Battle_Manager battle_manager;

    // Using Awake allows these default values to be overwritten during instantation if necessary.
    void Awake()
    {
        EndXPos = -1.735f;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    //Pulses move left to right at a constant rate
    public void Move()
    {
        previousGridColumn = CurrentGridColumn;

        if (transform.position.x < EndXPos)
        {
            transform.Translate(Battle_Manager.pulseTravelSpeed * Time.deltaTime, 0, 0);
            DistanceTraveled += Battle_Manager.pulseTravelSpeed * Time.deltaTime;
            
            CurrentGridColumn = Mathf.FloorToInt(DistanceTraveled / 0.24f) - 1; //This -1 is a fairly jank way to offset the pulse's initial position offscreen to the left.

            //Check for source when currentGridColumn increments
            if (previousGridColumn != CurrentGridColumn)
            {
                CheckForSource();
            }
        }
        //Destroy this pulse and remove it from activePulses once it reaches its EndPos
        else
        {
            activePulses.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    //Trigger creation of energy if the block above this pulse is a source node
    public void CheckForSource()
    {
        if (CurrentGridColumn >= 0 && CurrentGridColumn < 20)
        {
            battle_manager.CreateEnergy(this);
        }
    }
}
