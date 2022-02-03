using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField]
    Transform player;

    [SerializeField]
    float accScaling = 1f;

    [SerializeField]
    float rotateSpeed = 10f;

    [SerializeField]
    float tiltSpeed = 10f;

    [SerializeField]
    float maxVel = 10f;

    Vector3 acceleration = new Vector3(0f,0f,0f);
    Vector2 angleVector = new Vector2(0f, 0f);
    Vector2 lastNonZeroVelocity = new Vector2(0f, 0f);
    Vector3 velocity;

    Rigidbody body;

    GameObject leftArm;
    GameObject rightArm;

    float angle;
    float tiltAngle;

    movableObject PickedUpObject = null;

    GameObject gameState;   

    void Awake()
    {
        body = player.GetComponent<Rigidbody>();
        gameState = GameObject.Find("Game State");

        leftArm = player.transform.Find("arm0").gameObject;
        rightArm = player.transform.Find("arm1").gameObject;

        leftArm.GetComponentInChildren<LineRenderer>().SetPosition(0, player.transform.GetChild(1).localPosition);
        rightArm.GetComponentInChildren<LineRenderer>().SetPosition(0, player.transform.GetChild(2).localPosition);

        leftArm.GetComponentInChildren<LineRenderer>().SetPosition(1, leftArm.transform.Find("Sphere").transform.localPosition);
        rightArm.GetComponentInChildren<LineRenderer>().SetPosition(1, rightArm.transform.Find("Sphere").transform.localPosition);
    }

    // Update is called once per frame
    void Update()
    {
        acceleration.x = Input.GetAxis("Horizontal");
        acceleration.y = Input.GetAxis("Vertical");

        // Normalize the input vector to make controls more uniform and scale accordingly.
        acceleration.Normalize();
        

        if (Mathf.Abs(acceleration.x) > 0.1f || Mathf.Abs(acceleration.y) > 0.1f)
        {
            angle = -1 * AngleFromUnitCirclePosition(acceleration.x, acceleration.y);

            tiltAngle = 10f;
        }
        else
        {
            tiltAngle = 0f;
        }

        acceleration *= accScaling;

        // Pickup or drop the closest item (if within the drop radius).
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
 
            if (PickedUpObject == null)
            {
                PickUpObject(2.5f);
                UpdateArms();
            }
            else
            {
                DropObject();
                UpdateArms();
            }
        }

        // Throw the item if picked-up.
        if (Input.GetKeyDown("z"))
        {
            if (PickedUpObject != null)
            {
                DropObject(10f);
                UpdateArms();
            }
        }

        if (Input.GetKeyDown("x"))
        {
            if (PickedUpObject != null)
            {
                FillMachine(2.5f);
                UpdateArms();
            }
            else
            {
                EmptyMachine(2.5f);
                UpdateArms();
            }
        }

    }

    void FixedUpdate()
    {
        //Get the current velocity from the rigidbody.
        velocity = body.velocity;

        //Get and clamp the new velocity.
        velocity.x += Time.deltaTime * acceleration.x;
        velocity.z += Time.deltaTime * acceleration.y;

        //velocity.x = Mathf.Min(maxVel, velocity.x);
        //velocity.z = Mathf.Min(maxVel, velocity.z);

        velocity = Vector3.ClampMagnitude(velocity, maxVel);

        //Update the velocity of the solidbody.
        body.velocity = velocity;

        Vector3 currentAngle = player.transform.localEulerAngles;

        currentAngle.y = Mathf.MoveTowardsAngle(currentAngle.y, angle, rotateSpeed * Time.deltaTime);
        currentAngle.z = tiltAngle != 0 ? Mathf.MoveTowardsAngle(currentAngle.z, tiltAngle, tiltSpeed * Time.deltaTime) : 0;
        currentAngle.x = 0;

        player.transform.localEulerAngles = currentAngle;

        // If item is picked up co-rotate the item with the player.
        if (PickedUpObject != null)
        {
            PickedUpObject.gameObject.transform.position = player.position;
            PickedUpObject.gameObject.transform.localEulerAngles = new Vector3(0f, currentAngle.y, 0f);
        }
    }

    void DropObject(float speed = 0f)
    {
        PickedUpObject.gameObject.GetComponent<Rigidbody>().freezeRotation = false;
        PickedUpObject.gameObject.GetComponent<Collider>().enabled = true;
        PickedUpObject.gameObject.GetComponent<Rigidbody>().useGravity = true;

        PickedUpObject.gameObject.GetComponent<Rigidbody>().velocity = player.GetComponent<Rigidbody>().velocity + (PickedUpObject.gameObject.transform.right + new Vector3(0f, .2f, 0f)) * speed;

        PickedUpObject.gameObject.transform.position = PickedUpObject.gameObject.transform.GetChild(0).position;
        PickedUpObject.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);

        PickedUpObject.gameObject.transform.GetChild(0).localPosition = new Vector3(0f, 0f, 0f);
        PickedUpObject.gameObject.transform.GetChild(1).localPosition = new Vector3(0f, 0f, 0f);
        PickedUpObject.gameObject.transform.GetChild(2).localPosition = new Vector3(0f, 0f, 0f);


        PickedUpObject.gameObject.transform.GetChild(1).gameObject.SetActive(false);


        PickedUpObject = null;
    }

    void PickUpObject(float pickupRadius)
    {
        var nearestObject = gameState.GetComponent<ObjectHandler>().NearestObjectWithinGrabRadius(pickupRadius, player.transform.position);

        
        if (nearestObject != null)
        {
            PickedUpObject = nearestObject;

            PickedUpObject.gameObject.GetComponent<Rigidbody>().freezeRotation = true;
            PickedUpObject.gameObject.GetComponent<Collider>().enabled = false;
            PickedUpObject.gameObject.GetComponent<Rigidbody>().useGravity = false;

            PickedUpObject.gameObject.transform.GetChild(0).localPosition = new Vector3(1.5f, 0.5f, 0f);
            PickedUpObject.gameObject.transform.GetChild(1).localPosition = new Vector3(1.5f, 0.5f, 0f);
            PickedUpObject.gameObject.transform.GetChild(2).localPosition = new Vector3(1.5f, 0.5f, 0f);


            PickedUpObject.gameObject.transform.GetChild(0).localEulerAngles = new Vector3(0f, 0f, 0f);
        }
    }

    void UpdateArms()
    {
        if (PickedUpObject != null)
        {
            leftArm.GetComponentInChildren<LineRenderer>().SetPosition(1, PickedUpObject.gameObject.transform.GetChild(1).localPosition + Vector3.Scale(PickedUpObject.gameObject.transform.GetChild(1).localScale, PickedUpObject.gameObject.transform.GetChild(1).GetChild(0).localPosition));
            rightArm.GetComponentInChildren<LineRenderer>().SetPosition(1, PickedUpObject.gameObject.transform.GetChild(1).localPosition + Vector3.Scale(PickedUpObject.gameObject.transform.GetChild(1).localScale, PickedUpObject.gameObject.transform.GetChild(1).GetChild(1).localPosition));

            leftArm.transform.GetChild(1).gameObject.SetActive(false);
            rightArm.transform.GetChild(1).gameObject.SetActive(false);

            PickedUpObject.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            leftArm.GetComponentInChildren<LineRenderer>().SetPosition(1, leftArm.transform.Find("Sphere").transform.localPosition);
            rightArm.GetComponentInChildren<LineRenderer>().SetPosition(1, rightArm.transform.Find("Sphere").transform.localPosition);

            leftArm.transform.GetChild(1).gameObject.SetActive(true);
            rightArm.transform.GetChild(1).gameObject.SetActive(true);
        }

    }

    void FillMachine(float interactionRadius)
    {

        var nearestMachines = gameState.GetComponent<MachineHandler>().MachinesWithinGrabRadius(interactionRadius, player.transform.position);

        
        if (nearestMachines != null)
        {
            foreach (Machine x in nearestMachines)
            {
                if (PickedUpObject.interactionType == x.interactionType)
                {
                    PickedUpObject.gameObject.GetComponent<Rigidbody>().freezeRotation = true;
                    PickedUpObject.gameObject.GetComponent<Rigidbody>().useGravity = false;
                    PickedUpObject.gameObject.GetComponent<Collider>().enabled = false;
                    PickedUpObject.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);

                    x.PlaceObject(PickedUpObject);

                    PickedUpObject = null;

                    break;
                }    

     
            }
        }
    }

    void EmptyMachine(float interactionRadius)
    {
        var nearestMachine = gameState.GetComponent<MachineHandler>().MachinesWithinGrabRadius(interactionRadius, player.transform.position);

        if (nearestMachine != null)
        {

        }
    }

    float AngleFromUnitCirclePosition(float x, float y)
    {
        if (x > 0f && y > 0f)
            return Mathf.Asin(y) * 180f/Mathf.PI;

        if (x > 0f && y < 0f)
            return 360f - Mathf.Asin(-y) * 180f/Mathf.PI;

        if (x < 0f && y > 0f)
            return 180f - Mathf.Asin(y) * 180f/Mathf.PI;

        if (x < 0f && y < 0f)
            return Mathf.Asin(-y) * 180f/Mathf.PI + 180f;

        if (y == 0f && x > 0f)
            return 0f;

        if (y == 0f && x < 0f)
            return 180f;

        if (y > 0f && x == 0f)
            return 90f;

        if (y < 0f && x == 0f)
            return 270f;

        return 0f;
    }


}
