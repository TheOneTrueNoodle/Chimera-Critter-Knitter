using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayerMovement : MonoBehaviour
{
    private bool inCombat;
    private bool inDialogue;

    [SerializeField] private float runSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float walkTurnSpeed = 210;
    [SerializeField] private float runTurnSpeed = 180;
    private Vector3 _input;

    [SerializeField] private float animSmoothingSpeed = 2f;
    private float animSpeed;
    private float animRotation;

    private Animator anim;
    private Vector3 oldForward;

    private StudioEventEmitter barkSFX;

    private bool doingBigTurn;

    private void Start()
    {
        if (CombatEvents.current != null)
        {
            CombatEvents.current.onStartCombat += StartCombat;
            CombatEvents.current.onEndCombat += EndCombat;
            CombatEvents.current.onStartDialogue += StartDialogue;
            CombatEvents.current.onEndDialogue += EndDialogue;
        }
        anim = GetComponentInChildren<Animator>();
        Debug.Log(anim);
        barkSFX = GetComponent<StudioEventEmitter>();

        oldForward = transform.forward;
    }

    private void Update()
    {
        if (inCombat || inDialogue) { return; }
        GatherInput();
        Look();
    }

    private void FixedUpdate()
    {
        if (inCombat || inDialogue) { return; }
        Move();
    }

    private void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (Input.GetButton("Bark")) { Bark(); }
    }

    private void Bark()
    {
        //Do bark code
        if (barkSFX.IsPlaying()) { return; }
        anim.Play("Bark");
        barkSFX.Play();
    }

    private void Look()
    {
        if (_input != Vector3.zero)
        {

            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;

            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            Vector3 RelativeVerticalInput = _input.z * forward;
            Vector3 RelativeHorizontalInput = _input.x * right;

            Vector3 cameraRelativeInput = RelativeVerticalInput + RelativeHorizontalInput;
            cameraRelativeInput.Normalize();

            var relative = (transform.position + cameraRelativeInput) - transform.position;
            var rot = Quaternion.LookRotation(relative, Vector3.up);

            bool running = Input.GetButton("Run");

            var turnSpeed = running ? runTurnSpeed : walkTurnSpeed;

            float angle = Quaternion.Angle(transform.rotation, rot);
            float rotationDirection = Mathf.Sign(Vector3.Dot(Vector3.up, Vector3.Cross(transform.forward, relative.normalized)));

            if(angle > 140 && !running)
            {
                //Trigger a "180" turn
                //rb.MoveRotation(rot);

                doingBigTurn = true;
                rb.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * 3 * Time.deltaTime);
                float newAngle = Quaternion.Angle(transform.rotation, rot);
                if (newAngle < 15) { doingBigTurn = false; }
            }
            else if(doingBigTurn)
            {
                rb.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * 3 * Time.deltaTime);
                float newAngle = Quaternion.Angle(transform.rotation, rot);
                if (newAngle < 15) { doingBigTurn = false; }
            }
            else
            {
                rb.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
            }

            if (angle < 1.0f) { rotationDirection = 0f; }
            if(rotationDirection > 0f) { rotationDirection = 1f; }
            else if (rotationDirection < 0f) { rotationDirection = -1f; }
            Animate(rotationDirection, angle);
        }
        else { Animate(0f, 0f); }
    }

    private void Move()
    {
        var speed = Input.GetButton("Run") ? runSpeed : walkSpeed;
        rb.MovePosition(transform.position + (transform.forward * _input.normalized.magnitude) * speed * Time.deltaTime);
    }

    private void Animate(float rotationDirection, float angle)
    {
        var moveSpeed = Input.GetButton("Run") ? 0.7f : 0.4f;
        var targetSpeed = _input.normalized.magnitude * moveSpeed;
        var targetRot = 0f;

        if(angle > 40)
        {
            targetRot = 0.4f * rotationDirection;
        }
        else
        {
            targetRot = (0.01f * angle) * rotationDirection;
        }

        if(Mathf.Abs(animSpeed - targetSpeed) < 0.1f) { animSpeed = targetSpeed; }
        if (Mathf.Abs(animRotation - targetRot) < 0.1f) { animRotation = targetRot; }

        if (animSpeed < targetSpeed)
            animSpeed += Time.deltaTime * animSmoothingSpeed;
        else if(animSpeed > targetSpeed)
            animSpeed -= Time.deltaTime * animSmoothingSpeed;

        if (animRotation < targetRot)
            animRotation += Time.deltaTime * animSmoothingSpeed;
        else if (animRotation > targetRot)
            animRotation -= Time.deltaTime * animSmoothingSpeed;

        anim.SetFloat("Speed", animSpeed);
        anim.SetFloat("Rotation", animRotation);
    }

    private void StartCombat()
    {
        inCombat = true;
    }
    private void EndCombat()
    {
        inCombat = false;
    }
    private void StartDialogue()
    {
        inDialogue = true;
    }
    private void EndDialogue()
    {
        inDialogue = false;
    }
}
