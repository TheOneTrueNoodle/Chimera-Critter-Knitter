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

    private Entity oscarData;

    [SerializeField] private Animator anim;
    [SerializeField] private Animator cutsceneAnim;

    private DialogueManager dm;

    private Vector3 oldCamForward;
    private Vector3 oldCamRight;
    private bool cameraChange;

    private bool doingBigTurn;
    [HideInInspector] public FootstepInstance footstepInstance;

    public bool idle;
    public float idleTimer;
    private static float sitDownTime = 7;
    private static float lieDownTime = 14;

    [Header("Injured Textures")]
    private bool goreActive;

    [SerializeField] private Material defaultMat;
    [SerializeField] private Material injuredMat;

    [SerializeField] private GameObject DogsHead;
    [SerializeField] private GameObject DogsBody;
    [SerializeField] private GameObject DogsFrontLegs;
    [SerializeField] private GameObject DogsBackLegs;
    [SerializeField] private GameObject DogsTail;
    [SerializeField] private GameObject DogsEyes;

    public List<Interactable> nearbyInteractions;
    public Interactable nearestInteraction;

    private void Start()
    {
        if (CombatEvents.current != null)
        {
            CombatEvents.current.onStartCombat += StartCombat;
            CombatEvents.current.onEndCombat += EndCombat;
        }
        if (DialogueEvents.current != null)
        {
            DialogueEvents.current.onStartDialogue += StartDialogue;
            DialogueEvents.current.onEndDialogue += EndDialogue;
        }
        footstepInstance = GetComponent<FootstepInstance>();
        oscarData = GetComponent<Entity>();

        oldCamForward = Camera.main.transform.forward;
        oldCamRight = Camera.main.transform.right;

        dm = GameObject.Find("DialogueManagement").GetComponent<DialogueManager>();

        nearbyInteractions = new List<Interactable>();
    }

    private void Update()
    {
        Injured();
        CheckInteractables();
        if (inCombat || inDialogue) { return; }
        GatherInput();
        Look();
    }

    private void FixedUpdate()
    {
        if (inCombat || inDialogue) { return; } 
        Move();
        bool playing = _input != Vector3.zero ? true : false;
        footstepInstance.UpdateSound(playing, Input.GetButton("Run") ? 1f : 0.4f);
    }

    private void GatherInput()
    {
        Vector3 newInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (newInput == Vector3.zero)
        {
            if (!idle)
            {
                idleTimer = 0f;
                idle = true;
                anim.SetBool("Idle", true);
            }
        }
        else if (idle)
        {
            idle = false;
            anim.SetFloat("Idle Time", 0f);
            anim.SetBool("Idle", false);
        }

        _input = newInput;
        if (Input.GetButton("Bark")) { Bark(); }
        if (Input.GetButton("Smell")) { Smell(); }
        if (Input.GetButtonDown("Interact")) { Interact(); }
    }

    private void Bark()
    {
        //Do bark code
        if (anim.GetCurrentAnimatorStateInfo(1).IsName("Bark")) { return; }
        anim.Play("Bark");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.oscarBark, transform.position);
        idleTimer = 0;
        anim.SetFloat("Idle Time", 0);
    }

    private void Smell()
    {
        //Do smell code
        if (anim.GetCurrentAnimatorStateInfo(1).IsName("Smell")) { return; }
        anim.Play("Smell");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.oscarSmell, transform.position);
        idleTimer = 0;
        anim.SetFloat("Idle Time", 0);
    }

    private void CheckInteractables()
    {
        if (nearbyInteractions.Count <= 0) 
        {
            nearestInteraction = null;

            //hide interaction UI
            MenuEvent.current.HideInteractUI();

            return; 
        }

        //Get closest interactable
        Interactable closestInteraction = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Interactable potentialInteractable in nearbyInteractions)
        {
            if (potentialInteractable.gameObject.activeInHierarchy == true)
            {
                Vector3 directionToTarget = potentialInteractable.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    closestInteraction = potentialInteractable;
                }
            }
        }

        nearestInteraction = closestInteraction;

        //Update Displays for Interacting
        if (nearestInteraction != null)
        {
            if (nearestInteraction.interactSprite == null)
            {
                MenuEvent.current.ShowInteractUI(nearestInteraction.interactSprite);
            }
            else
            {
                MenuEvent.current.ShowInteractUI(nearestInteraction.interactSprite);
            }
        }
    }

    private void Interact()
    {
        if (nearestInteraction == null) { return; }

        //Call interactable interaction
        nearestInteraction.CallInteraction();
    }

    private void Look()
    {
        if (_input != Vector3.zero)
        {
            if (!cameraChange)
            {
                oldCamForward = Camera.main.transform.forward;
                oldCamRight = Camera.main.transform.right;
            }

            Vector3 forward = oldCamForward;
            Vector3 right = oldCamRight;

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
        var moveSpeed = Input.GetButton("Run") ? 1f : 0.4f;
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

        if (idle)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer > lieDownTime)
            {
                anim.SetFloat("Idle Time", 1f);
            }
            else if (idleTimer > sitDownTime)
            {
                anim.SetFloat("Idle Time", 0.5f);
            }
        }

        anim.SetFloat("Speed", animSpeed);
        anim.SetFloat("Rotation", animRotation);
    }
    public void Injured()
    {
        if (oscarData.activeStatsDir == null) { AssignOscarData(); }

        float percentageHealthMissing = ((oscarData.activeStatsDir["MaxHP"].baseStatValue - oscarData.activeStatsDir["MaxHP"].statValue) / oscarData.activeStatsDir["MaxHP"].baseStatValue) * 100;
        if (percentageHealthMissing > 70)
        {
            dm.isInjured = true;

            //YOU ARE INJURED SILLY
            if (anim.GetBool("Injured") == false)
            {
                anim.SetBool("Injured", true);

                if (PlayerPrefs.GetInt("Remove Gore") != 1)
                {
                    ApplyInjuredTextures();
                }
            }
            if (goreActive && PlayerPrefs.GetInt("Remove Gore") == 1)
            {
                ApplyDefaultTextures();
            }
            else if (!goreActive && PlayerPrefs.GetInt("Remove Gore") != 1)
            {
                ApplyInjuredTextures();
            }
        }
        else
        {
            dm.isInjured = false;

            if (anim.GetBool("Injured") == true)
            {
                anim.SetBool("Injured", false);
                ApplyDefaultTextures();
            }
        }
    }

    private void ApplyDefaultTextures()
    {
        //Apply default textures
        DogsHead.GetComponent<Renderer>().material = defaultMat;
        DogsBody.GetComponent<Renderer>().material = defaultMat;
        DogsFrontLegs.GetComponent<Renderer>().material = defaultMat;
        DogsBackLegs.GetComponent<Renderer>().material = defaultMat;
        DogsTail.GetComponent<Renderer>().material = defaultMat;
        DogsEyes.GetComponent<Renderer>().material = defaultMat;
        goreActive = false;
    }
    private void ApplyInjuredTextures()
    {
        //Apply injured textures
        DogsHead.GetComponent<Renderer>().material = injuredMat;
        DogsBody.GetComponent<Renderer>().material = injuredMat;
        DogsFrontLegs.GetComponent<Renderer>().material = injuredMat;
        DogsBackLegs.GetComponent<Renderer>().material = injuredMat;
        DogsTail.GetComponent<Renderer>().material = injuredMat;
        DogsEyes.GetComponent<Renderer>().material = injuredMat;
        goreActive = true;
    }

    public IEnumerator resetCameraForward()
    {
        cameraChange = true;

        yield return new WaitForSeconds(0.5f);

        cameraChange = false;
    }
    private void AssignOscarData()
    {
        if (oscarData.level < 1) { oscarData.level = 1; }
        oscarData.CharacterData.SetDictionaryStats(oscarData.level);
        if (oscarData.CharacterData.Weapon != null)
        {
            oscarData.AttackDamageType = oscarData.CharacterData.Weapon.damageType;
            oscarData.WeaponRange = oscarData.CharacterData.Weapon.attackRange;
        }
        else
        {
            oscarData.AttackDamageType = oscarData.CharacterData.defaultAttack;
            oscarData.WeaponRange = 1;
        }

        if (oscarData.activeStatsDir == null)
        {
            oscarData.activeStatsDir = new Dictionary<string, Stat>();
            foreach (KeyValuePair<string, Stat> item in oscarData.CharacterData.statsDir)
            {
                oscarData.activeStatsDir.Add(item.Value.name, new Stat(item.Value.name, item.Value.baseStatValue));
            }
        }

        oscarData.CharacterData.SetEquipment();
        oscarData.activeAbilities = oscarData.CharacterData.SetAbilities(null);
        oscarData.Resistances = oscarData.CharacterData.SetResistances();
        oscarData.Weaknesses = oscarData.CharacterData.SetWeaknesses();

        List<EquipmentStatChanges> equipmentStatChanges = oscarData.CharacterData.SetEquipmentStatChanges();
        List<ScriptableEffect> equipmentEffects = oscarData.CharacterData.SetStartingEffects();

        CombatEvents.current.UnitStartingEffects(oscarData, equipmentStatChanges, equipmentEffects);
    }

    private void StartCombat(string combatName, List<CombatAIController> enemies, List<CombatAIController> others, List<CombatRoundEventData> RoundEvents, float BattleTheme)
    {
        inCombat = true;
        anim.SetBool("Idle", true);
        footstepInstance.UpdateSound(false, 0f);
    }
    private void EndCombat(string combatName)
    {
        inCombat = false;
        anim.Play("Movement Blend");
    }
    private void StartDialogue()
    {
        anim.SetBool("Idle", true);
        idle = true;
        inDialogue = true;
        footstepInstance.UpdateSound(false, 0f);
    }
    private void EndDialogue()
    {
        inDialogue = false;
    }
}
