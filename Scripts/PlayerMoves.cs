using UnityEngine;

public class PlayerMoves : MonoBehaviour
{
    private Vector3 direction;
    public float forwardSpeed;
    private Rigidbody player;
    //Movement left/right
    private int desiredLane = 1;
    public float laneDistance = 15;
    //Distance Up/Down
    public int distancePlayerSpots = 1;
    public float distanceRange = 15;
    private float distanceTime = 0;
    private bool playerGoBack = false;
    //jump 
    public float gravityModifier;
    public float jumpForce;
    public bool isOnTheGround = true;
    //Fire
    public GameObject projectilePrefab;
    private bool hasTakenMolotov = false;
    private float fireTimer = 0;
    //Sound
    public AudioClip collisionSound; // sound sfx

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
    }

    void Update()
    {
        direction.z = forwardSpeed;

        if (Input.GetKeyDown(KeyCode.D))
        {
            desiredLane = Mathf.Min(desiredLane + 1, 2);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            desiredLane = Mathf.Max(desiredLane - 1, 0);
        }

        float targetPositionX = 0f;
        switch (desiredLane)
        {
            case 0: targetPositionX = -laneDistance; break;
            case 1: targetPositionX = 0; break;
            case 2: targetPositionX = laneDistance; break;
        }
        ///////////////////////////////// hit on a object make skiiMan go 1 spot back  ////////////////////////////////  

        float targetPositionZ = 100f;

        if (distanceTime > 10)
        {
            distancePlayerSpots = Mathf.Min(distancePlayerSpots + 1, 2);
            distanceTime = 0f;
        }
        if (playerGoBack)
        {
            playerGoBack = false;
            distancePlayerSpots = Mathf.Max(distancePlayerSpots - 1, 0);
            distanceTime = 0f;
        }

        switch (distancePlayerSpots)
        {
            case 0: targetPositionZ -= distanceRange; break;
            case 1: targetPositionZ = 100; break;
            case 2: targetPositionZ += distanceRange; break;
        }

        //////////////////////////////////////////////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.Space) && isOnTheGround)
        {
            player.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnTheGround = false;
        }
        if (Input.GetKeyDown(KeyCode.E) && hasTakenMolotov)
        {
            if (fireTimer < 5)
                Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
            hasTakenMolotov = false;
            fireTimer++;
        }

        Vector3 targetPosition = new Vector3(targetPositionX, transform.position.y, targetPositionZ);

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);

        distanceTime += Time.deltaTime;

        stayOnTerrain();

        //Debug.Log("Distance Time: " + distancePlayerSpots);
    }

    private void OnCollisionEnter(Collision collision)
    {
        isOnTheGround = true;

        if (collision.gameObject.CompareTag("Obstacles"))
        {
            playerGoBack = true;
            if (collisionSound != null)                 // Play the SFX
            {
                Vector3 collisionPoint = collision.contacts.Length > 0 ? collision.contacts[0].point : transform.position;
                float volume = 1.5f;
                AudioSource.PlayClipAtPoint(collisionSound, collisionPoint, volume);
            }
        }
        if (collision.gameObject.CompareTag("Molotov"))
        {
            hasTakenMolotov = true;
            Destroy(collision.gameObject);
            fireTimer = 0;
        }
    }
    void stayOnTerrain()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
        {
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Terrain"))
            {
                float targetHeight = hit.point.y + 10;
                Vector3 targetPosition = new Vector3(transform.position.x, targetHeight, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10);
            }
        }
    }

    public void CollectMolotov()
    {
        hasTakenMolotov = true;
    }

    public bool HasTakenMolotov()
    {
        return hasTakenMolotov;
    }

    public void SetHasTakenMolotov(bool status)
    {
        hasTakenMolotov = status;
    }

    public void SetPlayerSpot(int spot)         // Public method to set the position from another script
    {
        spot = distancePlayerSpots;
    }
}

