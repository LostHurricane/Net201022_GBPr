using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridComponent : MonoBehaviour, IInitializable
{
    [SerializeField]
    private float _cellBorder;
    
    [SerializeField] 
    private GameObject _cellObject;

    private List<GameObject> _cells ;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        generateCells();
    }

    private void generateCells()
    {
        _cells = new List<GameObject>(100);
        var cellSprite = _cellObject.GetComponent<SpriteRenderer>();
        var cellsize = cellSprite.bounds;
        Vector2 center = new Vector2((float)(4.5 * (cellsize.size.x + _cellBorder)), (float)(4.5 * (cellsize.size.y + _cellBorder)));
        for (var i = 0; i < 10; i++)
        {
            for (var j = 0; j < 10; j++)
            {
                var x = (i * (cellsize.size.x + _cellBorder)) - center.x;
                var y = (j * (cellsize.size.y + _cellBorder)) - center.y;
                var cell = Instantiate(_cellObject, new Vector3(x, y), Quaternion.identity, transform);
                var component = cell.GetComponent<CellComponent>();
                if (component)
                {
                    component.Configure(new Vector2Int(i, j));
                }
                _cells.Add(cell);
            }
        }

    }
}
