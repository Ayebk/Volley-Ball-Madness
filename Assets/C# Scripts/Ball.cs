using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{

    public Vector3 _initialPosition;
    public bool _birdWasLaunched;
    public float _timeSettingAround;


    [SerializeField] private float _launchPower = 500;

    /*
    LineRenderer linePrefab;
    private LineRenderer lr;
    private LineRenderer lr2;

    void Start()
    {
        lr = Instantiate(linePrefab, transform);
        lr2 = Instantiate(linePrefab, transform);

     
    }
    */


    public void Awake()
    {
        _initialPosition = transform.position;
    }
    private void Update()
    {

        Vector3 moveDirection = gameObject.transform.position;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg * 10;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

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

    private void OnMouseDown()
    {
        GetComponent<SpriteRenderer>().color = Color.blue;
        GetComponent<LineRenderer>().enabled = true;
    }

    public void OnMouseUp()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        Vector2 directionToInitialPosition = _initialPosition - transform.position;
        GetComponent<Rigidbody2D>().AddForce(directionToInitialPosition * _launchPower);
        GetComponent<Rigidbody2D>().gravityScale = 1;
        _birdWasLaunched = true;

        GetComponent<LineRenderer>().enabled = false;


    }

    private void OnMouseDrag()
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(newPosition.x, newPosition.y);
    }
}
