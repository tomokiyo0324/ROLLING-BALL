using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public float rotationSpeed = 50f; // ��]�̑��x
    //public Transform player; // �v���C���[��Transform


    void Start()
    {

    }

    void Update()
    {
        {
            // ���E�̃L�[���͂����o���ĉ�]������
            float horizontalInput = Input.GetAxis("Horizontal");
            transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
        }

        //// ���E�L�[�̓��͂��擾
        //float horizontalInput = Input.GetAxis("Horizontal");

        //// ���E�L�[�������ꂽ��J��������]������
        //if (horizontalInput != 0)
        //{
        //    // ���������̉�]�ʂ��v�Z
        //    float rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;

        //    // �J��������]������
        //    transform.Rotate(Vector3.up, rotationAmount);

        //    // �v���C���[�̈ʒu����]�ɍ��킹�ĕύX����
        //    RotatePlayer(rotationAmount);
    }
}

    //void RotatePlayer(float rotationAmount)
    //{
    //    // �v���C���[�̈ʒu���X�e�[�W�̉�]�ɍ��킹�ĕύX����
    //    player.RotateAround(transform.position, Vector3.up, rotationAmount);
    //}
