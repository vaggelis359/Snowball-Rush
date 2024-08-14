using UnityEngine;

public class SnowballRoll : MonoBehaviour
{
    private const float SnowballGrowthRate = 0.01f;
    private const float MaxSnowballSize = 60f;
    private const float MinSnowballSize = 1f;
    private const float MeltRateMultiplier = 3f;
    private const float HoverHeight = 5f;
    private const float RollingSpeed = 100f;
    private const float SnowballRotateSpeed = 30f;
    private const float DistanceRange = 15f;
    private const float LerpSpeed = 5f;
    private const float PositionLerpSpeed = 10f;

    private int snowballSpots = 0;
    private float distanceTime = 0f;
    private bool isMelting = false;
    private bool stateA = true;

    public GameObject skiiMan;
    public PlayerMoves playerSpot;

    private Rigidbody rb;

    public AudioClip collisionSound; // Add the audio file through the Inspector

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (Time.timeScale == 0) return;                                                                    // Check if the game is paused

        if (stateA)
        {
            RotateSnowball();                                                                               // rotate the snowball

            if (isMelting)
            {
                MeltSnowball();                                                                             //make snowball smaller when touch fire
            }
            else
            {
                GrowSnowball();                                                                             //make snowball bigger
            }

            float targetPositionZ = UpdateSnowballSpots();                                                  //Show us the Snowball Spot( 0,1,2)

            RollSnowball();                                                                                 //keep snowball on terrain

        }
        else
        {
            RollSnowballEnd();                                                                              //snowball goes forward
        }
        distanceTime += Time.deltaTime;
    }

    private void RotateSnowball()
    {
        transform.Rotate(SnowballRotateSpeed * Time.deltaTime, 0, 0);
    }

    private void GrowSnowball()
    {
        if (transform.localScale.x < MaxSnowballSize)
        {
            transform.localScale += Vector3.one * SnowballGrowthRate;
        }
    }

    private void MeltSnowball()
    {
        if (transform.localScale.x > MinSnowballSize)
        {
            transform.localScale -= Vector3.one * SnowballGrowthRate * MeltRateMultiplier;
            if (transform.localScale.x < 10f)
            {
                isMelting = false;
            }
        }
    }

    private void RollSnowball()
    {
        float targetPositionZ = UpdateSnowballSpots();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
        {
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Terrain"))
            {
                float targetHeight = hit.point.y + HoverHeight;
                Vector3 targetPosition = new Vector3(transform.position.x, targetHeight, targetPositionZ);
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * LerpSpeed);
            }
        }
    }
    private void RollSnowballEnd()
    {
        Vector3 direction = Vector3.forward * RollingSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position + direction;

        if (Physics.Raycast(newPosition + Vector3.up * 10f, Vector3.down, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
        {
            if (hit.collider.CompareTag("Terrain"))
            {
                newPosition.y = hit.point.y + HoverHeight;
            }
        }
        transform.position = newPosition;
        RotateSnowball();
    }

    private float UpdateSnowballSpots()
    {
        float targetPositionZ = -DistanceRange;

        if (transform.localScale.x >= MaxSnowballSize / 2)
        {
            snowballSpots = 1;
        }
        if (transform.localScale.x >= MaxSnowballSize - 2)
        {
            snowballSpots = 2;
        }

        if (isMelting)
        {
            snowballSpots = Mathf.Max(snowballSpots - 1, 0);
        }
        switch (snowballSpots)
        {
            case 0:
                targetPositionZ -= DistanceRange;
                break;
            case 1:
                targetPositionZ = DistanceRange;
                break;
            case 2:
                targetPositionZ += DistanceRange;
                break;
        }
        if (playerSpot.distancePlayerSpots == 0 & snowballSpots == 2)
        {
            stateA = false;
        }
        Debug.Log("Distance Time: " + snowballSpots);
        return targetPositionZ;

    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Terrain"))
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Fire"))
        {
            isMelting = true;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayCollisionSound(collision);
        }
    }

    private void PlayCollisionSound(Collision collision)
    {
        if (collisionSound != null)
        {
            Vector3 collisionPoint = collision.contacts.Length > 0 ? collision.contacts[0].point : transform.position;
            float volume = 2f;
            AudioSource.PlayClipAtPoint(collisionSound, collisionPoint, volume);
        }
    }
}
