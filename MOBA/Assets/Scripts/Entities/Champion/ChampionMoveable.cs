using System;
using System.Runtime.CompilerServices;
using Entities.Capacities;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;


namespace Entities.Champion
{
    [RequireComponent(typeof(NavMeshAgent))]
    public partial class Champion : IMoveable
    {
        public float referenceMoveSpeed;
        public float referenceRotateSpeed;
        public float currentMoveSpeed;
        public float currentRotateSpeed;
        public bool canMove;
        private Vector3 moveDirection;

        // === League Of Legends
        private int mouseTargetIndex;
        private bool isFollowing;
        private Entity entityFollow;
               private Vector3 moveDestination;

               private Vector3 oldMoveDestination;
        private IAimable currentIAimable;
        private ActiveCapacity currentCapacityAimed;
        private ITargetable targetEntity;
        public event GlobalDelegates.ThirdParameterDelegate<byte, int[], Vector3[]> currentTargetCapacityAtRangeEvent;
        [SerializeField]
        private float rotateSpeed;
  
        //NavMesh

        [SerializeField]
        private NavMeshAgent agent;


        public bool CanMove()
        {
            return canMove;
        }

        void SetupNavMesh()
        {
        
            if (!photonView.IsMine) agent.enabled = false;
            else{ obstacle.enabled = false;
                agent.enabled = true;
            moveDestination = transform.position;
            agent.speed = currentMoveSpeed;
            agent.SetDestination(transform.position);
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
        }

        [PunRPC]
        public void SetCanMoveRPC(bool value)
        {
        }

        public event GlobalDelegates.OneParameterDelegate<bool> OnSetCanMove;
        public event GlobalDelegates.OneParameterDelegate<bool> OnSetCanMoveFeedback;

        public void RequestSetReferenceMoveSpeed(float value)
        {
        }

        [PunRPC]
        public void SyncSetReferenceMoveSpeedRPC(float value)
        {
        }

        [PunRPC]
        public void SetReferenceMoveSpeedRPC(float value)
        {
        }

        public event GlobalDelegates.OneParameterDelegate<float> OnSetReferenceMoveSpeed;
        public event GlobalDelegates.OneParameterDelegate<float> OnSetReferenceMoveSpeedFeedback;

        public void RequestIncreaseReferenceMoveSpeed(float amount)
        {
        }

        [PunRPC]
        public void SyncIncreaseReferenceMoveSpeedRPC(float amount)
        {
        }

        [PunRPC]
        public void IncreaseReferenceMoveSpeedRPC(float amount)
        {
        }

        public event GlobalDelegates.OneParameterDelegate<float> OnIncreaseReferenceMoveSpeed;
        public event GlobalDelegates.OneParameterDelegate<float> OnIncreaseReferenceMoveSpeedFeedback;

        public void RequestDecreaseReferenceMoveSpeed(float amount)
        {
        }

        [PunRPC]
        public void SyncDecreaseReferenceMoveSpeedRPC(float amount)
        {
        }

        [PunRPC]
        public void DecreaseReferenceMoveSpeedRPC(float amount)
        {
        }

        public event GlobalDelegates.OneParameterDelegate<float> OnDecreaseReferenceMoveSpeed;
        public event GlobalDelegates.OneParameterDelegate<float> OnDecreaseReferenceMoveSpeedFeedback;

        public void RequestSetCurrentMoveSpeed(float value)
        {
        }

        [PunRPC]
        public void SyncSetCurrentMoveSpeedRPC(float value)
        {
        }

        [PunRPC]
        public void SetCurrentMoveSpeedRPC(float value)
        {
        }

        public event GlobalDelegates.OneParameterDelegate<float> OnSetCurrentMoveSpeed;
        public event GlobalDelegates.OneParameterDelegate<float> OnSetCurrentMoveSpeedFeedback;

        public void RequestIncreaseCurrentMoveSpeed(float amount)
        {
        }

        [PunRPC]
        public void SyncIncreaseCurrentMoveSpeedRPC(float amount)
        {
        }

        [PunRPC]
        public void IncreaseCurrentMoveSpeedRPC(float amount)
        {
        }

        public event GlobalDelegates.OneParameterDelegate<float> OnIncreaseCurrentMoveSpeed;
        public event GlobalDelegates.OneParameterDelegate<float> OnIncreaseCurrentMoveSpeedFeedback;

        public void RequestDecreaseCurrentMoveSpeed(float amount)
        {
        }

        [PunRPC]
        public void SyncDecreaseCurrentMoveSpeedRPC(float amount)
        {
        }

        [PunRPC]
        public void DecreaseCurrentMoveSpeedRPC(float amount)
        {
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
            isFollowing = false;
            moveDestination = position;
            moveDestination.y = transform.position.y;
            Debug.Log(moveDestination);
        }

        public void StartMoveToTarget(Entity _entity, ActiveCapacity capacityWhichAimed,
            GlobalDelegates.ThirdParameterDelegate<byte, int[], Vector3[]> currentTargetCapacityAtRangeEvent)
        {
            if (!isFollowing)
            {
                entityFollow = _entity;
                isFollowing = true;
                targetEntity = (ITargetable)entityFollow;
                currentIAimable = (IAimable)capacityWhichAimed;
                currentCapacityAimed = capacityWhichAimed;
                this.currentTargetCapacityAtRangeEvent += currentTargetCapacityAtRangeEvent;
            }
        }

        private void FollowEntity()
        {
            if (!photonView.IsMine) return;
            if (!isFollowing) return;

            //Debug.Log("follow");
            if (targetEntity.CanBeTargeted())
            {
               // Debug.Log("canbetarget");
                if (fowm.CheckEntityIsVisibleForPlayer(entityFollow))
                {
                    //Debug.Log("visible");
                    if (currentIAimable.TryAim(entityIndex, entityFollow.entityIndex,
                            entityFollow.transform.position))
                    {
                        //Debug.Log("tryaim");
                        if ((this.transform.position - entityFollow.transform.position).sqrMagnitude >
                            currentIAimable.GetSqrtMaxRange())
                        {
                            //Debug.Log("not distance");
                            moveDestination= entityFollow.transform.position;
                            moveDestination.y = entityFollow.transform.position.y;
                            Debug.Log(moveDestination);
                        }
                        else
                        {
                            //Debug.Log("to distance");
                            currentTargetCapacityAtRangeEvent.Invoke(currentCapacityAimed.indexOfSOInCollection, new[]
                            {
                                entityFollow.entityIndex
                            }, new[]
                            {
                                entityFollow.transform.position
                            });
                          moveDestination = transform.position;
                          Debug.Log(moveDestination);
                        }
                    }
                }
                else
                {
                    Debug.Log("not visible");
                    currentTargetCapacityAtRangeEvent = null;
                }
            }
            else
            {
                Debug.Log("can'be target");
                currentTargetCapacityAtRangeEvent = null;
                isFollowing = false;
            }
        }


        private void CheckMoveDistance()
        {
            if (agent == null ||!agent.isOnNavMesh) return;

          
            
                if (agent.velocity.magnitude > 0.3f)
                    rotateParent.forward = Vector3.MoveTowards(rotateParent.forward, agent.velocity.normalized,
                        rotateSpeed * Time.deltaTime);
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