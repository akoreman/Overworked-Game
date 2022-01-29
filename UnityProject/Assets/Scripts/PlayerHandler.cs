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
    float jumpVel = 5f;

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

    //bool wantJump = false;

    Rigidbody body;

    GameObject leftArm;
    GameObject rightArm;

    float Angle;
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
        acceleration *= accScaling;

        /*
        //Rotate player model to face correct direction.
        Vector3 currentAngle = player.transform.localEulerAngles;

        currentAngle.y = Mathf.MoveTowardsAngle(currentAngle.y, Angle, rotateSpeed * Time.deltaTime);
        currentAngle.z = Mathf.MoveTowardsAngle(currentAngle.z, tiltAngle, tiltSpeed * Time.deltaTime);
        currentAngle.x = 0;

        player.transform.localEulerAngles = currentAngle;

        // If item is picked up co-rotate the item with the player.
        if (PickedUpObject != null)
        {
            PickedUpObject.objectTransform.position = player.position;
            PickedUpObject.objectTransform.localEulerAngles = new Vector3(0f,currentAngle.y,0f);
        }
        */
        // Set that the player wants to jump, jump itself is handled in FixedUpdate(). 
        /*
        if (Input.GetButtonDown("Jump"))
        {
            wantJump = true;
        }
        */

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

        velocity.x = Mathf.Min(maxVel, velocity.x);
        velocity.z = Mathf.Min(maxVel, velocity.z);

        //Perform the jump.
        /*
        if (wantJump)
        {
            velocity.y += jumpVel;
            wantJump = false;
        }
        */

        //Update the velocity of the solidbody.
        body.velocity = velocity;

        if (Mathf.Abs(body.velocity.x) > 0.1f || Mathf.Abs(body.velocity.z) > 0.1f)
        {
            lastNonZeroVelocity.x = velocity.x / Mathf.Sqrt(velocity.x * velocity.x + velocity.z * velocity.z);
            lastNonZeroVelocity.y = velocity.z / Mathf.Sqrt(velocity.x * velocity.x + velocity.z * velocity.z);

            Angle = -1 * AngleFromUnitCirclePosition(lastNonZeroVelocity.x, lastNonZeroVelocity.y);

            tiltAngle = 10f;
        }
        else
        {
            tiltAngle = 0f;
        }

        Vector3 currentAngle = player.transform.localEulerAngles;

        currentAngle.y = Mathf.MoveTowardsAngle(currentAngle.y, Angle, rotateSpeed * Time.deltaTime);
        currentAngle.z = Mathf.MoveTowardsAngle(currentAngle.z, tiltAngle, tiltSpeed * Time.deltaTime);
        currentAngle.x = 0;

        player.transform.localEulerAngles = currentAngle;

        // If item is picked up co-rotate the item with the player.
        if (PickedUpObject != null)
        {
            PickedUpObject.objectTransform.position = player.position;
            PickedUpObject.objectTransform.localEulerAngles = new Vector3(0f, currentAngle.y, 0f);
        }
    }

    void DropObject(float speed = 0f)
    {
        PickedUpObject.item.GetComponent<Rigidbody>().freezeRotation = false;
        PickedUpObject.item.GetComponent<Collider>().enabled = true;
        PickedUpObject.item.GetComponent<Rigidbody>().useGravity = true;

        PickedUpObject.item.GetComponent<Rigidbody>().velocity = player.GetComponent<Rigidbody>().velocity + (PickedUpObject.objectTransform.right + new Vector3(0f, .2f, 0f)) * speed;

        PickedUpObject.objectTransform.position = PickedUpObject.objectTransform.GetChild(0).position;
        PickedUpObject.objectTransform.localEulerAngles = new Vector3(0f, 0f, 0f);

        PickedUpObject.objectTransform.GetChild(0).localPosition = new Vector3(0f, 0f, 0f);
        PickedUpObject.objectTransform.GetChild(1).localPosition = new Vector3(0f, 0f, 0f);

        PickedUpObject.objectTransform.GetChild(1).gameObject.SetActive(false);

        PickedUpObject = null;
    }

    void PickUpObject(float pickupRadius)
    {
        var nearestObject = gameState.GetComponent<ObjectHandler>().NearestObjectWithinGrabRadius(pickupRadius, player.transform.position);

        if (nearestObject != null)
        {
            PickedUpObject = nearestObject;

            PickedUpObject.item.GetComponent<Rigidbody>().freezeRotation = true;
            PickedUpObject.item.GetComponent<Collider>().enabled = false;
            PickedUpObject.item.GetComponent<Rigidbody>().useGravity = false;

            PickedUpObject.objectTransform.GetChild(0).localPosition = new Vector3(1.5f, 0.5f, 0f);
            PickedUpObject.objectTransform.GetChild(1).localPosition = new Vector3(1.5f, 0.5f, 0f);

            PickedUpObject.objectTransform.GetChild(0).localEulerAngles = new Vector3(0f, 0f, 0f);
        }
    }

    void UpdateArms()
    {
        if (PickedUpObject != null)
        {
            leftArm.GetComponentInChildren<LineRenderer>().SetPosition(1, PickedUpObject.objectTransform.GetChild(1).localPosition + Vector3.Scale(PickedUpObject.objectTransform.GetChild(1).localScale, PickedUpObject.objectTransform.GetChild(1).GetChild(0).localPosition));
            rightArm.GetComponentInChildren<LineRenderer>().SetPosition(1, PickedUpObject.objectTransform.GetChild(1).localPosition + Vector3.Scale(PickedUpObject.objectTransform.GetChild(1).localScale, PickedUpObject.objectTransform.GetChild(1).GetChild(1).localPosition));

            leftArm.transform.GetChild(1).gameObject.SetActive(false);
            rightArm.transform.GetChild(1).gameObject.SetActive(false);

            PickedUpObject.objectTransform.GetChild(1).gameObject.SetActive(true);
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
        print("yay");

        var nearestMachine = gameState.GetComponent<MachineHandler>().MachinesWithinGrabRadius(interactionRadius, player.transform.position);

        
        if (nearestMachine != null)
        {
            foreach (Machine x in nearestMachine)
            {
                if (PickedUpObject.interactionType == x.interactionType)
                {
                    PickedUpObject.item.GetComponent<Rigidbody>().freezeRotation = true;
                    PickedUpObject.item.GetComponent<Rigidbody>().useGravity = false;
                    PickedUpObject.item.GetComponent<Collider>().enabled = false;
                    PickedUpObject.item.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);

                    x.PlaceObject(PickedUpObject);

                    print(PickedUpObject.objectTransform.position);

                    PickedUpObject = null;
                    print("yay2");

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
