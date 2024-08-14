using UnityEngine;

public class sceneMoving : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.Translate(Vector3.back * Time.deltaTime * speed);
    }
}
