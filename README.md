
![MeshGen](https://github.com/user-attachments/assets/5f414fb9-bcac-4b55-b32f-06b26ab950ac)


This MeshGenerator script in Unity procedurally generates a 3D mesh composed of randomly placed points constrained within a specific spherical range. Using KDTree for efficient spatial queries, it connects each vertex to its two nearest neighbors to form triangles. It dynamically ensures the mesh faces outward by checking normals and flipping triangles if necessary. Additionally, each vertex is visualized using a prefab (e.g., a sphere) instantiated at its location. The script showcases procedural mesh generation, nearest neighbor search, and normal correction within Unity using C#.
