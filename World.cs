using Godot;
using System;

public partial class World : Node2D
{
	public override void _Ready()
	{
		TileMap tileMap = new TileMap();
		tileMap.TileSet = GD.Load<TileSet>("res://Tiles/MyTileSet.tres");
		AddChild(tileMap);
	}
}
