namespace FiremelonEditor2
{
    public enum MapWidgetType
    {
        SpawnPoint = 0,
        ParticleEmitter = 1,
        AudioSource = 2,
        None = 3,
        WorldGeometry = 4,
        TileObject = 5,
        Event = 6,
        Actor = 7,
        HudElement
    }

    public class MapWidgetFactory : IMapWidgetFactory
    {
        #region Constructors

        public MapWidgetFactory(IProjectController projectController)
        {
            projectController_ = projectController;
        }

        #endregion

        #region Private Variables

        IProjectController projectController_;

        #endregion

        #region Properties

        public int TileSize
        {
            set { tileSize_ = value; }
        }
        private int tileSize_;

        #endregion

        #region Public Functions

        public MapWidgetDto CreateMapWidget(MapWidgetCreationParametersDto creationParams)
        {
            MapWidgetDto newMapWidget = null;

            // Add Map Widgets Here
            switch (creationParams.Type)
            {
                case MapWidgetType.SpawnPoint:

                    newMapWidget = new SpawnPointWidgetDto();

                    newMapWidget.Controller = new SpawnPointWidgetController(projectController_);

                    break;

                case MapWidgetType.ParticleEmitter:
            
                    newMapWidget = new ParticleEmitterWidgetDto();

                    newMapWidget.Controller = new ParticleEmitterWidgetController(projectController_);

                    break;

                case MapWidgetType.AudioSource:

                    newMapWidget = new AudioSourceWidgetDto();

                    newMapWidget.Controller = new AudioSourceWidgetController(projectController_);

                    break;

                case MapWidgetType.WorldGeometry:

                    newMapWidget = new WorldGeometryWidgetDto();

                    newMapWidget.Controller = new WorldGeometryWidgetController(projectController_, tileSize_);

                    break;

                case MapWidgetType.TileObject:

                    newMapWidget = new TileObjectWidgetDto();

                    TileObjectMapWidgetCreationParametersDto tileObjectCreationParams = (TileObjectMapWidgetCreationParametersDto)creationParams;
                    
                    newMapWidget.Controller = new TileObjectWidgetController(projectController_, tileObjectCreationParams.TileObjectId);
                  
                    break;

                case MapWidgetType.Event:

                    newMapWidget = new EventWidgetDto();

                    EventMapWidgetCreationParametersDto eventWidgetCreationParams = (EventMapWidgetCreationParametersDto)creationParams;

                    newMapWidget.Controller = new EventWidgetController(projectController_, eventWidgetCreationParams.EventId);

                    ((EventWidgetDto)newMapWidget).EntityId = eventWidgetCreationParams.EventId;

                    break;

                case MapWidgetType.Actor:

                    newMapWidget = new ActorWidgetDto();

                    ActorMapWidgetCreationParametersDto actorWidgetCreationParams = (ActorMapWidgetCreationParametersDto)creationParams;

                    newMapWidget.Controller = new ActorWidgetController(projectController_, actorWidgetCreationParams.ActorId);

                    ((ActorWidgetDto)newMapWidget).EntityId = actorWidgetCreationParams.ActorId;

                    break;

                case MapWidgetType.HudElement:

                    newMapWidget = new HudElementWidgetDto();

                    HudElementMapWidgetCreationParametersDto hudElementWidgetCreationParams = (HudElementMapWidgetCreationParametersDto)creationParams;

                    newMapWidget.Controller = new HudElementWidgetController(projectController_, hudElementWidgetCreationParams.HudElementId);

                    ((HudElementWidgetDto)newMapWidget).EntityId = hudElementWidgetCreationParams.HudElementId;

                    break;
            }

            newMapWidget.Controller.MapWidget = newMapWidget;

            Rectangle bounds = newMapWidget.Controller.GetBoundingRect();

            newMapWidget.BoundingBox.Left = bounds.Left;
            newMapWidget.BoundingBox.Top = bounds.Top;
            newMapWidget.BoundingBox.Width = bounds.Width;
            newMapWidget.BoundingBox.Height = bounds.Height;

            newMapWidget.Type = creationParams.Type;
            
            return newMapWidget;
        }

        #endregion
    }

    public interface IMapWidgetFactory
    {
        int TileSize { set; }

        MapWidgetDto CreateMapWidget(MapWidgetCreationParametersDto creationParams);
    }
}
