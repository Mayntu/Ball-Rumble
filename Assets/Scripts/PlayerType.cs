using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerType : MonoBehaviour
{
    public bool isRed, isBlue, isLightBlue, isYellow, isPurple, isBlack, isWhite, isPink;

    public Animator animator;
    public Avatar redAvatar, blueAvatar, lightBlueAvatar, yellowAvatar, purpleAvatar, blackAvatar, whiteAvatar, pinkAvatar;
    public GameObject redSkin, blueSkin, lightBlueSkin, yellowSkin, purpleSkin, blackSkin, whiteSkin, pinkSkin;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (isRed)
        {
            animator.avatar = redAvatar;
            redSkin.SetActive(true);
        }
        else if (isBlue)
        {
            animator.avatar = blueAvatar;
            blueSkin.SetActive(true);
        }
        else if (isLightBlue)
        {
            animator.avatar = lightBlueAvatar;
            lightBlueSkin.SetActive(true);
        }
        else if (isYellow)
        {
            animator.avatar = yellowAvatar;
            yellowSkin.SetActive(true);
        }
        else if (isPurple)
        {
            animator.avatar = purpleAvatar;
            purpleSkin.SetActive(true);
        }
        else if (isBlack)
        {
            animator.avatar = blackAvatar;
            blackSkin.SetActive(true);
        }
        else if (isWhite)
        {
            animator.avatar = whiteAvatar;
            whiteSkin.SetActive(true);
        }
        else if (isPink)
        {
            animator.avatar = pinkAvatar;
            pinkSkin.SetActive(true);
        }
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
        isPink = false;
    }
}
