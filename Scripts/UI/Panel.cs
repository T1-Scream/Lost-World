using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Panel : MonoBehaviour
{
    public Animator mainMenu;

    private int Parameter_ID;
    private Animator OpenAni;
    private GameObject selectedObject;

    public void OnEnable()
    {
        Parameter_ID = Animator.StringToHash("Open");

        if (mainMenu == null) return;

        OpenPanel(mainMenu);
    }

    public void OpenPanel(Animator anim)
    {
        if (OpenAni == anim) return;

        anim.gameObject.SetActive(true);
        var newSelected = EventSystem.current.currentSelectedGameObject;

        CloseCurPanel();

        selectedObject = newSelected;
        OpenAni = anim;
        OpenAni.SetBool(Parameter_ID, true);
        EventSystem.current.SetSelectedGameObject(anim.gameObject);
    }

    public void CloseCurPanel()
    {
        if (OpenAni == null) return;

        OpenAni.SetBool(Parameter_ID, false);
        EventSystem.current.SetSelectedGameObject(selectedObject);
        StartCoroutine(DisableDelayed(OpenAni));
        OpenAni = null;
    }

    IEnumerator DisableDelayed(Animator anim)
    {
        bool closedState = false;
        bool close = true;
        while(!closedState && close)
        {
            if (!anim.IsInTransition(0))
                closedState = anim.GetCurrentAnimatorStateInfo(0).IsName("Closed");

            close = !anim.GetBool(Parameter_ID);

            if (close)
                anim.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.1f);
        }
    }
}
