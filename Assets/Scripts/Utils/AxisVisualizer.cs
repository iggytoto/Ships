using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisVisualizer : MonoBehaviour
{
    public float axisLength = 1f;
    public float lineWidth = 0.02f;
    public Color xAxisColor = Color.red;
    public Color yAxisColor = Color.green;
    public Color zAxisColor = Color.blue;

    private void OnDrawGizmos()
    {
        Vector3 position = transform.position;

        // Отрисовка оси X
        Gizmos.color = xAxisColor;
        Gizmos.DrawLine(position, position + transform.right * axisLength);

        // Отрисовка оси Y
        Gizmos.color = yAxisColor;
        Gizmos.DrawLine(position, position + transform.up * axisLength);

        // Отрисовка оси Z
        Gizmos.color = zAxisColor;
        Gizmos.DrawLine(position, position + transform.forward * axisLength);

        // Отрисовка линий для улучшения видимости
        Gizmos.color = Color.white;
        Gizmos.DrawLine(position + transform.up * axisLength, position + transform.up * axisLength + transform.right * lineWidth);
        Gizmos.DrawLine(position + transform.up * axisLength, position + transform.up * axisLength - transform.right * lineWidth);
        Gizmos.DrawLine(position + transform.forward * axisLength, position + transform.forward * axisLength + transform.right * lineWidth);
        Gizmos.DrawLine(position + transform.forward * axisLength, position + transform.forward * axisLength - transform.right * lineWidth);
        Gizmos.DrawLine(position + transform.right * axisLength, position + transform.right * axisLength + transform.up * lineWidth);
        Gizmos.DrawLine(position + transform.right * axisLength, position + transform.right * axisLength - transform.up * lineWidth);
    }
}
