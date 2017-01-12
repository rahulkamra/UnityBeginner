using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateCollider
{
    private bool[,] dotsArray;
    private int rows;
    private int cols;

    private List<Cell> allList;
    private bool[,] traversed;

    private List<List<Cell>> result;
    private List<Cell> currentList;

    private int numItemsTraversed = 0;
    private MarchingSquares march;

    public List<List<Cell>> Create(bool[,] dotsArray , MarchingSquares march)
    {
        result = new List<List<Cell>>();
        this.march = march;

        this.dotsArray = dotsArray;
        this.rows = dotsArray.GetLength(0) - 1; // the blocks are one less than points
        this.cols = dotsArray.GetLength(1) - 1;
        execute();
        return result;
        //We need to find shapes in this 2dArray which are connected
    }

    private void execute()
    {
        allList = new List<Cell>();
        currentList = new List<Cell>();

        traversed = new bool[rows, cols];
        for (int col = 0; col < cols; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                allList.Add(new Cell(row, col));
            }
        }
        Cell cell = allList[0];
        allList.RemoveAt(0);
        fillCells(cell);

    }
    private void fillCells(Cell targetCell)
    {
        if (numItemsTraversed >= rows * cols)
        {
            //we are done here
            return;
        }


        if (traversed[targetCell.row, targetCell.col])
        {
            Cell cell = allList[0];
            allList.RemoveAt(0);
            fillCells(cell);
            return;
        }

        traversed[targetCell.row, targetCell.col] = true;
        numItemsTraversed++;
        if (isCountourCell(targetCell))
        {
            currentList.Add(targetCell);
            Cell neightbourCell = getValidNeighbour(targetCell);
            if (neightbourCell != null)
            {
                fillCells(neightbourCell);
            }
            else
            {
                finishCurrent();
                Cell cell = allList[0];

                allList.RemoveAt(0);
                fillCells(cell);
            }
        }
        else
        {
            finishCurrent();
            Cell cell = allList[0];
            allList.RemoveAt(0);
            fillCells(cell);
        }
        // we need to find the first free item.

    }
    
    private bool isCountourCell(Cell cell)
    {
        int row = cell.row;
        int col = cell.col;

        bool tlBit = dotsArray[row + 1, col];
        bool trBit = dotsArray[row + 1, col + 1];
        bool brBit = dotsArray[row, col + 1];
        bool blBit = dotsArray[row, col];

        if (tlBit && trBit && brBit && blBit)//if all true .. means no countour
        {
            return false;
        }

        if (!(tlBit || trBit || brBit || blBit))//if all false .. means no countour
        {
            return false;
        }

        return true;
    }
    private void finishCurrent()
    {
        if (currentList != null && currentList.Count > 0)
        {
            result.Add(currentList);
            currentList = new List<Cell>();
        }
    }


    public Cell getValidNeighbour(Cell cell)
    {
        uint value = march.GetValue(cell.row, cell.col);
     
        Cell left = getValidLeftCell(cell);
        Cell right = getValidRightCell(cell);
        Cell bot = getValidBotCell(cell);
        Cell top = getValidTopCell(cell);

        switch (value)
        {
            case 1110:
            case 0001:
                //left and bot
                if (left != null)
                    return left;
                if (bot != null)
                    return bot;

                break;


            case 1101:
            case 0010:
                //right and bot
                //left and bot
                if (right != null)
                    return right;
                if (bot != null)
                    return bot;
                break;

            case 1011:
            case 0100:
                //top and right
                if (right != null)
                    return right;
                if (top != null)
                    return top;
                break;

            case 0111:
            case 1000:
                //top and left

                if (left != null)
                    return left;
                if (top != null)
                    return top;

                break;

            case 1001:
            case 0110:
                // top and bot
                if (bot != null)
                    return bot;
                if (top != null)
                    return top;

                break;

            case 1100:
            case 0011:
                //left and right
                if (left != null)
                    return left;
                if (right != null)
                    return right;

                break;
        }
        return null;
    }

    private Cell getValidLeftCell(Cell cell)
    {
        Cell left;
        if (cell.col != 0)
        {
            left = new Cell(cell.row, cell.col - 1);
            if (isCountourCell(left) && !traversed[left.row, left.col])
            {
                return left;
            }
        }
        return null;
    }


    private Cell getValidRightCell(Cell cell)
    {
        Cell right;
        if (cell.col < cols - 1)
        {
            right = new Cell(cell.row, cell.col + 1);
            if (isCountourCell(right) && !traversed[right.row, right.col])
            {
                return right;
            }
        }

        return null;
    }

    private Cell getValidTopCell(Cell cell)
    {

        Cell top;
        if (cell.row < rows - 1)
        {
            top = new Cell(cell.row + 1, cell.col);
            if (isCountourCell(top) && !traversed[top.row, top.col])
            {
                return top;
            }
        }
        return null;
    }

    private Cell getValidBotCell(Cell cell)
    {
        Cell bot;
        if (cell.row != 0)
        {
            bot = new Cell(cell.row - 1, cell.col);
            if (isCountourCell(bot) && !traversed[bot.row, bot.col])
            {
                return bot;
            }
        }
        return null;
    }


}


public class Cell
{
    public Cell()
    {

    }

    public Cell(int row , int col)
    {
        this.row = row;
        this.col = col;
    }

    public int row;
    public int col;

    public string toString()
    {
        return "Row : " + row + ", Col : " + col;
    }
}
