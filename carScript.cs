using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
enum CarStates{
    DRIVING, SETDESTINATION, FILLINGGAS
}
public class carScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float gasLevel;
    public float gasMax;
    private string typeGas = "gasoline";
    private string carType = "sedan";
    private NavMeshAgent ai;
    private Vector3 destination;
    public Transform gasPump;
    public Transform[] GasPumps;
    public bool isStopped = false;
    bool lowGas = false;
    pumpScript ps;

    CarStates currentState = CarStates.DRIVING;
    void Start()
    {
        gasMax = Random.Range(29,30);
        gasLevel = Random.Range(5,6);
        ai = this.GetComponent<NavMeshAgent>();
        destination = new Vector3(170.23f, 2.68f, 77.46f);
         
        //destination = gasPump.position;
        ai.SetDestination(destination);
    }

    // Update is called once per frame
    void Update()
    {
        // if (isStopped == false)
        // {
        //     ai.SetDestination(destination);
        // }
        if (ps != null)
        {
            //Debug.Log(ps.occupied);
        }
        if (ps != null && ps.occupied == true)
        {
            // findGasStation();
            // currentState = CarStates.SETDESTINATION;
        }

        switch (currentState){
            case CarStates.DRIVING:
                if (gasLevel < 5 && lowGas == false)
                {
                    currentState = CarStates.SETDESTINATION;
                    lowGas = true;
                   findGasStation();
                }
                if (checkGasPump() == true && gasLevel < 5)
                {
                    findGasStation();
                    Debug.Log("looked for a new gas station");
                    currentState = CarStates.SETDESTINATION;
                }
                break;

            case CarStates.SETDESTINATION:
                if (lowGas)
                {
                    ai.SetDestination(gasPump.position);
                }
                currentState = CarStates.DRIVING;
                break;


        }

    }

    void FixedUpdate()
    {
        gasLevel -= 0.1f * Time.deltaTime;


    }

    public void stop()
    {
        ai.SetDestination(new Vector3(ai.transform.position.x, ai.transform.position.y, ai.transform.position.z));
        isStopped = true;
        currentState = CarStates.FILLINGGAS;
    }

    public void start()
    {
        ai.SetDestination(destination);
        isStopped = false;
        currentState = CarStates.DRIVING;
    }

    public void addGas(float gasRate)
    {
        gasLevel += gasRate;
        currentState = CarStates.FILLINGGAS;
    }

    public bool fullTank()
    {
        if (gasLevel >= gasMax)
        {
            currentState = CarStates.DRIVING;
            Debug.Log("I am now driving again!");
            return true;
        }
        else{
            return false;
        }
    }

    public void findGasStation()
    {
         //float shortest = Vector3.Distance(transform.position, GasPumps[0].transform.position);
         float shortest = 1000000;
            gasPump = GasPumps[0];
            for (int i = 1; i< GasPumps.Length; i++)
            {
                float d = Vector3.Distance(transform.position, GasPumps[i].transform.position);
                ps = GasPumps[i].GetComponent<pumpScript>();
                Debug.Log(ps.occupied);

                if (d< shortest && ps.occupied == false)
                {
                    shortest = d;
                    gasPump = GasPumps[i];
                }
            }
            //Debug.Log("Found Gas Station that Is Closest");
    }

    public bool checkGasPump()
    {
        return gasPump.GetComponent<pumpScript>().occupied;
    }
}
