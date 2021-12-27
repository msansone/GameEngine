using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public class WorldGeometryDto : BaseDto
    {
    }

    class WorldGeometryDtoProxy// : IWorldGeometryDtoProxy
    {
        //IProjectController projectController_;
        //Guid worldGeometryId_;

        //public WorldGeometryDtoProxy(IProjectController projectController, Guid worldGeometryId)
        //{
        //    projectController_ = projectController;
        //    worldGeometryId_ = worldGeometryId;
        //}

        //[CategoryAttribute("(ID Settings)"),
        // DescriptionAttribute("Unique Name String"),
        // ParenthesizePropertyName(true),
        // BrowsableAttribute(false)]
        //public string Name
        //{
        //    get
        //    {
        //        // You shouldn't have to set world geometry name. They're just chunks of the world, you don't access them.
        //        // Or maybe just name them World Chunk 1, World Chunk 2, etc. so their properties can be set. findmelater
        //        return string.Empty;
        //    }
        //    set
        //    {
        //    }
        //}

        //[BrowsableAttribute(false)]
        //public Guid Id
        //{
        //    get
        //    {
        //        return worldGeometryId_;
        //    }
        //}

        //[BrowsableAttribute(false)]
        //public Guid OwnerId
        //{
        //    get
        //    {
        //        SpawnPointDto spawnPoint = projectController_.GetSpawnPoint(worldGeometryId_);

        //        return spawnPoint.OwnerId;
        //    }
        //}
    }
}
