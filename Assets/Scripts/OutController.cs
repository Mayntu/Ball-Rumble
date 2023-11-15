using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutController : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject ballTrail;

    private float outThrowForce;
    private bool isOut = false;
    private Vector3 OutEntry;

    void Start()
    {

    }

    private void Update()
    {
        if (isOut)
        {
            GameObject[] redPlayers = GameObject.FindGameObjectsWithTag("RedPlayer");
            GameObject[] bluePlayers = GameObject.FindGameObjectsWithTag("BluePlayer");
            foreach (GameObject redPlayer in redPlayers)
            {
                redPlayer.GetComponent<CatchBall>().DropBall();
            }
            foreach (GameObject bluePlayer in bluePlayers)
            {
                bluePlayer.GetComponent<CatchBall>().DropBall();
            }

            ballTrail.SetActive(true);
            if (ball.transform.position.z < -30)
            {
                OutEntry.z = -33;
                ball.transform.position = OutEntry;
                Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(0, 1f), Random.Range(0, 1f)).normalized;
                outThrowForce = Random.Range(8f, 13f);
                ball.GetComponent<Rigidbody>().AddForce(randomDirection * outThrowForce, ForceMode.Impulse);

                isOut = false;
                ball.GetComponent<Ball>().canCatch = true;
            }
            if (ball.transform.position.z > 30)
            {
                OutEntry.z = 33;
                ball.transform.position = OutEntry;
                Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(0, 1f), Random.Range(-1f, 0)).normalized;
                outThrowForce = Random.Range(8f, 13f);
                ball.GetComponent<Rigidbody>().AddForce(randomDirection * outThrowForce, ForceMode.Impulse);

                isOut = false;
                ball.GetComponent<Ball>().canCatch = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {

            OutEntry = other.gameObject.transform.position;
            OutEntry.y = 3;

            ball.GetComponent<Rigidbody>().isKinematic = true;

            ballTrail.SetActive(false);
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        ball.GetComponent<Rigidbody>().isKinematic = false;
        isOut = true;
        ball.GetComponent<Ball>().canCatch = false;
    }
}
