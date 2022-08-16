using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowForCamera : MonoBehaviour
{
    [SerializeField] Transform follow;
    [SerializeField] float smooth;

    private void Start()
    {
        follow = GameObject.Find("Player-Dunco").transform;
        transform.position = follow.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, follow.position, smooth * Time.deltaTime);
        //transform.position = Vector3.MoveTowards(transform.position, follow.position, smooth * Time.deltaTime);
        //Debug.Log("target: " + follow.position);
    }
}
