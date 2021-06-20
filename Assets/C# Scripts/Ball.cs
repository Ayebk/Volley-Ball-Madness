using Assets.C__Scripts;
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
        }
        else
        {
            StartCoroutine(DragCheckAndSend());
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

    private void onBallPositionDraggingReceived(string position)
    {
        string[] splitMessage = position.Split();
        Vector3 ballPosition = new Vector3(float.Parse(splitMessage[1]), float.Parse(splitMessage[2]));
        StartCoroutine(MoveToPositionInTime(ballPosition));
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

    IEnumerator MoveToPositionInTime(Vector3 destination) 
    {
        float timeToArrive = 0.095f;
        float timePercentage = 0f;
        Vector3 startPosition = transform.position;
        while (timePercentage < 1)
        {
            timePercentage += Time.deltaTime / timeToArrive;
            transform.position = Vector3.Lerp(startPosition, destination, timePercentage);
            yield return null;
        }
    }
}
