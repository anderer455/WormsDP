using System;
using Unity.MLAgents.Sensors;
using Unity.MLAgentsExamples;

/// <summary>
/// A simple example of a SensorComponent.
/// This should be added to the same GameObject as the BasicController
/// </summary>
public class WormSensorComponent : SensorComponent
{
    public WormMoveController basicController;

    /// <summary>
    /// Creates a BasicSensor.
    /// </summary>
    /// <returns></returns>
    public override ISensor[] CreateSensors()
    {
        return new ISensor[] { new WormSensor(basicController) };
    }
}

/// <summary>
/// Simple Sensor implementation that uses a one-hot encoding of the Agent's
/// position as the observation.
/// </summary>
public class WormSensor : SensorBase
{
    public WormMoveController basicController;

    public WormSensor(WormMoveController controller)
    {
        basicController = controller;
    }

    /// <summary>
    /// Generate the observations for the sensor.
    /// In this case, the observations are all 0 except for a 1 at the position of the agent.
    /// </summary>
    /// <param name="output"></param>
    public override void WriteObservation(float[] output)
    {
        // One-hot encoding of the position
        Array.Clear(output, 0, output.Length);
        output[basicController.position] = 1;
    }

    /// <inheritdoc/>
    public override ObservationSpec GetObservationSpec()
    {
        return ObservationSpec.Vector(WormMoveController.k_Extents);
    }

    /// <inheritdoc/>
    public override string GetName()
    {
        return "Worm";
    }

}
