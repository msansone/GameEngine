using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace FiremelonEditor2
{
    public class RoomDto : BaseDto
    {
        public RoomDto()
        {
            foreach (MapWidgetType mapWidgetType in Enum.GetValues(typeof(MapWidgetType)))
            {
                mapWidgetIdsByType_.Add(mapWidgetType, new List<Guid>());
            }
        }

        private Guid loadingScreenId_ = Guid.Empty;
        public Guid LoadingScreenId
        {
            get { return loadingScreenId_; }
            set { loadingScreenId_ = value; }
        }

        private Guid transitionId_ = Guid.Empty;
        public Guid TransitionId
        {
            get { return transitionId_; }
            set { transitionId_ = value; }
        }
        
        private List<Guid> mapWidgetIds_ = new List<Guid>();
        public List<Guid> MapWidgetIds
        {
            get { return mapWidgetIds_; }
        }
        
        private Dictionary<MapWidgetType,List<Guid>> mapWidgetIdsByType_ = new Dictionary<MapWidgetType, List<Guid>>();
        public Dictionary<MapWidgetType, List<Guid>> MapWidgetIdsByType
        {
            get { return mapWidgetIdsByType_; }
        }
    }

    public class RoomDtoProxy : IRoomDtoProxy
    {
        IProjectController projectController_;
        Guid roomId_;

        public RoomDtoProxy(IProjectController projectController, Guid roomId)
        {
            projectController_ = projectController;
            roomId_ = roomId;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                RoomDto room = projectController_.GetRoom(roomId_);

                return room.Name;
            }
            set
            {
                try
                {
                    projectController_.SetRoomName(roomId_, value);
                }
                catch (InvalidNameException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        [TypeConverter(typeof(LoadingScreenConverter))]
        public string LoadingScreen
        {
            get
            {
                RoomDto room = projectController_.GetRoom(roomId_);

                Guid loadingScreenId = room.LoadingScreenId;

                if (loadingScreenId == Guid.Empty)
                {
                    return string.Empty;
                }
                else
                {
                    LoadingScreenDto loadingScreen = projectController_.GetLoadingScreen(loadingScreenId);

                    return loadingScreen.Name;
                }
            }
            set
            {
                RoomDto room = projectController_.GetRoom(roomId_);

                Guid loadingScreenId = projectController_.GetLoadingScreenIdFromName(value);

                projectController_.SetRoomLoadingScreen(roomId_, loadingScreenId);
            }
        }

        [TypeConverter(typeof(TransitionConverter))]
        public string Transition
        {
            get
            {
                RoomDto room = projectController_.GetRoom(roomId_);

                Guid transitionId = room.TransitionId;

                if (transitionId == Guid.Empty)
                {
                    return string.Empty;
                }
                else
                {
                    TransitionDto transition = projectController_.GetTransition(transitionId);

                    return transition.Name;
                }
            }
            set
            {
                RoomDto room = projectController_.GetRoom(roomId_);

                Guid transitionId = projectController_.GetTransitionIdFromName(value);

                projectController_.SetRoomTransition(roomId_, transitionId);
            }
        }

        [CategoryAttribute("Data Source Settings"),
        DescriptionAttribute("The location of the python script file"),
        Editor(typeof(PythonScriptFilePathUiTypeEditor), typeof(UITypeEditor))]
        public string Script
        {
            get
            {
                ScriptDto script = projectController_.GetScriptByOwnerId(roomId_);

                if (script == null)
                {
                    return string.Empty;
                }
                else
                {
                    return script.ScriptPath;
                }
            }
            set
            {
                ScriptDto script = projectController_.GetScriptByOwnerId(roomId_);

                projectController_.SetScriptPath(script.Id, value);
            }
        }

        public ScriptDto GetScript()
        {
            ScriptDto script = projectController_.GetScriptByOwnerId(roomId_);

            return script;
        }

        [BrowsableAttribute(false)]
        public Guid Id
        {
            get
            {
                return roomId_;
            }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                RoomDto room = projectController_.GetRoom(roomId_);

                return room.OwnerId;
            }
        }
    }

    public interface IRoomDtoProxy : IBaseDtoProxy
    {
        ScriptDto GetScript();
    }
}
