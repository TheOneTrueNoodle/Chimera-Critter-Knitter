using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CombatActionButtonVisuals : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Transform selectedParent;
    [SerializeField] private Transform defaultParent;

    public void OnSelect(BaseEventData eventData)
    {
        gameObject.transform.parent = selectedParent;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        gameObject.transform.parent = defaultParent;
    }
}
