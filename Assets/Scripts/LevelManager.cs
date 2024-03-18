using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public bool SHOW_COLLIDER = false;  //$$

    public static LevelManager Instance { set; get; }
        
    //nivel de desove
    private const float DISTANCIA_ANTES_PUESTA = 100.0f;
    private const float SEGMENTOS_INICIALES = 10;
    private const float INICIAL_TRANSICION_SEGMENTOS = 2;
    private const int MAXIMO_SEGMENTOS_POR_PANTALLA = 15;
    private Transform cameraContainer;
    private int amountOfActiveSegments;
    private int continuousSegments;
    private int currentSpawnZ;
    private int currentLevel;
    private int y1, y2, y3;

    // Lista de piezas
    public List<Piece> rampas = new List<Piece>();
    public List<Piece> bloqueslargos = new List<Piece>();
    public List<Piece> jumps = new List<Piece>();
    public List<Piece> slides = new List<Piece>();
    [HideInInspector]
    public List<Piece> pieces = new List<Piece>();  //todas las piezas

    //Lista de segmentos
    public List<Segment> availableSegments = new List<Segment>();
    public List<Segment> availableTransitions = new List<Segment>();
    [HideInInspector]
    public List<Segment> segments = new List<Segment>();

    //Empezar a jugar
    private bool isMoving = false;

    private void Awake()
    {
        Instance = this;
        cameraContainer = Camera.main.transform;
        currentSpawnZ = 0;
        currentLevel = 0;
    }
    private void Start()
    {
        for (int i = 0; i < SEGMENTOS_INICIALES; i++)
            if (i < INICIAL_TRANSICION_SEGMENTOS)
                SpawnTransition();
            else
                GenerateSegments();
        
    }

    private void Update()
    {
        if(currentSpawnZ - cameraContainer.position.z < DISTANCIA_ANTES_PUESTA)
             GenerateSegments();

        if(amountOfActiveSegments >= MAXIMO_SEGMENTOS_POR_PANTALLA)
        {
            segments[amountOfActiveSegments - 1].DeSpawn();
            amountOfActiveSegments--;
        }
        
    }

    private void GenerateSegments()
    {
        SpawnSegment();

        if (Random.Range(0f, 1f) < (continuousSegments * 0.25f))
        {
            //Spawn transitoion seg
            continuousSegments = 0;
            SpawnTransition();
        }
        else
        {
            continuousSegments++;
        }
           
    }

    private void SpawnSegment()
    {
        List<Segment> possibleSeg = availableSegments.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
        int id = Random.Range(0, possibleSeg.Count);

        Segment s = GetSegment(id, false);

        y1 = s.endY1;
        y2 = s.endY2;
        y3 = s.endY3;

        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.forward * currentSpawnZ;

        currentSpawnZ += s.lenght;
        amountOfActiveSegments++;
        s.Spawn();
      
    }
    private void SpawnTransition()
    {
        List<Segment> possibleTransition = availableTransitions.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
        int id = Random.Range(0, possibleTransition.Count);

        Segment s = GetSegment(id, true);

        y1 = s.endY1;
        y2 = s.endY2;
        y3 = s.endY3;

        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.forward * currentSpawnZ;

        currentSpawnZ += s.lenght;
        amountOfActiveSegments++;
        s.Spawn();
    }

    public Segment GetSegment(int id,bool _transition)
    {
        Segment s = null;
        s = segments.Find(x => x.SegId == id && x.transition == _transition && !x.gameObject.activeSelf);

        if(s == null)
        {
            GameObject go = Instantiate((_transition) ? availableTransitions[id].gameObject : availableSegments[id].gameObject) as GameObject;
            s = go.GetComponent<Segment>();
            
            s.SegId = id;
            s.transition = _transition;

            segments.Insert(0, s);
        }
        else
        {
            segments.Remove(s);
            segments.Insert(0, s);
            
        }

        return s;
    }
    public Piece GetPiece(PieceType _pieceType, int _visualIndex)
    {
        Piece p = pieces.Find(x => x.type == _pieceType && x.visualIndex == _visualIndex && !x.gameObject.activeSelf);

        if (p == null)
        {
            //If all gameobjects are in use, spawn another one
            GameObject go = null;
            switch (_pieceType)
            {
                case PieceType.rampa:
                    go = rampas[_visualIndex].gameObject;
                    break;

                case PieceType.bloquelargo:
                    go = bloqueslargos[_visualIndex].gameObject;
                    break;

                case PieceType.jump:
                    go = jumps[_visualIndex].gameObject;
                    break;

                case PieceType.slide:
                    go = slides[_visualIndex].gameObject;
                    break;
            }

            go = Instantiate(go);
            p = go.GetComponent<Piece>();
            pieces.Add(p);
        }

        return p;
    }


}
