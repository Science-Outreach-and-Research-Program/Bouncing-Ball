using UnityEngine;
using UnityEngine.UI;

public class BallManager : MonoBehaviour
{
    public float ballHeight;

    private bool hasBounceStarted;
    private bool hasStartedRecordHeight;
    private float bounceHeight;
    [SerializeField] private Text bounceHeightText;
    private Rigidbody gameObjectRigidBody;

    void Start()
    {
        ballHeight = gameObject.transform.position.y;
        gameObjectRigidBody = gameObject.GetComponent<Rigidbody>();
        Init();
    }
    
    void Update()
    {
        if (hasBounceStarted && !hasStartedRecordHeight)
            if (gameObject.transform.position.y < 0.7)
                hasStartedRecordHeight = true;

        if (hasStartedRecordHeight)
        {
            // update bounce height
            if (gameObject.transform.position.y / InputFieldHandler.inputOffsetTimes > bounceHeight)
            {
                bounceHeight = gameObject.transform.position.y / InputFieldHandler.inputOffsetTimes;
                bounceHeightText.text = string.Format("Bounce Height:  {0:N2} m", bounceHeight);
            }

            // add to chart when stopped bouncing
            if (gameObjectRigidBody.velocity.y == 0)
            {
                GameObject.Find("ScatterChart").GetComponent<ChartManager>()
                    .AddData(ballHeight / InputFieldHandler.inputOffsetTimes, bounceHeight)
                    .RefreshAllComponent();
                hasBounceStarted = false;
                hasStartedRecordHeight = false;
            }
        }
    }
    
    public void Init()
    {
        hasBounceStarted = false;
        hasStartedRecordHeight = false;
        Rigidbody ballPhysics = gameObject.GetComponent<Rigidbody>();
        ballPhysics.velocity = new Vector3(0, 0, 0);
        ballPhysics.useGravity = false;
        
        // set ball and camera position
        gameObject.transform.position = new Vector3(0, ballHeight, 0);
        float cameraDistance = Mathf.Min(-2.2f, -Mathf.Pow(ballHeight, 1.2f));
        float cameraHeight = 1.2f * Mathf.Pow(1.1f, ballHeight);
        GameObject.Find("Main Camera").transform.position = new Vector3(0.2f, cameraHeight, cameraDistance);

        bounceHeightText.text = "Bounce Height:  N/A";
        bounceHeight = 0.0f;
    }

    public void StartBounce()
    {
        // reset ball
        Init();
        
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        hasBounceStarted = true;
    }
}
