using System;
using UnityEngine;

public class BinarySearch
{
    private int _leftBound;
    private int _rightBound;

    public int Index { get; private set; }

    public BinarySearch(int count)
    {
        if (count < 1)
            throw new ArgumentOutOfRangeException(); 

        _leftBound = 0;
        _rightBound = count;
        Index = _rightBound / 2;
    }

    public bool TryMoveForward()
    {
        _leftBound = Index;
        Index = (_leftBound + _rightBound) / 2;

        if (_leftBound == _rightBound - 1)
            Index = _rightBound;

        return _leftBound + 1 < _rightBound;
    }

    public bool TryMoveBackward()
    {
        _rightBound = Index;
        Index = (_leftBound + _rightBound) / 2;

        return _leftBound < _rightBound;
    }
}
