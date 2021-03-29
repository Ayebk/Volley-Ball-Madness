using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{

    public SpriteRenderer ball1;
    public GameObject Ball2Prefab;
  //  public GameObject BallOpponent; // LATER.. FOR KILL POWER UP
    private Vector3 _initialPosition;
    bool powerSpawned = false;
    public GameObject[] powerups = new GameObject[4] ;


    [SerializeField] private float _launchPower = 500;


    public float xMin, xMax;
    public float yMin, yMax;



    // for double checking speed ball
    public float speed = 0;
    Vector3 lastPosition = Vector3.zero;


    private void spawnrandom()
    {
        while (!powerSpawned)
        {     
            Vector3 powerPosition = new Vector3(Random.Range(-2, 2), Random.Range(0, 8), 0);
            Instantiate(powerups[Random.Range(0, powerups.Length)], powerPosition, Quaternion.identity);
            powerSpawned = true;
        }
    }


    private void Start() // for stucked new values
    {
        spawnrandom();
        ball1.GetComponent<Rigidbody2D>().sharedMaterial.bounciness = 0.6f; // IMPORTENT(this will decide every time) : 
                                                                            //when you hit power up heavy, 
                                                                            //the bounciness stuck to the new vlaue so we 
                                                                            //need to start again
        powerups[0] = GameObject.FindGameObjectWithTag("PU_kill");
        powerups[1] = GameObject.FindGameObjectWithTag("PU_heavy");
        powerups[2] = GameObject.FindGameObjectWithTag("PU_speed");
        powerups[3] = GameObject.FindGameObjectWithTag("PU_double");
    }

    void FixedUpdate()
    {
        speed = (ball1.transform.position - lastPosition).magnitude;
        lastPosition = ball1.transform.position;
    }

    private void spawnBall2()
    {

        Ball ball1script = ball1.GetComponent<Ball>();
        Vector2 directionToInitialPosition = ball1script._initialPosition;
        GameObject a = Instantiate(Ball2Prefab, ball1.transform.position,ball1.transform.rotation) as GameObject;
        Physics2D.IgnoreCollision(a.GetComponent<Collider2D>(), ball1.GetComponent<Collider2D>());    
        a.GetComponent<Rigidbody2D>().AddForce(-directionToInitialPosition * a.GetComponent<Rigidbody2D>().mass / Time.fixedDeltaTime);
        a.GetComponent<Rigidbody2D>().gravityScale = 1;

    }


    void OnTriggerEnter2D(Collider2D collider)
    {

        _initialPosition = ball1.transform.position;

        if (collider.gameObject.tag == "Ball")
        {
            powerSpawned = true;
            Destroy(gameObject);
            if (gameObject.tag == "PU_double")
            {

                spawnBall2();

            }

            if (gameObject.tag == "PU_speed")
            {


                ball1.color = Color.blue;

                Ball ball1script = ball1.GetComponent<Ball>();

                if (ball1script)
                {
                    Vector2 directionToInitialPosition = ball1script._initialPosition - ball1script.transform.position;
                    ball1.GetComponent<Rigidbody2D>().AddForce(-directionToInitialPosition * _launchPower);


                }
            }

            if (gameObject.tag == "PU_heavy")
            {

                ball1.color = Color.grey;
                ball1.GetComponent<Rigidbody2D>().sharedMaterial.bounciness = 0.1f;
            }

            if (gameObject.tag == "PU_kill")
            {

                ball1.color = Color.yellow;
                // Destroy(BallOpponent); // LATER

            }



        }
    }
}