using Entities.Capacities;
using UnityEngine;

namespace Entities.Champion
{
    [CreateAssetMenu(menuName = "Champion", fileName = "new Champion")]
    public class ChampionSO : ScriptableObject
    {
        [Header("Mesh")] public GameObject championMeshPrefab;
        
        [Header("Stats")]
        public float maxHp;
        public float maxRessource;
        public float viewRange;
        public float referenceMoveSpeed;
        
        [Header("Attack")]
        public ActiveCapacitySO attackAbility;
        public byte attackAbilityIndex;
        public float attackDamage;
        
        [Header("Abilities")]
        public PassiveCapacitySO[] passiveCapacities;
        public byte[] passiveCapacitiesIndexes;
        public ActiveCapacitySO[] activeCapacities; 
        public byte[] activeCapacitiesIndexes;
        public ActiveCapacitySO ultimateAbility;
        public byte ultimateAbilityIndex;

        public void SetIndexes()
        {
            // Attack
            attackAbilityIndex = CapacitySOCollectionManager.GetActiveCapacitySOIndex(attackAbility);
            // Passives
            passiveCapacitiesIndexes = new byte[passiveCapacities.Length];
            for (var index = 0; index < passiveCapacities.Length; index++)
            {
                var passiveCapacitySo = passiveCapacities[index];
                passiveCapacitiesIndexes[index] =
                    CapacitySOCollectionManager.GetPassiveCapacitySOIndex(passiveCapacitySo);
            }
            // Actives
            activeCapacitiesIndexes = new byte[activeCapacitiesIndexes.Length];
            for (var index = 0; index < activeCapacitiesIndexes.Length; index++)
            {
                var activeCapacitySo = activeCapacities[index];
                activeCapacitiesIndexes[index] =
                    CapacitySOCollectionManager.GetActiveCapacitySOIndex(activeCapacitySo);
            }

            ultimateAbilityIndex = CapacitySOCollectionManager.GetActiveCapacitySOIndex(ultimateAbility);
        }
    }
}


