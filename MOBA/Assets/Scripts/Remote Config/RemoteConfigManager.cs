using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace RemoteConfig {
    public class RemoteConfigManager : MonoBehaviour {
        [SerializeField] private RemoteConfigVariables variables = null;
        [SerializeField] private bool updateAtStart = true;

        private struct userAttributes {}
        private struct appAttributes {}

        /// <summary>
        /// Method called before the start of the game
        /// </summary>
        private async void Awake() {
            if (Utilities.CheckForInternetConnection()) await InitializeRemoteConfigAsync();

            RemoteConfigService.Instance.FetchCompleted += ApplySettings;
            if (updateAtStart) CallFetch();
        }

        /// <summary>
        /// Async Method which allow to wait for connection before doing something else
        /// </summary>
        private async Task InitializeRemoteConfigAsync() {
            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn) {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }

        /// <summary>
        /// Method which call the appConfig to retrieve all the data and be able to update the value
        /// </summary>
        public void CallFetch() => RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());

        /// <summary>
        /// Method which is called when the fetching is done
        /// </summary>
        /// <param name="configResponse"></param>
        private void ApplySettings(ConfigResponse configResponse) {
            if (configResponse.requestOrigin != ConfigOrigin.Remote) return;

            SetChampion01Variables();
            SetChampion02Variables();
            SetGeneratorVariables();
            SetRelaiVariables();
        }

        /// <summary>
        /// Set the variables of the champion 01 : Esquirrel
        /// </summary>
        private void SetChampion01Variables() {
            //BASE STAT
            variables.CHAMP01_BaseVariables.maxHp = RemoteConfigService.Instance.appConfig.GetFloat("CHAMP01_BASE_MaxHP");
            variables.CHAMP01_BaseVariables.maxRessource = RemoteConfigService.Instance.appConfig.GetFloat("CHAMP01_BASE_MaxResources");
            variables.CHAMP01_BaseVariables.viewRange = RemoteConfigService.Instance.appConfig.GetFloat("CHAMP01_BASE_ViewRange");
            variables.CHAMP01_BaseVariables.referenceMoveSpeed = RemoteConfigService.Instance.appConfig.GetFloat("CHAMP01_BASE_MoveSpeed");
            
            //AUTO-ATTACK
            variables.CHAMP01_AutoAttack.cooldown = RemoteConfigService.Instance.appConfig.GetFloat("CHAMP01_AA_Cooldown");
            variables.CHAMP01_AutoAttack.maxRange = RemoteConfigService.Instance.appConfig.GetFloat("CHAMP01_AA_MaxRange");
            variables.CHAMP01_AutoAttack.damage = RemoteConfigService.Instance.appConfig.GetFloat("CHAMP01_AA_Damage");
            variables.CHAMP01_AutoAttack.damageBeginTime = RemoteConfigService.Instance.appConfig.GetFloat("CHAMP01_AA_DamageBeginTime");
            variables.CHAMP01_AutoAttack.damageTime = RemoteConfigService.Instance.appConfig.GetFloat("CHAMP01_AA_DamageTime");
            variables.CHAMP01_AutoAttack.fxTime = RemoteConfigService.Instance.appConfig.GetFloat("CHAMP01_AA_FxTime");
            
            //CAPACITY 01

            //CAPACITY 02
            
            //ULTIMATE
        }

        /// <summary>
        /// Set the variables of the champion 01 : Poumf
        /// </summary>
        private void SetChampion02Variables() {
            //BASE STAT
            variables.CHAMP02_BaseVariables.maxHp = RemoteConfigService.Instance.appConfig.GetFloat("CHAMP02_BASE_MaxHP");
            variables.CHAMP02_BaseVariables.maxRessource = RemoteConfigService.Instance.appConfig.GetFloat("CHAMP02_BASE_MaxResources");
            variables.CHAMP02_BaseVariables.viewRange = RemoteConfigService.Instance.appConfig.GetFloat("CHAMP02_BASE_ViewRange");
            variables.CHAMP02_BaseVariables.referenceMoveSpeed = RemoteConfigService.Instance.appConfig.GetFloat("CHAMP02_BASE_MoveSpeed");
            
            //AUTO-ATTACK

            //CAPACITY 01

            //CAPACITY 02
            
            //ULTIMATE
        }

        /// <summary>
        /// Set the variables of the generator
        /// </summary>
        private void SetGeneratorVariables() {
            variables.generatorCapturePoint.baseViewRange = RemoteConfigService.Instance.appConfig.GetFloat("GEN_BaseViewRange");
            variables.generatorCapturePoint.viewRange = RemoteConfigService.Instance.appConfig.GetFloat("GEN_ViewRange");
            variables.generatorCapturePoint.capturePointSpeed = RemoteConfigService.Instance.appConfig.GetFloat("GEN_CapturePointSpeed");
            variables.generatorCapturePoint.firstTeamState.stabilityPoint = RemoteConfigService.Instance.appConfig.GetFloat("GEN_TEAM01_StabilityPoint");
            variables.generatorCapturePoint.firstTeamState.captureValue = RemoteConfigService.Instance.appConfig.GetFloat("GEN_TEAM01_CaptureValue");
            variables.generatorCapturePoint.firstTeamState.maxValue = RemoteConfigService.Instance.appConfig.GetFloat("GEN_TEAM01_MaxValue");
            variables.generatorCapturePoint.secondTeamState.stabilityPoint = RemoteConfigService.Instance.appConfig.GetFloat("GEN_TEAM02_StabilityPoint");
            variables.generatorCapturePoint.secondTeamState.captureValue = RemoteConfigService.Instance.appConfig.GetFloat("GEN_TEAM02_CaptureValue");
            variables.generatorCapturePoint.secondTeamState.maxValue = RemoteConfigService.Instance.appConfig.GetFloat("GEN_TEAM02_MaxValue");
            variables.generatorCapturePoint.neutralState.stabilityPoint = RemoteConfigService.Instance.appConfig.GetFloat("GEN_NEUTRAL_StabilityPoint");
        }

        /// <summary>
        /// Set the variables of the relai
        /// </summary>
        private void SetRelaiVariables() {
            variables.relaiCapturePoint.baseViewRange = RemoteConfigService.Instance.appConfig.GetFloat("RELAI_BaseViewRange");
            variables.relaiCapturePoint.viewRange = RemoteConfigService.Instance.appConfig.GetFloat("RELAI_ViewRange");
            variables.relaiCapturePoint.capturePointSpeed = RemoteConfigService.Instance.appConfig.GetFloat("RELAI_CapturePointSpeed");
            variables.relaiCapturePoint.firstTeamState.stabilityPoint = RemoteConfigService.Instance.appConfig.GetFloat("RELAI_TEAM01_StabilityPoint");
            variables.relaiCapturePoint.firstTeamState.captureValue = RemoteConfigService.Instance.appConfig.GetFloat("RELAI_TEAM01_CaptureValue");
            variables.relaiCapturePoint.firstTeamState.maxValue = RemoteConfigService.Instance.appConfig.GetFloat("RELAI_TEAM01_MaxValue");
            variables.relaiCapturePoint.secondTeamState.stabilityPoint = RemoteConfigService.Instance.appConfig.GetFloat("RELAI_TEAM02_StabilityPoint");
            variables.relaiCapturePoint.secondTeamState.captureValue = RemoteConfigService.Instance.appConfig.GetFloat("RELAI_TEAM02_CaptureValue");
            variables.relaiCapturePoint.secondTeamState.maxValue = RemoteConfigService.Instance.appConfig.GetFloat("RELAI_TEAM02_MaxValue");
            variables.relaiCapturePoint.neutralState.stabilityPoint = RemoteConfigService.Instance.appConfig.GetFloat("RELAI_NEUTRAL_StabilityPoint");
        }
    }
}