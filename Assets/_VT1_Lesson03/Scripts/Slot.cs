using UnityEngine;

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
        { 0, 0, 0 },
        { 1, 1, 1 },
        { 2, 2, 2 },
        { 3, 3, 3 },
        { 4, 4, 4 }
    };

    void Start()
    {
        SlotLog(_slotMatrix);

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
        ReelLoop(_reel);
        ReelLog(_reel);
    }

    public void ReelLoop(int[] reel)
    {
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
        for( int index = reel.Length - 1; index >= 1; index-- )
        {
            reel[index] = reel[index - 1];
        }

        // ③　0番目に保持していた要素を代入
        reel[0] = temp;
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
}
