using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class UnitZPos : MonoBehaviour
{
    void Update()
    {
        Vector3 tPos = new Vector3(transform.position.x, transform.position.y, 0);
        transform.localPosition = tPos;
    }
}
