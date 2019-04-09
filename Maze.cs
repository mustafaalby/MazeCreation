using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {
    [SerializeField] GameObject wallParent;
    [SerializeField] GameObject cellParent;
    [SerializeField] GameObject WallV;    
    [SerializeField] int xAxisLength;
    [SerializeField] int yAxisLength;
    [SerializeField] int CurrentCell;
    [SerializeField] CellScript[] CellArray;
    Stack<int> stack = new Stack<int>();
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void createCells()
    {
        for (int i = 0; i <= yAxisLength; i++)
        {

            for (int j = 0; j < xAxisLength; j++)
            {
                Instantiate(WallV, new Vector3(j+1, i, 0), Quaternion.AngleAxis(90,Vector3.forward), wallParent.transform); 
                /*
                 * 
                 *                  
                */
            }
        }
        for (int i = 0; i < yAxisLength; i++)
        {

            for (int j = 0; j <= xAxisLength; j++)
            {
                Instantiate(WallV, new Vector3(j, i, 0), Quaternion.identity, wallParent.transform);
            }
        }
        
        CreateCellObjects();
    }
   
    public void CreateCellObjects()
    {
        int columnCounter = 0;
        int EastWestCounter = 0;
        
        GameObject[] allWalls = new GameObject[wallParent.transform.childCount];
        CellArray = new CellScript[yAxisLength * xAxisLength];
        for (int i = 0; i < wallParent.transform.childCount; i++)
        {
            allWalls[i] = wallParent.transform.GetChild(i).gameObject;
        }
        
        for (int i = 0; i < CellArray.Length; i++)
        {
            CellArray[i] = new CellScript();
            CellArray[i].south = allWalls[i];
            CellArray[i].north = allWalls[i + xAxisLength];
            if (columnCounter == xAxisLength)
            {
                EastWestCounter ++;
                columnCounter = 0;
            }
            
           
            
            columnCounter++;
            
            CellArray[i].west = allWalls[(yAxisLength + 1) * xAxisLength + i+EastWestCounter];
            CellArray[i].east=allWalls[(yAxisLength + 1) * xAxisLength + i+1+EastWestCounter];
        }

        
    }
    
    public List<int> FindCurrentUnvisitedCellNeighbours(int CellIndex)
    {
        List<int> neighbours = new List<int>();
        neighbours.Clear();
        //int index = 0;
        //int[] neighbours = new int[4];
        Debug.Log("Current Cell: " + CurrentCell);
        //For West
        if ((CellIndex % xAxisLength)!=0)
        {
            if(CellArray[CellIndex - 1].isVisited==false)
            {
                neighbours.Add(CellIndex - 1);
               //neighbours[index] = CellIndex - 1;
                //Debug.Log(neighbours[index]);
                //index++;
            }
        }
        //For East
        if ((CellIndex % xAxisLength) != (xAxisLength-1))
        {
            if (CellArray[CellIndex + 1].isVisited == false)
            {
                neighbours.Add(CellIndex + 1);
                
            }
        }
        //For North
        if ((CellIndex + xAxisLength) < (xAxisLength*yAxisLength))
        {
            if (CellArray[CellIndex + xAxisLength].isVisited==false)
            {
                neighbours.Add(CellIndex + xAxisLength);
                
            }
        }

        //For South
        if ((CellIndex - xAxisLength) >=0)
        {
            if (CellArray[CellIndex - xAxisLength].isVisited == false)
            {
                neighbours.Add(CellIndex - xAxisLength);
                
            }
        }
        for (int i = 0; i < neighbours.Count; i++)
        {
            Debug.Log("neighbour: "+ neighbours[i]);
        }
        return neighbours;

    }

    public void CreateMaze()
    {
        
        
        List<int> neighboursArray = new List<int>();

        CellArray[CurrentCell].isVisited = true;
        

        while (checkIfThereIsUnVisitedCell())
        {
            neighboursArray.Clear();
          neighboursArray = FindCurrentUnvisitedCellNeighbours(CurrentCell);
          //foreach (int index in neighbours)
          //       neighboursArray.Add(index);

            
                
            if (neighboursArray.Count != 0)//if
            {
                Debug.Log("there are ng");
                int randomNeighbourIndex = Random.Range(0, neighboursArray.Count );
                //Debug.Log("Count of Neighbours: " + neighboursArray.Count);
                //Debug.Log("rand neighbour index: " + randomNeighbourIndex);
                stack.Push(CurrentCell);                
                foreach (int index in stack)
                    Debug.Log("Stack icerisi: " + index);
                Debug.Log("stack count  "+stack.Count);

                if (neighboursArray[randomNeighbourIndex]==(CurrentCell-1))//for West
                {
                    Destroy(CellArray[CurrentCell].west.gameObject);
                }
                else if (neighboursArray[randomNeighbourIndex] == (CurrentCell + 1))//For East
                {
                    Destroy(CellArray[CurrentCell].east.gameObject);
                }
                else if (neighboursArray[randomNeighbourIndex] == (CurrentCell +xAxisLength))//For North
                {
                    Destroy(CellArray[CurrentCell].north.gameObject);
                }
                else if (neighboursArray[randomNeighbourIndex] == CurrentCell - xAxisLength)//For South
                {
                    Destroy(CellArray[CurrentCell].south.gameObject);
                }
                
                CurrentCell = neighboursArray[randomNeighbourIndex];
                CellArray[CurrentCell].isVisited= true;
            }
            else if (stack.Count!=0)
            {
                Debug.Log("stack is not empty");
                CurrentCell=stack.Pop();

            }
                
        }
    }
    bool checkIfThereIsUnVisitedCell()
    {
        
        for (int i = 0; i < CellArray.Length; i++)
        {
            if (CellArray[i].isVisited == false)
            {                
                return true;
            }
        }
        return false;
    }

}
