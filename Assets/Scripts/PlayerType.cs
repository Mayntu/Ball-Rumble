using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerType : MonoBehaviour
{
    public bool isRed, isBlue, isLightBlue, isYellow, isPurple, isBlack, isWhite, isPink;

    public Animator animator;
    public Avatar blackAvatar, yellowAvatar;
    public GameObject blackSkin, yellowSkin;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (isBlack)
        {
            animator.avatar = blackAvatar;
            blackSkin.SetActive(true);
        }
        else if (isYellow)
        {
            animator.avatar = yellowAvatar;
            yellowSkin.SetActive(true);
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
