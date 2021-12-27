using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;

namespace FiremelonEditor2
{
    public static class Globals
    {
        static Globals()
        {
        }

        public static string executableName = "Firemelon.exe";
        public static string executableWithConsoleName = "Firemelon_Console.exe";
        
        public static bool waitForMenuClose = false;

        public static Pen pMouse = new Pen(Color.RoyalBlue, 2.0f);
        public static Pen pMouseDownDelete = new Pen(Color.Red, 2.0f);
        public static Pen pMouseDownCreate = new Pen(Color.ForestGreen, 2.0f);

        public static Pen pTileSelector = new Pen(Color.Blue, 2.0f);
        public static SolidBrush bTileSelector = new SolidBrush(Color.FromArgb(110, 0, 0, 255));

        public static Pen pSpawnPoint = new Pen(Color.Red, 1.0f);

        public static Pen pWorldGeometryCursorOutline = new Pen(Color.Black, 1.0f);
        public static SolidBrush bWorldGeometryCursorFill = new SolidBrush(Color.White);

        public static Pen pWorldGeometryWidgetOutline = new Pen(Color.Black, 2.0f);
        public static Brush bWorldGeometryWidgetFill = new HatchBrush(HatchStyle.DiagonalBrick, Color.Black, Color.FromArgb(128, 128, 128, 128));

        public static Pen pSnapArrow = new Pen(Color.Yellow, 2.0f);
        public static Brush bSnapArrow = new SolidBrush(Color.Yellow);

        public static Pen pParticleEmitter = new Pen(Color.DarkBlue, 1.0f);
        public static Brush bParticleEmitter = new SolidBrush(Color.White);

        public static Pen pAnimationSlotOutline = new Pen(Color.Orange, 1.0f) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
        public static Pen pAnimationSlotOriginPoint = new Pen(Color.Red, 1.0f);

        public static Pen pStageOriginPoint = new Pen(Color.Magenta, 1.0f);
        
        public static Color actorSelectorOutline = Color.Green;
        public static Color actorSelectorFill = Color.FromArgb(110, 0, 128, 0);
        public static Color eventSelectorOutline = Color.FromArgb(255, 128, 0, 128);
        public static Color eventSelectorFill = Color.FromArgb(110, 0, 0, 255);

        public static Color grabberOutlineColor = Color.Black;
        public static Color grabberActiveFillColor = Color.White;
        public static Color grabberInactiveFillColor = Color.Gray;
        public static Color actorOutlineColor = Color.FromArgb(255, 0, 255, 0);
        public static Color actorFillColor = Color.FromArgb(110, 0, 255, 0);
        public static Color eventOutlineColor = Color.Magenta;
        public static Color eventFillColor = Color.FromArgb(110, 255, 0, 255);
        public static Color cameraViewportColor = Color.Blue;
        public static Color audioMinimumDistanceColor = Color.FromArgb(255, 255, 0, 0);
        public static Color audioMaximumDistanceColor = Color.FromArgb(255, 0, 0, 255);

        public static int selectedEntityInstanceCount = 0;
        public static int minimumEventSize = 16;
        public static int minimumTileSize = 8;
        public static int minimumCameraWidth = 640;
        public static int minimumCameraHeight = 480;
        public static int grabberSize = 8;

        public static int maxImageListWidth = 256;
        public static int maxImageListHeight = 256;

        public static Guid UiManagerId = new Guid("{1F5ECFC2-6BDF-4B26-BEE9-8E96B3D54C92}");
        public static Guid NetworkHandlerId = new Guid("{A82EF81D-CF10-4657-BB38-87115F1A73EB}");
        public static Guid CameraScriptId = new Guid("{4A3CB8C9-7BA1-4C20-8BC6-2819B49C0467}");
        public static Guid EngineScriptId = new Guid("{34A2FF9A-C043-4902-9EF4-E312BFA0A689}");
        public static Guid PanelsJsonFileId = new Guid("{5FA6B863-C4DF-403B-9A9D-7B502A7308B7}");        
    }
}