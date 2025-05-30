using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace WormComponents.Agents.Hard
{
    public class WormAgentHardMultiWeapon : WormAgent
    {
        protected bool _ignoreEpisodeBegin = false;
        [SerializeField] private List<WormController> m_friendly_Puppets;
        [SerializeField] private List<WormController> m_enemy_Puppets;
        [SerializeField] private TeamSO m_MyTeamSettings;
        [SerializeField] private TeamSO m_EnemyTeamSettings;
        [SerializeField] private Camera m_WormsCamera;

        private bool _endEpisodeRequested = false;

        protected void Start()
        {
            m_WormController.SetColor(m_MyTeamSettings.m_UI_Color);
            m_WormController.ApplyLayer(m_MyTeamSettings.m_WormLayerId);
            ApplyEnemyLayers(m_MyTeamSettings.m_EnemyLayers);
            m_WormController.m_IsSelfDestroyAllowed = false;

            for (int i = 0; i < m_friendly_Puppets.Count; i++)
            {
                m_friendly_Puppets[i].m_IsSelfDestroyAllowed = false;
                m_friendly_Puppets[i].SetColor(m_MyTeamSettings.m_UI_Color);
                m_friendly_Puppets[i].ApplyLayer(m_MyTeamSettings.m_WormLayerId);
                m_friendly_Puppets[i].m_ProjectileTarget.OnTouch += damage =>
                {
                    SetReward(-1f); //  penalty for friends
                    if(!_endEpisodeRequested)
                        StartCoroutine(EndEpisodeIn(1));
                };
            }

            for (int i = 0; i < m_enemy_Puppets.Count; i++)
            {
                m_enemy_Puppets[i].m_IsSelfDestroyAllowed = false;
                m_enemy_Puppets[i].SetColor(m_EnemyTeamSettings.m_UI_Color);
                m_enemy_Puppets[i].ApplyLayer(m_EnemyTeamSettings.m_WormLayerId);
                m_enemy_Puppets[i].m_ProjectileTarget.OnTouch += damage =>
                {
                    SetReward(1f); //  rewards for enemies
                    if(!_endEpisodeRequested)
                        StartCoroutine(EndEpisodeIn(1));
                };
            }

            m_WormController.m_EnvironmentController.OnTurnStarted += env =>
            {
                m_WormController.m_State.m_State = WormState.States.MOVING;
            };

            m_WormController.m_EnvironmentController.m_Timer.OnTimeOver += () =>
            {
                if (m_WormController.m_State.m_State != WormState.States.ESCAPING)
                {
                    SetReward(-1);
                }

                EndEpisode();
            };
            
            m_WormController.m_Health.OnDie += () =>
            {
                SetReward(-1);
                EndEpisode();
            };
        }

        protected IEnumerator EndEpisodeIn(float sec)
        {
            _endEpisodeRequested = true;
            yield return new WaitForSeconds(sec);
            _endEpisodeRequested = false;
            EndEpisode();
        }

        public override void OnEpisodeBegin()
        {
            if (_ignoreEpisodeBegin)
                return;

            _episodesCount++;
            if (_episodesCount >= m_RefreshMapAfterEpisodes)
            {
                _episodesCount -= m_RefreshMapAfterEpisodes;
                m_WormController.m_EnvironmentController.RefreshMap();
            }
            
            m_WormController.Reset();
            m_WormController.m_ActiveWeapon.RandomConfig();

            foreach (var puppet in m_friendly_Puppets)
            {
                puppet.Reset();
            }

            foreach (var puppet in m_enemy_Puppets)
            {
                puppet.Reset();
            }

            var worms = new List<WormController>();
            worms.Add(m_WormController);
            m_friendly_Puppets.ForEach(p => worms.Add(p));
            m_enemy_Puppets.ForEach(p => worms.Add(p));
            m_WormController.m_EnvironmentController.PlaceWormRandomly(worms);
            m_WormController.m_EnvironmentController.Reset();
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(m_WormController.m_EnvironmentController.m_Timer.m_TimeLeft
                                  / m_WormController.m_EnvironmentController.m_Timer
                                      .m_TurnLength); // time to end of the phase
            sensor.AddObservation(m_WormController.m_ActiveWeapon.transform.right); // weapon direction
            sensor.AddObservation(m_WormController.m_ActiveWeapon.m_Power); // weapon force
            sensor.AddObservation(m_WormController.m_GroundDetector.m_IsGrounded); // Is Grounded

            sensor.AddObservation(m_WormController.m_State.m_State == WormState.States.IDLE); // current phase
            sensor.AddObservation(m_WormController.m_State.m_State == WormState.States.MOVING); // current phase
            sensor.AddObservation(m_WormController.m_State.m_State == WormState.States.SHOOTING); // current phase
            sensor.AddObservation(m_WormController.m_State.m_State == WormState.States.ESCAPING); // current phase
            
            for (int i = 0; i < m_WormController.m_Weapons.Count; i++)
            {
                sensor.AddObservation(i == m_WormController.m_ControllingSignals.m_WeaponId);
            }
        }
        
        public override void ApplyEnemyLayers(LayerMask lm)
        {
            m_WormsCamera.cullingMask = lm;
        }
        
        public void SetPlayEnemies(List<WormController> friends, List<WormController> enemies)
        {
            m_friendly_Puppets = friends;
            m_enemy_Puppets = enemies;

            m_WormController.SetColor(m_MyTeamSettings.m_UI_Color);
            m_WormController.ApplyLayer(m_MyTeamSettings.m_WormLayerId);
            ApplyEnemyLayers(m_MyTeamSettings.m_EnemyLayers);
        }
        
        public override void OnActionReceived(ActionBuffers actions) {

            // Actions processing 
            var xAxis = actions.DiscreteActions[0] - 1; 
            var yAxis = actions.DiscreteActions[1] - 1; 
            var jumpButton = actions.DiscreteActions[2]; 
            var fireButton = actions.DiscreteActions[3];
            int newWeaponId = 0;
            if (m_WormController.m_Weapons.Count > 1)
            {
                newWeaponId = actions.DiscreteActions[4];
            }

            switch (m_WormController.m_State.m_State)
            {
                case WormState.States.MOVING:
                    m_WormController.m_ControllingSignals.m_HorizontalMoving = xAxis;
                    m_WormController.m_ControllingSignals.m_Jump = jumpButton == 1;
                    m_WormController.m_ControllingSignals.m_Aim = fireButton == 1;
                    if (m_WormController.m_Weapons.Count > 1)
                    {
                        m_WormController.m_ControllingSignals.m_TargetWeaponId = newWeaponId;
                    }
                    break;
                case WormState.States.SHOOTING:
                    m_WormController.m_ControllingSignals.m_Aimning = xAxis;
                    m_WormController.m_ControllingSignals.m_PowerChanging = yAxis;
                    m_WormController.m_ControllingSignals.m_Fire = fireButton == 1;
                    if (m_WormController.m_Weapons.Count > 1)
                    {
                        m_WormController.m_ControllingSignals.m_TargetWeaponId = newWeaponId;
                    }
                    break;
                case WormState.States.ESCAPING:
                    m_WormController.m_ControllingSignals.m_HorizontalMoving = xAxis;
                    m_WormController.m_ControllingSignals.m_Jump = jumpButton == 1;
                    break;
                default:
                    break;
            }
        }

        protected int GetNewWeaponIdFromInput()
        {
            int newWeaponId = m_WormController.m_ControllingSignals.m_WeaponId;

            if (Input.GetKey(KeyCode.Alpha1)) newWeaponId = 0;
            if (Input.GetKey(KeyCode.Alpha2)) newWeaponId = 1;
            if (Input.GetKey(KeyCode.Alpha3)) newWeaponId = 2;

            return newWeaponId;
        }
        
        public override void Heuristic(in ActionBuffers actionsOut) {
            ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
            discreteActions[0] = Input.GetAxis("Horizontal") > 0f ? 2 : Input.GetAxis("Horizontal") < 0f ? 0 : 1;
            discreteActions[1] = Input.GetAxis("Vertical") > 0f ? 2 : Input.GetAxis("Vertical") < 0f ? 0 : 1;
            discreteActions[2] = Input.GetButton("Jump") ? 1 : 0;
            discreteActions[3] = Input.GetButton("Fire1") ? 1 : 0;
            if (m_WormController.m_Weapons.Count > 1)
            {
                discreteActions[4] = GetNewWeaponIdFromInput();
            }
        }
        
        public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
        {
            if (m_WormController.m_State.m_State == WormState.States.IDLE)
            {
                // X axis
                actionMask.SetActionEnabled(0, 0, false); 
                actionMask.SetActionEnabled(0, 1, true);
                actionMask.SetActionEnabled(0, 2, false); 
                
                // Y axis
                actionMask.SetActionEnabled(1, 0, false); 
                actionMask.SetActionEnabled(1, 1, true);
                actionMask.SetActionEnabled(1, 2, false); 
            
                // Jump
                actionMask.SetActionEnabled(2, 0, true);
                actionMask.SetActionEnabled(2, 1, false);

                // Fire
                actionMask.SetActionEnabled(3, 0, true);
                actionMask.SetActionEnabled(3, 1, false);
                
                // WeaponID
                if (m_WormController.m_Weapons.Count > 1)
                {
                    actionMask.SetActionEnabled(4, 0, m_WormController.m_ControllingSignals.m_WeaponId == 0);
                    actionMask.SetActionEnabled(4, 1, m_WormController.m_ControllingSignals.m_WeaponId == 1);
                    actionMask.SetActionEnabled(4, 2, m_WormController.m_ControllingSignals.m_WeaponId == 2);
                }
            }
            else if (m_WormController.m_State.m_State == WormState.States.MOVING)
            {
                // X axis. Moving left / right
                actionMask.SetActionEnabled(0, 0, true); 
                actionMask.SetActionEnabled(0, 1, true); 
                actionMask.SetActionEnabled(0, 2, true); 
                
                // Y axis. Not Used
                actionMask.SetActionEnabled(1, 0, false); 
                actionMask.SetActionEnabled(1, 1, true);
                actionMask.SetActionEnabled(1, 2, false); 
            
                // Jump
                actionMask.SetActionEnabled(2, 0, true); 
                actionMask.SetActionEnabled(2, 1, m_WormController.m_GroundDetector.m_IsGrounded);

                // Fire -- Switching to aim
                actionMask.SetActionEnabled(3, 0, true); 
                actionMask.SetActionEnabled(3, 1, true);
                
                // WeaponID
                if (m_WormController.m_Weapons.Count > 1)
                {
                    actionMask.SetActionEnabled(4, 0, m_WormController.m_ControllingSignals.m_WeaponId == 0);
                    actionMask.SetActionEnabled(4, 1, m_WormController.m_ControllingSignals.m_WeaponId == 1);
                    actionMask.SetActionEnabled(4, 2, m_WormController.m_ControllingSignals.m_WeaponId == 2);
                }
            } 
            else if (m_WormController.m_State.m_State == WormState.States.SHOOTING)
            {
                // X axis. Aim rotate left / right
                actionMask.SetActionEnabled(0, 0, true); 
                actionMask.SetActionEnabled(0, 1, true); 
                actionMask.SetActionEnabled(0, 2, true); 
                
                // Y axis. Shot Power + -
                actionMask.SetActionEnabled(1, 0, true); 
                actionMask.SetActionEnabled(1, 1, true);
                actionMask.SetActionEnabled(1, 2, true); 
            
                // Jump.
                actionMask.SetActionEnabled(2, 0, true); 
                actionMask.SetActionEnabled(2, 1, false);

                // Fire. 
                actionMask.SetActionEnabled(3, 0, true); 
                actionMask.SetActionEnabled(3, 1, true);
                
                // WeaponID
                if (m_WormController.m_Weapons.Count > 1)
                {
                    actionMask.SetActionEnabled(4, 0, true);
                    actionMask.SetActionEnabled(4, 1, true);
                    actionMask.SetActionEnabled(4, 2, true);
                }
            }
            else if (m_WormController.m_State.m_State == WormState.States.ESCAPING)
            {
                // X axis. Moving left / right
                actionMask.SetActionEnabled(0, 0, true); 
                actionMask.SetActionEnabled(0, 1, true); 
                actionMask.SetActionEnabled(0, 2, true); 
                
                // Y axis. Not Used
                actionMask.SetActionEnabled(1, 0, false); 
                actionMask.SetActionEnabled(1, 1, true);
                actionMask.SetActionEnabled(1, 2, false); 
            
                // Jump
                actionMask.SetActionEnabled(2, 0, true); 
                actionMask.SetActionEnabled(2, 1, m_WormController.m_GroundDetector.m_IsGrounded);

                // Fire. Not Used
                actionMask.SetActionEnabled(3, 0, true);
                actionMask.SetActionEnabled(3, 1, false);
                
                // WeaponID
                if (m_WormController.m_Weapons.Count > 1)
                {
                    actionMask.SetActionEnabled(4, 0, m_WormController.m_ControllingSignals.m_WeaponId == 0);
                    actionMask.SetActionEnabled(4, 1, m_WormController.m_ControllingSignals.m_WeaponId == 1);
                    actionMask.SetActionEnabled(4, 2, m_WormController.m_ControllingSignals.m_WeaponId == 2);
                }
            }
        }
    }
}
