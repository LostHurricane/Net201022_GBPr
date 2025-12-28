using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBlockView : MonoBehaviour
{
    [SerializeField]
    private CircleCollider2D _collider;

    public CircleCollider2D Collider { get => _collider; }
}
