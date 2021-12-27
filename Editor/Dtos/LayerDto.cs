using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FiremelonEditor2
{
    public class LayerDto : BaseDto
    {
        public LayerDto(string name, int columns, int rows)
        {
            base.Name = name;

            cols_ = columns;
            rows_ = rows;

            foreach (MapWidgetType mapWidgetType in Enum.GetValues(typeof(MapWidgetType)))
            {
                mapWidgetIdsByType_.Add(mapWidgetType, new List<List<List<Guid>>>());
            }

            for (int i = 0; i < rows_; i++)
            {
                mapWidgetIds_.Add(new List<List<Guid>>());

                foreach (MapWidgetType mapWidgetType in Enum.GetValues(typeof(MapWidgetType)))
                {
                    mapWidgetIdsByType_[mapWidgetType].Add(new List<List<Guid>>());
                }

                for (int j = 0; j < cols_; j++)
                {
                    mapWidgetIds_[i].Add(new List<Guid>());

                    foreach (MapWidgetType mapWidgetType in Enum.GetValues(typeof(MapWidgetType)))
                    {
                        mapWidgetIdsByType_[mapWidgetType][i].Add(new List<Guid>());
                    }
                }
            }
        }

        private int cols_;
        public int Cols
        {
            get 
            { 
                return cols_; 
            }
            set 
            { 
                int oldColumns = cols_;

                if (oldColumns != value)
                {
                    cols_ = value;

                    // If the number of columns has shrunk, remove the extra columns.
                    // if it has grown, add new columns
                    if (cols_ < oldColumns)
                    {
                        for (int i = 0; i < rows_; i++)
                        {
                            for (int j = oldColumns - 1; j >= cols_; j--)
                            {
                                mapWidgetIds_[i].RemoveAt(j);

                                foreach (MapWidgetType mapWidgetType in Enum.GetValues(typeof(MapWidgetType)))
                                {
                                    mapWidgetIdsByType_[mapWidgetType][i].RemoveAt(j);
                                }
                            }
                        }
                    }
                    else if (cols_ > oldColumns)
                    {
                        for (int i = 0; i < rows_; i++)
                        {
                            for (int j = oldColumns; j < cols_; j++)
                            {
                                mapWidgetIds_[i].Add(new List<Guid>());

                                foreach (MapWidgetType mapWidgetType in Enum.GetValues(typeof(MapWidgetType)))
                                {
                                    mapWidgetIdsByType_[mapWidgetType][i].Add(new List<Guid>());
                                }
                            }
                        }
                    }
                }
            }
        }

        private int rows_;
        public int Rows
        {
            get 
            { 
                return rows_; 
            }
            set 
            { 
                int oldRows = rows_;

                rows_ = value;

                if (rows_ < oldRows)
                {
                    for (int i = oldRows - 1; i >= rows_; i--)
                    {
                        mapWidgetIds_.RemoveAt(i);

                        foreach (MapWidgetType mapWidgetType in Enum.GetValues(typeof(MapWidgetType)))
                        {
                            mapWidgetIdsByType_[mapWidgetType].RemoveAt(i);
                        }
                    }
                }
                else if (rows_ > oldRows)
                {
                    for (int i = oldRows; i < rows_; i++)
                    {
                        mapWidgetIds_.Add(new List<List<Guid>>());

                        foreach (MapWidgetType mapWidgetType in Enum.GetValues(typeof(MapWidgetType)))
                        {
                            mapWidgetIdsByType_[mapWidgetType].Add(new List<List<Guid>>());
                        }

                        for (int j = 0; j < cols_; j++)
                        {
                            mapWidgetIds_[i].Add(new List<Guid>());

                            foreach (MapWidgetType mapWidgetType in Enum.GetValues(typeof(MapWidgetType)))
                            {
                                mapWidgetIdsByType_[mapWidgetType][i].Add(new List<Guid>());
                            }
                        }
                    }
                }
            }
        }
        
        private int adjustmentX_;
        public int AdjustmentX
        {
            get { return adjustmentX_; }
            set { adjustmentX_ = value; }
        }

        private int adjustmentY_;
        public int AdjustmentY
        {
            get { return adjustmentY_; }
            set { adjustmentY_ = value; }
        }

        // Deprecated, but still needs to exist for project upgrader.
        private List<List<List<Guid>>> actorIds_ = new List<List<List<Guid>>>();
        public List<List<List<Guid>>> ActorIds
        {
            get { return actorIds_; }
        }

        // Deprecated but still needs to exist for project upgrader.
        private List<List<List<Guid>>> eventIds_ = new List<List<List<Guid>>>();
        public List<List<List<Guid>>> EventIds
        {
            get { return eventIds_; }
        }

        // This is a dictionary which maps a widget type ID to a 3D array of GUIDs (Widget IDs).
        // The 3D array dimensions are [Row][Column][WidgetId]
        private List<List<List<Guid>>> mapWidgetIds_ = new List<List<List<Guid>>>();
        public List<List<List<Guid>>> MapWidgetIds
        {
            get { return mapWidgetIds_; }
        }

        // This is a dictionary which maps a widget type ID to a 3D array of GUIDs (Widget IDs).
        // The 3D array dimensions are [Row][Column][WidgetId]
        private Dictionary<MapWidgetType, List<List<List<Guid>>>> mapWidgetIdsByType_ = new Dictionary<MapWidgetType, List<List<List<Guid>>>>();
        public Dictionary<MapWidgetType, List<List<List<Guid>>>> MapWidgetIdsByType
        {
            get { return mapWidgetIdsByType_; }
        }

        private Guid bitmapResourceId_ = Guid.Empty;
        public Guid BitmapResourceId
        {
            get { return bitmapResourceId_; }
            set { bitmapResourceId_ = value; }
        }
    }
}
