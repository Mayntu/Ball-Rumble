using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool canCatch = true;
    public IEnumerator DoCanCatchFalse(float delay)
    {
        canCatch = false;
        Vector3 throwDirection = Quaternion.Euler(-90, transform.eulerAngles.y, 0f) * Vector3.forward;
        gameObject.GetComponent<Rigidbody>().AddForce(throwDirection * 500);
        yield return new WaitForSeconds(delay);
        canCatch = true;
    }
}
