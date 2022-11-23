using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Color _color;

    private string _name;

    private int _countCoins;

    public Color Color { get { return _color; } set { _color = value; } }
    public string Name { get { return _name; } set { _name = value; } }
    public int CountCoins { get { return _countCoins; } set { _countCoins = value; } }
}
