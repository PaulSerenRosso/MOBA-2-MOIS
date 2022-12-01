using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAimable
{

  public float GetMaxRange();
  
  public float GetSqrtMaxRange();
  public bool  TryAim(int casterIndex, int targetsEntityIndex, Vector3 targetPosition);
}
