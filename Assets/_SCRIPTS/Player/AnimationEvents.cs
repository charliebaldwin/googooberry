using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public PlayerController player;
    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    /// <summary>
    /// slow the player to a stop for more impact on the third hit
    /// </summary>
    public void ThirdHit()
    {
        player.ThirdHitHelper(0, false);
    }

    /// <summary>
    /// resume the player's movement speed
    /// </summary>
    public void ThirdHitPost()
    {
        player.ThirdHitHelper(0, true);
    }
}