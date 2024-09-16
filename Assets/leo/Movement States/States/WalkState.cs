using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WalkState : MovementBaseState
{
    public override void EnterState(PlayerScript movement)
    {
        movement.animator.SetBool("Walking", true);
    }

    public override void UpdateState(PlayerScript movement)
    {
        if (Input.GetKey(KeyCode.LeftShift)) ExitState(movement, movement.Run);
        else if (Input.GetKeyDown(KeyCode.C)) ExitState(movement, movement.Crouch);
        else if (movement.Dir.magnitude < 0.1f) ExitState(movement, movement.Idle);

        if (movement.vInput < 0) movement.PlayerSpeed = movement.walkBackSpeed;
        else movement.PlayerSpeed = movement.walkSpeed;
    }

    void ExitState(PlayerScript movement, MovementBaseState state)
    {
        movement.animator.SetBool("Walking", false);
        movement.SwitchState(state);
    }
}
