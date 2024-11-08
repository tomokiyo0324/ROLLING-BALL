using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Ball")]
    private GameObject target;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        // �Q�[���J�n���_�̃J�����ƃ^�[�Q�b�g�̋����i�I�t�Z�b�g�j���擾
        offset = gameObject.transform.position - target.transform.position;
    }

    /// <summary>
    /// �v���C���[���ړ�������ɃJ�������ړ�����悤�ɂ��邽�߂�LateUpdate�ɂ���B
    /// </summary>
    void LateUpdate()
    {
        // �J�����̈ʒu���^�[�Q�b�g�̈ʒu�ɃI�t�Z�b�g�𑫂����ꏊ�ɂ���B
        gameObject.transform.position = target.transform.position + offset;
    }
}