using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class ColorHandler : NetworkBehaviour
{
    [SyncVar, SerializeField] private int _countAvailableColors = 6; 

    private SyncDictionary<int, bool> _availableColors = new SyncDictionary<int, bool>();

    public int Length => _availableColors.Count;

    private NetworkManagerLobby _room;

    private NetworkManagerLobby Room
    {
        get
        {
            if (_room != null)
            {
                return _room;
            }

            return _room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public void InitializeColorHandler()
    {
        for (int i = 0; i < _countAvailableColors; i++)
        {
            _availableColors.Add(i, false);
        }
    }

    public int GetFreePlayerColorUI()
    {
        for (int i = 0; i < _availableColors.Count; i++)
        {
            if (_availableColors[i] == false)
            {
                _availableColors[i] = true;

                return i;
            }
        }
        throw new System.Exception("ColorHadnler can't find free color in UI, check count availableColors and working cycle!");
    }

    public bool CheckFreePlayerColorUI(int oldIndexColor, int newIndexColor)
    {
        if (_availableColors[newIndexColor] == false)
        {
            _availableColors[oldIndexColor] = false;
            _availableColors[newIndexColor] = true;

            return true;
        }
        else
        {
            return false;
        }
    }
}
