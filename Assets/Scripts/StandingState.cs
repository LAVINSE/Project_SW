using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingState : State
{
    #region 변수
    private float gravityValue;
    private bool isJump;
    private bool isCrouch;
    private bool isGround;
    private bool isSprint;
    private Vector2 currentVelocity;
    private float moveSpeed;

    private Vector3 cVelocity;
    #endregion // 변수

    #region 생성자
    public StandingState(Character character, StateMachine stateMachine) : base(character, stateMachine)
    {
        this.character = character;
        this.stateMachine = stateMachine;
    }
    #endregion // 생성자

    #region 함수
    public override void Enter()
    {
        base.Enter();

        isJump = false;
        isCrouch = false;
        isSprint = false;
        input = Vector2.zero;
        velocity = Vector3.zero;
        currentVelocity = Vector3.zero;
        gravityVelocity.y = 0f;

        moveSpeed = character.moveSpeed;
        isGround = character.characterController.isGrounded;
        gravityValue = character.gravityValue;
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (jumpAction.triggered)
        {
            isJump = true;
        }
        if (crouchAction.triggered)
        {
            isCrouch = true;
        }
        if (sprintAction.triggered)
        {
            isSprint = true;
        }

        input = moveAction.ReadValue<Vector2>();
        velocity = new Vector3(input.x, 0, input.y);

        velocity = velocity.x * character.cameraTransform.right.normalized + velocity.z * character.cameraTransform.forward.normalized;
        velocity.y = 0f;
    }
    #endregion // 함수

}
