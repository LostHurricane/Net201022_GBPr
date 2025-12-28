using System;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    public event Action IsDragFinished;
    public bool IsDragged {get; private set;}
    private float _distance_to_screen;

    public void SetUpVariables ()
    {

    }
    private void OnMouseDrag()
    {
        Debug.Log("drag is happening");
        IsDragged = true;
        Vector3 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _distance_to_screen));
        transform.position = new Vector3(pos_move.x, pos_move.y, transform.position.z);
    }
    private void OnMouseDown()
    {
        _distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        
    }
    private void OnMouseUp()
    {
        if (IsDragged)
        {
            IsDragFinished?.Invoke();
            Debug.Log("drag is finished");
        }
        IsDragged = false;
    }
}
