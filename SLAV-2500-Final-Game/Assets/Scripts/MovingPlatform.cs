using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public List<Transform> points;
    public Transform platform;
    int goalPoints = 0;
    public float moveSpeed = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveToNextPoint();
    }

    void MoveToNextPoint() {
        platform.position = Vector2.MoveTowards(platform.position, points[goalPoints].position, Time.deltaTime*moveSpeed);
        if(Vector2.Distance(platform.position, points[goalPoints].position)<0.1f) {
            if(goalPoints == points.Count - 1) {
                goalPoints = 0;
            } else {
                goalPoints++;
            }
        }
    }
}
