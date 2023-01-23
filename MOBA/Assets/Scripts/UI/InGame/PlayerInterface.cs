using System;
using GameStates;
using RessourceProduction;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInterface : MonoBehaviour
{
    [Header("Player Health")] [SerializeField]
    private Image healthBar = null;

    [SerializeField] private Image allyHealthBar = null;

    [Header("Player Image")] [SerializeField]
    private Image playerCharacterImage = null;

    [Header("Spell")] [SerializeField] private Image autoAttackImage = null;
    [SerializeField] private Image spell01Image = null;
    [SerializeField] private Image spell01RecastImage = null;
    [SerializeField] private Image spell01Cooldown = null;
    [SerializeField] private TextMeshProUGUI spell01CooldownTxt = null;
    [SerializeField] private Image spell02Image = null;
    [SerializeField] private Image spell02Cooldown = null;
    [SerializeField] private TextMeshProUGUI spell02CooldownTxt = null;
    [SerializeField] private Image wardImage = null;
    [SerializeField] private Image wardCooldown = null;
    [SerializeField] private TextMeshProUGUI wardCooldownTxt = null;

    [Header("Gold")] [SerializeField] private TextMeshProUGUI team01GoldValue = null;
    [SerializeField] private TextMeshProUGUI team02GoldValue = null;
    [SerializeField] private TextMeshProUGUI team01StreakValue = null;
    [SerializeField] private TextMeshProUGUI team02StreakValue = null;
    [SerializeField] private TextMeshProUGUI team01GoldStreak = null;
    [SerializeField] private TextMeshProUGUI team02GoldStreak = null;

    [Header("Victory")] [SerializeField] private Image team01VictoryPoint = null;
    [SerializeField] private Image team02VictoryPoint = null;

    [Header("Aura")] [SerializeField] private TextMeshProUGUI AADamageText = null;
    [SerializeField] private Button AADamagebtn = null;
    [SerializeField] private TextMeshProUGUI COMP01DamageText = null;
    [SerializeField] private Button COMP01Damagebtn = null;
    [SerializeField] private TextMeshProUGUI LifePointText = null;
    [SerializeField] private Button LifePointbtn = null;

    /// <summary>
    /// Update the visual of the Health Bar
    /// </summary>
    /// <param name="currentHealth"></param>
    /// <param name="maxHealth"></param>
    public void UpdateHealth(float currentHealth, float maxHealth) => healthBar.fillAmount = currentHealth / maxHealth;

    public void UpdateAllyHealth(float currentHealth, float maxHealth) =>
        allyHealthBar.fillAmount = currentHealth / maxHealth;

    /// <summary>
    /// Update the visual of a player Interface Element
    /// </summary>
    /// <param name="imageType"></param>
    /// <param name="imageSprite"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void SetUpImageVisual(PlayerUIImage imageType, Sprite imageSprite)
    {
        switch (imageType)
        {
            case PlayerUIImage.PlayerCharacter:
                if (playerCharacterImage == null) return;
                playerCharacterImage.sprite = imageSprite;
          
                break;

            case PlayerUIImage.AutoAttack:
                if (autoAttackImage == null) return;
                autoAttackImage.sprite = imageSprite;
                break;

            case PlayerUIImage.Spell01:
                if (spell01Image == null) return;
                spell01Image.sprite = imageSprite;
                spell01CooldownTxt.enabled = false;
                spell01Cooldown.fillAmount = 0;
                break;

            case PlayerUIImage.Spell02:
                if (spell02Image == null) return;
                spell02Image.sprite = imageSprite;
                spell02CooldownTxt.enabled = false;
                spell02Cooldown.fillAmount = 0;
                break;

            case PlayerUIImage.Ward:
                if (wardImage == null) return;
                wardImage.sprite = imageSprite;
                wardCooldownTxt.enabled = false;
                wardCooldown.fillAmount = 0;
                break;

            default: throw new ArgumentOutOfRangeException(nameof(imageType), imageType, null);
        }
    }

    /// <summary>
    /// Update the timer of the spells
    /// </summary>
    /// <param name="spellType"></param>
    /// <param name="cooldownValue"></param>
    /// <param name="maxCooldownValue"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void UpdateSpellTimer(PlayerUIImage spellType, float cooldownValue, float maxCooldownValue)
    {
        switch (spellType)
        {
            case PlayerUIImage.PlayerCharacter: break;

            case PlayerUIImage.AutoAttack: break;
            
            case PlayerUIImage.Spell01:
                spell01CooldownTxt.enabled = true;
                spell01Cooldown.fillAmount = 1-cooldownValue / maxCooldownValue;
                spell01CooldownTxt.text = (Mathf.Round((maxCooldownValue-cooldownValue) * 10f) / 10f).ToString();
                if (cooldownValue == 0)
                {
                    spell01Cooldown.fillAmount = 0;
                    spell01CooldownTxt.enabled = false;
                }
                break;
            
            case PlayerUIImage.Spell01Recast:
                spell01RecastImage.fillAmount = 1-cooldownValue / maxCooldownValue;
                spell01Cooldown.fillAmount = 0;
                if (cooldownValue == 0) {
                    spell01RecastImage.fillAmount = 0;
                }
                break;
            
            case PlayerUIImage.Spell02:
                spell02CooldownTxt.enabled = true;
                spell02Cooldown.fillAmount = 1-cooldownValue / maxCooldownValue;
                spell02CooldownTxt.text = (Mathf.Round((maxCooldownValue-cooldownValue) * 10f) / 10f).ToString();
                if (cooldownValue == 0)
                {
                    spell02Cooldown.fillAmount = 0;
                    spell02CooldownTxt.enabled = false;
                }
                break;

            case PlayerUIImage.Ward:
                wardCooldownTxt.enabled = true;
                wardCooldown.fillAmount = 1-cooldownValue / maxCooldownValue;
                wardCooldownTxt.text = (Mathf.Round((maxCooldownValue-cooldownValue) * 10f) / 10f).ToString();
                if (cooldownValue == 0)
                {
                    wardCooldown.fillAmount = 0;
                    wardCooldownTxt.enabled = false;
                }
                break;

            default: throw new ArgumentOutOfRangeException(nameof(spellType), spellType, null);
        }
    }

    /// <summary>
    /// Change the state of the aura (enable, disable)
    /// </summary>
    /// <param name="isActiv"></param>
    public void ChangeAuraState(bool isActiv)
    {
        AADamagebtn.interactable = isActiv;
        COMP01Damagebtn.interactable = isActiv;
        LifePointbtn.interactable = isActiv;
    }

    /// <summary>
    /// Update the gold value of each team
    /// </summary>
    /// <param name="value"></param>
    public void UpdateGoldTeam01(int value) => team01GoldValue.text = $"{value} gold";

    public void UpdateGoldTeam02(int value) => team02GoldValue.text = $"{value} gold";

    /// <summary>
    /// Update the Streak of each team
    /// </summary>
    /// <param name="value"></param>
    public void UpdateStreakTeam01(int value) => team01StreakValue.text = $"Streak : {value}";

    public void UpdateStreakTeam02(int value) => team02StreakValue.text = $"Streak : {value}";

    /// <summary>
    /// Update the Streak gold value of each team
    /// </summary>
    /// <param name="value"></param>
    public void UpdateGoldStreakTeam01(int value) => team01GoldStreak.text = value.ToString();

    public void UpdateGoldStreakTeam02(int value) => team02GoldStreak.text = value.ToString();

    /// <summary>
    /// Update the victory point of each team
    /// </summary>
    /// <param name="currentValue"></param>
    /// <param name="maxValue"></param>
    public void UpdateVictoryTeam01(float currentValue,float maxValue) =>
        team01VictoryPoint.fillAmount = currentValue / maxValue;

    public void UpdateVictoryTeam02(float currentValue,float maxValue) =>
        team02VictoryPoint.fillAmount = currentValue / maxValue;

    public void SetColorVictoryTeam01() =>
        team01VictoryPoint.color = GameStateMachine.Instance.GetTeamColor(Enums.Team.Team1);

    public void SetColorVictoryTeam02() =>
        team02VictoryPoint.color = GameStateMachine.Instance.GetTeamColor(Enums.Team.Team2);
    
    public void UpdateAuraValue(int auraValue,AuraUIImage auraUIImage)
    {
        switch (auraUIImage)
        {
            case AuraUIImage.Comp01Damage :
            {
                COMP01DamageText.text = auraValue.ToString();
                break;
            }
            case AuraUIImage.AADamage :
            {
                AADamageText.text = auraValue.ToString();
                break;
            }
            case AuraUIImage.LifePoint :
            {
                LifePointText.text = auraValue.ToString();
                break;
            }
        }
    }
    public void SetUpAuraSprite(int auraValue, Sprite sprite, AuraUIImage auraUIImage,GlobalDelegates.NoParameterDelegate capacity)
    {
       UpdateAuraValue(auraValue, auraUIImage);
       switch (auraUIImage)
       {
           case AuraUIImage.Comp01Damage :
           {
               COMP01Damagebtn.image.sprite = sprite;
               COMP01Damagebtn.onClick.AddListener(new UnityAction(capacity));
               break;
           }
           case AuraUIImage.AADamage :
           { AADamagebtn.image.sprite = sprite;
              AADamagebtn.onClick.AddListener(new UnityAction(capacity));
               break;
           }
           case AuraUIImage.LifePoint :
           {
               LifePointbtn.image.sprite = sprite;
              LifePointbtn.onClick.AddListener(new UnityAction(capacity));
               break;
           }
       }
    }
}

/// <summary>
/// Player Interface Element
/// </summary>
public enum PlayerUIImage
{
    PlayerCharacter,
    AutoAttack,
    Spell01,
    Spell01Recast,
    Spell02,
    Ward
}

public enum AuraUIImage
{
    Comp01Damage, LifePoint, AADamage
}
