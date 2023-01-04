using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class UnitZPos : MonoBehaviour
{
    void Update()
    {
        Vector3 tPos = new Vector3(transform.position.x, transform.position.y, transform.position.y * 1.1f);
        transform.localPosition = tPos;
    }
}
