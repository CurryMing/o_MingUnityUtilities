using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class LongClickButton : MonoBehaviour,IPointerUpHandler, IPointerDownHandler
{
    private bool pointerDown;

    public UnityEvent onLongClick;

    private float num = 0f;
    public Text numText;

    private void Update()
    {
        if (pointerDown)
        {
            if (onLongClick != null)
            {
                onLongClick.Invoke();
            }
        }
        else
        {
        }
    }

    //UIMethod
    public void Plus()
    {
        num += Time.deltaTime;
        numText.text = num.ToString("f1");
    }

    public void OnPointerDown(PointerEventData ped)
    {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDown = false;
    }
}
