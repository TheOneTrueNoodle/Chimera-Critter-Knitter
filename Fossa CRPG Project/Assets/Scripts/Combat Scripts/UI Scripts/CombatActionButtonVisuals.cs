using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CombatActionButtonVisuals : MonoBehaviour, IPointerExitHandler
{
    [SerializeField] private Transform selectedParent;
    [SerializeField] private Transform defaultParent;

    [SerializeField] private GameObject selectedVisuals;
    [SerializeField] private GameObject defaultVisuals;

    [SerializeField] private GameObject disabledVisuals;

    [field: SerializeField] private FMODUnity.EventReference hoverSFX;

    private bool disabled;
    private bool selected;

    public void enableButton()
    {
        disabled = false;
        disabledVisuals.SetActive(false);
        shrink();
    }

    public void disableButton()
    {
        disabled = true;
        disabledVisuals.SetActive(true);
        shrink();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (disabled) { return; }
        AudioManager.instance.PlayOneShot(hoverSFX, transform.position);
        selected = true;
        expand();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selected = false;
        shrink();
    }

    private void expand()
    {
        gameObject.transform.SetParent(selectedParent);
        selectedVisuals.SetActive(true);
        defaultVisuals.SetActive(false);
    }
    private void shrink()
    {
        gameObject.transform.SetParent(defaultParent);
        selectedVisuals.SetActive(false);
        defaultVisuals.SetActive(true);
    }
}
