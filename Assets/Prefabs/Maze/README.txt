quick little tutorial on how to create a maze:

1. instantiate the 2 prefabs in this folder.

2. your scene is gonna need a few things:
- you'll probably already have a Grid, but if not, make one. both the Maze and MazeGridSpawner (MGS) need a ref to this
- you'll need to create an empty game object, and assign it to the "Origin" slot on MGS. the only thing that matters is
  the transform of this object. this object will be the upper-left corner of the maze.
- you might need to assign a new sprite to the "wall tile" slot on the Maze object.

3. few final tweaks:
- On the Maze object, for testing with pathfinding, enable the "generate" and "bottom row empty" flags. Without the second flag,
  it may create a maze with "loops" that the pathfinding will get stuck on. you can mess around with right wall/bottom wall probabilities
  if you want, it'll just change how the maze is generated a bit. I like rwp = 0.5 and bwp = 0.8. I'd keep doDebug off, but do what you want.
- On MGS, you can change the size of the grid with the width/height variables. you can also change the size of each cell in the maze
  by modifying the maze cell size variable.