namespace FiremelonEditor2
{
    public class MapWidgetControllerFactory : IMapWidgetControllerFactory
    {
        IProjectController projectController_;
        IFiremelonEditorFactory firemelonEditorFactory_;

        public MapWidgetControllerFactory(IProjectController projectController)
        {
            projectController_ = projectController;
            firemelonEditorFactory_ = new FiremelonEditorFactory();
        }

        public IMapWidgetController CreateMapWidgetController(MapWidgetType mapWidgetType)
        {
            IMapWidgetController newMapWidgetController = null;

            switch (mapWidgetType)
            {
                case MapWidgetType.SpawnPoint:
                    newMapWidgetController = firemelonEditorFactory_.NewSpawnPointWidgetController(projectController_);
                    break;

                case MapWidgetType.ParticleEmitter:
                    newMapWidgetController = firemelonEditorFactory_.NewParticleEmitterWidgetController(projectController_);
                    break;
            }

            return newMapWidgetController;
        }
    }

    public interface IMapWidgetControllerFactory
    {
        IMapWidgetController CreateMapWidgetController(MapWidgetType mapWidgetType);
    }
}
