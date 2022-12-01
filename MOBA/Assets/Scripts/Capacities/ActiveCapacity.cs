using GameStates;
using UnityEngine;

namespace Entities.Capacities
{
    public abstract class ActiveCapacity
    {
        public byte indexOfSOInCollection;
        public Entity caster;
        public double cooldownTimer;
        public bool onCooldown;
        private double feedbackTimer;
        public event GlobalDelegates.TwoParameterDelegate<byte, bool> cooldownIsReadyEvent;
        public GameObject instantiateFeedbackObj;

        protected int target;

        protected ActiveCapacitySO AssociatedActiveCapacitySO()
        {
            return CapacitySOCollectionManager.GetActiveCapacitySOByIndex(indexOfSOInCollection);
        }

        public virtual void SetUpActiveCapacity(byte soIndex, Entity caster)
        {
            indexOfSOInCollection = soIndex;
            this.caster = caster;
        }

        #region Cast

        /// <summary>
        /// Check if the target is in range.
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// Initialize the cooldown of the capacity when used.
        /// </summary>
        protected virtual void InitiateCooldown()
        {
            onCooldown = true;
            cooldownTimer = AssociatedActiveCapacitySO().cooldown;
            cooldownIsReadyEvent?.Invoke(indexOfSOInCollection, true);
            GameStateMachine.Instance.OnTick += CooldownTimer;
        }

        /// <summary>
        /// Method which update the timer.
        /// </summary>
        protected virtual void CooldownTimer()
        {
            cooldownTimer -= 1.0 / GameStateMachine.Instance.tickRate;

            if (cooldownTimer <= 0)
            {
                onCooldown = false;

                cooldownIsReadyEvent?.Invoke(indexOfSOInCollection, false);
                GameStateMachine.Instance.OnTick -= CooldownTimer;
            }
        }


        /// <summary>
        /// Called when trying cast a capacity.
        /// </summary>
        /// <param name="casterIndex"></param>
        /// <param name="targetsEntityIndexes"></param>
        /// <param name="targetPositions"></param>
        /// <returns></returns>
        public abstract bool TryCast(int casterIndex, int[] targetsEntityIndexes, Vector3[] targetPositions);

        #endregion

        #region Feedback

        public abstract void PlayFeedback(int casterIndex, int[] targetsEntityIndexes, Vector3[] targetPositions);

        protected virtual void InitializeFeedbackCountdown()
        {
            feedbackTimer = AssociatedActiveCapacitySO().feedbackDuration;
            GameStateMachine.Instance.OnTick += FeedbackCountdown;
        }

        protected virtual void FeedbackCountdown()
        {
            feedbackTimer -= GameStateMachine.Instance.tickRate;

            if (feedbackTimer <= 0)
            {
                DisableFeedback();
            }
        }

        protected virtual void DisableFeedback()
        {
            PoolLocalManager.Instance.EnqueuePool(AssociatedActiveCapacitySO().feedbackPrefab, instantiateFeedbackObj);
            GameStateMachine.Instance.OnTick -= FeedbackCountdown;
        }

        #endregion
    }
}