using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitable
{
    void Hit(RaycastHit hit, int damage = 1);
}
