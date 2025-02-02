using System;
using Entities.Champion;
using Photon.Pun;
using UnityEngine;

namespace GameStates
{
    public class MapLoaderManager : MonoBehaviourPun
    {
        public static MapLoaderManager Instance;
        
        public ChampionSpawner[] firstTeamBasePoint;
        public ChampionSpawner[] secondTeamBasePoint;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                DestroyImmediate(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            GameStateMachine.Instance.LoadMap();
            
           // if (PhotonNetwork.IsMasterClient) PhotonNetwork.IsMessageQueueRunning = true;
        }

        private int countForSendIsReady = 0;
        public void AddCountForSendIsReady()
        {
            countForSendIsReady++;
            if(countForSendIsReady == GameStateMachine.Instance.playersReadyDict.Count)
            GameStateMachine.Instance.SendSetToggleReady(true);
        }

        [Serializable]
        public class ChampionSpawner
        {
            public Transform position;
            public Champion champion;
        }
    }
}