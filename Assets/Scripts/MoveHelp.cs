using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHelp : MonoBehaviour
{
    [SerializeField] PlayerMover script;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveTrue()
    {
        script.SetMoveTrue();
    }

    public void MoveFalse()
    {
        script.SetMoveFalse();
    }
}
