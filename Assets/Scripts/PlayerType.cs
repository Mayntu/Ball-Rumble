using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerType : MonoBehaviour
{
    public bool isRed, isBlue, isLightBlue, isYellow, isPurple, isBlack, isWhite;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    
    public void ResetColor()
    {
        isRed = false;
        isBlue = false;
        isLightBlue = false;
        isYellow = false;
        isPurple = false;
        isBlack = false;
        isWhite = false;
    }
}
