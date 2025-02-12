using UnityEngine;

[RequireComponent(typeof(SliderJoint2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class RaycastSliderMotorControllerCircleLerp : MonoBehaviour
{
    [Header("Distance Settings")]
    // The allowed radius (max distance) for the mouse cursor.
    public float maxDistance = 5f;

    [Header("Motor Settings")]
    // The absolute motor speed value (used as a target value).
    public float motorSpeed = 100f;
    // The maximum torque the motor can apply.
    public float motorTorque = 1000f;

    [Header("Damping Settings")]
    // Damping factor used for Lerp transitions.
    // A higher value makes the transition faster.
    public float damping = 5f;

    // Cached reference to the SliderJoint2D component.
    private SliderJoint2D sliderJoint;
    // Cached reference to the Rigidbody2D component.
    private Rigidbody2D rb;

    void Awake()
    {
        sliderJoint = GetComponent<SliderJoint2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 mouseWorldPos = GetMouseWorldPosition();
        DrawDebugRay(mouseWorldPos);
        ProcessMotor(mouseWorldPos);
    }

    // Converts the mouse position from screen space to world space.
    private Vector2 GetMouseWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(screenPos);
    }

    // Draws a debug ray from this object's position toward the mouse (up to maxDistance).
    private void DrawDebugRay(Vector2 mouseWorldPos)
    {
        Vector2 direction = (mouseWorldPos - (Vector2)transform.position).normalized;
        Debug.DrawRay(transform.position, direction * maxDistance, Color.green);
    }

    // Processes motor behavior based on the distance between this object and the mouse.
    // When the distance exceeds maxDistance, the motor is enabled and its speed is smoothly adjusted.
    // When inside, the motor and the rigidbody's velocity are damped to zero.
    private void ProcessMotor(Vector2 mouseWorldPos)
    {
        float distance = Vector2.Distance(transform.position, mouseWorldPos);

        if (distance > maxDistance)
        {
            EnableMotor(mouseWorldPos);
        }
        else
        {
            StopMotor();
        }
    }

    // Enables the motor and smoothly Lerp's its speed toward the target speed,
    // which is positive if the cursor is to the right or negative if to the left.
    private void EnableMotor(Vector2 mouseWorldPos)
    {
        // Determine target motor speed based on horizontal cursor position.
        float targetSpeed = (mouseWorldPos.x > transform.position.x) ? motorSpeed : -motorSpeed;

        JointMotor2D motor = sliderJoint.motor;
        // Smoothly transition the current motor speed toward the target speed.
        motor.motorSpeed = Mathf.Lerp(motor.motorSpeed, targetSpeed, damping * Time.deltaTime);
        motor.maxMotorTorque = motorTorque;
        sliderJoint.motor = motor;
        sliderJoint.useMotor = true;
    }

    // Smoothly Lerp's the motor speed to zero and the rigidbody's velocity to zero.
    // When the motor's speed is nearly zero, the motor is disabled.
    private void StopMotor()
    {
        JointMotor2D motor = sliderJoint.motor;
        motor.motorSpeed = Mathf.Lerp(motor.motorSpeed, 0f, damping * Time.deltaTime);
        sliderJoint.motor = motor;
        // If the motor speed is almost zero, disable the motor.
        if (Mathf.Abs(motor.motorSpeed) < 0.01f)
        {
            sliderJoint.useMotor = false;
        }
        
        // Smoothly damp the rigidbody's velocity to prevent sliding.
        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, damping * Time.deltaTime);
    }
}
