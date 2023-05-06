using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToEnemy : Agent
{
    [SerializeField] private Player enemy;
    private WormController wormController;
    [SerializeField] private Gameplay gameplayInstance;

    private GameObject[] subjects;
    private GameObject[] enemies;

    public override void OnEpisodeBegin() {
        gameplayInstance.EpisodeBegin(transform.parent, enemy.transform);
    }

    public override void CollectObservations(VectorSensor sensor) {
        GetAllWorms();
        
        foreach (GameObject obj in subjects) {
            sensor.AddObservation(obj.transform.localPosition);
        }

        foreach (GameObject obj in enemies) {
            sensor.AddObservation(obj.transform.localPosition);
        }
    }

    public override void OnActionReceived(ActionBuffers actions) {
        float moveX = actions.ContinuousActions[0];
        float jump = actions.DiscreteActions[0];

        wormController = GetComponent<WormController>();
        float distance1 = Vector3.Distance(transform.localPosition, enemies[0].transform.localPosition);

        if (wormController != null) {
            if (jump == 1) {
                wormController.Jump();
            }
            float movementSpeed = wormController.MovementSpeed;
            wormController.MyFixedUpdate(moveX);
        }

        float distance2 = Vector3.Distance(transform.localPosition, enemies[0].transform.localPosition);

        if (distance2 <= 1f) {
            AddReward(+1f);
            EndEpisode();
        }
        
        if (distance2 < distance1) {
            AddReward(+0.1f);
        } else {
            AddReward(-0.1f);
        }
        //AddReward(-1f / MaxStep);
    }

    /*private void OnCollisionEnter2D(Collision2D collision) {
        //Debug.Log("collided");
        if (collision.gameObject.TryGetComponent<Bottom>(out Bottom bottom)) {
            SetReward(-1f);
            EndEpisode();
        }
    }*/

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent<Bottom>(out Bottom bottom)) {
            SetReward(-1f);
            EndEpisode();
        }
    }

    private void GetAllWorms() {
        subjects = gameplayInstance.GetPlayerWorms(transform.parent);
        enemies = gameplayInstance.GetPlayerWorms(enemy.transform);
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        continuousActions[0] = Input.GetAxisRaw("Worm_Move");
        
        switch (Mathf.RoundToInt(Input.GetAxisRaw("Worm_Jump"))) {
            case 0: discreteActions[0] = 0; break;
            case 1: discreteActions[0] = 1; break;
        }
    }
}
