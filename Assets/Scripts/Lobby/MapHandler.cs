using System;
using System.Linq;
using System.Collections.Generic;

public class MapHandler
{
    private readonly IReadOnlyCollection<string> _maps;
    private readonly int _numberFirstMap = 0;

    private int _currentMap;
    private List<string> _remainingMaps;

    public MapHandler(MapSet mapSet, int numberOfMap)
    {
        _maps = mapSet.Maps;
        _currentMap = numberOfMap;

        ResetMaps();
    }

    public string GetMap
    {
        get
        {
            if(_remainingMaps.Count == 0) 
                ResetMaps();

            string map = _remainingMaps[_currentMap];

            _remainingMaps.Remove(map);

            return map;
        }
    }

    private void ResetMaps() => _remainingMaps = _maps.ToList();
}
