using UnityEngine;
using System.Collections.Generic;
using System;

public class BoardManager : MonoBehaviour
{

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int selectionX = -1;
    private int selectionY = -1;

    public List<GameObject> ChessmanPrefabs;
    private List<GameObject> activeChessmans;

    public Chessman[,] Chessmans { get; set; }
    private Chessman selectedChessman;
    private bool isWhiteTurn = true;

    public static BoardManager Instance { get; set; }
    private bool[,] allowedMoves { get; set; }

    public int[] EnPassantMove { set; get; }

    public CameraButtonManager buttonManager;

    private void Start()
    {
        Instance = this;
        Chessmans = new Chessman[8, 8];
        EnPassantMove = new int[2] { -1, -1 };
        SpawnAllChessmans();
    }

    private void Update()
    {
        UpdateSelection();
        DrawChessBoard();

        if (Input.GetMouseButtonDown(0))
        {
            if (selectionX >= 0 && selectionY >= 0)
            {
                if (selectedChessman == null)
                {
                    // select the chessman
                    SelectChessman(selectionX, selectionY);
                }
                else
                {
                    // move the chessman
                    MoveChessman(selectionX, selectionY);
                }
            }
        }
    }

    private void UpdateSelection()
    {
        if (!Camera.main)
            return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25f, LayerMask.GetMask("ChessPlan")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    private void DrawChessBoard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heightLine = Vector3.forward * 8;

        for (int i = 0; i <= 8; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);

            for (int j = 0; j <= 8; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }

        // Draw selection
        if (selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));

            Debug.DrawLine(Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
                Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
        }
    }

    public List<GameObject> GetAllPieces()
    {
        return activeChessmans;
    }

    private void SpawnChessman(int index, int x, int y)
    {
        GameObject obj = Instantiate(
            ChessmanPrefabs[index],
            GetTileCenter(x, y),
            ChessmanPrefabs[index].transform.rotation) as GameObject;

        obj.transform.SetParent(this.transform);
        Chessmans[x, y] = obj.GetComponent<Chessman>();
        Chessmans[x, y].SetPosition(x, y);
        activeChessmans.Add(obj);
    }

    private Vector3 GetTileCenter(int x, int z)
    {
        Vector3 origin = new Vector3();
        origin.x = TILE_SIZE * x + TILE_OFFSET;
        origin.z = TILE_SIZE * z + TILE_OFFSET;

        return origin;
    }

    //private Vector3 AdjustChessmanPosition(Vector3 pos)
    //{
    //    return pos + new Vector3(dx, dy, dz);
    //}

    private void SpawnAllChessmans()
    {
        activeChessmans = new List<GameObject>();

        // White team
        // King
        SpawnChessman(0, 4, 0);
        // Queen
        SpawnChessman(1, 3, 0);
        // Rooks
        SpawnChessman(2, 0, 0);
        SpawnChessman(2, 7, 0);
        // Bishops
        SpawnChessman(3, 2, 0);
        SpawnChessman(3, 5, 0);
        // Knights
        SpawnChessman(4, 1, 0);
        SpawnChessman(4, 6, 0);
        // Pawns
        for (int i = 0; i < 8; i++)
            SpawnChessman(5, i, 1);

        // Black team
        // King
        SpawnChessman(6, 4, 7);
        // Queen
        SpawnChessman(7, 3, 7);
        // Rooks
        SpawnChessman(8, 0, 7);
        SpawnChessman(8, 7, 7);
        // Bishops
        SpawnChessman(9, 2, 7);
        SpawnChessman(9, 5, 7);
        // Knights
        SpawnChessman(10, 1, 7);
        SpawnChessman(10, 6, 7);
        // Pawns
        for (int i = 0; i < 8; i++)
            SpawnChessman(11, i, 6);
    }

    public void SelectChessman(int x, int y)
    {
        if (Chessmans[x, y] == null)
            return;
        if (Chessmans[x, y].isWhite != isWhiteTurn)
            return;

        bool hasAtLeastOneMove = false;
        allowedMoves = Chessmans[x, y].PossibleMove();

        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                if (allowedMoves[i, j])
                {
                    hasAtLeastOneMove = true;
                    break;
                }

        if (!hasAtLeastOneMove)
            return;

        selectedChessman = Chessmans[x, y];

        // change chessman material
        //previousMat = selectedChessman.GetComponentInChildren<MeshRenderer>().material;
        //selectedMat.mainTexture = previousMat.mainTexture;
        //selectedChessman.GetComponentInChildren<MeshRenderer>().material = selectedMat;

        BoardHighlights.Instance.HighlightAllowedMoves(allowedMoves);
    }

    public void MoveChessman(int x, int y)
    {
        if (allowedMoves[x, y])
        {
            Chessman c = Chessmans[x, y];

            // Eat chessman
            if (c != null && c.isWhite != isWhiteTurn)
            {
                if (c.GetType() == typeof(King))
                {
                    EndGame();
                    return;
                }

                activeChessmans.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            if (x == EnPassantMove[0] && y == EnPassantMove[1])
            {
                if (isWhiteTurn)
                    c = Chessmans[x, y - 1];
                else
                    c = Chessmans[x, y + 1];
                activeChessmans.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            EnPassantMove[0] = -1;
            EnPassantMove[1] = -1;
            // EnPassant
            if (selectedChessman.GetType() == typeof(Pawn))
            {
                if (y == 7)
                {
                    activeChessmans.Remove(selectedChessman.gameObject);
                    Destroy(selectedChessman.gameObject);
                    SpawnChessman(1, x, y);
                    selectedChessman = Chessmans[x, y];
                }
                else if (y == 0)
                {
                    activeChessmans.Remove(selectedChessman.gameObject);
                    Destroy(selectedChessman.gameObject);
                    SpawnChessman(7, x, y);
                    selectedChessman = Chessmans[x, y];
                }

                if (selectedChessman.CurrentY == 1 && y == 3)
                {
                    EnPassantMove[0] = x;
                    EnPassantMove[1] = y - 1;
                }
                else if (selectedChessman.CurrentY == 6 && y == 4)
                {
                    EnPassantMove[0] = x;
                    EnPassantMove[1] = y + 1;
                }
            }

            Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;
            selectedChessman.transform.position = GetTileCenter(x, y);
            selectedChessman.SetPosition(x, y);
            Chessmans[x, y] = selectedChessman;

            // Checkmate
            bool[,] allowedMoves = Chessmans[x, y].PossibleMove();
            if (IsCheckmate(allowedMoves))
            {
                if (isWhiteTurn)
                    Debug.Log("Black team is checkmated");
                else
                    Debug.Log("White team is checkmated");
            }

            // Change turn
            isWhiteTurn = !isWhiteTurn;

            // Change Camera Position to other team
            //if (isWhiteTurn)
            //    buttonManager.MoveCamera("white");
            //else
            //    buttonManager.MoveCamera("black");
        }

        //selectedChessman.GetComponentInChildren<MeshRenderer>().material = previousMat;
        BoardHighlights.Instance.HideHighlights();
        selectedChessman = null;
    }

    private bool IsCheckmate(bool[,] allowedMoves)
    {
        if (allowedMoves.Length == 0)
            return false;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (allowedMoves[i, j] && Chessmans[i, j] != null && Chessmans[i, j].GetType() == typeof(King))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void EndGame()
    {
        if (isWhiteTurn)
            Debug.Log("White team win");
        else
            Debug.Log("Black team win");

        foreach (GameObject go in activeChessmans)
        {
            Destroy(go);
        }

        BoardHighlights.Instance.HideHighlights();
        SpawnAllChessmans();
    }

    public Location Find(int turn, char c, Location dst, string disam)
    {
        Location lo = new Location();
        bool isWhite = (turn == 0);
        foreach (GameObject chessObj in activeChessmans)
        {
            Chessman chess = chessObj.GetComponent<Chessman>();
            if ((chess.isWhite == isWhite) && (chess.Annotation().Equals(c.ToString())))
            {
                if (chess.CanGo(dst.x, dst.y))
                {
                    if (disam.Length == 1) //Disambiguating moves
                    {
                        if (disam[0] >= '1' && disam[0] <= '9') //rank have to the same
                        {
                            if (chess.CurrentY == (disam[0]-'1'))
                            {
                                return new Location(chess.CurrentX, chess.CurrentY);
                            }
                        }
                        else if (disam[0] >= 'a' && disam[0] <= 'z') //file have to the same
                        {
                            if (chess.CurrentX == (disam[0]-'a'))
                            {
                                return new Location(chess.CurrentX, chess.CurrentY);
                            }
                        }
                        else
                        {
                            Debug.Log("Unexpected result! " + disam);
                        }
                    }
                    else
                    {
                        return new Location(chess.CurrentX, chess.CurrentY);
                    }
                }
            }
        }

        return lo;
    }

    public void KingSideCastling(int turn)
    {
        bool isWhite = (turn == 0);
        int rank = isWhite ? 0 : 7;
        if (Chessmans[7, rank] && Chessmans[7, rank].Annotation().Equals("R") 
            && Chessmans[4, rank] && Chessmans[4, rank].Annotation().Equals("K"))
        {
            MoveFromTo(7, rank, 5, rank);
            MoveFromTo(4, rank, 6, rank);
        }
    }

    private GameObject GetChessmanObj(int x, int y)
    {
        foreach (GameObject pieceObj in activeChessmans)
        {
            Chessman piece = pieceObj.GetComponent<Chessman>();
            if (piece.CurrentX == x && piece.CurrentY == y)
                return pieceObj;
        }
        return null;
    }

    private void MoveFromTo(int srcX, int srcY, int dstX, int dstY)
    {
        Chessman chess = Chessmans[srcX, srcY];

        Chessmans[srcX, srcY] = null;
        chess.transform.position = GetTileCenter(dstX, dstY);
        chess.SetPosition(dstX, dstY);
        Chessmans[dstX, dstY] = chess;
    }

    public void QueenSideCastling(int turn)
    {
        bool isWhite = (turn == 0);
        int rank = isWhite ? 0 : 7;
        if (Chessmans[0, rank] && Chessmans[0, rank].Annotation().Equals("R")
            && Chessmans[4, rank] && Chessmans[4, rank].Annotation().Equals("K"))
        {
            MoveFromTo(0, rank, 3, rank);
            MoveFromTo(4, rank, 2, rank);
        }
    }
}
