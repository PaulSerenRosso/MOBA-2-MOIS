using GameStates;
using UnityEngine;

namespace Entities.Capacities
{
    public abstract class ActiveCapacity
    {
        public byte indexOfSOInCollection;
        public Entity caster;
        private float cooldownTimer;
        public bool onCooldown;
        private float feedbackTimer;
        
        public GameObject instantiateFeedbackObj;
        
        public ActiveCapacitySO AssociatedActiveCapacitySO()
        {
            return CapacitySOCollectionManager.GetActiveCapacitySOByIndex(indexOfSOInCollection);
        }

        public void InitiateCooldown()
        {
            cooldownTimer = AssociatedActiveCapacitySO().cooldown;
            GameStateMachine.Instance.OnTick += CooldownTimer;
        }

        private void CooldownTimer()
        {
            cooldownTimer -= GameStateMachine.Instance.tickRate;
            
            if (cooldownTimer <= 0)
            {
                onCooldown = false;
                GameStateMachine.Instance.OnTick -= CooldownTimer;
            }
        }

        public virtual bool TryCast(int casterIndex, int[] targetsEntityIndexes, Vector3[] targetPositions)
        {
            if (!onCooldown)
            {
                InitiateCooldown();
                return true;
            }

            return false;
        }

        public abstract void PlayFeedback(int casterIndex, int[] targetsEntityIndexes, Vector3[] targetPositions);

        public virtual void InitializeFeedbackCountdown()
        {
            feedbackTimer = AssociatedActiveCapacitySO().feedbackDuration;
            GameStateMachine.Instance.OnTick += FeedbackCountdown;
        }

        public  virtual void FeedbackCountdown()
        {
            feedbackTimer -= GameStateMachine.Instance.tickRate;

            if (feedbackTimer <= 0)
            {
                DisableFeedback();
            }
        }
        
        public virtual void DisableFeedback()
        {
            PoolLocalManager.Instance.EnqueuePool(AssociatedActiveCapacitySO().feedbackPrefab, instantiateFeedbackObj);
        }
    }
}

