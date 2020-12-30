using System;
using System.Collections;
using System.Collections.Generic;
using constants;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    public GameObject model;
    public float MoveSpeed = 1.5f;
    public float RunMultiple = 2.0f;


    private PlayerInput _playerInput;
    private Animator _animator;
    private Vector3 MoveVec;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _animator = model.GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat(AnimationConstants.Forwards,
            _playerInput.ForwardVal * Mathf.Lerp(_animator.GetFloat(AnimationConstants.Forwards),
                _playerInput.IsRun ? 2.0f : 1.0f, 0.25f));
        
        //转身
        if (_playerInput.ForwardVal > 0.1f)
        {
            //使用Vector3.Slerp() 使转身动作速度更加均匀
            model.transform.forward = Vector3.Slerp(model.transform.forward, _playerInput.FaceDirection, 0.25f);
        }

        if (_playerInput.IsJump)
        {
            _animator.SetTrigger(AnimationConstants.Jump);
        }

        MoveVec = _playerInput.ForwardVal * RunMultiple * model.transform.forward;
    }

    private void FixedUpdate()
    {
        //1.可以通过改变rigidbody.position 来使物体移动 ,此时要 乘上 Time.fixedDeltaTime;
        _rigidbody.position += Time.fixedDeltaTime * MoveSpeed * MoveVec;

        //2.也可以改变rigidbody.velocity 来使物体移动, 此时不需要乘 Time.fixedDeltaTime;
        //_rigidbody.velocity = new Vector3(newVelocity.x,newVelocity.y,newVelocity.z)
    }
}