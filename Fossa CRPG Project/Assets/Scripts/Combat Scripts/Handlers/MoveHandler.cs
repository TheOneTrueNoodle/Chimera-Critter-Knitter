using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHandler
{
    private static int unitSpeed = 3;
    private static int unitTurnSpeed = 320;
    static private float animSmoothingSpeed = 2f;
    private float animSpeed;
    private float animRotation;

    public IEnumerator MoveAlongPath(Entity entity, List<OverlayTile> path)
    {
        while (path.Count != 0)
        {
            var step = unitSpeed * Time.deltaTime;
            var zIndex = path[0].transform.position.z;
            Look(entity, path[0].transform.position);
            Move(entity);

            if (Vector2.Distance(entity.transform.position, path[0].transform.position) < 0.5f)
            {
                if (path.Count == 1) { CombatEvents.current.TilePositionEntity(entity, path[0]); }
                path.RemoveAt(0);
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (path.Count == 0)
        {
            if (entity.GetComponentInChildren<Animator>() != null) { Animate(entity.GetComponentInChildren<Animator>(), 0, 0); }
            CombatEvents.current.ActionComplete(); 
        }
    }
    private void Move(Entity entity)
    {
        Rigidbody rb = entity.gameObject.GetComponent<Rigidbody>();
        rb.MovePosition(entity.transform.position + entity.transform.forward * unitSpeed * Time.deltaTime);
    }

    private void Look(Entity entity, Vector3 target)
    {
        Rigidbody rb = entity.gameObject.GetComponent<Rigidbody>();

        var relativePos = target - entity.transform.position;
        var rot = Quaternion.LookRotation(relativePos, Vector3.up);

        rb.rotation = Quaternion.RotateTowards(entity.transform.rotation, rot, unitTurnSpeed * Time.deltaTime);

        float angle = Quaternion.Angle(entity.transform.rotation, rot);
        float rotationDirection = Mathf.Sign(Vector3.Dot(Vector3.up, Vector3.Cross(entity.transform.forward, target.normalized)));

        if (angle < 1.0f) { rotationDirection = 0f; }
        if (rotationDirection > 0f) { rotationDirection = 1f; }
        else if (rotationDirection < 0f) { rotationDirection = -1f; }
        if (entity.GetComponentInChildren<Animator>() != null) { Animate(entity.GetComponentInChildren<Animator>(), rotationDirection, angle); }
    }

    private void Animate(Animator anim, float rotationDirection, float angle)
    {
        var targetSpeed = unitSpeed;
        var targetRot = 0f;

        if (angle > 40)
        {
            targetRot = 0.4f * rotationDirection;
        }
        else
        {
            targetRot = (0.01f * angle) * rotationDirection;
        }

        if (Mathf.Abs(animSpeed - targetSpeed) < 0.1f) { animSpeed = targetSpeed; }
        if (Mathf.Abs(animRotation - targetRot) < 0.1f) { animRotation = targetRot; }

        if (animSpeed < targetSpeed)
            animSpeed += Time.deltaTime * animSmoothingSpeed;
        else if (animSpeed > targetSpeed)
            animSpeed -= Time.deltaTime * animSmoothingSpeed;

        if (animRotation < targetRot)
            animRotation += Time.deltaTime * animSmoothingSpeed;
        else if (animRotation > targetRot)
            animRotation -= Time.deltaTime * animSmoothingSpeed;

        anim.SetFloat("Speed", animSpeed);
        anim.SetFloat("Rotation", animRotation);
    }
}
