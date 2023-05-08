using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class WormsDPAgent : Agent
{
    [SerializeField] private Player enemy;
    private WormController wormController;
    [SerializeField] private Gameplay gameplayInstance;

    private GameObject[] subjects;
    private GameObject[] enemies;
    private int subjectsHealths;
    private int enemiesHealths;
    private int subjectsHealthsPrevious;
    private int enemiesHealthsPrevious;
    private int subjectsHealthsMax = 400;
    private int enemiesHealthsMax = 400;

    public override void OnEpisodeBegin() {
        gameplayInstance.EpisodeBegin(transform.parent, enemy.transform);
        subjectsHealths = subjectsHealthsMax;
        enemiesHealths = enemiesHealthsMax;
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(Gameplay.turnNumber);
        sensor.AddObservation((int)Gameplay.activeTeamColor);
        sensor.AddObservation((int)Gameplay.activeMap);
        
        GetAllWorms();
        GameObject[] miscs = gameplayInstance.GetAllMiscs();
        int counter = 0;
        
        foreach (GameObject obj in subjects) {
            sensor.AddObservation(new Vector2(obj.transform.localPosition.x, obj.transform.localPosition.y));
            sensor.AddObservation(1);
            sensor.AddObservation(obj.GetComponent<WormController>().health);
            counter++;
        }

        for (int i = counter; i < 4; i++) {
            sensor.AddObservation(new Vector2(-9999, -9999));
            sensor.AddObservation(0);
            sensor.AddObservation(0);
        }

        counter = 0;

        foreach (GameObject obj in enemies) {
            sensor.AddObservation(new Vector2(obj.transform.localPosition.x, obj.transform.localPosition.y));
            sensor.AddObservation(1);
            sensor.AddObservation(obj.GetComponent<PlayerController>().health);
            counter++;
        }

        for (int i = counter; i < 4; i++) {
            sensor.AddObservation(new Vector2(-9999, -9999));
            sensor.AddObservation(0);
            sensor.AddObservation(0);
        }

        counter = 0;

        foreach (GameObject obj in miscs) {
            sensor.AddObservation(new Vector2(obj.transform.localPosition.x, obj.transform.localPosition.y));
            if (obj.TryGetComponent<FirstAid>(out FirstAid firstAid)) { sensor.AddObservation(0); }
            else if (obj.TryGetComponent<Crate>(out Crate crate)) { sensor.AddObservation(1); }
            else if (obj.TryGetComponent<Barrel>(out Barrel barrel)) { sensor.AddObservation(2); }
            else { sensor.AddObservation(3); }
            
            counter++;
        }

        for (int i = counter; i < 5; i++) {
            sensor.AddObservation(new Vector2(-9999, -9999));
            sensor.AddObservation(3);
        }

        sensor.AddObservation(GetComponent<WormController>().bulletAmmo);
        sensor.AddObservation(GetComponent<WormController>().buckshotAmmo);
        sensor.AddObservation(GetComponent<WormController>().rocketBlueAmmo);
        sensor.AddObservation(GetComponent<WormController>().homingRocketAmmo);
        sensor.AddObservation(GetComponent<WormController>().c4Ammo);
        sensor.AddObservation(GetComponent<WormController>().grenadeAmmo);
        sensor.AddObservation(GetComponent<WormController>().dynamiteAmmo);
        sensor.AddObservation(GetComponent<WormController>().clusterBombAmmo);
        sensor.AddObservation(GetComponent<WormController>().holyGrenadeAmmo);
        sensor.AddObservation(GetComponent<WormController>().homingClusterBombAmmo);
        sensor.AddObservation(GetComponent<WormController>().mbBombAmmo);
        sensor.AddObservation(GetComponent<WormController>().mineAmmo);
        sensor.AddObservation(GetComponent<WormController>().bananaAmmo);
    }


    public override void OnActionReceived(ActionBuffers actions) {
        float moveX = actions.ContinuousActions[0];
        int jump = actions.DiscreteActions[0];
        int projectileType = actions.DiscreteActions[1];
        float direction1 = actions.ContinuousActions[1];
        float direction2 = actions.ContinuousActions[2];
        float distance = actions.ContinuousActions[2];

        wormController = GetComponent<WormController>();

        if (wormController != null) {
            if (jump == 1) {
                wormController.Jump();
            }
            wormController.MyFixedUpdate(moveX);

            if (projectileType != 0) {
                wormController.AILaunching(direction1, direction2, distance, projectileType);
            }
        }

        GetAllWormsHealths();

        if (subjectsHealths < subjectsHealthsPrevious) { AddReward(-0.1f); }
        else if (subjectsHealths > subjectsHealthsPrevious) { AddReward(+0.5f); }
        else { AddReward(+0.1f); }
        if (enemiesHealths < enemiesHealthsPrevious) { AddReward(+1f); StartCoroutine(MyEndEpisode()); }
        else if (enemiesHealths > enemiesHealthsPrevious) { AddReward(-0.5f); }
        else { AddReward(-0.1f); }

        if (subjectsHealths <= 0) {
            AddReward(-1f);
            StartCoroutine(MyEndEpisode());
        }

        if (enemiesHealths <= 0) {
            AddReward(+1f);
            StartCoroutine(MyEndEpisode());
        }
    }

    /*private void OnCollisionEnter2D(Collision2D collision) {
        //Debug.Log("collided");
        if (collision.gameObject.TryGetComponent<Bottom>(out Bottom bottom)) {
            SetReward(-1f);
            MyEndEpisode();
        }
    }*/

    private IEnumerator MyEndEpisode() {
        gameplayInstance.ResetWormUI();
        yield return new WaitForSeconds(0.2f);
        EndEpisode();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent<Bottom>(out Bottom bottom)) {
            SetReward(-1f);
            MyEndEpisode();
        }
    }

    private void GetAllWorms() {
        subjects = gameplayInstance.GetPlayerWorms(transform.parent);
        enemies = gameplayInstance.GetPlayerWorms(enemy.transform);
    }

    private void GetAllWormsHealths() {
        subjectsHealthsPrevious = subjectsHealths;
        enemiesHealthsPrevious = enemiesHealths;
        subjectsHealths = gameplayInstance.GetWormsHealths(transform.parent);
        enemiesHealths = gameplayInstance.GetPlayerWormsHealths(enemy.transform);
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        continuousActions[0] = Input.GetAxisRaw("Worm_Move");
        continuousActions[1] = GetComponent<WormController>().GetDirection('x');
        continuousActions[2] = GetComponent<WormController>().GetDirection('y');
        continuousActions[3] = GetComponent<WormController>().GetDistance();
        
        switch (Mathf.RoundToInt(Input.GetAxisRaw("Worm_Jump"))) {
            case 0: discreteActions[0] = 0; break;
            case 1: discreteActions[0] = 1; break;
        }

        discreteActions[1] = GetComponent<WormController>().GetActiveLaunching();
    }
}
