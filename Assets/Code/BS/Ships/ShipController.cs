using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public bool Locked { get; private set; }

    [SerializeField]
    private ShipSize _shipSize;
    [SerializeField]
    private Transform _center;

    private Vector2Int [] _coordinate;
    private Vector2Int [] _damagedParts;
    
    private ShipBlockView [] _shipParts;

    public void LockShip ()
    {

    }

    void Awake()
    {
        _shipParts = this.transform.GetComponentsInChildren<ShipBlockView>();
        ArrangeShipParts();
    }

    private void ArrangeShipParts()
    {
        var offset = -(_shipParts[0].Collider.radius * (_shipParts.Length - 1));//some mistake
        for (int i = 0; i < _shipParts.Length; i++)
        {
            _shipParts[i].transform.localPosition = Vector3.zero;
            _shipParts[i].transform.localPosition = new Vector3() { x = offset, y = _shipParts[i].transform.localPosition.y };
            offset += _shipParts[i].Collider.radius * 2;
        }
    }

    public bool Hitcheck(Vector2Int coordinate)
    {
        if (!Locked) { throw new Exception("Attempt to hit ship off lock"); }

        if (_damagedParts.Contains(coordinate))
        {
            Debug.Log($"Hit! Damaged Part");
            return false;
        }
        int i;
        for (i = 0; i < _coordinate.Length; i++)
        {
            if (_coordinate[i] == coordinate)
            {
                Debug.Log($"Hit! {_coordinate.ToString()}");
                _damagedParts[i] = _coordinate[i];
                return true;
            }
        }
        return false;
    }
    public enum ShipSize
    {
        One = 1, Two = 2, Three = 3, Four = 4
    }
}


