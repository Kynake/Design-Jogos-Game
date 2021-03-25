using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class RocketController : MonoBehaviour
{

    public GameObject _leftThruster;
    public GameObject _rightThruster;

    public float thrusterStrength;
    public float forwardStrength;
    public float recoilStrength;
    public float recoilTimeout;

    public delegate void ColisaoAcao();
    public static event ColisaoAcao colisaoAcao;

    // Considered to be at low speed if under this velocity and moving forward
    public float lowSpeedLimit;
    public float lowSpeedBoostStrength;

    // If the angle between current momentum and rocket facing is less than this,
    // consider it to be moving backwards
    public float backwardsAngle;
    public float backwardStrength;

    public LayerMask scoreLayer;
    public LayerMask asteroidLayer;
    public LayerMask flamingAsteroidsLayer;
    public LayerMask indestructableLayer;

    private static GameObject gameControllerObject = null;
    private static GameController gameController = null;

    public GameObject smokeTrail;
    public Queue<GameObject> smokeTrailList;

    [SerializeField] private GameObject _frontMarker = null;
    private const float _markerMargin = 0.1f;

    private Rigidbody2D _rbMain;
    private Rigidbody2D _rbLeft;
    private Rigidbody2D _rbRight;

    // Initial direction of the thruster on the Rocket prefab
    private Vector2 _prefabDirection = Vector2.down;
    private Vector2 _leftDirection;
    private Vector2 _rightDirection;

    private Coroutine recoilFromWall = null;

    private void Start()
    {
        smokeTrailList = new Queue<GameObject>();
        _rbMain = GetComponent<Rigidbody2D>();
        _rbLeft = _leftThruster.GetComponent<Rigidbody2D>();
        _rbRight = _rightThruster.GetComponent<Rigidbody2D>();
        _leftDirection = _prefabDirection;
        _rightDirection = _prefabDirection;

        // Get gameController reference to update game stats
        if(gameControllerObject == null) {
            gameControllerObject = GameObject.Find("Canvas");
        }

        if(gameControllerObject != null && gameController == null) {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        StartCoroutine(LeaveTrail());
        StartCoroutine(EraseTrail());
    }


    private void FixedUpdate()
    {
        updateRotation(_rbLeft, _leftDirection);
        updateRotation(_rbRight, _rightDirection);

        updateThrust(_rbLeft);
        updateThrust(_rbRight);

        updateCentralStrength();

        updateIsStuck();
    }


    // Capture Inputs
    private void OnLeftThruster(InputValue input) => _leftDirection = getThrusterDirection(input);
    private void OnRightThruster(InputValue input) => _rightDirection = getThrusterDirection(input);

    // Level change stuff
    private void OnResetLevel(InputValue input) {
        GameController.restarts++;
        gameController.addLevelTimeToTotal();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnNextLevel(InputValue input) {
        gameController.addLevelTimeToTotal();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnPreviousLevel(InputValue input) {
        gameController.addLevelTimeToTotal();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private Vector2 getThrusterDirection(InputValue input)
    {
        var direction = input.Get<Vector2>();
        return direction == Vector2.zero ? _prefabDirection : direction.normalized;
    }

    // Rotate thrusters according to input
    private void updateRotation(Rigidbody2D thruster, Vector2 direction)
    {
        var relativeDirection = (Vector2)transform.TransformDirection(direction);
        var relativeAngle = Vector2.SignedAngle(_prefabDirection, relativeDirection);

#if UNITY_EDITOR
        Debug.DrawRay(thruster.position, relativeDirection, Color.red);
        Debug.DrawRay(thruster.position, direction, Color.green);
#endif

        // thruster.MoveRotation(angle); // Applies rotation inertia to ship. Can hit walls
        thruster.SetRotation(relativeAngle); // Does not apply inertia to ship on Rotation. Will teleport inside walls
    }

    // Update force for each thruster
    private void updateThrust(Rigidbody2D thruster)
    {
        var relativeDown = thruster.transform.TransformDirection(_prefabDirection);

        // Negate speed and apply force to direction inverse of thrust vector
        thruster.AddForce(relativeDown * -thrusterStrength);
    }

    // Update force that is always applied to the rocket
    private void updateCentralStrength()
    {
        var angleFromHeading = Vector2.Angle(_rbMain.velocity, _rbMain.transform.TransformDirection(_prefabDirection));
        float strength;
        if (angleFromHeading < backwardsAngle)
        {
            strength = backwardStrength;
        }
        else if (_rbMain.velocity.magnitude > lowSpeedLimit)
        {
            strength = forwardStrength;
        }
        else
        {
            strength = lowSpeedBoostStrength;
        }

        var force = _rbMain.transform.TransformDirection(_prefabDirection) * -strength;
        _rbMain.AddForce(force);

#if UNITY_EDITOR
        Debug.DrawRay(_rbMain.position, force * -1, Color.blue);
#endif
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var layer = collider.gameObject.layer.toLayerMask();
        if ((layer & scoreLayer) != 0)
        {
            colisaoAcao?.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // print("Collison");
        var layer = collision.gameObject.layer.toLayerMask();
        if (layer == flamingAsteroidsLayer)
        {
            // dead
            GameController.deaths++;
            gameController.addLevelTimeToTotal();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        } else if(layer == indestructableLayer) {
            GameController.wallCollisions++;
        }
    }

    public IEnumerator LeaveTrail()
    {

        while (true)
        {
            print("LeaveTrail");
            Vector3 pos = transform.position;
            GameObject trail = Instantiate(this.smokeTrail, pos, Quaternion.identity);
            smokeTrailList.Enqueue(trail);
            trail.SetActive(true);
            yield return new WaitForSeconds(0.5f);

        }

    }

    IEnumerator EraseTrail()
    {

        while (true)
        {
            if (smokeTrailList.Count == 0)
            {
                yield return new WaitForSeconds(5);
            }
            print("EraseTrail");
            GameObject trail = smokeTrailList.Dequeue();
            Destroy(trail);
            yield return new WaitForSeconds(0.7f);
        }
    }

    private void updateIsStuck()
    {
        #if UNITY_EDITOR
        Debug.DrawRay(_frontMarker.transform.position, _rbMain.transform.TransformDirection(_prefabDirection) * -_markerMargin, Color.red);
        #endif

        var hit = Physics2D.Raycast(_frontMarker.transform.position, _rbMain.transform.TransformDirection(_prefabDirection) * -1, _markerMargin, indestructableLayer);
        if(hit.collider != null && recoilFromWall == null) {
            recoilFromWall = StartCoroutine(recoil());
        } else if(hit.collider == null && recoilFromWall != null) {
            StopCoroutine(recoilFromWall);
            recoilFromWall = null;
        }
    }

    private IEnumerator recoil()
    {
        yield return new WaitForSeconds(recoilTimeout);
        var force = _rbMain.transform.TransformDirection(_prefabDirection) * recoilStrength;
        _rbMain.AddForce(force, ForceMode2D.Impulse);

    }

}
