using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FireMove : MonoBehaviour
{
    public float speed = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMoves>().CollectMolotov();
            Destroy(gameObject);
        }
    }
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}