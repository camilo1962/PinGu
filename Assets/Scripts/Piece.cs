using UnityEngine;

public enum PieceType
{
    none = -1,
    rampa = 0,
    bloquelargo = 1,
    jump = 2,
    slide = 3,

}


public class Piece : MonoBehaviour
{
    public PieceType type;
    public int visualIndex;
}
