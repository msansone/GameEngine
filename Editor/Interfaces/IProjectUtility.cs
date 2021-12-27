using System;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public interface IProjectUtility
    {
        void AddMapWidgetToGrid(MapWidgetDto mapWidget, ProjectDto project);

        void AddMapWidgetToRoom(MapWidgetDto mapWidget, ProjectDto project);

        LayerDto GetLayerById(Guid layerId, ProjectDto project);

        RoomDto GetRoom(Guid roomId, ProjectDto project);

        int GetRoomIndexFromId(Guid roomId, ProjectDto project);

        void RemoveMapWidgetFromGrid(MapWidgetDto mapWidget, ProjectDto project);

        void RemoveMapWidgetFromRoom(MapWidgetDto mapWidget, ProjectDto project);
    }
}
