using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimation : MonoBehaviour
{
    private Animator _animator;// Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public void NPCRunAnimation()
    {
        _animator.SetBool("run", true);
        _animator.SetBool("quiet", false);
    }

    public void NPCStayAnimation()
    {
        _animator.SetBool("run", false);
        _animator.SetBool("quiet", true);
    }

    public void NPCUpgradeAnimation(int grade)
    {
        _animator.SetInteger("upgrade", grade);
    }
    private float GetCurrentAnimationLength()
    {
        float animationLength = _animator.GetCurrentAnimatorStateInfo(0).length;
        return animationLength;
    }

}
