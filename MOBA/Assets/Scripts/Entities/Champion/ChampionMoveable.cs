using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Entities.Capacities;
using ExitGames.Client.Photon.StructWrapping;
using GameStates;
using UnityEngine;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine.AI;


namespace Entities.Champion
{
    [RequireComponent(typeof(NavMeshAgent))]
    public partial class Champion : IMoveable
    {
        public float referenceMoveSpeed;

        public float CurrentMoveSpeed
        {
            get => currentMoveSpeed;
            set
            {
                currentMoveSpeed = value;
                animator.SetFloat("speedRatio", currentMoveSpeed / referenceMoveSpeed);
                if (photonView.IsMine)
                    agent.speed = currentMoveSpeed;
            }
        }

        private float currentMoveSpeed;

        public bool canMove = true;
        private Vector3 moveDirection;


        public bool IsMoved
        {
            get => isMoved;
            set
            {
                isMoved = value;
                if (animator)
                {
                    RequestChangeBoolParameterAnimator("Move", value);
                }
            }
        }

        private bool isMoved;
        
        // === League Of Legends
        private int mouseTargetIndex;
        public bool isFollowing;
        public Champion championFollow;
        public Vector3 moveDestination;

        public bool inCurveMovement;
        private Vector3 oldMoveDestination;
        private IAimable currentIAimable;
      public  ActiveCapacity currentCapacityAimed;
        private ITargetable targetEntity;
        public event GlobalDelegates.ThirdParameterDelegate<byte, int[], Vector3[]> currentTargetCapacityAtRangeEvent;

        [SerializeField] private ChampionMoveablePrevisualisation championMoveablePrevisualisationPrefab;

        //NavMesh
        [SerializeField] public NavMeshAgent agent;


        public bool CanMove()
        {
            return canMove;
        }


        void SetupNavMesh()
        {
            if (!photonView.IsMine)
            {
                agent.enabled = false;
                obstacle.enabled = true;
            }
            else
            {
                obstacle.enabled = false;
                agent.enabled = true;
                moveDestination = transform.position;
                agent.speed = currentMoveSpeed;
                var championMoveablePrevisualisation = Instantiate(championMoveablePrevisualisationPrefab, Vector3.zero,
                    quaternion.identity);
                championMoveablePrevisualisation.champion = this;
            }

            //NavMeshBuilder.ClearAllNavMeshes();
            //NavMeshBuilder.BuildNavMesh();
        }

        public float GetReferenceMoveSpeed()
        {
            return referenceMoveSpeed;
        }

        public float GetCurrentMoveSpeed()
        {
            return currentMoveSpeed;
        }

        public void RequestSetCanMove(bool value)
        {
        }

        [PunRPC]
        public void SyncSetCanMoveRPC(bool value)
        {
            if ((value && !isAlive) || (value && inCurveMovement))
            {
               
                return;
            }
            canMove = value;
            
            if (photonView.IsMine)
            {
                //    moveDestination = transform.position;
                agent.enabled = value;
            }

            if (!value)
            {
                IsMoved = false;
            }
        }

        [PunRPC]
        public void SetCanMoveRPC(bool value)
        {
            photonView.RPC("SyncSetCanMoveRPC", RpcTarget.All, value);
        }

        public event GlobalDelegates.OneParameterDelegate<bool> OnSetCanMove;
        public event GlobalDelegates.OneParameterDelegate<bool> OnSetCanMoveFeedback;

        public void RequestSetReferenceMoveSpeed(float value)
        {
            photonView.RPC("SetReferenceMoveSpeedRPC", RpcTarget.All, value);
            OnSetReferenceMoveSpeed?.Invoke(value);
        }


        [PunRPC]
        public void SetReferenceMoveSpeedRPC(float value)
        {
            referenceMoveSpeed = value;
            OnSetReferenceMoveSpeedFeedback?.Invoke(value);
        }

        public event GlobalDelegates.OneParameterDelegate<float> OnSetReferenceMoveSpeed;
        public event GlobalDelegates.OneParameterDelegate<float> OnSetReferenceMoveSpeedFeedback;

        public void RequestIncreaseReferenceMoveSpeed(float amount)
        {
            photonView.RPC("IncreaseReferenceMoveSpeedRPC", RpcTarget.All, amount);
            OnIncreaseReferenceMoveSpeed?.Invoke(amount);
        }

        public void IncreaseReferenceMoveSpeedRPC(float amount)
        {
            referenceMoveSpeed += amount;
            OnIncreaseReferenceMoveSpeedFeedback?.Invoke(amount);
        }


        public event GlobalDelegates.OneParameterDelegate<float> OnIncreaseReferenceMoveSpeed;
        public event GlobalDelegates.OneParameterDelegate<float> OnIncreaseReferenceMoveSpeedFeedback;

        public void RequestDecreaseReferenceMoveSpeed(float amount)
        {
            photonView.RPC("DecreaseReferenceMoveSpeedRPC", RpcTarget.All, amount);
            OnDecreaseReferenceMoveSpeed?.Invoke(amount);
        }

        [PunRPC]
        public void DecreaseReferenceMoveSpeedRPC(float amount)
        {
            referenceMoveSpeed -= amount;
            OnDecreaseReferenceMoveSpeedFeedback?.Invoke(amount);
        }

        public event GlobalDelegates.OneParameterDelegate<float> OnDecreaseReferenceMoveSpeed;
        public event GlobalDelegates.OneParameterDelegate<float> OnDecreaseReferenceMoveSpeedFeedback;

        public void RequestSetCurrentMoveSpeed(float value)
        {
            photonView.RPC("SetCurrentMoveSpeedRPC", RpcTarget.All, value);
            OnSetCurrentMoveSpeed?.Invoke(value);
        }

        [PunRPC]
        public void SetCurrentMoveSpeedRPC(float value)
        {
            CurrentMoveSpeed = value;
            OnSetCurrentMoveSpeedFeedback?.Invoke(value);
        }

        public event GlobalDelegates.OneParameterDelegate<float> OnSetCurrentMoveSpeed;
        public event GlobalDelegates.OneParameterDelegate<float> OnSetCurrentMoveSpeedFeedback;

        public void RequestIncreaseCurrentMoveSpeed(float amount)
        {
            photonView.RPC("IncreaseCurrentMoveSpeedRPC", RpcTarget.All, amount);

            OnIncreaseCurrentMoveSpeed?.Invoke(amount);
        }

        [PunRPC]
        public void IncreaseCurrentMoveSpeedRPC(float amount)
        {
            CurrentMoveSpeed += amount;
            OnIncreaseCurrentMoveSpeedFeedback?.Invoke(amount);
        }

        public event GlobalDelegates.OneParameterDelegate<float> OnIncreaseCurrentMoveSpeed;
        public event GlobalDelegates.OneParameterDelegate<float> OnIncreaseCurrentMoveSpeedFeedback;

        public void RequestDecreaseCurrentMoveSpeed(float amount)
        {
            photonView.RPC("DecreaseCurrentMoveSpeedRPC", RpcTarget.All, amount);
            OnDecreaseCurrentMoveSpeedFeedback?.Invoke(amount);
        }


        [PunRPC]
        public void DecreaseCurrentMoveSpeedRPC(float amount)
        {
            CurrentMoveSpeed -= amount;
            OnDecreaseCurrentMoveSpeedFeedback?.Invoke(amount);
        }

        public event GlobalDelegates.OneParameterDelegate<float> OnDecreaseCurrentMoveSpeed;
        public event GlobalDelegates.OneParameterDelegate<float> OnDecreaseCurrentMoveSpeedFeedback;

        #region Battlerite

        public void SetMoveDirection(Vector3 direction)
        {
            moveDirection = direction;
        }

        #endregion

        #region League Of Legends

        public void MoveToPosition(Vector3 position)
        {
            RequestCancelAutoAttack();
            RequestResetCapacityAimed();
            isFollowing = false;
            moveDestination = position;
            moveDestination.y = 0;
        }


        public void StartMoveToTarget(Entity _entity, ActiveCapacity capacityWhichAimed,
            GlobalDelegates.ThirdParameterDelegate<byte, int[], Vector3[]> currentTargetCapacityAtRangeEvent)
        {
            if (!isFollowing ||
                (isFollowing && (championFollow != _entity || currentCapacityAimed != capacityWhichAimed)))
            {
                RequestCancelAutoAttack();
                RequestResetCapacityAimed();
                championFollow =(Champion) _entity;
                championFollow.ActivateOutline();
                isFollowing = true;
                targetEntity = (ITargetable)championFollow;
                currentIAimable = (IAimable)capacityWhichAimed;
                currentCapacityAimed = capacityWhichAimed;
                this.currentTargetCapacityAtRangeEvent = currentTargetCapacityAtRangeEvent;
            }
        }

        private void FollowEntity()
        {
            if (!photonView.IsMine) return;
            if (!isFollowing) return;
            if (!canMove) return;

            //Debug.Log("follow");
            if (targetEntity.CanBeTargeted())
            {
                // Debug.Log("canbetarget");
                if (fowm.CheckEntityIsVisibleForPlayer(championFollow))
                {
                    //Debug.Log("visible");
                    if (currentIAimable.TryAim(entityIndex, championFollow.entityIndex,
                            championFollow.transform.position))
                    {
                        //Debug.Log("tryaim");
                        if (currentCapacityUsed == null)
                        {
                            if ((this.transform.position - championFollow.transform.position).sqrMagnitude >
                                currentIAimable.GetSqrMaxRange())
                            {
                                //Debug.Log("not distance");
                                moveDestination = championFollow.transform.position;
                                moveDestination.y = championFollow.transform.position.y;
                            }
                            else
                            {
                                LaunchAimedCapacity();
                            }
                        }
                        else
                        {
                            LaunchAimedCapacity();
                        }
                    }
                }
                else
                {
                    RequestCancelAutoAttack();
                    RequestResetCapacityAimed();
                }
            }
            else
            {
                moveDestination = transform.position;
                RequestCancelAutoAttack();
                RequestResetCapacityAimed();
               
              
            }
        }

        private void LaunchAimedCapacity()
        {
            //Debug.Log("to distance");

            moveDestination = transform.position;
            currentTargetCapacityAtRangeEvent.Invoke(currentCapacityAimed.indexOfSOInCollection, new[]
            {
                championFollow.entityIndex
            }, new[]
            {
                championFollow.transform.position
            });
        }


        public void RequestRotateMeshChampion(Vector3 direction)
        {
            photonView.RPC("RotateMeshChampionRPC", RpcTarget.All, direction);
        }

        [PunRPC]
        public void RotateMeshChampionRPC(Vector3 direction)
        {
            rotateParent.forward = direction;
        }


        public void RequestMoveChampion(Vector3 newPos)
        {
            receiveMoveChampionCount = 0;
            receiveStartMoveChampionCount = 0;
            photonView.RPC("StartMoveChampionRPC", RpcTarget.All, newPos);
        }

        [PunRPC]
        public void StartMoveChampionRPC(Vector3 newPos)
        {
            OnStartMoveChampion?.Invoke();
            transformView.enabled = false;
            if (!photonView.IsMine)
            {
                obstacle.enabled = false;
            }

            SyncSetCanMoveRPC(false);

            photonView.RPC("WaitForAllReceiveStartMoveChampion", RpcTarget.MasterClient, newPos);
        }

        private int receiveStartMoveChampionCount = 0;

        [PunRPC]
        void WaitForAllReceiveStartMoveChampion(Vector3 pos)
        {
            receiveStartMoveChampionCount++;
            if (receiveStartMoveChampionCount == GameStateMachine.Instance.playersReadyDict.Count)
                photonView.RPC("MoveChampionRPC", RpcTarget.All, pos);
        }

        private int receiveMoveChampionCount = 0;

        [PunRPC]
        public void MoveChampionRPC(Vector3 newPos)
        {
            transform.position = newPos;
            //   moveDestination = newPos;
            photonView.RPC("WaitForAllReceiveMoveChampion", RpcTarget.MasterClient);
        }

        [PunRPC]
        void WaitForAllReceiveMoveChampion()
        {
            receiveMoveChampionCount++;
            if (receiveMoveChampionCount == GameStateMachine.Instance.playersReadyDict.Count)
                photonView.RPC("EndMoveChampion", RpcTarget.All);
        }

        [PunRPC]
        void EndMoveChampion()
        {
            Debug.Log("endmove");
            transformView.enabled = true;
            if (!photonView.IsMine)
            {
                obstacle.enabled = true;
            }

            SyncSetCanMoveRPC(true);

            OnEndMoveChampion?.Invoke();
        }

        public event GlobalDelegates.NoParameterDelegate OnStartMoveChampion;
        public event GlobalDelegates.NoParameterDelegate OnEndMoveChampion;

        private void CheckMoveDistance()
        {
            if (agent == null || !agent.isOnNavMesh || !canMove) return;
            if (agent.velocity.magnitude > 0.3f)
            {
                if (!IsMoved) IsMoved = true;
                rotateParent.forward = agent.velocity.normalized;
            }
            else IsMoved = false;

            agent.velocity = agent.desiredVelocity;
            if (moveDestination != oldMoveDestination)
            {
                agent.SetDestination(moveDestination);
                oldMoveDestination = moveDestination;
            }
            moveDestination = agent.destination;
        }

        #endregion

        public event GlobalDelegates.OneParameterDelegate<Vector3> OnMove;
        public event GlobalDelegates.OneParameterDelegate<Vector3> OnMoveFeedback;
    }
}