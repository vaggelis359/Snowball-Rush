using UnityEngine;

public class TerrainMove : MonoBehaviour
{
    public float speed = 1;

    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * speed);
    }
}
