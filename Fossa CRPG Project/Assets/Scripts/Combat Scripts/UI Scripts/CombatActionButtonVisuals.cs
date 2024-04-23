using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CombatActionButtonVisuals : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Transform selectedParent;
    [SerializeField] private Transform defaultParent;

    [SerializeField] private GameObject selectedVisuals;
    [SerializeField] private GameObject defaultVisuals;

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.SetParent(selectedParent);
        selectedVisuals.SetActive(true);
        defaultVisuals.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.SetParent(defaultParent);
        selectedVisuals.SetActive(false);
        defaultVisuals.SetActive(true);
    }
}
