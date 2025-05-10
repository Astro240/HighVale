using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeMechanic : MonoBehaviour
{
    private Animator mAnimator;
    void Start()
    {
        mAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (mAnimator != null) {
            //if (Input.GetKeyDown(KeyCode.C)) {
                //mAnimator.SetTrigger("Dodge");
            //}
        //}
    }
}
