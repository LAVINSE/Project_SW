using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    #region ����
    [Header("=====> Controls <=====")]
    [SerializeField] public float moveSpeed = 5.0f;
    [SerializeField] private float crouchSpeed = 2.0f;
    [SerializeField] private float sprintSpeed = 7.0f;
    [SerializeField] private float jumpHeight = 0.8f;
    [SerializeField] private float gravityMultiplier = 2f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float crouchColliderHeight = 1.35f;

    [Header("=====> Animation Smoothing <=====")]
    [Range(0, 1)][SerializeField] private float speedDampTime = 0.1f;
    [Range(0, 1)][SerializeField] private float velocityDampTime = 0.9f;
    [Range(0, 1)][SerializeField] private float rotationDampTime = 0.2f;
    [Range(0, 1)][SerializeField] private float airControl = 0.5f;

    public StateMachine stateMachine;
    /*
    public StandingState standingState;
    public JumpingState jumpingState;
    public CrouchingState crouchingState;
    public LandingState landingState;
    public SprintState sprintState;
    public SprintJumpState sprintJumpState;
    */

    public float gravityValue = -9.81f;
    public float normalColliderHeight;
    public CharacterController characterController;
    public PlayerInput playerInput;
    public Transform cameraTransform;
    public Animator animator;
    public Vector3 playerVelocity;
    #endregion // ����

    #region ������Ƽ
    #endregion // ������Ƽ

    #region �Լ�
    /** �ʱ�ȭ */
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        stateMachine = new StateMachine();
        /*
        standingState = new StandingState(this, stateMachine);
        jumpingState = new JumpingState(this, stateMachine);
        crouchingState = new CrouchingState(this, stateMachine);
        landingState = new LandingState(this, stateMachine);
        sprintState = new SprintState(this, stateMachine);
        sprintJumpState = new SprintJumpState(this, stateMachine);
        */

        normalColliderHeight = characterController.height;
        gravityValue *= gravityMultiplier;
    }

    /** �ʱ�ȭ => ���¸� �����Ѵ� */
    private void Update()
    {
        stateMachine.currentState.HandleInput();
        stateMachine.currentState.LogicUpdate();
    }

    /** �ʱ�ȭ => ���¸� �����Ѵ� */
    private void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }
    #endregion // �Լ�
}
