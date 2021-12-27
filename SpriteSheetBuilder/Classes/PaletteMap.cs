﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace SpriteSheetBuilder
{
    public class PaletteMap
    {
        #region Constructors

        public PaletteMap()
        {

        }

        #endregion

        #region Private Variables

        private Dictionary<Color, Color> _paletteMap = new Dictionary<Color, Color>();

        #endregion

        #region Properties
        
        public Dictionary<Color, Color> ColorMap
        {
            get { return _paletteMap; }
        }
        
        #endregion

        #region Public Functions

        public void Add(Color key, Color value)
        {
            _paletteMap.Add(key, value);
        }

        public void Clear()
        {
            _paletteMap.Clear();
        }

        public bool ContainsKey(Color key)
        {
            return _paletteMap.ContainsKey(key);
        }

        public void Deserialize(string data)
        {
            string[] colorMaps = data.Split(',');

            foreach (string colorMap in colorMaps)
            {
                string[] colors = colorMap.Split(':');

                string[] fromColorComponents = colors[0].Split(' ');

                string[] toColorComponents = colors[1].Split(' ');

                Color fromColor = Color.FromArgb(255,
                                                 Convert.ToInt32(fromColorComponents[0]),
                                                 Convert.ToInt32(fromColorComponents[1]),
                                                 Convert.ToInt32(fromColorComponents[2]));

                Color toColor = Color.FromArgb(255,
                                               Convert.ToInt32(fromColorComponents[0]),
                                               Convert.ToInt32(fromColorComponents[1]),
                                               Convert.ToInt32(fromColorComponents[2]));

                if (_paletteMap.ContainsKey(fromColor) == false)
                {
                    _paletteMap.Add(fromColor, toColor);
                }
                else
                {
                    // Not sure how this would happen, other than manually editing the file data, but I did see it happen somehow.
                    bool debug = true;
                }

                System.Diagnostics.Debug.Print(_paletteMap.Count.ToString());
            }
        }

        public string Serialize()
        {
            string returnVal = string.Empty;

            foreach (KeyValuePair<Color, Color> colorMap in _paletteMap)
            {
                if (string.IsNullOrEmpty(returnVal) == false)
                {
                    returnVal += ",";
                }

                Color fromColor = colorMap.Key;
                Color toColor = colorMap.Value;

                returnVal += fromColor.R.ToString() + " " +
                                fromColor.G.ToString() + " " +
                                fromColor.B.ToString() + " " +
                                fromColor.A.ToString() + ":" +
                                toColor.R.ToString() + " " +
                                toColor.G.ToString() + " " +
                                toColor.B.ToString() + " " +
                                toColor.A.ToString();
            }

            return returnVal;
        }

        public override string ToString()
        {
            return string.Empty;
        }

        #endregion

        #region Protected Functions
        #endregion

        #region Private Functions
        #endregion

        #region Event Handlers
        #endregion
    }
}
