using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;

public class CreatureBodyPartSlot : MonoBehaviour
{
    public List<BodyPart> acceptedPartTypes;
    public CreatureBodyPart activePart;
    [HideInInspector] public CreatureBodyPart oldPart;

    private SkinnedMeshRenderer meshRenderer;

    public void SelectNewPart(CreatureBodyPart newPart)
    {
        if(acceptedPartTypes.Contains(newPart.partType))
        {
            oldPart = activePart;
            activePart = newPart;

            meshRenderer.sharedMesh = activePart.mesh;
            meshRenderer.materials = activePart.materials;
        }
    }
}
