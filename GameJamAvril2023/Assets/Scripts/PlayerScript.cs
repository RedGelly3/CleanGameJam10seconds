using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public double time;
    public List<GameObject> transformations;

    public Transform player;
    private Rigidbody2D animalBody;
    [SerializeField]
    private float walkingSpeed = 5.0f;
    [SerializeField]
    private float jumpPower = 3.50f;
    private bool isGrounded;

    public GameObject nextTransformation;
    public GameObject currentTransformation;
    public GameObject Logo;

    private void Awake()
    {
        player = gameObject.transform;

        gameObject.transform.position = player.position;

        animalBody = gameObject.GetComponent<Rigidbody2D>();

        isGrounded = false;

        time = 10f;
        Cursor.visible = false;
        StartCoroutine(LaunchTime());
        currentTransformation = FindTransformationActuelle(gameObject);
        nextTransformation = null;

    }


    IEnumerator LaunchTime()
    {
        float oldTime = Time.realtimeSinceStartup;
        while (true)
        {   
           
            time -= Time.realtimeSinceStartup - oldTime;
            oldTime = Time.realtimeSinceStartup;

            if (time <= 0.0f)
            {
                Transformation();
                time = 10.0f;
            }
            else if (time <= 5.0f && nextTransformation==null)
            {
                nextTransformation = TransformationRoulette();
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    void Transformation()
    {
        Destroy(currentTransformation);
        nextTransformation.transform.parent = gameObject.transform;
        nextTransformation.transform.localPosition = Vector3.zero;
        animalBody.mass = nextTransformation.GetComponent<ValuesAnimal>().mass;
        jumpPower = nextTransformation.GetComponent<ValuesAnimal>().jumpForce;
        walkingSpeed = nextTransformation.GetComponent<ValuesAnimal>().speed;
        currentTransformation = nextTransformation;
        nextTransformation = null;
        Logo.GetComponent<LogoTalisman>().nextTransformation = nextTransformation;
        Logo.GetComponent<LogoTalisman>().currentTransformation = currentTransformation;
        Logo.GetComponent<LogoTalisman>().updated = true;

    }
    GameObject TransformationRoulette()
    {   
        currentTransformation = FindTransformationActuelle(gameObject);
        Debug.Log(currentTransformation.name);
        GameObject newTransformation = Instantiate(transformations[0], gameObject.transform.position, Quaternion.identity);
        //Vector2 playerVelocity = transformationActuelle.GetComponent<Rigidbody2D>().velocity;
        do
        {
            Destroy(newTransformation);
            double dice = Random.Range(0f, 100f);
            // set new prefab et lui donner la transform player
            if (dice > 50)
            {

                newTransformation = Instantiate(transformations[0], gameObject.transform.position, Quaternion.identity);

            }
            else
            {
                newTransformation = Instantiate(transformations[1], gameObject.transform.position, Quaternion.identity);
            }
            
        } while (GetObjectNameWithoutCareForClone(currentTransformation) == GetObjectNameWithoutCareForClone(newTransformation));
        
        /*
        if (GetObjectNameWithoutCareForClone(newTransformation) == "Sanglier")
        {
            newTransformation.GetComponent<ComportementSanglier>().player = gameObject.transform;
        }
        else if (GetObjectNameWithoutCareForClone(newTransformation) == "Lapin")
        {
            newTransformation.GetComponent<ComportementLapin>().player = gameObject.transform;
        }
        else if (GetObjectNameWithoutCareForClone(newTransformation) == "Escargot")
        {
            //newTransformation.GetComponent<ComportementEscargot>().player = gameObject.transform;
        }
        else if (GetObjectNameWithoutCareForClone(newTransformation) == "Herisson")
        {
            //newTransformation.GetComponent<ComportementHerisson>().player = gameObject.transform;
        }
        */
        newTransformation.transform.tag = "Transformation";
        newTransformation.transform.position =new Vector3(-1000,-1000,-1000);
        Logo.GetComponent<LogoTalisman>().nextTransformation = newTransformation;
        Logo.GetComponent<LogoTalisman>().currentTransformation = currentTransformation;
        Logo.GetComponent<LogoTalisman>().updated = true;
        //newTransformation.GetComponent<Rigidbody2D>().velocity += playerVelocity;
        return newTransformation;

    }

    public static string GetObjectNameWithoutCareForClone(GameObject gameObject) {
        string name = gameObject.transform.name;
        return name.Split('(')[0];    
    }
    public static GameObject FindTransformationActuelle(GameObject parent)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).gameObject.tag == "Transformation")
            {
                return parent.transform.GetChild(i).gameObject;
            }

        }
        print("error transformationActuelle not found");
        return null;
    }
    void Update()
    {

        Mouvement();

    }

    void Mouvement()
    {
        //vitesse - InputVelocity 

        if (Input.GetKeyDown(KeyCode.W)) //up key  
        {
            if (isGrounded)
            {
                animalBody.AddForce(Vector3.up * jumpPower * 100);
            }
        }
        if (Input.GetKey(KeyCode.S)) //down key 
        {
            //Nothing To Do
        }
        if (Input.GetKey(KeyCode.D)) // Right key 
        {
            if (isGrounded)
            {
                player.transform.Translate(Vector3.right * walkingSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                animalBody.AddForce(Vector3.right * walkingSpeed);
            }
        }
        if (Input.GetKey(KeyCode.A)) // Left key
        {
            if (isGrounded)
            {
                player.transform.Translate(-Vector3.right * walkingSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                animalBody.AddForce(Vector3.right * walkingSpeed);
            }
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = true;
            Debug.Log("On ground");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = false;
            Debug.Log("ExitGround");
        }
    }
}