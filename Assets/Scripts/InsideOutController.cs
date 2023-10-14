using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideOutController : MonoBehaviour
{

    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject ballTrail;

    [SerializeField] private Transform topTeleport;
    [SerializeField] private Transform bottomTeleport;

    private GameObject player;

    private float outThrowForce;
    private bool isOut = false;
    private Vector3 OutEntry;

    void Update()
    {
        if (isOut)
        {
            Vector3 DirectionToCenter = (new Vector3(0, 40, 0) - ball.transform.position).normalized;
            outThrowForce = Random.Range(20f, 25f);
            ball.GetComponent<Rigidbody>().AddForce(DirectionToCenter * outThrowForce, ForceMode.Impulse);

            isOut = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("LeftInsideOut"))
        {
            if (other.CompareTag("BluePlayer") && other.GetComponent<CatchBall>().isCatched)
            {
                player = other.gameObject;

                OutEntry = ball.gameObject.transform.position;
                OutEntry.y = 2;

                StartCoroutine(Wait());
            }
        }
        else if (gameObject.CompareTag("RightInsideOut"))
        {
            if (other.CompareTag("RedPlayer") && other.GetComponent<CatchBall>().isCatched)
            {
                player = other.gameObject;

                OutEntry = ball.gameObject.transform.position;
                OutEntry.y = 2;

                StartCoroutine(Wait());
            }
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);


        player.GetComponent<CatchBall>().DropBall();
        ball.GetComponent<Rigidbody>().isKinematic = true;
        ballTrail.SetActive(false);

        if (OutEntry.z >= 0)
        {
            ball.transform.position = topTeleport.position;
        }
        else if (OutEntry.z < 0)
        {
            ball.transform.position = bottomTeleport.position;
        }

        yield return new WaitForSeconds(1f);

        ball.GetComponent<Rigidbody>().isKinematic = false;
        ballTrail.SetActive(true);
        isOut = true;
    }
}
