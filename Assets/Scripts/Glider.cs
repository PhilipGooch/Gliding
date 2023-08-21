using UnityEngine;
using UnityEngine.UIElements;

public class Glider : MonoBehaviour
{
    [SerializeField]
    new private Rigidbody rigidbody;

    [SerializeField]
    private Camera groundCamera;
    [SerializeField]
    private Camera gliderCamera;

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
    public float coefficientOfLift; // CL
    private Vector3 lift; // L
    public float coefficientOfDrag; // CD
    private Vector3 drag; // D
    [SerializeField]
    private float profileDragCoefficient = 0.0072f;
    [SerializeField]
    private float shapeFactor = 1.17f;
    [SerializeField]
    private float aspectRatio = 25f;

    // profile drag coefficient (drag from shape of body through air) 0.0072f
    // shape factor (induced/vortex drag, tips of wing) 1.17f
    // aspect ratio (wing span / width of wing) 25f

    [SerializeField]
    private float startingSpeed;
    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private Vector3 startingVelocity;

    private float horizontalInput;
    private float verticalInput;

    public float speed;

    // dubug
    public float liftMagnitude;
    public float dragMagnitude;


    //public float minSpeedReached = 99999;
    //public float maxSpeedReached;
    //public float height;
    //public float minHeightThisCycle;
    //public float maxHeightThisCycle;
    //private float lastHeight;
    //private float lastLastHeight;
    //public float loopRadius;

    private void Awake()
    {
        // debug
        //speed = startingSpeed;
        //minSpeedReached = speed;

        startingPosition = transform.position;
        startingRotation = transform.rotation;
        startingVelocity = startingRotation * Vector3.forward * startingSpeed;
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

        groundCamera.transform.SetPositionAndRotation(groundCamera.transform.position, Quaternion.LookRotation(transform.position - groundCamera.transform.position));
    }

    private void FixedUpdate()
    {
        Vector3 swing = rigidbody.velocity;
        Vector3 twist = transform.rotation * Quaternion.Euler(0, 0, horizontalInput * rollScaler * Time.fixedDeltaTime) * Vector3.up;
        transform.rotation = Quaternion.LookRotation(swing, twist);
        coefficientOfLift = Map(verticalInput, -1, 1, elevatorMax, elevatorMin);
        lift = transform.up * coefficientOfLift * (airDensity * Mathf.Pow(rigidbody.velocity.magnitude, 2) / 2) * wingArea;
        coefficientOfDrag = profileDragCoefficient + shapeFactor * Mathf.Pow(coefficientOfLift, 2) / (Mathf.PI * aspectRatio); 
        drag = -transform.forward * coefficientOfDrag * (airDensity * Mathf.Pow(rigidbody.velocity.magnitude, 2) / 2) * wingArea;
        rigidbody.AddForce(lift);
        rigidbody.AddForce(drag);


        speed = rigidbody.velocity.magnitude;
        liftMagnitude = lift.magnitude;
        dragMagnitude = drag.magnitude;

        //Vector3 tempVelocity = rigidbody.velocity;
        //Quaternion tempRotation = transform.rotation;
        //Vector3 averageLift = Vector3.zero;
        //int steps = 20;
        //for (int i = 0; i < steps; i++)
        //{
        //    tempRotation = Quaternion.LookRotation(tempVelocity, tempRotation * Quaternion.Euler(0, 0, horizontalInput * rollScaler) * Vector3.up);
        //    coefficientOfLift = tempRotation * Vector3.up * Map(verticalInput, -1, 1, elevatorMax, elevatorMin);
        //    lift = coefficientOfLift * (airDensity * Mathf.Pow(tempVelocity.magnitude, 2)) / 2 * wingArea;
        //    tempVelocity += lift * Time.fixedDeltaTime / steps;
        //    averageLift += lift;
        //}
        //averageLift /= steps;
        //transform.rotation = tempRotation;
        //rigidbody.AddForce(lift);

        // debug

        //height = transform.position.y;

        //if (speed < minSpeedReached)
        //{
        //    minSpeedReached = speed;
        //}
        //if (speed > maxSpeedReached)
        //{
        //    maxSpeedReached = speed;
        //}
        //if (height > lastHeight && lastHeight < lastLastHeight)
        //{
        //    minHeightThisCycle = transform.position.y;
        //    loopRadius = maxHeightThisCycle - minHeightThisCycle;
        //}
        //if (height < lastHeight && lastHeight > lastLastHeight)
        //{
        //    maxHeightThisCycle = transform.position.y;
        //}

        //if (Mathf.Abs(transform.rotation.x) < 0.01f)
        //{
        //    //  Debug.Break();
        //}

        //lastLastHeight = lastHeight;
        //lastHeight = height;
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(transform.position, transform.position + coefficientOfLift * 4);
    }

    private static float Map(float value, float istart, float istop, float ostart, float ostop)
    {
        return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
    }
}

