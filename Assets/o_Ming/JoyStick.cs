using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using o_Ming;
using UnityEngine.UI;

public class JoyStick : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
{
    public Image joystickBgImage;
    public Image joystickImage;

    void Awake()
    {
        Ming.SetImage(joystickBgImage, joystickImage);
    }

    public void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }
    public void OnDrag(PointerEventData ped)
    {
        Ming.JoyStick(ped);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Ming.ResetJoyImage();
    }
}
