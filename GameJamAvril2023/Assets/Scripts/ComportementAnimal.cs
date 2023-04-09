using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportementAnimal : MonoBehaviour
{
    public Transform player;
    private Rigidbody2D animalBody;
    [SerializeField]
    private float walkingSpeed = 1.0f;
    [SerializeField]
    private float jumpPower = 1.0f;
    private void Awake()
    {
        player = gameObject.transform;
        if (transform.parent is not null) {
            player = gameObject.transform.parent.transform;
        }
       
        gameObject.transform.position = player.position;

        animalBody = gameObject.GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {

        Mouvement();

    }

    void Mouvement() {

        //vitesse - InputVelocity 

        if (Input.GetKeyDown(KeyCode.W)) //up key  
        {
            // TODO => if grounded : 
            animalBody.AddForce(Vector3.up * jumpPower *100);
        }
        if (Input.GetKey(KeyCode.S)) //down key 
        {

        }
        if (Input.GetKeyDown(KeyCode.D)) // Right key 
        {

            animalBody.AddForce(Vector3.right * walkingSpeed*100);

        }
        if (Input.GetKeyUp(KeyCode.D)) // Right key 
        {

            animalBody.AddForce(-Vector3.right * walkingSpeed*100);

        }

        if (Input.GetKeyDown(KeyCode.A)) // Left key
        {
            animalBody.AddForce(Vector3.left * walkingSpeed*100);
            
        }
        if (Input.GetKeyUp(KeyCode.A)) // Right key 
        {

            animalBody.AddForce(-Vector3.left * walkingSpeed*100);

        }
        player.position = animalBody.position;
        gameObject.transform.position = player.position;
       
    }
}
