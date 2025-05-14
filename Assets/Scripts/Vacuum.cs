using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Vacuum : MonoBehaviour
{
    public int speed; 
    public GameObject player; //this will hold a link to the player
    private bool isFalling = false;   // Flag to check if vacuum should start falling
    private float delayTime = 2f;     // The delay before the vacuum starts falling
    private float timer = 0f; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        speed = Random.Range(10,20);    
    }

    // Update is called once per frame
    void Update() {
        if (!isFalling)
        {
            timer += Time.deltaTime;  // Increment the timer by the time passed since the last frame
            if (timer >= delayTime)
            {
                isFalling = true;  // Start falling once the delay time has passed
            }
            return;  // Skip the falling logic if the object is not ready to fall yet
        }
        //Moves object down the screen
        //Vector3 x, y, z 0,-1,0 
        transform.Translate(Vector3.down * Time.deltaTime * speed);
        
        //Check to see if it has reached the bottom of the screen
        if (transform.position.y < -18.9)
        {
            MoveToTop();

        }
    }
//Moves object back to top of the screen and give a new random x coordinate   
    void MoveToTop() {
        //generate a random number
        float randomNumber = Random.Range(-31f, 31f);
        //new vector3 to store the new position for the object to move 
        Vector3 newPos = new Vector3(randomNumber,18.9f,0);
        //Moves object to the new positon
        transform.position = newPos;
        //Give the object a new random speed
        speed = Random.Range(10,20);        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            player.SendMessage("LoseCourage");
        }
        MoveToTop();
      
    }
}
