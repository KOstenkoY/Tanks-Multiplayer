using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private int _mapNumber = 0;

    private void OnEnable()
    {
        NetworkManagerLobby.OnMapChanged += ChangeMapNumber;
    }

    private void OnDisable()
    {
        NetworkManagerLobby.OnMapChanged -= ChangeMapNumber;
    }

    private int ChangeMapNumber()
    {
        return _mapNumber;
    }

    public void SetMapNumber(int mapNumber)
    {
        _mapNumber = mapNumber;
    }
}
