using Photon.Pun;

namespace GameStates
{
    public class MapLoaderManager : MonoBehaviourPun
    {
        private void Start()
        {
            GameStateMachine.Instance.LoadMap();
            if (PhotonNetwork.IsMasterClient) PhotonNetwork.IsMessageQueueRunning = true;
        }
    }
}