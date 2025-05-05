/*using UnityEngine;

public class Board : MonoBehaviour
{
    
    public Color lightCol = Color.white;
    public Color darkCol = Color.gray;
    public GameObject squarePrefab; // A prefab for drawing squares
    public GameObject[] piecePrefabs; // Chess piece prefabs
    //important note regarding this: the prefabs are needed to be attached to the object board in the inspector

    void Start() //start of this program
    {
        CreateGraphicalBoard();
    }
    
    void CreateGraphicalBoard() //main function that is doing all of the heavy lifting
    {
        for (int file = 0; file < 8; file++) //files are columns
        {
            for (int rank = 0; rank < 8; rank++) //ranks are rows
            {
                int index = rank*8+file; 
                //for example, the square B2 has the index values [1,1], here- the value returning to index is= 9, meaning, if A1 is 0, then B2 is the 9th square from A1
                bool isLightSquare = (file + rank) % 2 != 0; 
                var squareColor = isLightSquare ? lightCol : darkCol;
                var position = new Vector2(-3.5f + file, -3.5f + rank);
                DrawSquare(squareColor, position);

                int pieceCode = BoardState.Square[index]; 
                if (pieceCode != Piece.None)
                {
                    DrawPiece(pieceCode, position);
                }
                
            }
        }
    }

    void DrawSquare(Color color, Vector2 position) //we spawning the pieces on the board with this one 
    {
        GameObject square = Instantiate(squarePrefab, position, Quaternion.identity, this.transform);
        square.GetComponent<SpriteRenderer>().color = color;
    }

    void DrawPiece(int pieceCode, Vector2 position)
    {
    int pieceType = pieceCode & 0b00111;  // isolate piece type
    int pieceColor = pieceCode & 0b11000; // isolate color

    int prefabIndex = -1;

    // Assign prefab index based on color and type
    if (pieceColor == Piece.White)
    {
        prefabIndex = pieceType ; // white pieces at 0-5
    }
    else if (pieceColor == Piece.Black)
    {
        prefabIndex = pieceType + 6; // black pieces at 6-11
    }

    if (prefabIndex >= 0 && prefabIndex < piecePrefabs.Length && piecePrefabs[prefabIndex] != null)
    {
        Instantiate(piecePrefabs[prefabIndex], position, Quaternion.identity, this.transform);
    }
    }

}

//a class keeping the list of all of the pieces inside the game of chess
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
//now the current boardstate, meaning where each pieces are going to be spawned at
    public static class BoardState{
        public static int [] Square;

        static BoardState(){
            Square = new int[64];
            Square[0] = Piece.White | Piece.Bishop;
            Square[52] = Piece.Black | Piece.Queen;
        }
    }




*/

using UnityEngine;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    public Color lightCol = Color.white;
    public Color darkCol = Color.gray;
    public GameObject squarePrefab; // A prefab for drawing squares
    public GameObject[] piecePrefabs; // Chess piece prefabs

    private BoardState boardState;

    void Start()
    {
        boardState = new BoardState();
        CreateGraphicalBoard();
        RenderPieces();
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

    void RenderPieces()
    { 
        for (int index = 0; index <64; index++)
        {
            int pieceCode = boardState.Square[index];
            if (pieceCode != Piece.None)
            {
                int file = index % 8;
                int rank = index / 8;
                var position = new Vector2(-3.5f + file, -3.5f + rank);
                DrawPiece(pieceCode, position);
            }
        }
    }

    void DrawPiece(int pieceCode, Vector2 position)
    {
        int pieceType = pieceCode & 0b00111;  // isolate piece type
        int pieceColor = pieceCode & 0b11000; // isolate color

        int prefabIndex = -1;

        if (pieceColor == Piece.White)
        {
            prefabIndex = pieceType ; // white pieces at 0-5
        }
        else if (pieceColor == Piece.Black)
        {
            prefabIndex = pieceType + 6; // black pieces at 6-11
        }

        if (prefabIndex >= 0 && prefabIndex < piecePrefabs.Length && piecePrefabs[prefabIndex] != null)
        {
            Instantiate(piecePrefabs[prefabIndex], position, Quaternion.identity, this.transform);
        }
    }
}

public static class Piece
{
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

public class BoardState
{
    public int[] Square { get; private set; }
    //private const string startFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    //test fen below
    private const string startFEN = "4B3/2p3P1/p7/1r3PB1/1Pk5/3nKpP1/1p3R2/7R w - - 0 1";


    public BoardState()
    {
        Square = new int[64];
        LoadPositionFromFen(startFEN);
    }

    public void LoadPositionFromFen(string fen) //loads position from top left to bottom right
    {
        var pieceTypeFromSymbol = new Dictionary<char, int>
        {
            ['k'] = Piece.King, ['p'] = Piece.Pawn, ['n'] = Piece.Knight,
            ['b'] = Piece.Bishop, ['r'] = Piece.Rook, ['q'] = Piece.Queen,
        };

        string fenBoard = fen.Split(' ')[0];
        int file = 0, rank = 7;

        foreach (char symbol in fenBoard) 
        {
            if (symbol == '/')
            {
                file = 0;
                rank--;
            }
            else if (char.IsDigit(symbol))
            {
                file += (int)char.GetNumericValue(symbol);
            }
            else
            {
                int pieceColor = char.IsUpper(symbol) ? Piece.White : Piece.Black;
                int pieceType = pieceTypeFromSymbol[char.ToLower(symbol)];
                Square[rank * 8 + file] = pieceType | pieceColor;
                file++;
            }
        }
    }
}
