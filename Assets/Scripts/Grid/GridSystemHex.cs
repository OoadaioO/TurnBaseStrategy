using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemHex<TGridObject>
{
    private const float HEX_VERTICAL_OFFSET_MULTIPLYER = 0.75f;

    private int width;
    private int height;
    private float cellSize;

    private TGridObject[,] gridObjectArray;

    public GridSystemHex(int width, int height, float cellSize, Func<GridSystemHex<TGridObject>, GridPosition, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.gridObjectArray = new TGridObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArray[x, z] = createGridObject(this, gridPosition);
            }
        }
    }



    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, 0) * cellSize +
            HEX_VERTICAL_OFFSET_MULTIPLYER * cellSize * new Vector3(0, 0, gridPosition.z) +
            ((gridPosition.z % 2 == 1) ? 0.5f * cellSize * new Vector3(1, 0, 0) : Vector3.zero);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(Mathf.RoundToInt(worldPosition.x / cellSize), Mathf.RoundToInt(worldPosition.z / cellSize));
    }

    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition.x, gridPosition.z];
    }

    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));

            }
        }
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 &&
        gridPosition.z >= 0 &&
        gridPosition.x < width &&
        gridPosition.z < height;
    }

    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
}
