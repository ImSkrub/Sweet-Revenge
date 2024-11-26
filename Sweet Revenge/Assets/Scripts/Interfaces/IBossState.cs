using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossState
{
    void Enter(Boss boss);
    void UpdateState();
    void Exit();
    float GetKnockbackForce();
    float GetRange();
    float GetDamage();
}
