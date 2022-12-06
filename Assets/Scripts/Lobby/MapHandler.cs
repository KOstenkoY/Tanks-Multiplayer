using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    private readonly IReadOnlyCollection<string> _maps;
    private readonly int _numberOfRound;

    private int _currentRound;
    private List<string> _remainingMaps;

    public MapHandler(MapSet mapSet, int numberOfRounds)
    {
        _maps = mapSet.Maps;
        _numberOfRound = numberOfRounds;

        ResetMaps();
    }

    public bool IsComplete => _currentRound == _numberOfRound;

    public string NextMap
    {
        get
        {
            if (IsComplete) { return null; }

            _currentRound++;

            if (_remainingMaps.Count == 0) { ResetMaps(); }

            string map = _remainingMaps[UnityEngine.Random.Range(0, _remainingMaps.Count)];

            _remainingMaps.Remove(map);

            return map;
        }
    }

    private void ResetMaps() => _remainingMaps = _maps.ToList();
}
