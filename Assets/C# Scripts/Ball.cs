using Assets.C__Scripts;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{


    public int ControlingPlayer = 0;
    public Vector3 _initialPosition;
    public bool _birdWasLaunched;
    public float _timeSettingAround;
    public string PowerUpType;
    public bool playerControls = false;
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
        }
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
            string ballPosition = "BALLPOS "+Player.GameRoomId+" " + transform.position.x + " " + transform.position.y;
            //UdpClient.GetComponent<UdpClientHandler>().SendUdpMessage(ballPosition,true);
        }

    }
    private void onBallPositionDraggingReceived(string position)
    {
        string[] splitMessage = position.Split();
        Vector3 ballPosition = new Vector3(float.Parse(splitMessage[1]), float.Parse(splitMessage[2]));
        transform.position = ballPosition;
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


}
