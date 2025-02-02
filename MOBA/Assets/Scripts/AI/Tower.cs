using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Capacities;
using Photon.Pun;
using UnityEngine;

public partial class Tower : Building
{
    [Space]
    [Header("Tower settings")]
    public float detectionRange;
    public List<Entity> enemiesInRange = new List<Entity>();
    public int damage;
    public float delayBeforeAttack;
    public float detectionDelay;
    public float brainSpeed;
    public float timeBewteenShots;
    public LayerMask canBeHitByTowerMask;
    public bool isCycleAttack = false;
    public string enemyUnit;
    private float brainTimer;

    protected override void OnUpdate()
    {
        // Créer des tick pour éviter le saut de frame en plus avec le multi ça risque d'arriver
        brainTimer += Time.deltaTime;
        if (brainTimer > brainSpeed)
        {
            TowerDetection();
            Debug.Log("TowerDetection() " + gameObject.name);
            brainTimer = 0;
        }
    }

    private void TowerDetection()
    {
        enemiesInRange.Clear();
        
        var size = Physics.OverlapSphere(transform.position, detectionRange, canBeHitByTowerMask);
        

        foreach (var result in size)
        {
            if (result.CompareTag(enemyUnit))
            {
                enemiesInRange.Add(result.GetComponent<Entity>());
            }
        }

        if (isCycleAttack == false && enemiesInRange.Count > 0)
        {
            StartCoroutine(AttackTarget());
        }
    }

    private IEnumerator AttackTarget()
    {
        isCycleAttack = true;
        
        yield return new WaitForSeconds(detectionDelay);
        
        int[] targetEntity = new[] { enemiesInRange[0].GetComponent<Entity>().entityIndex };
        
        AttackRPC(3, targetEntity, Array.Empty<Vector3>());
        
        yield return new WaitForSeconds(timeBewteenShots);
        isCycleAttack = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

public partial class Tower : IAttackable, IActiveLifeable, IDeadable
{
    public bool CanAttack()
    {
        throw new System.NotImplementedException();
    }

    public void RequestSetCanAttack(bool value)
    {
        throw new System.NotImplementedException();
    }

    public void SetCanAttackRPC(bool value)
    {
        throw new System.NotImplementedException();
    }

    public void SyncSetCanAttackRPC(bool value)
    {
        throw new System.NotImplementedException();
    }

    public event GlobalDelegates.OneParameterDelegate<bool> OnSetCanAttack;
    public event GlobalDelegates.OneParameterDelegate<bool> OnSetCanAttackFeedback;
    public float GetAttackDamage()
    {
        throw new System.NotImplementedException();
    }

    public void RequestSetAttackDamage(float value)
    {
        throw new System.NotImplementedException();
    }

    public void SyncSetAttackDamageRPC(float value)
    {
        throw new System.NotImplementedException();
    }

    public void SetAttackDamageRPC(float value)
    {
        throw new System.NotImplementedException();
    }

    public event GlobalDelegates.OneParameterDelegate<float> OnSetAttackDamage;
    public event GlobalDelegates.OneParameterDelegate<float> OnSetAttackDamageFeedback;
    public void RequestAttack(byte capacityIndex, int[] targetedEntities, Vector3[] targetedPositions)
    {
        throw new System.NotImplementedException();
    }

    [PunRPC]
    public void SyncAttackRPC(byte capacityIndex, int[] targetedEntities, Vector3[] targetedPositions)
    {
        var attackCapacity = CapacitySOCollectionManager.CreateActiveCapacity(capacityIndex,this);
        
        OnAttackFeedback?.Invoke(capacityIndex,targetedEntities,targetedPositions);
    }

    [PunRPC]
    public void AttackRPC(byte capacityIndex, int[] targetedEntities, Vector3[] targetedPositions)
    {
        var attackCapacity = CapacitySOCollectionManager.CreateActiveCapacity(capacityIndex,this);

        if (!attackCapacity.TryCast( targetedEntities, targetedPositions)) return;
            
        OnAttack?.Invoke(capacityIndex,targetedEntities,targetedPositions);
        photonView.RPC("SyncAttackRPC",RpcTarget.All,capacityIndex,targetedEntities,targetedPositions);
    }

    public event GlobalDelegates.ThirdParameterDelegate<byte , int[] , Vector3[]> OnAttack;
    public event GlobalDelegates.ThirdParameterDelegate<byte , int[] , Vector3[]> OnAttackFeedback;
    public float GetMaxHp()
    {
        throw new NotImplementedException();
    }

    public float GetCurrentHp()
    {
        throw new NotImplementedException();
    }

    public float GetCurrentHpPercent()
    {
        throw new NotImplementedException();
    }

    public bool GetCanDecreaseCurrentHp()
    {
        throw new NotImplementedException();
    }

    public void RequestSetCanDecreaseCurrentHp(bool value)
    {
        throw new NotImplementedException();
    }

    public void SyncSetCanDecreaseCurrentHpRPC(bool value)
    {
        throw new NotImplementedException();
    }

    public void RequestSetMaxHp(float value)
    {
        throw new NotImplementedException();
    }

    public void SyncSetMaxHpRPC(float value)
    {
        throw new NotImplementedException();
    }

    public void SetMaxHpRPC(float value)
    {
        throw new NotImplementedException();
    }

    public event GlobalDelegates.OneParameterDelegate<float> OnSetMaxHp;
    public event GlobalDelegates.OneParameterDelegate<float> OnSetMaxHpFeedback;
    public void RequestIncreaseMaxHp(float amount)
    {
        throw new NotImplementedException();
    }

    public void SyncIncreaseMaxHpRPC(float amount)
    {
        throw new NotImplementedException();
    }

    public void IncreaseMaxHpRPC(float amount)
    {
        throw new NotImplementedException();
    }

    public event GlobalDelegates.OneParameterDelegate<float> OnIncreaseMaxHp;
    public event GlobalDelegates.OneParameterDelegate<float> OnIncreaseMaxHpFeedback;
    public void RequestDecreaseMaxHp(float amount)
    {
        throw new NotImplementedException();
    }

    public void SyncDecreaseMaxHpRPC(float amount)
    {
        throw new NotImplementedException();
    }

    public void DecreaseMaxHpRPC(float amount)
    {
        throw new NotImplementedException();
    }

    public event GlobalDelegates.OneParameterDelegate<float> OnDecreaseMaxHp;
    public event GlobalDelegates.OneParameterDelegate<float> OnDecreaseMaxHpFeedback;
    public void RequestSetCurrentHp(float value)
    {
        throw new NotImplementedException();
    }

    public void SyncSetCurrentHpRPC(float value)
    {
        throw new NotImplementedException();
    }

    public void SetCurrentHpRPC(float value)
    {
        throw new NotImplementedException();
    }

    public event GlobalDelegates.OneParameterDelegate<float> OnSetCurrentHp;
    public event GlobalDelegates.OneParameterDelegate<float> OnSetCurrentHpFeedback;
    public void RequestSetCurrentHpPercent(float value)
    {
        throw new NotImplementedException();
    }

    public void SyncSetCurrentHpPercentRPC(float value)
    {
        throw new NotImplementedException();
    }

    public void SetCurrentHpPercentRPC(float value)
    {
        throw new NotImplementedException();
    }

    public event GlobalDelegates.OneParameterDelegate<float> OnSetCurrentHpPercent;
    public event GlobalDelegates.OneParameterDelegate<float> OnSetCurrentHpPercentFeedback;
    public void RequestIncreaseCurrentHp(float amount)
    {
        throw new NotImplementedException();
    }

    public void SyncIncreaseCurrentHpRPC(float amount)
    {
        throw new NotImplementedException();
    }

    public void IncreaseCurrentHpRPC(float amount)
    {
        throw new NotImplementedException();
    }

    public event GlobalDelegates.OneParameterDelegate<float> OnIncreaseCurrentHp;
    public event GlobalDelegates.OneParameterDelegate<float> OnIncreaseCurrentHpFeedback;
    
    public void RequestDecreaseCurrentHp(float amount)
    {
        photonView.RPC("DecreaseCurrentHpRPC", RpcTarget.MasterClient, amount);
    }
    
    [PunRPC]
    public void SyncDecreaseCurrentHpRPC(float amount)
    {
        currentHealth = amount;
    }

    [PunRPC]
    public void DecreaseCurrentHpRPC(float amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;
        
        photonView.RPC("SyncDecreaseCurrentHpRPC", RpcTarget.All, currentHealth);
        
        if (currentHealth <= 0 && isAlive)
        {
            RequestDie();
            isAlive = false;
        }
    }

    public event GlobalDelegates.OneParameterDelegate<float> OnDecreaseCurrentHp;
    public event GlobalDelegates.OneParameterDelegate<float> OnDecreaseCurrentHpFeedback;
    public bool IsAlive()
    {
        throw new NotImplementedException();
    }

    public bool CanDie()
    {
        throw new NotImplementedException();
    }

    public void RequestSetCanDie(bool value)
    {
        throw new NotImplementedException();
    }

    public void SyncSetCanDieRPC(bool value)
    {
        throw new NotImplementedException();
    }

    public void SetCanDieRPC(bool value)
    {
        throw new NotImplementedException();
    }

    public event GlobalDelegates.OneParameterDelegate<bool> OnSetCanDie;
    public event GlobalDelegates.OneParameterDelegate<bool> OnSetCanDieFeedback;
    
    
    public void RequestDie()
    {
        photonView.RPC("DieRPC", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void SyncDieRPC()
    {
        isAlive = false;
        Destroy(gameObject);
    }

    [PunRPC]
    public void DieRPC()
    {
        photonView.RPC("SyncDieRPC", RpcTarget.All);
    }

    public event GlobalDelegates.NoParameterDelegate OnDie;
    public event GlobalDelegates.NoParameterDelegate OnDieFeedback;
    public void RequestRevive()
    {
        throw new NotImplementedException();
    }

    public void SyncReviveRPC()
    {
        throw new NotImplementedException();
    }

    public void ReviveRPC()
    {
        throw new NotImplementedException();
    }

    public event GlobalDelegates.NoParameterDelegate OnRevive;
    public event GlobalDelegates.NoParameterDelegate OnReviveFeedback;
}
