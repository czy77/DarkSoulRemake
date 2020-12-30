using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("按键设置")] public KeyCode Up = KeyCode.W;
    public KeyCode Down = KeyCode.S;
    public KeyCode Left = KeyCode.A;
    public KeyCode Right = KeyCode.D;
    public KeyCode Dash = KeyCode.LeftShift;
    public KeyCode Jump = KeyCode.Space;


    [Header("暴露出来的变量")]
    //是否可以操作
    public bool CanOperation = true;

    public float VerticalAxios;
    public float HorizontalAxios;
    public float ForwardVal;
    public Vector3 FaceDirection;
    public bool IsRun;
    public bool IsJump;
    private bool lastJump;


    private float _targetVerticalVal;
    private float _targetHorizontalVal;
    private float _currentVerticalVelocity;
    private float _currentHorizontalVelocity;
    private const float SmoothTime = 0.1f;


    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _targetVerticalVal = (Input.GetKey(Up) ? 1.0f : 0.0f) - (Input.GetKey(Down) ? 1.0f : 0.0f);
        _targetHorizontalVal = (Input.GetKey(Right) ? 1.0f : 0.0f) - (Input.GetKey(Left) ? 1.0f : 0.0f);

        if (!CanOperation)
        {
            Debug.Log("????");
            _targetVerticalVal = 0;
            _targetHorizontalVal = 0;
        }

        IsRun = Input.GetKey(Dash);

        var newJump = Input.GetKey(Jump);
        IsJump = newJump != lastJump;
        lastJump = newJump;


        // Vector3.SmoothDamp()
        VerticalAxios = Mathf.SmoothDamp(VerticalAxios, _targetVerticalVal, ref _currentVerticalVelocity, SmoothTime);
        HorizontalAxios = Mathf.SmoothDamp(HorizontalAxios, _targetHorizontalVal, ref _currentHorizontalVelocity,
            SmoothTime);

        (float newX, float newY) averageAxisValue = AverageAxisValue(HorizontalAxios, VerticalAxios);

        //向前的数值和朝向
        ForwardVal = Mathf.Sqrt(averageAxisValue.newY * averageAxisValue.newY +
                                averageAxisValue.newX * averageAxisValue.newX);
        FaceDirection = averageAxisValue.newX * transform.right + averageAxisValue.newY * transform.forward;
    }


    /// <summary>
    /// 解决斜方向移动更快的问题
    /// </summary>
    /// <param name="oldX"></param>
    /// <param name="oldY"></param>
    /// <returns></returns>
    private (float, float) AverageAxisValue(float oldX, float oldY)
    {
        float newX = oldX * Mathf.Sqrt(1 - oldY * oldY / 2.0f);
        float newY = oldY * Mathf.Sqrt(1 - oldX * oldX / 2.0f);
        return (newX, newY);
    }


    /// <summary>
    /// 跳跃时禁止操作
    /// </summary>
    private void OnJump()
    {
        // Debug.Log("lock operation");
        CanOperation = false;
    }
    
    /// <summary>
    /// 退出跳跃后允许操作
    /// </summary>
    private void OnQuitJump()
    {
        // Debug.Log("lock operation");
        CanOperation = true;
    }
    
    
}