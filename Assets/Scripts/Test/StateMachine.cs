using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    #region ����
    public State currentState;
    #endregion // ����

    #region �Լ�
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
    #endregion // �Լ�
}
