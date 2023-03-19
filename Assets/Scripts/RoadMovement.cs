using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadMovement : MonoBehaviour
{
    public float speed = 5f; 
    public Transform[] roads;
    public bool allRoadsArrived = false;

    private Vector3[] targetPositions; 

    private void Start() {
        targetPositions = new Vector3[roads.Length];

        for (int i = 0; i < roads.Length; i++) {
            targetPositions[i] = roads[i].position;
            roads[i].position += new Vector3(Random.Range(0,3), Random.Range(3, 7), Random.Range(0, 3))*50f; 
        }
    }

    private void Update() {
        Movement();
    }
    void Movement() {
        for (int i = 0; i < roads.Length; i++) {
            roads[i].position = Vector3.Lerp(roads[i].position, targetPositions[i], speed * Time.deltaTime);

            if (Vector3.Distance(roads[i].position, targetPositions[i]) <0.2f) {
                allRoadsArrived = true;
            }
        }
      
    }
}
