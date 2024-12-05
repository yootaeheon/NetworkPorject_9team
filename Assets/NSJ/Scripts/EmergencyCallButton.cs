using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EmergencyCallButton : MonoBehaviour, 
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerEnterHandler, 
    IPointerExitHandler
{
    public bool OnButton;

    public event UnityAction OnClickDown;
    public event UnityAction OnClickUp;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnClickDown?.Invoke();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        OnClickUp?.Invoke();    
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnButton = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnButton = false;
    }


}
