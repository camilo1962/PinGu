using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{

    public PieceType type;
    private Piece currentPiece;

    public void Spawn()
    {
        int amtObj = 0;
        switch (type)
        {
            case PieceType.jump:
                amtObj = LevelManager.Instance.jumps.Count;
                break;
            case PieceType.slide:
                amtObj = LevelManager.Instance.slides.Count;
                break;
            case PieceType.bloquelargo:
                amtObj = LevelManager.Instance.bloqueslargos.Count;
                break;
            case PieceType.rampa:
                amtObj = LevelManager.Instance.rampas.Count;
                break;

        }

        currentPiece = LevelManager.Instance.GetPiece(type,Random.Range(0,amtObj));
        currentPiece.gameObject.SetActive(true);
        currentPiece.transform.SetParent(transform, false);
    } 

    public void Despawn()
    {
        currentPiece.gameObject.SetActive(false);
    }
}
