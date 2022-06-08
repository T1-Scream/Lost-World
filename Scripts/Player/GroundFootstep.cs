using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GroundType
{
    public string name;
    public AudioClip[] footStep;
}

public class GroundFootstep : MonoBehaviour
{
    public List<GroundType> groundList = new List<GroundType>();
    public string curGround;

    PlayerMovement Player;

    private void Start()
    {
        Player = GetComponent<PlayerMovement>();
        SetGround(groundList[0]);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Ground")
            SetGround(groundList[0]);
        else if (hit.transform.tag == "Untagged")
            SetGround(groundList[1]);
    }

    public void SetGround(GroundType ground)
    {
        if (curGround != ground.name) {
            Player.footStep = ground.footStep;
            curGround = ground.name;
        }
    }
}


