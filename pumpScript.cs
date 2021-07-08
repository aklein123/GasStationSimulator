using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// We need to constantly update the pump script variable in the car. Right now, it is just grabbing a snapshot and not updating.

public class pumpScript : MonoBehaviour
{
    public bool occupied = false;
    carScript car;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter(Collider c){
        Debug.Log("I have been triggered by an unidentified object");
        if (c.CompareTag("Car") && occupied == false)
        {
            car = c.GetComponent<carScript>();
            occupied = true;
            car.stop();
            Debug.Log("car has entered the station");
            Debug.Log(occupied);
        }
    }

    public void FixedUpdate()
    {
        if (occupied)
        {
            
            car.addGas(Time.deltaTime * 2);
            if (car.fullTank())
            {
                occupied = false;
                car.start();
                car = null;

            }
        }
    }
}
