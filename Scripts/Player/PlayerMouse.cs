using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouse : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform player;

    // recoil
    public AnimationCurve recoilCurve;
    public Vector2 recoilRange;
    public float fadeOutTime = 0.2f;

    float curTime;
    Vector2 curRecoil;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
         float x = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
         float y = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

         CalcRecoil();

         //Rotate mouse
         xRotation -= y;
         xRotation = Mathf.Clamp(xRotation, -90f, 90f);
         xRotation -= curRecoil.y;
         transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
         player.Rotate(Vector3.up * x);
    }


    // recoil
    private void CalcRecoil()
    {
        curTime += Time.deltaTime;
        float transition = curTime / fadeOutTime;
        float result = recoilCurve.Evaluate(transition);
        // smooth
        curRecoil = Vector2.Lerp(Vector2.zero, curRecoil, result);

    }

    public void FiringRecoil()
    {
        curRecoil += recoilRange;
        curTime = 0;
    }
}
