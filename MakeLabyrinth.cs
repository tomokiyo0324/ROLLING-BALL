using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MakeLabyrinth : MonoBehaviour
{
    // �ݒ肷��l
    public int max = 41;       //�����l�Ƃ��ẴT�C�Y
    public GameObject wall;    //�Ǘp�I�u�W�F�N�g
    public GameObject floor;   //���p�I�u�W�F�N�g
    public GameObject start;   //�X�^�[�g�n�_�ɔz�u����I�u�W�F�N�g
    public GameObject goal;    //�S�[���n�_�ɔz�u����I�u�W�F�N�g

    // �����p�����[�^
    private enum CellType { Wall, Path };   //�Z���̎��
    private CellType[,] cells;

    private Vector2Int startPos;    //�X�^�[�g�̍��W
    private Vector2Int goalPos;     //�S�[���̍��W


    private void Start()
    {
        //�}�b�v��ԏ�����
        cells = new CellType[max, max];

        //�X�^�[�g�n�_�̎擾
        startPos = GetStartPosition();

        //�ʘH�̐���
        //����̓S�[���n�_��ݒ肷��
        goalPos = MakeMapInfo(startPos);

        //�ʘH�������J��Ԃ��đ܏��H�����炷
        var tmpStart = goalPos;
        for (int i = 0; i < max * 41; i++)
        {
            MakeMapInfo(tmpStart);
            tmpStart = GetStartPosition();
        }

        //�}�b�v�̏�Ԃɉ����ĕǂƒʘH�𐶐�����
        BuildDungeon();

        //�X�^�[�g�n�_�ƃS�[���n�_�ɃI�u�W�F�N�g��z�u����
        //����Ŏ擾�����X�^�[�g�n�_�ƃS�[���n�_�͕K���Ȃ����Ă���̂Ŕj�]���Ȃ�
        var startObj = Instantiate(start, new Vector3(startPos.x, 0, startPos.y), Quaternion.identity);
        var goalObj = Instantiate(goal, new Vector3(goalPos.x, 0, goalPos.y), Quaternion.identity);

        startObj.transform.parent = this.transform;
        goalObj.transform.parent = this.transform;

    }


    // �X�^�[�g�n�_�̎擾
    private Vector2Int GetStartPosition()
    {
        //�����_����x,y��ݒ�
        int randomX = Random.Range(0, max);
        int randomY = Random.Range(0, max);

        //x�Ay�������������ɂȂ�܂ŌJ��Ԃ�
        while (!(randomX % 2 == 0 && randomY % 2 == 0))
        {
            randomX = Mathf.RoundToInt(Random.Range(0, max));
            randomY = Mathf.RoundToInt(Random.Range(0, max));
        }

        return new Vector2Int(randomX, randomY);
    }


    // �}�b�v����
    private Vector2Int MakeMapInfo(Vector2Int _startPos)
    {
        //�X�^�[�g�ʒu�z��𕡐�
        var tmpStartPos = _startPos;

        //�ړ��\�ȍ��W�̃��X�g���擾
        var movablePositions = GetMovablePositions(tmpStartPos);

        //�ړ��\�ȍ��W���Ȃ��Ȃ�܂ŒT�����J��Ԃ�
        while (movablePositions != null)
        {
            //�ړ��\�ȍ��W���烉���_����1�擾���ʘH�ɂ���
            var tmpPos = movablePositions[Random.Range(0, movablePositions.Count)];
            cells[tmpPos.x, tmpPos.y] = CellType.Path;

            //���̒n�_�ƒʘH�ɂ������W�̊Ԃ�ʘH�ɂ���
            var xPos = tmpPos.x + (tmpStartPos.x - tmpPos.x) / 2;
            var yPos = tmpPos.y + (tmpStartPos.y - tmpPos.y) / 2;
            cells[xPos, yPos] = CellType.Path;

            //�ړ���̍��W���ꎞ�ϐ��Ɋi�[���A�ēx�ړ��\�ȍ��W��T������
            tmpStartPos = tmpPos;
            movablePositions = GetMovablePositions(tmpStartPos);
        }

        //�T���I�����̍��W��Ԃ�
        return tmpStartPos;
    }


    // �ړ��\�ȍ��W�̃��X�g���擾����
    private List<Vector2Int> GetMovablePositions(Vector2Int _startPos)
    {
        //�ǐ��̂��ߍ��W��ϐ��Ɋi�[
        var x = _startPos.x;
        var y = _startPos.y;

        //�ړ���������2���x,y���W�����v�Z
        var positions = new List<Vector2Int> {
            new Vector2Int(x, y + 2),
            new Vector2Int(x, y - 2),
            new Vector2Int(x + 2, y),
            new Vector2Int(x - 2, y)
        };

        //�ړ��������Ɉړ���̍��W���͈͓����ǂł��邩�𔻒肷��
        //�^�ł���΁A�ԋp�p���X�g�ɒǉ�����
        var movablePositions = positions.Where(p => !IsOutOfBounds(p.x, p.y) && cells[p.x, p.y] == CellType.Wall);

        return movablePositions.Count() != 0 ? movablePositions.ToList() : null;
    }


    //�^����ꂽx�Ay���W���͈͊O�̏ꍇ�^��Ԃ�
    private bool IsOutOfBounds(int x, int y) => (x < 0 || y < 0 || x >= max || y >= max);


    //�p�����[�^�ɉ����ăI�u�W�F�N�g�𐶐�����
    private void BuildDungeon()
    {
        //�c��1�}�X���傫�����[�v���񂵁A�O�ǂƂ���
        for (int i = -1; i <= max; i++)
        {
            for (int j = -1; j <= max; j++)
            {
                //�͈͊O�A�܂��͕ǂ̏ꍇ�ɕǃI�u�W�F�N�g�𐶐�����
                if (IsOutOfBounds(i, j) || cells[i, j] == CellType.Wall)
                {
                    var wallObj = Instantiate(wall, new Vector3(i, 0, j), Quaternion.identity);
                    wallObj.transform.parent = this.transform;
                }

                //�S�Ă̏ꏊ�ɏ��I�u�W�F�N�g�𐶐�
                var floorObj = Instantiate(floor, new Vector3(i, -1, j), Quaternion.identity);
                floorObj.transform.parent = this.transform;
            }
        }
    }
}