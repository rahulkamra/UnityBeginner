using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utils;

public class Maze : MonoBehaviour {

	// Use this for initialization

    public IntVector2 Size;

    public MazeCell MazeCell;

    public MazePassage PassagePrefab;

    public MazeDoor DoorPrefab;

    [Range(0f,1f)]
    public float DoorProbability;

    public MazeWall[] WallPrefabs;

    public float GenerationDelay;

    public PickType CurrentPickType;

    public MazeRoomSettings[] RoomSettings;

    private MazeCell[,] Cells;
    private delegate int PickTypeFunction(List<MazeCell> activeCells);
    private List<MazeRoom> MazeRooms = new List<MazeRoom>();
    public enum PickType
    {
        Last, First, Random, Middle
    }

    private PickTypeFunction[] PickTypeFunctions =
    {
        LastItem,FirstItem,RandomItem,MiddleItem
    };
   
   
    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator Generate()
    {
        WaitForSeconds wait =  new WaitForSeconds(GenerationDelay);
        Cells = new MazeCell[Size.x, Size.z];
        
        List<MazeCell> activeCells = new List<MazeCell>();
        DoFirstGenerationStep(activeCells);
        
        while (activeCells.Count > 0)
        {
            yield return wait;
            DoNextGenerationStep(activeCells);
        }

    }

    private void DoFirstGenerationStep(List<MazeCell> activeCells)
    {
        IntVector2 coordinates = new IntVector2(0,0);//RandomCoordinates);
        MazeCell cell =  CreateCell(coordinates);
        MazeRoom room = createMazeRoom();
        cell.Initialize(room);
        activeCells.Add(cell);
    }

    private void DoNextGenerationStep(List<MazeCell> activeCells)
    {
        PickTypeFunction pickFunction = PickTypeFunctions[(int) CurrentPickType];
        int currentIndex = pickFunction(activeCells);
        MazeCell currentMazeCell = activeCells[currentIndex];
        if (currentMazeCell.IsFulltInitialized)
        {
            activeCells.RemoveAt(currentIndex);
            return;
        }
        MazeDirection direction = currentMazeCell.RandomUnitializedDirection();
        IntVector2 nextCoordinates = currentMazeCell.Coordinate + direction.ToIntVector2();
      
        if (ContainsCoordinates(nextCoordinates))
        {
            MazeCell nextCell = GetMazeCell(nextCoordinates);
            if (nextCell == null)
            {
                nextCell = CreateCell(nextCoordinates);
                CreatePassage(currentMazeCell,nextCell,direction);
                activeCells.Add(nextCell);
            }else if(nextCell.Room.SettingsIndex == currentMazeCell.Room.SettingsIndex)
            {
                CreatePassageInSameRoom(currentMazeCell, nextCell, direction);
                activeCells.Add(nextCell);
            }
            else
            {
                CreateWall(currentMazeCell, nextCell, direction);
                
            }
            
        }
        else
        {
          //we need to add a wall because we are outisde the maze;
            CreateWall(currentMazeCell, null,direction);
        }

        
    }

    

    private MazeCell CreateCell(IntVector2 coordinates)
    {
       // Debug.Log("Creating Cell " + coordinates.ToString());
        MazeCell newCell = Instantiate(MazeCell);
        newCell.Coordinate = coordinates;
        Cells[coordinates.x, coordinates.z] = newCell;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(coordinates.x - Size.x * 0.5f + 0.5f, 0f, coordinates.z - Size.z * 0.5f + 0.5f);
        return newCell;
    }

    private void CreateWall(MazeCell from , MazeCell to , MazeDirection direction)
    {
        MazeWall wall = Instantiate(WallPrefabs[Random.Range(0,WallPrefabs.Length)]);
        wall.Initialize(from,to,direction);

        if (to != null)
        {
            wall = Instantiate(WallPrefabs[Random.Range(0, WallPrefabs.Length)]);
            wall.Initialize(to, from, direction.GetOpposite());
        }
    }

    private void CreatePassageInSameRoom(MazeCell from, MazeCell to, MazeDirection direction)
    {
        MazePassage passage = Instantiate(PassagePrefab);
        passage.Initialize(from, to, direction);
        
        passage = Instantiate(PassagePrefab);
        passage.Initialize(to, from, direction.GetOpposite());
        if(from.Room != to.Room)
        {
            MazeRoom roomToDestroy = to.Room;
            from.Room.Assimilate(roomToDestroy);
            this.MazeRooms.Remove(roomToDestroy);
            Destroy(roomToDestroy);
        }
    }
    private void CreatePassage(MazeCell from, MazeCell to, MazeDirection direction)
    {
        MazePassage Prefab = Random.value <= DoorProbability ? DoorPrefab : PassagePrefab;
        MazePassage passage = Instantiate(Prefab);
        passage.Initialize(from,to,direction);

        passage = Instantiate(PassagePrefab);
        if(Prefab is MazeDoor)
        {
            MazeRoom room =  createMazeRoom();
            to.Initialize(room);
        }
        else
        {
            to.Initialize(from.Room);
        }
        passage.Initialize(to,from,direction.GetOpposite());
    }

    private MazeRoom createMazeRoom()
    {
        int index = this.MazeRooms.Count % this.RoomSettings.Length;
        MazeRoomSettings roomSettings = this.RoomSettings[index];

        MazeRoom mazeRoom = ScriptableObject.CreateInstance<MazeRoom>();
        mazeRoom.MazeRoomSettings = roomSettings;
        mazeRoom.SettingsIndex = index;
        this.MazeRooms.Add(mazeRoom);
        return mazeRoom;
    }



    public IntVector2 RandomCoordinates
    {
        get
        {
            return new IntVector2(Random.Range(0,Size.x), Random.Range(0, Size.z));
        }
    }
    public bool ContainsCoordinates(IntVector2 coordinates)
    {
        return coordinates.x >= 0 && coordinates.x < Size.x && coordinates.z >= 0 && coordinates.z < Size.z;
    }

    public MazeCell GetMazeCell(IntVector2 coordinates)
    {
        return Cells[coordinates.x, coordinates.z];
    }

    private static int LastItem(List<MazeCell> activeCells)
    {
        return activeCells.Count - 1;
    }

    private static int FirstItem(List<MazeCell> activeCells)
    {
        return 0;
    }

    private static int RandomItem(List<MazeCell> activeCells)
    {
        return Random.Range(0, activeCells.Count);
    }

    private static int MiddleItem(List<MazeCell> activeCells)
    {
        return activeCells.Count/2;
    }
}


