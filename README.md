# Rail Resource Network Simulator

![Demo Preview](https://via.placeholder.com/800x400.png?text=Rail+Network+Demo)  
*(Replace with actual screenshot)*

## 📝 Project Description
**Test Assignment**  
This project demonstrates a rail network simulation where trains transport resources between mines and base stations.  
Key technical aspects include:
- Node-based graph system with pathfinding (Dijkstra's algorithm)
- Dynamic route optimization with economic calculations
- Runtime-editable train parameters
- Event-driven architecture for network changes
- Zenject dependency injection

## 🏗️ Architecture Overview

### Core Network System
| Component | Purpose | Key Features |
|-----------|---------|--------------|
| **Node** | Base network element | - Manages edges and connections<br>- Stores precalculated paths<br>- Position data |
| **Edge** | Directional connection | - Contains length/weight<br>- References end node |
| **RailNetworkGraph** | Graph manager | - Maintains node relationships<br>- Precomputes critical paths<br>- Event-driven updates |
| **DijkstraPathFinder** | Pathfinding | - Shortest path algorithm<br>- Distance tracking<br>- Path reconstruction |

### Specialized Nodes
| Component | Functionality |
|-----------|---------------|
| **Mine** | Mining node | - Time multiplier logic<br>- Non-negative validation |
| **BaseStation** | Processing hub | - Resource multiplier<br>- Change tracking |
| **PrecalculatedPath** | Path caching | - Stores target/next node<br>- Full path sequence |

### Train Simulation
| Component | Responsibility |
|-----------|----------------|
| **TrainController** | Central coordinator | - Lifecycle management<br>- Dependency injection |
| **TrainModel** | Train entity | - State machine (Moving/Mining/Delivering)<br>- Path following logic |
| **RouteManager** | Economic system | - Profit calculations<br>- Route caching |
| **TrainPathUpdater** | Dynamic routing | - Mid-journey rerouting<br>- Path reversal logic |
| **TrainSpawner** | Instantiation | - Config-based spawning<br>- Component setup |

### Visualization & Debug
| Component | Function |
|-----------|----------|
| **GraphRenderer** | 3D visualization | - Line rendering<br>- Edge type styling |
| **GraphEdge** | Edge metadata | - Type definitions (Correct/One-way/Invalid) |
| **GraphRendererEdgeGenerator** | Edge processing | - Duplicate prevention<br>- Connection validation |
| **TrainDebugger** | Runtime monitoring | - State visualization<br>- Gizmo drawing |
| **TrainDebugInfo** | UI overlay | - Textual status display |

### Resource Management
| Component | Purpose |
|-----------|---------|
| **ResourceWallet** | Economy system | - Resource tracking<br>- Event-driven updates |
| **TrainConfig** | Settings template | - Prefab/speed/mining presets |

### Dependency Management
| Component | Role |
|-----------|------|
| **GameInstaller** | DI Setup | - Zenject bindings<br>- Component linking |
| **TrainSettings** | Runtime config | - Live parameter tuning<br>- Change propagation |

## 🚂 Tutorial: Train Configuration

### Scene Setup
1. **Main Scene** contains:
   - `Graph` GameObject with:
     - **RailNetworkGraph** component (manages all nodes)
     - Child node GameObjects representing stations/mines

![Demo Preview](https://github.com/Akynin99/Resource-Rail-Network/blob/dev/Screenshots/settings.png) 

2. **Node Configuration**:
   - Select any child node in Graph hierarchy
   - Configure connections in **Node** component:
     - Edges array (connected nodes)
     - Connection lengths
   - Special node types:
     - **Mine**: Set `Time Mult` (mining speed multiplier)
     - **BaseStation**: Set `Resource Mult` (processing efficiency)

![Demo Preview](https://github.com/Akynin99/Resource-Rail-Network/blob/dev/Screenshots/base%20station.png)  ![Demo Preview](https://github.com/Akynin99/Resource-Rail-Network/blob/dev/Screenshots/mine.png) 

### Train Configuration
1. **Initial Setup**:
   - Preconfigure trains using `TrainConfig` assets:
     - Path: `Assets/Configs/`
     - Set base parameters:
       - Movement Speed
       - Mining Duration

![Demo Preview](https://github.com/Akynin99/Resource-Rail-Network/blob/dev/Screenshots/config.png) 

2. **Runtime Adjustments**:
   - Locate `TrainController` GameObject in hierarchy
   - Expand to access spawned train GameObjects
   - Modify live parameters in **TrainSettings** component:
     - Dynamic speed adjustment
     - Mining time tuning
   - Changes automatically affect:
     - Route calculations
     - Movement behavior
     - Resource collection rates

![Demo Preview](https://github.com/Akynin99/Resource-Rail-Network/blob/dev/Screenshots/settings2.png) 
