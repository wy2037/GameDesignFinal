using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lr;
    [SerializeField]
    private Transform[] points;

    private void Start() {
        lr.positionCount = points.Length;
        this.points = points;
    }

    private void Update() {
        for (int i = 0; i < points.Length; i++) {
            lr.SetPosition(i, points[i].position);
        }
    }
}
