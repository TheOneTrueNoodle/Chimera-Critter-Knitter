using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

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

    private bool doingBigTurn;
    [HideInInspector] public FootstepInstance footstepInstance;

    private void Start()
    {
        if (CombatEvents.current != null)
        {
            CombatEvents.current.onStartCombat += StartCombat;
            CombatEvents.current.onEndCombat += EndCombat;
            if(DialogueEvents.current != null)
            {
                DialogueEvents.current.onStartDialogue += StartDialogue;
                DialogueEvents.current.onEndDialogue += EndDialogue;
            }
        }
        anim = GetComponentInChildren<Animator>();
        footstepInstance = GetComponent<FootstepInstance>();
        Debug.Log(anim);

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
        bool playing = _input != Vector3.zero ? true : false;
        Debug.Log(playing);
        footstepInstance.UpdateSound(playing, Input.GetButton("Run") ? 1f : 0.4f);
    }

    private void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (Input.GetButton("Bark")) { Bark(); }
    }

    private void Bark()
    {
        //Do bark code
        if (anim.GetCurrentAnimatorStateInfo(1).IsName("Bark")) { return; }
        anim.Play("Bark");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.oscarBark, transform.position);
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

            rb.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * 3 * Time.deltaTime);

            /*
            if (angle > 140 && !running)
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
            }*/

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

    private void StartCombat(List<CombatAIController> enemies, List<CombatAIController> others, List<CombatRoundEventData> RoundEvents, float BattleTheme)
    {
        inCombat = true;
        footstepInstance.UpdateSound(false, 0f);
    }
    private void EndCombat()
    {
        inCombat = false;
        anim.Play("Movement Blend");
    }
    private void StartDialogue()
    {
        inDialogue = true;
        footstepInstance.UpdateSound(false, 0f);
    }
    private void EndDialogue()
    {
        inDialogue = false;
    }
}
