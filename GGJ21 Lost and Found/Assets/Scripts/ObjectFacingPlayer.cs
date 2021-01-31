using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFacingPlayer : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        // Make the asset to always face the player - only rotate on y axis
        var lookPos = player.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 1f);
    }
}
