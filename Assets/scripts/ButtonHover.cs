using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler{

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Button: game");
        AudioController.PlayButtonFillSound(1f);
    }
}
