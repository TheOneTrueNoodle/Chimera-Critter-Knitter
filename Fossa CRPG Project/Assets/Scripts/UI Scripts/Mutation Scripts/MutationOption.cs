using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MutationOption : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    public AbilityData mutationAbility;
    
    [Header("Displays")]
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text nameDisp;
    [SerializeField] private Image equippedDisp;

    private bool selected;
    private bool equipped;
    private MutationSlot currentSlot;

    private List<MutationSlot> availableSlots = new List<MutationSlot>();

    private Vector3 dragOffset;
    private static float Speed = 10f;
    private static float LockOnRange = 0.5f;
    private Camera cam;
    public Transform contentParent;
    public Transform dragParent;

    private void Awake()
    {
        cam = Camera.main;
    }

    public void Setup(AbilityData ability)
    {
        mutationAbility = ability;
        image.sprite = ability.symbol;
        nameDisp.text = ability.Name;

        contentParent = transform.parent;
        dragParent = contentParent.parent;

        selected = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicked");
        if (currentSlot == null)
        {
            MutationSlot[] slots = FindObjectsOfType<MutationSlot>();
            foreach (MutationSlot slot in slots)
            {
                if (!slot.hasEquippedMutation)
                {
                    availableSlots.Add(slot);
                }
            }

            if (availableSlots.Count <= 0)
            {
                //Dont pick it up.
                selected = false;
            }
            else
            {
                selected = true;
                //Mouse just clicked this, so pick it up to move it.
                dragOffset = transform.position - GetMousePos();
                transform.SetParent(dragParent);
                availableSlots.Clear();
            }
        }
        else
        {
            currentSlot.UnequipMutation();
            Unequip();
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (!selected) { return; }

        transform.position = Vector3.MoveTowards(transform.position, GetMousePos() + dragOffset, Speed * Time.deltaTime);
        foreach(MutationSlot slot in availableSlots)
        {
            slot.hasMutationDisp.enabled = Vector3.Distance(slot.transform.position, transform.position) <= LockOnRange;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!selected) { return; }
        if (eventData.pointerId == 0)
        {
            foreach (MutationSlot slot in availableSlots)
            {
                if (Vector3.Distance(slot.transform.position, transform.position) <= LockOnRange)
                {
                    //Equip slot!
                    if (equipped)
                    {
                        //Swap to the new slot
                        if (slot.hasEquippedMutation)
                        {
                            //Swap the two mutations
                            currentSlot.UnequipMutation();
                            currentSlot.EquipMutation(slot.currentSelectedMutation);
                            slot.UnequipMutation();
                            slot.EquipMutation(this);
                            currentSlot = slot;
                        }
                        else
                        {
                            currentSlot.UnequipMutation();
                            currentSlot = slot;
                            currentSlot.EquipMutation(this);
                        }
                    }
                    else
                    {
                        //Normal Equip
                        if (slot.hasEquippedMutation)
                        {
                            slot.UnequipMutation();
                        }

                        currentSlot = slot;
                        slot.EquipMutation(this);
                        equippedDisp.enabled = true;
                    }
                }
                else
                {
                    selected = false;
                }
            }
            Debug.Log("Released Click");
            transform.SetParent(contentParent);
        }
    }

    public void Unequip()
    {
        currentSlot = null;
        equippedDisp.enabled = false;
    }

    Vector3 GetMousePos()
    {
        var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }
}
