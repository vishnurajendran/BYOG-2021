using System;
using UnityEngine;
using Scripts.CamFeatures;
using Scripts.FootstepsSystem;
using Random = UnityEngine.Random;

namespace Scripts.FPSController
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    public class AdvancedFPSController : MonoBehaviour
    {

        //[Header("Toggle Options (Not Working ATM)")]
        //[SerializeField] private bool sprintToggle;
        //[SerializeField] private bool crouchToggle;

        [Header("Walk/Run")]
        [SerializeField] private bool m_IsWalking;
        [SerializeField] private bool m_IsJogging = true;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_JogSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] private float m_Speed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;

        [Header("Jump")]
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_JWalkSpeed;
        [SerializeField] private float m_JRunSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;

        [Header("Crouch")]
        [SerializeField] private bool m_CrouchToggle;
        [SerializeField] private bool m_IsCrouching;
        [SerializeField] private float m_CrouchHeight;
        [SerializeField] private float m_NormalHeight;
        [SerializeField] private float m_CrouchSpeed;

        [Header("Sliding")]
        [SerializeField] private bool m_IsSliding;
        [SerializeField] private float m_SlideSpeed;
        [SerializeField] private float m_SlideTimer;
        [SerializeField] private float m_SlideMaxTime;
        private Vector3 m_SlideDirection;


        [Header("Mouse Look")]
        [SerializeField] private MouseMovement m_MouseLook;
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private SprintFOVKick m_FovKick = new SprintFOVKick();

        [Header("Head Bob")]
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private MoveBob m_HeadBob = new MoveBob();
        [SerializeField] private JumpBob m_JumpBob = new JumpBob();
        [SerializeField] private BobVariables m_BobVariables = new BobVariables();
        

        [Header("Audio")]
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.

        [Header("Advanced Audio System")]
        [SerializeField] private AudioValues m_AdvancedAudioSystem = new AudioValues();


        private float m_StepInterval;
        private Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;
        private Vector3 temp;
        private float SlopeLimit;
        private float StepOffset;
        private Vector3 PrevPos;//slide
        private Vector3 NewPos;//slide
        private FootstepProducer FtProducer;

        // Use this for initialization
        private void Start()
        {
            //m_IsWalking = true;
            m_IsJogging = true;
            m_JumpSpeed = m_JWalkSpeed;
            temp = this.transform.position;
            m_StepInterval = 5;
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle / 2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
            m_MouseLook.Init(transform, m_Camera.transform);
            m_CrouchHeight = m_CharacterController.height;
            SlopeLimit = m_CharacterController.slopeLimit;
            StepOffset = m_CharacterController.stepOffset;
            FtProducer = this.GetComponent<FootstepProducer>();
        }


        // Update is called once per frame
        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.isPaused)
                return;

            InteractionChecker();

            bool forward;
            if (Input.GetAxis("Vertical") == 1)
                forward = true;
            else
                forward = false;

            //if (Input.GetButtonDown("Walk/Jog") && !m_IsCrouching )
            //    m_IsJogging = !m_IsJogging;

            if (!m_IsWalking && forward)
            {
                m_Speed = m_RunSpeed;
                m_JumpSpeed = m_JRunSpeed;
                m_StepInterval = 4;                             //for Step Sound
                m_HeadBob.HorizontalBobRange = m_BobVariables.RunHorizontalBob;          //
                m_HeadBob.VerticalBobRange = m_BobVariables.RunVerticalBob;              //for HeadBob Speed
                m_HeadBob.m_BobBaseInterval = m_BobVariables.RunBobInterval;             //
                FtProducer.distanceBetweenSteps = m_AdvancedAudioSystem.RunAudioSpeed;
            }
            else if (m_IsJogging && !m_IsCrouching)
            {
                m_Speed = m_JogSpeed;
                m_JumpSpeed = m_JWalkSpeed;
                m_StepInterval = 2.25f;
                m_HeadBob.HorizontalBobRange = m_BobVariables.JogHorizontalBob;
                m_HeadBob.VerticalBobRange = m_BobVariables.JogVerticalBob;
                m_HeadBob.m_BobBaseInterval = m_BobVariables.JogBobInterval;
                FtProducer.distanceBetweenSteps = m_AdvancedAudioSystem.JogAudioSpeed;
            }
            else if (!m_IsJogging && !m_IsCrouching)
            {
                m_Speed = m_WalkSpeed;
                m_JumpSpeed = m_JWalkSpeed;
                m_StepInterval = 1.25f;
                m_HeadBob.HorizontalBobRange = m_BobVariables.WalkHorizontalBob;
                m_HeadBob.VerticalBobRange = m_BobVariables.WalkVerticalBob;
                m_HeadBob.m_BobBaseInterval = m_BobVariables.WalkBobInterval;
                FtProducer.distanceBetweenSteps = m_AdvancedAudioSystem.WalkAudioSpeed;
            }
            else if (m_IsCrouching)
            {
                m_Speed = m_CrouchSpeed;
                m_StepInterval = 3f;
                m_HeadBob.HorizontalBobRange = m_BobVariables.CrouchHorizontalBob;
                m_HeadBob.VerticalBobRange = m_BobVariables.CrouchVerticalBob;
                m_HeadBob.m_BobBaseInterval = m_BobVariables.CrouchBobInterval;
            }

            //checking for Crouching/Sliding
            if (!m_IsWalking && forward && Input.GetAxis("Horizontal") == 0 && Input.GetButtonDown("Crouch") && m_CharacterController.isGrounded)
            {
                m_SlideTimer = 0.0f;
                m_IsSliding = true;
                m_SlideDirection = this.transform.forward;
                NewPos = m_SlideDirection;
            }
            else if (Input.GetButtonDown("Crouch") && m_CharacterController.isGrounded)
                m_CrouchToggle = true;
            //if (Input.GetButtonDown("Jump") && m_IsSliding)
            //  m_IsSliding = false;


            if (m_IsSliding)
            {
                if (Input.GetButtonDown("Jump"))
                    m_IsSliding = false;
                RaycastHit hit;
                Physics.Raycast(this.transform.position, Vector3.down, out hit);

                if (hit.point.y < PrevPos.y)
                    NewPos.y = -1;
                else
                    NewPos = m_SlideDirection;

                Slide();
            }
            else if (m_CrouchToggle && !m_IsCrouching)
                Crouch();
            else if ((Input.GetButtonDown("Jump") || Input.GetButtonDown("Sprint") || m_CrouchToggle) && m_IsCrouching)
                UnCrouch();


            temp = this.transform.position;
            var lastHeight = m_CharacterController.height;

            m_CharacterController.height = Mathf.Lerp(m_CharacterController.height, m_CrouchHeight, 5 * Time.deltaTime);

            temp.y += (m_CharacterController.height - lastHeight) / 2;
            transform.position = temp;



            RotateView();
            // the jump state needs to read here to make sure it is not missed
            //if (!m_Jump)
            //{
            //    m_Jump = Input.GetButtonDown("Jump");
            //}

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                //if (m_Built_inAudio)
                //    PlayLandingSound();                     //comment this line to stop the built_in AudioSystem Jump
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;
            /*}
             * 
             * 
            private void FixedUpdate()
            {*/
            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x * speed;
            m_MoveDir.z = desiredMove.z * speed;


            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;

                if (m_Jump)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    m_Jump = false;
                    m_Jumping = true;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
            }

            if (!m_IsSliding)
                m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);
            
            UpdateCameraPosition(speed);

            PrevPos = this.transform.position;

        }


        private GameObject heldObject = null;
        public void InteractionChecker()
        {
            RaycastHit hit;
            Ray ray = m_Camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            if (Physics.Raycast(ray, out hit, 2.3f))
            {
                if (hit.collider.gameObject.GetComponent<Interactable>() != null)
                {

                    if (Input.GetButtonDown("Interact"))
                    {
                        if(heldObject == null)
                        {
                            heldObject = hit.collider.gameObject;
                            hit.collider.gameObject.GetComponent<Interactable>().Hold(true);
                        }
                        else
                        {
                            heldObject.GetComponent<Interactable>().Hold(false);
                            heldObject = null;
                        }
                    }
                    if (Input.GetMouseButtonDown(0) && heldObject != null)
                    {
                        heldObject.GetComponent<Interactable>().Throw();
                        heldObject = null;
                    }
                }
                else
                {
                
                }
            }
            else
            {
            
            }
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed * (m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }


        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            bool waswalking = m_IsWalking;


            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            m_IsWalking = !Input.GetButton("Sprint");
            
            speed = m_Speed;
            //speed = m_IsWalking ? m_Speed : m_RunSpeed;

            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }


        private void RotateView()
        {
            m_MouseLook.LookRotation(transform, m_Camera.transform);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }

        private void Crouch()
        {
            m_CrouchToggle = false;
            m_CrouchHeight = m_NormalHeight * 0.5f;
            m_Speed = m_CrouchSpeed;
            m_AudioSource.volume = 0.1f;
            m_IsCrouching = true;
        }

        private void UnCrouch()
        {
            m_CrouchToggle = false;
            m_CrouchHeight = m_NormalHeight;
            m_AudioSource.volume = 0.2f;
            m_IsCrouching = false;
        }

        private void Slide()
        {
            if ((m_SlideTimer >= m_SlideMaxTime) || !m_IsSliding)
            {
                m_CrouchHeight = m_NormalHeight;
                m_IsSliding = false;
                m_AudioSource.volume = 0.2f;
                //}
                //if (!m_IsSliding)
                //{
                m_CharacterController.slopeLimit = 45f;
                m_CharacterController.stepOffset = 0.6f;
                return;
            }

            m_CrouchHeight = m_NormalHeight * 0.5f;
            m_Speed = m_SlideSpeed;

            //Change CharactterController Values to stop player up slopes and stairs
            m_CharacterController.stepOffset = 0.1f;
            m_CharacterController.slopeLimit = 25f;

            m_StepInterval = 4;                             //for Step Sound
            m_HeadBob.HorizontalBobRange = 0f;            //
            m_HeadBob.VerticalBobRange = 0f;              //for HeadBob Speed
            m_HeadBob.m_BobBaseInterval = 4;              //
            m_AudioSource.volume = 0f;                    //Change this Later
            m_CollisionFlags = m_CharacterController.Move(NewPos * m_SlideSpeed * Time.fixedDeltaTime);


            m_SlideTimer += Time.deltaTime;
        }
    }
}

