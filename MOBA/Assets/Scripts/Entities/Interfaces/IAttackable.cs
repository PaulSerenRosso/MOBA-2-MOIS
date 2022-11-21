﻿using UnityEngine;

namespace Entities
{
    public interface IAttackable
    {
        
        /// <returns>true if the entity can attack, false if not</returns>
        public bool CanAttack();
        /// <summary>
        /// Sends an RPC to the master to set if the entity can attack.
        /// </summary>
        public void RequestSetCanAttack(bool value);
        /// <summary>
        /// Sends an RPC to all clients to set if the entity can attack.
        /// </summary>
        public void SyncSetCanAttackRPC(bool value);
        /// <summary>
        /// Sets if the entity can attack.
        /// </summary>
        public void SetCanAttackRPC(bool value);

        public event GlobalDelegates.BoolDelegate OnSetCanAttack;
        
        /// <summary>
        /// Sends an RPC to the master to Attack.
        /// </summary>
        /// <param name="capacityIndex">the index on the CapacitySOCollectionManager of the activeCapacitySO to Attack</param>
        /// <param name="targetedEntities">the entities targeted by the activeCapacity</param>
        /// <param name="targetedPositions">the positions targeted by  the activeCapacities</param>
        public void RequestAttack(byte capacityIndex, uint[] targetedEntities, Vector3[] targetedPositions);
        /// <summary>
        /// Sends an RPC to all clients to Attack an ActiveCapacity.
        /// </summary>
        /// <param name="capacityIndex">the index on the CapacitySOCollectionManager of the activeCapacitySO to Attack</param>
        /// <param name="targetedEntities">the entities targeted by the activeCapacity</param>
        /// <param name="targetedPositions">the positions targeted by  the activeCapacities</param>
        public void SyncAttackRPC(byte capacityIndex, uint[] targetedEntities, Vector3[] targetedPositions);
        /// <summary>
        /// Attacks an ActiveCapacity.
        /// </summary>
        /// <param name="capacityIndex">the index on the CapacitySOCollectionManager of the activeCapacitySO to Attack</param>
        /// <param name="targetedEntities">the entities targeted by the activeCapacity</param>
        /// <param name="targetedPositions">the positions targeted by  the activeCapacities</param>
        public void AttackRPC(byte capacityIndex, uint[] targetedEntities, Vector3[] targetedPositions);

        public event GlobalDelegates.ByteUintArrayVector3ArrayDelegate OnAttack;
    }
}