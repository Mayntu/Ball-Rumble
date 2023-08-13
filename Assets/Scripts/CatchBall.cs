using UnityEngine;

public class CatchBall : MonoBehaviour
{
    [SerializeField] private GameObject ballTrigger;
    [SerializeField] private Transform handsPosition;
    [SerializeField] private GameObject ball;
    [SerializeField] private float throwForce;
    [SerializeField] private float throwAngle;
    [SerializeField] private float defaultThrowAngle;
    [SerializeField] private float kickForce;
    [SerializeField] private float kickAngle;
    [SerializeField] private float defaultKickAngle;
    [SerializeField] private bool isRed;

    [SerializeField] private GameObject[] playerPositions;
    public bool isCatched = false;

    //[SyncVar(hook = nameof(OnIsCatchedChanged))] public bool isCatched = false;
	
	public GameObject activePlayer;
    private Animator animator;
    private Rigidbody ballRigidbody;
    private float previousRotationAngle;
    private void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
        animator= GetComponent<Animator>();
        ballRigidbody = ball.GetComponent<Rigidbody>();

        defaultThrowAngle = throwAngle;
        defaultKickAngle = kickAngle;
    }

    private void Update()
    {
        float currentRotationAngle = transform.eulerAngles.y;

        if(isRed)
        {
            if (currentRotationAngle >= 205f && currentRotationAngle <= 325f)
            {
                if (Input.GetKeyDown(KeyCode.E) && isCatched)
                {
                    Debug.Log("Вперёд бросать нельзя");
                    return;
                }
            }
        }
        else if(!isRed)
        {
            if (currentRotationAngle >= 35f && currentRotationAngle <= 155f)
            {
                if (Input.GetKeyDown(KeyCode.E) && isCatched)
                {
                    Debug.Log("Вперёд бросать нельзя");
                    return;
                }
            }
        }

        previousRotationAngle = currentRotationAngle;

        if (isCatched)
        {
            ball.transform.position = handsPosition.position;
            activePlayer = gameObject;
        }

        if (Input.GetKeyDown(KeyCode.E) && isCatched)
        {
            ThrowBall();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && isCatched && isRed)
        {
            GetPlayerPositions("RedPlayer");
            // Находим самую маленькую позицию по X среди игроков
            float minX = Mathf.Infinity;

            foreach (GameObject playerPos in playerPositions)
            {
                if (playerPos.transform.position.x < minX)
                {
                    minX = playerPos.transform.position.x;
                }
            }

            // Проверяем, является ли текущий игрок самым "впереди стоящим" по X
            if (transform.position.x <= minX)
            {
                // Разрешаем удар, так как игрок впереди от других
                KickBall();
            }
            else
            {
                // Запрещаем удар, так как перед ним есть игроки
                Debug.Log("Впереди другие игроки, ударять нельзя");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q) && isCatched && !isRed)
        {
            GetPlayerPositions("BluePlayer");
            // Находим самую маленькую позицию по X среди игроков
            float maxX = Mathf.NegativeInfinity;

            foreach (GameObject playerPos in playerPositions)
            {
                if (playerPos.transform.position.x > maxX)
                {
                    maxX = playerPos.transform.position.x;
                }
            }

            // Выполняем проверку позиции текущего игрока и разрешаем или запрещаем удар
            if (transform.position.x >= maxX)
            {
                // Разрешаем удар, так как игрок впереди от других
                KickBall();
            }
            else
            {
                // Запрещаем удар, так как перед ним есть игроки
                Debug.Log("Впереди другие игроки, ударять нельзя");
            }
        }
    }

    private void GetPlayerPositions(string playerTag)
    {
        playerPositions = GameObject.FindGameObjectsWithTag(playerTag);
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Ball"))
        {
            isCatched = true;
        }
    }

    private void ThrowBall()
    {
        isCatched = false;
        
        throwAngle += gameObject.GetComponent<PlayerMovement>().ThrowAngleRange();
        Debug.Log(throwAngle);
        // Применение фиксированной силы и угла броска к мячу
        Vector3 throwDirection = Quaternion.Euler(-throwAngle, transform.eulerAngles.y, 0f) * Vector3.forward;
        ballRigidbody.AddForce(throwDirection * throwForce);
        throwAngle = defaultThrowAngle;
    }
    private void KickBall()
    {
        isCatched = false;
        
        kickAngle += gameObject.GetComponent<PlayerMovement>().KickAngleRange();
        Debug.Log(kickAngle);
        // Применение фиксированной силы и угла броска к мячу
        Vector3 kickDirection = Quaternion.Euler(-kickAngle, transform.eulerAngles.y, 0f) * Vector3.forward;
        ballRigidbody.AddForce(kickDirection * kickForce);
        kickAngle = defaultKickAngle;
    }


    // private void OnIsCatchedChanged(bool oldValue, bool newValue)
    // {
    //     if (newValue)
    //     {
    //         RpcUpdateBallPosition(handsPosition.position);
    //     }
    //     else
    //     {
    //         RpcResetBallPosition();
    //     }
    // }

    //[ClientRpc]
    // private void RpcUpdateBallPosition(Vector3 position)
    // {
    //     // if (isLocalPlayer)
    //     // {
    //     //     ball.transform.position = position;
    //     // }
    //     ball.transform.position = position;
    // }

    //[ClientRpc]
    // private void RpcResetBallPosition()
    // {
    //     // if (isLocalPlayer)
    //     // {
    //     //     ball.transform.position = Vector3.zero;
    //     // }
    //     ball.transform.position = Vector3.zero;
    // }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "BluePlayer" && gameObject.tag == "RedPlayer" && isCatched)
        {
            animator.Play("Knocked");
            isCatched = false;
            ball.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 5f, 0f), ForceMode.Force);
            StartCoroutine(gameObject.GetComponent<PlayerMovement>().DoMoveFalse(2.5f));
        }
        if (col.gameObject.tag == "RedPlayer" && gameObject.tag == "BluePlayer" && isCatched)
        {
            animator.Play("Knocked");
            isCatched = false;
            ball.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 5f, 0f), ForceMode.Force);
            StartCoroutine(gameObject.GetComponent<PlayerMovement>().DoMoveFalse(2.5f));
        }
    }
    
}
