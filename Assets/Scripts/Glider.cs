using UnityEngine;

public class Glider : MonoBehaviour
{
    [SerializeField]
    new private Rigidbody rigidbody;

    [SerializeField]
    private Camera groundCamera;

    [SerializeField]
    private Camera gliderCamera;

    [SerializeField]
    private Vector3 startingVelocity;

    [SerializeField]
    private float rollScaler = 1;

    [SerializeField]
    private float elevatorMin = 0;

    [SerializeField]
    private float elevatorMax = 1;

    [SerializeField]
    private float airDensity = 1; // P
    [SerializeField]
    private float wingArea = 1; // S

    private Vector3 coefficientOfLift; // C
    public Vector3 lift; // L

    public float speed;

    private float horizontalInput;
    private float verticalInput;
    private Quaternion stickRotation;

    private Vector3 startingPosition;
    private Quaternion startingRotation;

    public float maxSpeedReached;

    private void Awake()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;
        rigidbody.velocity = startingVelocity;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Reset"))
        {
            transform.SetPositionAndRotation(startingPosition, startingRotation);
            rigidbody.velocity = startingVelocity;
        }

        //if (transform.position.z > 120)
        //{
        //    transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
        //}

        groundCamera.transform.SetPositionAndRotation(groundCamera.transform.position, Quaternion.LookRotation(transform.position - groundCamera.transform.position));
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(rigidbody.velocity, transform.rotation * Quaternion.Euler(0, 0, horizontalInput * rollScaler) * Vector3.up);
        coefficientOfLift = transform.up * Map(verticalInput, -1, 1, elevatorMax, elevatorMin);
        lift = coefficientOfLift * (airDensity * Mathf.Pow(rigidbody.velocity.magnitude, 2)) / 2 * wingArea;
        rigidbody.AddForce(lift);
        speed = rigidbody.velocity.magnitude;

        // debug
        if (speed > maxSpeedReached)
        {
            maxSpeedReached = speed;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + coefficientOfLift * 4);
    }

    private static float Map(float value, float istart, float istop, float ostart, float ostop)
    {
        return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
    }
}

