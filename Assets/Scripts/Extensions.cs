using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    private static LayerMask layerMask = LayerMask.GetMask("Default");
    // 扩展方法，使用this关键字表明将该方法添加到那个类中
    public static bool Raycast(this Rigidbody2D rigidbody,Vector2 direction)
    {
        if (rigidbody.isKinematic)
        {
            return false;
        }

        float radius = 0.25f;
        float distance = 0.375f;

        RaycastHit2D hit =  Physics2D.CircleCast(rigidbody.position, radius, direction.normalized, distance, layerMask);  
        return hit.collider != null && hit.rigidbody != rigidbody;
    }

    public static bool DotTest(this Transform transform,Transform other,Vector2 testDirection)
    {
        // 计算角色和方块之间的向量
        Vector2 direction = other.position - transform.position;
        return Vector2.Dot(direction.normalized, testDirection) > 0.25f;
    }
}
