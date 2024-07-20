using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    #region 변수
    public State currentState;
    #endregion // 변수

    #region 함수
    public void Init(State state)
    {
        currentState = state;
        state.Enter();
    }

    public void ChangeState(State newState)
    {
        currentState.Exit();

        currentState = newState;
        newState.Enter();
    }
    #endregion // 함수
}
