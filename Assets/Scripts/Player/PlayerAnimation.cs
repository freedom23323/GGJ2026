using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayRunAnimation()
    {
        _animator.SetBool("run", true);
        _animator.SetBool("quiet", false);
    }

    public void PlayStayAnimation()
    {
        _animator.SetBool("run", false);
        _animator.SetBool("quiet", true);
    }

    public void PlayFrontAnimation()
    {
        _animator.SetBool("front", true);
        _animator.SetBool("back", false);
    }

    public void PlayBackAnimation()
    {
        _animator.SetBool("back", true);
        _animator.SetBool("front", false);
    }
}
