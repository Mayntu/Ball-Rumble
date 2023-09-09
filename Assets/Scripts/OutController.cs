using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutController : MonoBehaviour
{
    [SerializeField] GameObject ball;

    private float outThrowForce;
    private bool isOut = false;
    private Vector3 OutEntry;

    void Start()
    {

    }

    void Update()
    {
        if (isOut)
        {
            ball.SetActive(true);
            if (ball.transform.position.z < -30)
            {
                OutEntry.z = -33;
                ball.transform.position = OutEntry;
                Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(0, 1f), Random.Range(0, 1f)).normalized;
                outThrowForce = Random.Range(8f, 13f);
                ball.GetComponent<Rigidbody>().AddForce(randomDirection * outThrowForce, ForceMode.Impulse);

                isOut = false;
            }
            if (ball.transform.position.z > 30)
            {
                OutEntry.z = 33;
                ball.transform.position = OutEntry;
                Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(0, 1f), Random.Range(-1f, 0)).normalized;
                outThrowForce = Random.Range(8f, 13f);
                ball.GetComponent<Rigidbody>().AddForce(randomDirection * outThrowForce, ForceMode.Impulse);

                isOut = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            OutEntry = other.gameObject.transform.position;
            OutEntry.y = 2;

            ball.GetComponent<Rigidbody>().isKinematic = true;

            ball.SetActive(false);
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        ball.GetComponent<Rigidbody>().isKinematic = false;
        isOut = true;
    }
}
