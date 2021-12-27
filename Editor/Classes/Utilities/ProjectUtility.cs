using System;
using System.Collections.Generic;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    class ProjectUtility : IProjectUtility
    {
        public void AddMapWidgetToGrid(MapWidgetDto mapWidget, ProjectDto project)
        {
            int x = mapWidget.BoundingBox.Left;
            int y = mapWidget.BoundingBox.Top;

            int w = mapWidget.BoundingBox.Width;
            int h = mapWidget.BoundingBox.Height;

            int tileSize = project.TileSize;

            int startCellX = x / tileSize;
            int startCellY = y / tileSize;
            int endCellX = (x + w) / tileSize;
            int endCellY = (y + h) / tileSize;

            LayerDto layer = GetLayerById(mapWidget.OwnerId, project);

            if (layer != null)
            {
                if (startCellX < 0)
                {
                    startCellX = 0;
                }
                else if (startCellX >= layer.Cols)
                {
                    startCellX = layer.Cols - 1;
                }

                if (startCellY < 0)
                {
                    startCellY = 0;
                }
                else if (startCellY >= layer.Rows)
                {
                    startCellY = layer.Rows - 1;
                }

                if (endCellX < 0)
                {
                    endCellX = 0;
                }
                else if (endCellX >= layer.Cols)
                {
                    endCellX = layer.Cols - 1;
                }

                if (endCellY < 0)
                {
                    endCellY = 0;
                }
                else if (endCellY >= layer.Rows)
                {
                    endCellY = layer.Rows - 1;
                }

                for (int i = startCellY; i <= endCellY; i++)
                {
                    for (int j = startCellX; j <= endCellX; j++)
                    {
                        // Each grid cell in the layer contains a list of map widgets that are in it.                    
                        layer.MapWidgetIds[i][j].Add(mapWidget.Id);
                        layer.MapWidgetIdsByType[mapWidget.Type][i][j].Add(mapWidget.Id);

                        // And in the reverse direction, each entity contains a list of grid cells it is in.
                        mapWidget.GridCells.Add(new Point2D(j, i));
                    }
                }
            }
        }

        public void AddMapWidgetToRoom(MapWidgetDto mapWidget, ProjectDto project)
        {
            RoomDto room = GetRoom(mapWidget.RootOwnerId, project);

            // Each grid cell in the layer contains a list of map widgets that are in it.                    
            room.MapWidgetIds.Add(mapWidget.Id);
            room.MapWidgetIdsByType[mapWidget.Type].Add(mapWidget.Id);
        }

        public LayerDto GetLayerById(Guid layerId, ProjectDto project)
        {
            foreach (KeyValuePair<Guid, List<LayerDto>> layerList in project.Layers)
            {
                foreach (LayerDto layer in layerList.Value)
                {
                    if (layer.Id == layerId)
                    {
                        return layer;
                    }
                }
            }

            return null;
        }

        public RoomDto GetRoom(Guid roomId, ProjectDto project)
        {
            int index = GetRoomIndexFromId(roomId, project);

            return project.Rooms[index];
        }

        public int GetRoomIndexFromId(Guid roomId, ProjectDto project)
        {
            for (int i = 0; i < project.Rooms.Count; i++)
            {
                if (roomId == project.Rooms[i].Id)
                {
                    return i;
                }
            }

            return -1;
        }

        public void RemoveMapWidgetFromGrid(MapWidgetDto mapWidget, ProjectDto project)
        {
            RoomDto room = GetRoom(mapWidget.RootOwnerId, project);

            if (room.MapWidgetIds.Contains(mapWidget.Id))
            {
                room.MapWidgetIds.Remove(mapWidget.Id);
                room.MapWidgetIdsByType[mapWidget.Type].Remove(mapWidget.Id);
            }

            for (int i = 0; i < mapWidget.GridCells.Count; i++)
            {
                LayerDto layer = GetLayerById(mapWidget.OwnerId, project);

                int x = mapWidget.GridCells[i].X;
                int y = mapWidget.GridCells[i].Y;

                layer.MapWidgetIds[y][x].Remove(mapWidget.Id);
                layer.MapWidgetIdsByType[mapWidget.Type][y][x].Remove(mapWidget.Id);
            }

            mapWidget.GridCells.Clear();
        }

        public void RemoveMapWidgetFromRoom(MapWidgetDto mapWidget, ProjectDto project)
        {
            for (int i = 0; i < mapWidget.GridCells.Count; i++)
            {
                RoomDto room = GetRoom(mapWidget.RootOwnerId, project);

                int x = mapWidget.GridCells[i].X;
                int y = mapWidget.GridCells[i].Y;

                room.MapWidgetIds.Remove(mapWidget.Id);
                room.MapWidgetIdsByType[mapWidget.Type].Remove(mapWidget.Id);
            }

            mapWidget.GridCells.Clear();
        }
    }
}
