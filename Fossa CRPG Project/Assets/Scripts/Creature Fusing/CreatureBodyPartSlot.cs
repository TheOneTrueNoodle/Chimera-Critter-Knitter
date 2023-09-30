using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;

public class CreatureBodyPartSlot : MonoBehaviour
{
    public List<BodyPart> acceptedPartTypes;
    public CreatureBodyPart activePart;
    [HideInInspector] public CreatureBodyPart oldPart;

    private SkinnedMeshRenderer[] meshRenderers;

    private void Start()
    {
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    public void SelectNewPart(CreatureBodyPart newPart)
    {
        if(acceptedPartTypes.Contains(newPart.partType))
        {
            oldPart = activePart;
            if (activePart == null) { activePart = new CreatureBodyPart(); }
            activePart = newPart;

            for(int i = 0; i < activePart.mesh.Length && i < meshRenderers.Length; i++)
            {
                meshRenderers[i].sharedMesh = activePart.mesh[i];
            }

            foreach(SkinnedMeshRenderer renderer in meshRenderers)
            {
                renderer.materials = activePart.materials;
            }
        }
    }
}
