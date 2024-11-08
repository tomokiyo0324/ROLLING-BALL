using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public float rotationSpeed = 50f; // 回転の速度
    //public Transform player; // プレイヤーのTransform


    void Start()
    {

    }

    void Update()
    {
        {
            // 左右のキー入力を検出して回転させる
            float horizontalInput = Input.GetAxis("Horizontal");
            transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
        }

        //// 左右キーの入力を取得
        //float horizontalInput = Input.GetAxis("Horizontal");

        //// 左右キーが押されたらカメラを回転させる
        //if (horizontalInput != 0)
        //{
        //    // 水平方向の回転量を計算
        //    float rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;

        //    // カメラを回転させる
        //    transform.Rotate(Vector3.up, rotationAmount);

        //    // プレイヤーの位置も回転に合わせて変更する
        //    RotatePlayer(rotationAmount);
    }
}

    //void RotatePlayer(float rotationAmount)
    //{
    //    // プレイヤーの位置をステージの回転に合わせて変更する
    //    player.RotateAround(transform.position, Vector3.up, rotationAmount);
    //}
