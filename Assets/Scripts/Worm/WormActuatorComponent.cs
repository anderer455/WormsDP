using System;
using Unity.MLAgents.Actuators;
using UnityEngine;

/// <summary>
/// A simple example of a ActuatorComponent.
/// This should be added to the same GameObject as the BasicController
/// </summary>
public class WormActuatorComponent : ActuatorComponent
{
    public WormMoveController basicController;
    ActionSpec m_ActionSpec = ActionSpec.MakeDiscrete(3);

    /// <summary>
    /// Creates a BasicActuator.
    /// </summary>
    /// <returns></returns>
    public override IActuator[] CreateActuators()
    {
        return new IActuator[] { new WormActuator(basicController) };
    }

    public override ActionSpec ActionSpec
    {
        get { return m_ActionSpec; }
    }
}

/// <summary>
/// Simple actuator that converts the action into a {-1, 0, 1} direction
/// </summary>
public class WormActuator : IActuator
{
    public WormMoveController basicController;
    ActionSpec m_ActionSpec;

    public WormActuator(WormMoveController controller)
    {
        basicController = controller;
        m_ActionSpec = ActionSpec.MakeDiscrete(3);
    }

    public ActionSpec ActionSpec
    {
        get { return m_ActionSpec; }
    }

    /// <inheritdoc/>
    public String Name
    {
        get { return "Basic"; }
    }

    public void ResetData()
    {

    }

    public void OnActionReceived(ActionBuffers actionBuffers)
    {
        var movement = actionBuffers.DiscreteActions[0];

        var direction = 0;

        switch (movement)
        {
            case 1:
                direction = -1;
                break;
            case 2:
                direction = 1;
                break;
        }

        basicController.MoveDirection(direction);
    }

    public void Heuristic(in ActionBuffers actionBuffersOut)
    {
        var direction = Input.GetAxis("Horizontal");
        var discreteActions = actionBuffersOut.DiscreteActions;
        if (Mathf.Approximately(direction, 0.0f))
        {
            discreteActions[0] = 0;
            return;
        }
        var sign = Math.Sign(direction);
        discreteActions[0] = sign < 0 ? 1 : 2;
    }

    public void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {

    }

}
