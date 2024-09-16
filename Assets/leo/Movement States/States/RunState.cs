using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : MovementBaseState
{
    public override void EnterState(PlayerScript movement)
    {
        movement.animator.SetBool("Running", true);
    }

    public override void UpdateState(PlayerScript movement)
    {
        if (Input.GetKeyUp(KeyCode.LeftShift)) ExitState(movement, movement.Walk);
        else if (movement.Dir.magnitude < 0.1f) ExitState(movement, movement.Idle);

        if (movement.vInput < 0) movement.PlayerSpeed = movement.runBackSpeed;
        else movement.PlayerSpeed = movement.runSpeed;
    }

    void ExitState(PlayerScript movement, MovementBaseState state)
    {
        movement.animator.SetBool("Running", false);
        movement.SwitchState(state);
    }
}
