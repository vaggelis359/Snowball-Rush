using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float delayTimer = 10f;
    private bool hasStarted = true;

    public Transform skiiMan;               // camera follow skiiMan
    private float speed = 11f;
    private float rotationSpeed = 12.2f;
    private float desiredYRotation = 0f;
    public Vector3 offset = new Vector3(0, 0, -10);

    private bool hasReachedTarget = true;

    private void Update()
    {
        if (skiiMan == null)
        {
            return;
        }
        if (hasReachedTarget)
            if (hasStarted)
            {
                delayTimer -= Time.deltaTime;
                if (delayTimer <= 0f)
                {
                    hasStarted = false;
                }
            }
            else
            {
                Quaternion desiredRotation = Quaternion.Euler(0, desiredYRotation, 0);                                                  // rotate camera axis Y

                transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);     // moving and rotate 
                transform.position = Vector3.MoveTowards(transform.position, skiiMan.position + offset, speed * Time.deltaTime);
            }
    }
}
