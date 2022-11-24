using Entities.Capacities;
using Entities.FogOfWar;
using GameStates;
using Photon.Pun;
using UnityEngine;

namespace Entities.Champion
{
    public partial class Champion : Entity
    {
        public ChampionSO championSo;
        public Transform championInitPoint;
        public Transform championMesh;

        private FogOfWarManager fowm;
        private CapacitySOCollectionManager capacityCollection;
        private UIManager uiManager;

        protected override void OnStart()
        {
            fowm = FogOfWarManager.Instance;
            capacityCollection = CapacitySOCollectionManager.Instance;
            //fowm.allViewables.Add(entityIndex,this);
            if(uiManager != null)
            {
                uiManager.InstantiateHealthBarForEntity(entityIndex);
                uiManager.InstantiateResourceBarForEntity(entityIndex);
            }

            currentRotateSpeed = 10f; // A mettre dans prefab, je peux pas y toucher pour l'instant
        }

        protected override void OnUpdate()
        {
            Move();
            Rotate();
        }
        
        public override void OnInstantiated() { }

        public override void OnInstantiatedFeedback() { }

        [PunRPC]
        public void ApplyChampionSORPC(byte championSoIndex)
        {
            var so = GameStateMachine.Instance.allChampionsSo[championSoIndex];
            championSo = so;
            maxHp = championSo.maxHp;
            currentHp = maxHp;
            maxResource = championSo.maxRessource;
            currentResource = championSo.maxRessource;
            viewRange = championSo.viewRange;
            referenceMoveSpeed = championSo.referenceMoveSpeed;
            currentMoveSpeed = referenceMoveSpeed;
            attackDamage = championSo.attackDamage;
            attackAbilityIndex = championSo.attackAbilityIndex;
            abilitiesIndexes = championSo.activeCapacitiesIndexes;
            ultimateAbilityIndex = championSo.ultimateAbilityIndex;

            // TODO - Implement Model/Prefab/Animator
            
            var championMesh = Instantiate(championSo.championMeshPrefab, championInitPoint.position, Quaternion.identity, championInitPoint);
            championMesh.GetComponent<ChampionMeshLinker>().LinkTeamColor(team);
        }

        public void SyncApplyChampionSO(byte championSoIndex)
        {
            photonView.RPC("ApplyChampionSORPC", RpcTarget.All, championSoIndex);
        }
    }
}