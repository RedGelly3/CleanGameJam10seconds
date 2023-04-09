using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportementLapin : MonoBehaviour
{
    public Transform player;
    private Rigidbody2D animalBody;
    [SerializeField]
    private float walkingSpeed = 3.5f;
    [SerializeField]
    private float jumpPower = 3.0f;

    private Vector2 InputVelocity;

    private void Awake()
    {
        player = gameObject.transform;
        if (transform.parent is not null)
        {
            player = gameObject.transform.parent.transform;
        }

        gameObject.transform.position = player.position;

        animalBody = gameObject.GetComponent<Rigidbody2D>();
        InputVelocity = Vector2.zero;
    }
    // Update is called once per frame
    void Update()
    {

        Mouvement();

    }

    void Mouvement()
    {
        animalBody.velocity.Set(animalBody.velocity.x - InputVelocity.x, animalBody.velocity.y - InputVelocity.y);
        InputVelocity = Vector2.zero;
        //vitesse - InputVelocity 

        if (Input.GetKeyDown(KeyCode.W)) //up key  
        {
            // TODO => if grounded : 
            animalBody.AddForce(Vector3.up * jumpPower * 100);
        }
        if (Input.GetKey(KeyCode.S)) //down key 
        {

        }
        if (Input.GetKey(KeyCode.D)) // Right key 
        {
            InputVelocity += Vector2.right * walkingSpeed;
            //animalBody.AddForce(Vector3.right * walkingSpeed * 100);

        }
        if (Input.GetKeyUp(KeyCode.D)) // Right key 
        {

            //animalBody.AddForce(-Vector3.right * walkingSpeed * 100);

        }

        if (Input.GetKey(KeyCode.A)) // Left key
        {
            InputVelocity += Vector2.left * walkingSpeed;
            //animalBody.AddForce(Vector3.left * walkingSpeed * 100);
        }
        if (Input.GetKeyUp(KeyCode.A)) // Right key 
        {

            //animalBody.AddForce(-Vector3.left * walkingSpeed * 100);

        }
        animalBody.velocity.Set(animalBody.velocity.x + InputVelocity.x, animalBody.velocity.y + InputVelocity.y);
        player.position = animalBody.position;
        gameObject.transform.position = player.position;

    }
}