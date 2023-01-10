using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Entities.FogOfWar
{
public class EntityFogOfWarColliderLinker : MonoBehaviour
{
    [SerializeField] private Entity entity;
    [SerializeField] private Collider collider;

    public Entity GetEntity
    {
        get
        {
            Debug.Log("fx"+entity.name);
            return entity;
            
            
        }
    } 

    public Collider GetCollider => collider;
}
}
