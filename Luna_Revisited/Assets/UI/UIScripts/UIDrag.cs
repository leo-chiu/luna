using UnityEngine;
using System.Collections;

public class UIDrag : MonoBehaviour
{
    private float offsetX;
    private float offsetY;

    public void BeginDrag()
    {
        offsetX = transform.position.x - Input.mousePosition.x;
        offsetY = transform.position.y - Input.mousePosition.y;
    }

    public void OnDrag()
    {
      if(offsetX +Input.mousePosition.x < Screen.width - 100 && offsetX + Input.mousePosition.x > 0 && offsetY + Input.mousePosition.y < Screen.height && offsetY + Input.mousePosition.y > 0)
            transform.position = new Vector3(offsetX + Input.mousePosition.x, offsetY + Input.mousePosition.y);
    }
}