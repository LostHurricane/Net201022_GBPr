using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseClicksHandler : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    //[SerializeField] private SelectableValue _selectedObject;
    [SerializeField] private EventSystem _event;

    [SerializeField] private Transform _gridTransform;

    private void Start()
    {

    }

    private void Update()
    {
        if (_event.IsPointerOverGameObject())
        {
            Debug.Log($"IsPointerOverGameObject");
            return;

        }

        Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        var hits = Physics2D.RaycastAll(mousePos, Vector2.zero);
        if (Input.GetMouseButtonUp(0))
        {
            if (WeHit<SpriteRenderer>(hits, out var selectable))
            {
                if (selectable.TryGetComponent<CellComponent>(out var cell))
                {
                    Debug.Log($"coordinates is {cell.Coordinate.ToString()}");
                }
            }
        }

        else
        {
            //if (WeHit<IAttackable>(hits, out var attackable))
            //{
            //    _attackablesRMB.SetValue(attackable);
            //}
            //else if (_groundPlane.Raycast(ray, out var enter))
            //{
            //    _groundClicksRMB.SetValue(ray.origin + ray.direction * enter);
            //}

        }
    }


    private bool WeHit<T>(RaycastHit2D[] hits, out T result) where T : class
    {
        result = default;
        if (hits.Length == 0)
        {
            return false;
        }
        result = hits
        .Select(hit => hit.collider.GetComponentInParent<T>())
        .Where(c => c != null)
        .FirstOrDefault();
        return result != default;
    }
}
