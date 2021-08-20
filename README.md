# Marching Cubes
Simple Marching Cubes algorithm, made in unity,without any "holes" caused by ambiguous cases

![image](https://user-images.githubusercontent.com/73392575/130162656-ffaf0795-d1d6-4171-b207-cdcb7e146bc1.png)


## How to use:
- Q -> Pressing "Q" will create a grid with width, height and depth you can change in the Handle Script attached to  
the main camera.  
- E -> Pressing "E" will turn all "edge points" invisible, those point show you where the terrain starts and ends.  
- R -> Pressing "R" will turn all of those edge points back visible.  
- Z -> Pressing "Z" will delete all of the points, grid and the cubes mesh.  
- F -> Pressing "F" will spawn a debug grid, 2x2x2 grid where you can use your left and right arrow keys to navigate  
through all 255 possible triangulations 

## How i named the cube vertices:

![image](https://user-images.githubusercontent.com/73392575/130165896-c2bedc78-c01a-4254-98f7-3eacbfba762a.png)

## Reminders:

1- You can change the terrain threshold to a number you like, right now the algorithm create a number between -1f and
1f, if its less than 0 its considered off the terrain, or a black dot, otherwise its inside the terrain, or a white dot.  

2- You can also create a terrain based on a 3D Texture, the algorithm can already create and use it to create a mesh, but you
need to comment a few lines in and out to work, if you go to the function "CreateGrid", inside the triple for loop, you can see 
which lines you need to comment or not to make it work, in the same place you can choose to create a grid using normal noise, perlin noise, 
3D texture or a pre-made array.

3- The algorithm has a sort of interpolation function, to disable it you need to go to the funcion called "VertexPositionInterpolated"
and change the return value to be always 0.5f.

4- This algorithm comes with a version of a "Marching Squares" algorithm that basically fill in the gaps remain in the borders, to disable it you
can simply comment the last line in the "CreateGrid" function, where you have a "CreateBorderTriangles" function, just comment it out.

##   Ambiguous Cases  
  #### Case number 118  
            where only vertices A, D and H are off the surface in any rotation
            
  ![image](https://user-images.githubusercontent.com/73392575/130165429-01bb8814-655e-4469-a3b4-c070be0ec7e2.png)
            
  #### Case number 246  
            where only vertices A and D are off the surface in any rotation 
  
  ![image](https://user-images.githubusercontent.com/73392575/130166547-a07d5215-fccf-4afa-b734-af5b8b7faab8.png)

  
  #### Case number 214  
            where only vertices A, D and F are off the surface in any rotation 
    
 ![image](https://user-images.githubusercontent.com/73392575/130166597-2a20e757-54a2-4e17-bba0-21e429bad440.png)


**with theses changes made, no holes are formed in the middle of the mesh**
