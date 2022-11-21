using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Capacities
{
//Asset Menu Synthax :
//[CreateAssetMenu(menuName = "Capacity/PassiveCapacitySO", fileName = "new PassiveCapacitySO")]
    public abstract class PassiveCapacitySO : ScriptableObject
    {
        [Tooltip("GP Name")] public string referenceName;

        [Tooltip("GD Name")] public string descriptionName;

        [TextArea(4, 4)] [Tooltip("Description of the capacity")]
        public string description;
        
        /// <returns>the type of PassiveCapacity associated with this PassiveCapacitySO</returns>
        public abstract Type GetAssociatedType();

        [Tooltip("All types of the capacity")] public List<Enums.CapacityType> types;
    }
}