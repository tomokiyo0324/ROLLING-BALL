using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MakeLabyrinth : MonoBehaviour
{
    // 設定する値
    public int max = 41;       //初期値としてのサイズ
    public GameObject wall;    //壁用オブジェクト
    public GameObject floor;   //床用オブジェクト
    public GameObject start;   //スタート地点に配置するオブジェクト
    public GameObject goal;    //ゴール地点に配置するオブジェクト

    // 内部パラメータ
    private enum CellType { Wall, Path };   //セルの種類
    private CellType[,] cells;

    private Vector2Int startPos;    //スタートの座標
    private Vector2Int goalPos;     //ゴールの座標


    private void Start()
    {
        //マップ状態初期化
        cells = new CellType[max, max];

        //スタート地点の取得
        startPos = GetStartPosition();

        //通路の生成
        //初回はゴール地点を設定する
        goalPos = MakeMapInfo(startPos);

        //通路生成を繰り返して袋小路を減らす
        var tmpStart = goalPos;
        for (int i = 0; i < max * 41; i++)
        {
            MakeMapInfo(tmpStart);
            tmpStart = GetStartPosition();
        }

        //マップの状態に応じて壁と通路を生成する
        BuildDungeon();

        //スタート地点とゴール地点にオブジェクトを配置する
        //初回で取得したスタート地点とゴール地点は必ずつながっているので破綻しない
        var startObj = Instantiate(start, new Vector3(startPos.x, 0, startPos.y), Quaternion.identity);
        var goalObj = Instantiate(goal, new Vector3(goalPos.x, 0, goalPos.y), Quaternion.identity);

        startObj.transform.parent = this.transform;
        goalObj.transform.parent = this.transform;

    }


    // スタート地点の取得
    private Vector2Int GetStartPosition()
    {
        //ランダムでx,yを設定
        int randomX = Random.Range(0, max);
        int randomY = Random.Range(0, max);

        //x、yが両方共偶数になるまで繰り返す
        while (!(randomX % 2 == 0 && randomY % 2 == 0))
        {
            randomX = Mathf.RoundToInt(Random.Range(0, max));
            randomY = Mathf.RoundToInt(Random.Range(0, max));
        }

        return new Vector2Int(randomX, randomY);
    }


    // マップ生成
    private Vector2Int MakeMapInfo(Vector2Int _startPos)
    {
        //スタート位置配列を複製
        var tmpStartPos = _startPos;

        //移動可能な座標のリストを取得
        var movablePositions = GetMovablePositions(tmpStartPos);

        //移動可能な座標がなくなるまで探索を繰り返す
        while (movablePositions != null)
        {
            //移動可能な座標からランダムで1つ取得し通路にする
            var tmpPos = movablePositions[Random.Range(0, movablePositions.Count)];
            cells[tmpPos.x, tmpPos.y] = CellType.Path;

            //元の地点と通路にした座標の間を通路にする
            var xPos = tmpPos.x + (tmpStartPos.x - tmpPos.x) / 2;
            var yPos = tmpPos.y + (tmpStartPos.y - tmpPos.y) / 2;
            cells[xPos, yPos] = CellType.Path;

            //移動後の座標を一時変数に格納し、再度移動可能な座標を探索する
            tmpStartPos = tmpPos;
            movablePositions = GetMovablePositions(tmpStartPos);
        }

        //探索終了時の座標を返す
        return tmpStartPos;
    }


    // 移動可能な座標のリストを取得する
    private List<Vector2Int> GetMovablePositions(Vector2Int _startPos)
    {
        //可読性のため座標を変数に格納
        var x = _startPos.x;
        var y = _startPos.y;

        //移動方向毎に2つ先のx,y座標を仮計算
        var positions = new List<Vector2Int> {
            new Vector2Int(x, y + 2),
            new Vector2Int(x, y - 2),
            new Vector2Int(x + 2, y),
            new Vector2Int(x - 2, y)
        };

        //移動方向毎に移動先の座標が範囲内かつ壁であるかを判定する
        //真であれば、返却用リストに追加する
        var movablePositions = positions.Where(p => !IsOutOfBounds(p.x, p.y) && cells[p.x, p.y] == CellType.Wall);

        return movablePositions.Count() != 0 ? movablePositions.ToList() : null;
    }


    //与えられたx、y座標が範囲外の場合真を返す
    private bool IsOutOfBounds(int x, int y) => (x < 0 || y < 0 || x >= max || y >= max);


    //パラメータに応じてオブジェクトを生成する
    private void BuildDungeon()
    {
        //縦横1マスずつ大きくループを回し、外壁とする
        for (int i = -1; i <= max; i++)
        {
            for (int j = -1; j <= max; j++)
            {
                //範囲外、または壁の場合に壁オブジェクトを生成する
                if (IsOutOfBounds(i, j) || cells[i, j] == CellType.Wall)
                {
                    var wallObj = Instantiate(wall, new Vector3(i, 0, j), Quaternion.identity);
                    wallObj.transform.parent = this.transform;
                }

                //全ての場所に床オブジェクトを生成
                var floorObj = Instantiate(floor, new Vector3(i, -1, j), Quaternion.identity);
                floorObj.transform.parent = this.transform;
            }
        }
    }
}