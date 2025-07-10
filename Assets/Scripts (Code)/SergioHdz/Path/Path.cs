using UnityEngine;
using System.Collections.Generic;

public class Path : MonoBehaviour
{
    [Header("Lista de puntos del recorrido (se toma del orden en jerarquía)")]
    public List<Transform> waypoints = new List<Transform>();

    private void Awake()
    {
        waypoints.Clear();
        foreach (Transform child in transform)
        {
            waypoints.Add(child);
        }
    }

    public List<Transform> GetWaypoints()
    {
        return waypoints;
    }
}
