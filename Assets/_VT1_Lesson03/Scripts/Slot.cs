using UnityEngine;
using UnityEngine.InputSystem;

// ===========
// 配列を使ったスロットスクリプト
// ===========
public class Slot : MonoBehaviour
{
    // int型の配列
    private int[] _reel = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    private int[] _reel2 = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    private int[] _reel3 = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    private int[,] _slotMatrix = new int[,]
    {
        { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
        { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
        { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }
    };

    private int _stopIndex = 0;

    // =====================================
    // オブジェクトを保存する二次元配列
    // =====================================
    private GameObject[,] _slotObject;

    public GameObject symbolPrefab;

    void Start()
    {
        Application.targetFrameRate = 5;

        SlotLog(_slotMatrix);

        // =============================
        // 生成した二次元配列を取得する
        // =============================
        _slotObject = MakeReelObject(_slotMatrix, symbolPrefab);

        /*
        _reel = new int[10];
        for(int i = 0; i < _reel.Length; i++)
        {
            _reel[i] = i;
        }
        */
    }

    void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            _stopIndex++;

            if( HitCheck(_slotMatrix, 0) )
            {
                Debug.Log("0行目 がヒットしました。");
            }
        }

        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            _stopIndex = 0;
        }

        for (int i = _stopIndex; i < 3; i++)
        {
            // ReelLoop(スロット配列, リール番号)
            ReelLoop(_slotMatrix, i);
        }

        // ==========================================
        // オブジェクトの色をスロットの要素に合わせる
        // ==========================================
        SetObjectColor(_slotMatrix, _slotObject);

        ReelLog(_reel);
    }

    public void ReelLoop(int[,] slot, int index)
    {
        // 二次元配列から絵柄配列（列）を取り出す
        int[] reel = new int[slot.GetLength(1)];
        for(int i = 0; i < reel.Length; i++)
        {
            // reel[i番目] に slot[リール番号, i] のデータを代入
            reel[i] = slot[index, i];
        }

        // === リールの絵柄（配列）を入れ替える === //
        // ①　配列の最後の要素を変数に保持
        int temp = reel[reel.Length - 1];

        // ②　3番目から0番目まで要素を一つずらす
        // ※　n 番目の要素が n-1 番目の要素をコピーしている
        /*
        reel[4] = reel[4-1];
        reel[3] = reel[3-1];
        reel[2] = reel[2-1];
        reel[1] = reel[1-1];
        */

        // ※　indexを最後の添え字で初期化し、indexが 1以上 の間、indexを1ずつ減らしながら繰り返す
        for( int i = reel.Length - 1; i >= 1; i-- )
        {
            reel[i] = reel[i - 1];
        }

        // ③　0番目に保持していた要素を代入
        reel[0] = temp;

        // スロットの二次元配列に新しい絵柄配列を代入する
        for( int i = 0; i < reel.Length; i++ )
        {
            slot[index, i] = reel[i];
        }
    }

    public void ReelLog(int[] reel)
    {
        string log = "==== Reel log ==== \n";
        // indexを0で初期化し、indexが最後の要素になるまで、indexを1ずつ増やしながら繰り返す
        for(int index = 0; index < reel.Length; index++)
        {
            log += $"{index}番目 = { reel[index] }, ";
        }

        Debug.Log(log);
    }

    public void SlotLog(int[,] slot)
    {
        // 行数を取得
        int row = slot.GetLength(0);

        // 列数を取得
        int col = slot.GetLength(1);

        // 全ての要素の数
        int all = slot.Length;

        string log = "=== Slot log ===\n";
        // 行列を繰り返す for 文
        // ※　言語化してください。
        for(int r = 0; r < row; r++)
        {
            log += "\n";
            // ※　言語ry
            for(int c = 0; c < col; c++)
            {
                log += $"[{ r }行, { c }列]={ slot[r, c] }, ";
            }
        }

        Debug.Log(log);
    }

    public bool HitCheck(int[,] slot, int row)
    {
        bool hit = (slot[0, row] == slot[1, row]) && (slot[0, row] == slot[2, row]);
        return hit;
    }

    public GameObject[,] MakeReelObject( int[,] slot, GameObject prefab )
    {
        // オブジェクト型の二次元配列
        GameObject[,] slotObject = new GameObject[slot.GetLength(0), slot.GetLength(1)];

        // 二次元配列の要素（中身）を設定する
        for(int i = 0; i < slotObject.GetLength(0); i++)
        {
            for(int j = 0; j < slotObject.GetLength(1); j++)
            {
                // オブジェクトを生成する
                slotObject[i, j] = Instantiate( prefab );

                // オブジェクトの Sprite Renderer を取得
                SpriteRenderer renderer = slotObject[i, j].GetComponent<SpriteRenderer>();

                // 色を変更
                renderer.color = new Color( 1f / (slot[i,j] + 1), 1f / (slot[i,j] + 1), 1f / (slot[i,j] + 1) );

                // 位置をずらす
                Vector3 pos = transform.position;
                pos.x = i;
                pos.y = -j;
                slotObject[i, j].transform.position = pos;
            }
        }

        // 生成したオブジェクトの二次元配列を返却
        return slotObject;
    }

    // スロットの要素に合わせて、オブジェクトの色を変える
    public void SetObjectColor(int[,] slot, GameObject[,] obj)
    {
        // スロットの二次元配列の要素を繰り返す
        for(int i = 0; i < slot.GetLength(0); i++)
        {
            for(int j = 0; j < slot.GetLength(1); j++)
            {
                SpriteRenderer renderer = obj[i,j].transform.GetComponent<SpriteRenderer>();
                renderer.color = new Color(1f / (slot[i, j] + 1), 1f / (slot[i, j] + 1), 1f / (slot[i, j] + 1));
            }
        }
    }
}
