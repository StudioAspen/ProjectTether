using UnityEngine;
using TileSystem;

public class TileContainer : MonoBehaviour
{
    // Tiles Properties
    Tile _tileProp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTile(Tile tile) => _tileProp = tile;
}
