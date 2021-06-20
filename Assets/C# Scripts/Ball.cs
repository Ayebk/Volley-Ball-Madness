using Assets.C__Scripts;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{


    public Vector3 InitialSize;
    public int ControlingPlayer = 0;
    public Vector3 _initialPosition;
    public bool _birdWasLaunched;
    public float _timeSettingAround;
    public string PowerUpType;
    public bool playerControls = false;
    public bool BallDragging = false;
    public bool Hit = false;
    public GameObject UdpClient;
    [SerializeField] private float _launchPower = 500;



    /*
    LineRenderer linePrefab;
    private LineRenderer lr;
    private LineRenderer lr2;
    */


    void Start()
    {
        UdpClient = GameObject.Find("UdpClientHandler");
        if (!playerControls)
        {
            UdpClient.GetComponent<UdpClientHandler>().onBallPosition = onBallPositionDraggingReceived;
            UdpClient.GetComponent<UdpClientHandler>().onBallReleased = onBallShot;
            UdpClient.GetComponent<UdpClientHandler>().onBallCollision = onBallCollision;
            UdpClient.GetComponent<UdpClientHandler>().onBallReset = onBallReset;
            UdpClient.GetComponent<UdpClientHandler>().onBallCurrentPosition = onBallCurrentPosition;
        }
        else
        {
            StartCoroutine(DragCheckAndSend());
            StartCoroutine(CheckCurrentAndSend());
        }
        InitialSize = transform.localScale;
    }

    public void Awake()
    {
        _initialPosition = transform.position;
        if (Player.PlayerNumber == ControlingPlayer)
        {
            playerControls = true;
        }
    }
    private void Update()
    {

        if (playerControls)
        {
            /*
            lr = new GameObject().AddComponent<LineRenderer>();
            lr.SetPosition(1, _initialPosition);
            lr.SetPosition(0, transform.position);

            lr2 = new GameObject().AddComponent<LineRenderer>();
            lr2.SetPosition(1, _initialPosition);
            lr2.SetPosition(0, transform.position);

            */

            GetComponent<LineRenderer>().SetPosition(1, _initialPosition);
            GetComponent<LineRenderer>().SetPosition(0, transform.position);


            if (_birdWasLaunched &&
                GetComponent<Rigidbody2D>().velocity.magnitude <= 0.1)
            {
                _timeSettingAround += Time.deltaTime;
            }

            if (transform.position.y > 10 ||
                transform.position.y < -10 ||
                transform.position.x > 11 ||
                transform.position.x < -11 ||
                _timeSettingAround > 3)
            {
                string currentSceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentSceneName);
            }
        }


    }

    private void OnMouseDown()
    {
        if (playerControls)
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
            GetComponent<LineRenderer>().enabled = true;
        }
    }

    public void OnMouseUp()
    {
        if (playerControls)
        {
            BallDragging = false;
            GetComponent<SpriteRenderer>().color = Color.white;
            Vector2 directionToInitialPosition = _initialPosition - transform.position;
            string ballDirection = "BALLDIR " + Player.GameRoomId + " " + directionToInitialPosition.x + " " + directionToInitialPosition.y;
            GetComponent<Rigidbody2D>().AddForce(directionToInitialPosition * _launchPower);
            GetComponent<Rigidbody2D>().gravityScale = 1;
            _birdWasLaunched = true;
            GetComponent<LineRenderer>().enabled = false;
            SoundManger.PlaySound("Fly");
            UdpClient.GetComponent<UdpClientHandler>().SendUdpMessage(ballDirection,false);
        }
    }

    private void OnMouseDrag()
    {
        if (playerControls)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(newPosition.x, newPosition.y);
            BallDragging = true;
        }

    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        
        if (playerControls)
        {
            Vector3 position = transform.position;
            Vector2 velocity = transform.GetComponent<Rigidbody2D>().velocity;
            float rotation = transform.GetComponent<Rigidbody2D>().rotation;
            string ballCollision = "BALLCOL " + Player.GameRoomId + " " + position.x + " " + position.y;
            ballCollision += " " + velocity.x + " " + velocity.y;
            ballCollision += " " + rotation;
            UdpClient.GetComponent<UdpClientHandler>().SendUdpMessage(ballCollision, false);
        }
        
    }
    */


    private void OnCollisionExit2D(Collision2D collision)
    {
        //Ball ball = collision.collider.GetComponent<Ball>();
        if (playerControls)
        {
            /*
            if (ball != null && ball._birdWasLaunched)
            {
                Vector3 positionBallOne = transform.position;
                Vector2 velocityBallOne = transform.GetComponent<Rigidbody2D>().velocity;
                float rotationBallOne = transform.GetComponent<Rigidbody2D>().rotation;
                string ballCollision = "BALLHIT " + Player.GameRoomId + " " + positionBallOne.x + " " + positionBallOne.y;
                ballCollision += " " + velocityBallOne.x + " " + velocityBallOne.y;
                ballCollision += " " + rotationBallOne;

                Vector3 positionBallTwo = ball.transform.position;
                Vector2 velocityBallTwo = transform.GetComponent<Rigidbody2D>().velocity;
                float rotationBallTwo = transform.GetComponent<Rigidbody2D>().rotation;
                ballCollision += " " + positionBallTwo.x + " " + positionBallTwo.y;
                ballCollision += " " + velocityBallTwo.x + " " + velocityBallTwo.y;
                ballCollision += " " + rotationBallTwo;
                UdpClient.GetComponent<UdpClientHandler>().SendUdpMessage(ballCollision, false);
            }
            */
                Vector3 position = transform.position;
                Vector2 velocity = transform.GetComponent<Rigidbody2D>().velocity;
                float rotation = transform.GetComponent<Rigidbody2D>().rotation;
                string ballCollision = "BALLCOL " + Player.GameRoomId + " " + position.x + " " + position.y;
                ballCollision += " " + velocity.x + " " + velocity.y;
                ballCollision += " " + rotation;
                UdpClient.GetComponent<UdpClientHandler>().SendUdpMessage(ballCollision, false);
        }
    }

    private void onBallPositionDraggingReceived(string position)
    {
        string[] splitMessage = position.Split();
        Vector3 ballPosition = new Vector3(float.Parse(splitMessage[1]), float.Parse(splitMessage[2]));
        StartCoroutine(MoveToPositionInTime(ballPosition, 0.095f));
    }

    private void onBallShot(string info) 
    {
        string[] splitMessage = info.Split();
        Vector2 ballDirection = new Vector2(float.Parse(splitMessage[1]), float.Parse(splitMessage[2]));
        GetComponent<Rigidbody2D>().AddForce(ballDirection * _launchPower);
        GetComponent<Rigidbody2D>().gravityScale = 1;
        _birdWasLaunched = true;
        GetComponent<LineRenderer>().enabled = false;
        SoundManger.PlaySound("Fly");
    }

    public void onBallCollision(string info)
    {
        string[] splitMessage = info.Split();
        Vector2 position = new Vector2(float.Parse(splitMessage[1]), float.Parse(splitMessage[2]));
        Vector2 velocity = new Vector2(float.Parse(splitMessage[3]), float.Parse(splitMessage[4]));
        transform.position = position;
        transform.GetComponent<Rigidbody2D>().velocity = velocity;
        transform.GetComponent<Rigidbody2D>().rotation = float.Parse(splitMessage[5]);
        Debug.Log(info);
    }

    public void onBallCurrentPosition(string info)
    {
        string[] splitMessage = info.Split();
        Vector2 position = new Vector2(float.Parse(splitMessage[1]), float.Parse(splitMessage[2]));
        Vector2 velocity = new Vector2(float.Parse(splitMessage[3]), float.Parse(splitMessage[4]));

        if (Vector2.Distance(position, transform.position) > 0.2f)
        {
            StartCoroutine(MoveToPositionInTime(position, 0.02f));
            StartCoroutine(SetVelocityInTime(velocity, 0.02f));
            transform.GetComponent<Rigidbody2D>().rotation = float.Parse(splitMessage[5]);
            
        }
        
    

        Debug.Log(info);
    }

    public void ResetBall(bool SendCommand)
    {
        Hit = false;
        transform.position = _initialPosition;
        transform.localScale = InitialSize;
        transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        transform.GetComponent<Rigidbody2D>().rotation = 0;
        _birdWasLaunched = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        if (SendCommand)
        {
            string resetBall = "BALLRESET " + Player.GameRoomId;
            UdpClient.GetComponent<UdpClientHandler>().SendUdpMessage(resetBall, false);
        }
    }

    /*this players' ball state will be in the first data set
     * the second data set is the remote players ball state
     * SO [This Players Data Set] [Remote Players Data Set]
    */
    /// <summary>
    /// handler for the ball collision with one another
    /// syncs the current ball states if this handler was fired 
    /// by the UdpClientHandler delegate
    /// </summary>
    /// <param name="info"></param>
    private void onBallCollisionWithOther(string info)
    {
        //handling in case this is this players ball
        if (playerControls)
        {
            string[] splitMessage = info.Split();
            Vector2 position = new Vector2(float.Parse(splitMessage[1]), float.Parse(splitMessage[2]));
            Vector2 velocity = new Vector2(float.Parse(splitMessage[3]), float.Parse(splitMessage[4]));
            transform.position = position;
            transform.GetComponent<Rigidbody2D>().velocity = velocity;
            transform.GetComponent<Rigidbody2D>().rotation = float.Parse(splitMessage[5]);
        }
        //handling in case this is the remote players ball
        else
        {
            string[] splitMessage = info.Split();
            Vector2 position = new Vector2(float.Parse(splitMessage[6]), float.Parse(splitMessage[7]));
            Vector2 velocity = new Vector2(float.Parse(splitMessage[8]), float.Parse(splitMessage[9]));
            transform.position = position;
            transform.GetComponent<Rigidbody2D>().velocity = velocity;
            transform.GetComponent<Rigidbody2D>().rotation = float.Parse(splitMessage[10]);
        }
    }

    public void onBallReset(string info)
    {
        ResetBall(false);
    }

    IEnumerator DragCheckAndSend() 
    {
        while (true)
        {
            if (BallDragging)
            {
                string ballPosition = "BALLPOS " + Player.GameRoomId + " " + transform.position.x + " " + transform.position.y;
                UdpClient.GetComponent<UdpClientHandler>().SendUdpMessage(ballPosition, false);
            }
            yield return new WaitForSeconds(.1f);
        }
    }

    IEnumerator CheckCurrentAndSend()
    {
        while (true)
        {
            if (_birdWasLaunched)
            {
                Vector3 position = transform.position;
                Vector2 velocity = transform.GetComponent<Rigidbody2D>().velocity;
                float rotation = transform.GetComponent<Rigidbody2D>().rotation;
                string ballCollision = "BALLCU " + Player.GameRoomId + " " + position.x + " " + position.y;
                ballCollision += " " + velocity.x + " " + velocity.y;
                ballCollision += " " + rotation;
                UdpClient.GetComponent<UdpClientHandler>().SendUdpMessage(ballCollision, false);
            }
            yield return new WaitForSeconds(.05f);
        }
    }

    IEnumerator MoveToPositionInTime(Vector3 destination,float timeToArrive) 
    {
        float timePercentage = 0f;
        Vector3 startPosition = transform.position;
        while (timePercentage < 1)
        {
            timePercentage += Time.deltaTime / timeToArrive;
            transform.position = Vector3.Lerp(startPosition, destination, timePercentage);
            yield return null;
        }
    }

    IEnumerator SetVelocityInTime(Vector3 velocity, float timeToArrive)
    {
        float timePecentage = 0f;
        Vector2 startVelocity = transform.GetComponent<Rigidbody2D>().velocity;
        while (timePecentage < 1)
        {
            timePecentage += Time.deltaTime / timeToArrive;
            transform.GetComponent<Rigidbody2D>().velocity = Vector2.Lerp(startVelocity, velocity, timePecentage);
            yield return null;
        }
    }
}
