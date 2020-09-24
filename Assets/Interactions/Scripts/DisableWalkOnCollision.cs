﻿using UnityEngine;

/// <summary>
/// Class used for disable a Walk script when a hand collide with a collider
/// </summary>
public class DisableWalkOnCollision : MonoBehaviour
{
    /// <summary>
    /// The XR rig walk instance
    /// </summary>
    public Walk walk;
    /// <summary>
    /// A ghost hand game object
    /// </summary>
    public GameObject ghostHand;

    /// <summary>
    /// Active the ghost hand and deactive the walk
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        walk.enabled = false;
        ghostHand.SetActive(true);
    }

    /// <summary>
    /// Deactive the ghost hand and active the walk
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision)
    {
        walk.enabled = true;
        ghostHand.SetActive(false);
    }
}