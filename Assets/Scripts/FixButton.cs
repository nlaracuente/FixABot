using UnityEngine;
using UnityEngine.EventSystems;

public class FixButton : MonoBehaviour, IPointerClickHandler
{
    public delegate void OnFixButtonClicked();
    OnFixButtonClicked onFixButtonClicked;

    public void RegisterEvent(OnFixButtonClicked _event)
    {
        onFixButtonClicked += _event;
    }

    public void UnregisterEvent(OnFixButtonClicked _event)
    {
        onFixButtonClicked -= _event;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onFixButtonClicked?.Invoke();
    }
}
