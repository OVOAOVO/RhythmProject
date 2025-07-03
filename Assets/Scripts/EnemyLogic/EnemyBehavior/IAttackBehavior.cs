using System.Collections;
using UnityEngine;

public interface IAttackBehavior
{
    IEnumerator Attack(Enemy enemy, Transform target);
}
