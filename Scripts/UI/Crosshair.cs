using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public PlayerMovement player;
    public WeaponManager wm;
    public RectTransform rect;
    public float size;
    public float maxSize;

    float curSize;

    private void Update()
    {
        bool moving = player.isWalking;
        if (moving || wm.isFiring)
            curSize = Mathf.Lerp(curSize, maxSize, Time.deltaTime * 10);
        else
            curSize = Mathf.Lerp(curSize, size, Time.deltaTime * 10);

        rect.sizeDelta = new Vector2(curSize, curSize);
    }
}
