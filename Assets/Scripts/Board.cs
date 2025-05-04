using UnityEngine;

public class Board : MonoBehaviour
{
    
    public Color lightCol = Color.white;
    public Color darkCol = Color.gray;
    public GameObject squarePrefab; // A prefab for drawing squares
    public GameObject[] piecePrefabs;

    void Start()
    {
        CreateGraphicalBoard();
    }
    
    void CreateGraphicalBoard()
    {
        for (int file = 0; file < 8; file++)
        {
            for (int rank = 0; rank < 8; rank++)
            {
                bool isLightSquare = (file + rank) % 2 != 0;
                var squareColor = isLightSquare ? lightCol : darkCol;
                var position = new Vector2(-3.5f + file, -3.5f + rank);
                DrawSquare(squareColor, position);

                
            }
        }
    }

    void DrawSquare(Color color, Vector2 position)
    {
        GameObject square = Instantiate(squarePrefab, position, Quaternion.identity, this.transform);
        square.GetComponent<SpriteRenderer>().color = color;
    }
   
}

public static class Piece {
    public const int None = 0;    
    public const int King = 1;
    public const int Pawn = 2;
    public const int Knight = 3;
    public const int Bishop = 4;
    public const int Rook = 5;
    public const int Queen = 6;

    public const int White = 8;
    public const int Black = 16;
    }
public static class BoardState{
        public static int [] Square;

        static BoardState(){
            Square = new int[64];
            Square[0] = Piece.White | Piece.Bishop;
        }
    }
