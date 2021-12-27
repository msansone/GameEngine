using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    #region Enums

    public enum ImageSourceType
    {
        Single = 1,
        Strip = 2,
        Sheet = 3
    }
    
    #endregion

    public partial class SpriteSheetBuilderForm : Form, ISpriteSheetBuilderForm
    {
        #region Constructors

        public SpriteSheetBuilderForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Variables

        private string buildFileName_ = string.Empty;

        private bool changesMade_ = false;

        private Bitmap bmpSpriteSheet_;

        private SpriteSheetBuilderDto spriteSheetBuilderDto_ = new SpriteSheetBuilderDto();

        #endregion

        #region Public Functions

        new public void ShowDialog(IWin32Window owner)
        {
            base.ShowDialog(owner);
        }

        #endregion

        #region Private Functions

        private void addImages(ImageSourceType imageSourceType)
        {
            ofdAddImages.CheckFileExists = true;
            ofdAddImages.CheckPathExists = true;
            ofdAddImages.DefaultExt = "png";
            ofdAddImages.Filter = "PNG Files|*.png";
            ofdAddImages.FileName = string.Empty;
            ofdAddImages.Multiselect = true;
            ofdAddImages.RestoreDirectory = true;

            if (ofdAddImages.ShowDialog() == DialogResult.OK)
            {
                foreach (string filename in ofdAddImages.FileNames)
                {
                    SheetImageSourceDto newSheetImageSource = new SheetImageSourceDto(filename);

                    Bitmap spriteImage;

                    switch (imageSourceType)
                    {
                        case ImageSourceType.Single:

                            newSheetImageSource.CellCount = 1;

                            newSheetImageSource.Columns = 1;

                            // Get the height and width from the image.
                            spriteImage = new Bitmap(filename);

                            newSheetImageSource.CellWidth = spriteImage.Width;

                            newSheetImageSource.CellHeight = spriteImage.Height;

                            spriteImage.Dispose();

                            break;

                        case ImageSourceType.Strip:
                            
                            // Get the height and width from the image.
                            spriteImage = new Bitmap(filename);
                            
                            newSheetImageSource.CellHeight = spriteImage.Height;

                            spriteImage.Dispose();

                            break;
                    }

                    extractPaletteFromImage(filename);

                    spriteSheetBuilderDto_.ImageSourceList.Add(newSheetImageSource);

                    lstbxFiles.Items.Add(filename);

                    changesMade_ = true;
                }
            }
        }
        
        private void exportSheet()
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();

                saveDialog.AddExtension = true;
                saveDialog.RestoreDirectory = true;
                saveDialog.DefaultExt = "png";
                saveDialog.Filter = "PNG Files|*.png";

                // Bring up the save dialog if it has not been saved yet, or if save as was clicked.
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    bmpSpriteSheet_.Save(saveDialog.FileName);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        
        private void extractPaletteFromImage(string filename)
        {
            Bitmap spriteImage = new Bitmap(filename);

            for (int x = 0; x < spriteImage.Width; x++)
            {
                for (int y = 0; y < spriteImage.Height; y++)
                {
                    Color pixelColor = spriteImage.GetPixel(x, y);

                    if (spriteSheetBuilderDto_.PaletteMap.ContainsKey(pixelColor) == false)
                    {
                        spriteSheetBuilderDto_.PaletteMap.Add(pixelColor, pixelColor);
                    }
                }
            }

            spriteImage.Dispose();
        }

        private void newSpriteSheet()
        {
            // Create a new sprite sheet builder dto and clear the UI.
            spriteSheetBuilderDto_ = new SpriteSheetBuilderDto();
            
            lstbxFiles.Items.Clear();

            pgSpriteSheet.SelectedObject = spriteSheetBuilderDto_;

            tsbAddImages.Enabled = true;
            fromSheetToolStripMenuItem.Enabled = true;
            singleImageToolStripMenuItem.Enabled = true;
            stripToolStripMenuItem.Enabled = true;
            tsbExportSpriteSheet.Enabled = true;

            addImagesToolStripMenuItem.Enabled = true;
            buildSpriteSheetToolStripMenuItem.Enabled = true;
            saveSpriteSheetBuildFileToolStripMenuItem.Enabled = true;
            buildAlphaMaskSheetToolStripMenuItem.Enabled = true;

            pgSpriteSheet.Enabled = true;

            changesMade_ = false;

            buildFileName_ = string.Empty;
        }
        
        private void resize()
        {
            pbSheetPreview.Width = scImages.Panel2.ClientSize.Width - vsSpriteSheet.Width - 1;
            pbSheetPreview.Height = scImages.Panel2.ClientSize.Height - hsSpriteSheet.Height - 1;

            hsSpriteSheet.Top = pbSheetPreview.ClientSize.Height;
            hsSpriteSheet.Width = pbSheetPreview.ClientSize.Width;

            vsSpriteSheet.Left = pbSheetPreview.ClientSize.Width;
            vsSpriteSheet.Height = pbSheetPreview.ClientSize.Height;

            if (bmpSpriteSheet_ != null)
            {
                int vScrollMax = bmpSpriteSheet_.Height - pbSheetPreview.Height;

                if (vScrollMax > 0)
                {
                    vsSpriteSheet.Maximum = vScrollMax;
                }
                else
                {
                    vsSpriteSheet.Maximum = vsSpriteSheet.Minimum;
                }

                int hScrollMax = bmpSpriteSheet_.Width - pbSheetPreview.Width;

                if (hScrollMax > 0)
                {
                    hsSpriteSheet.Maximum = hScrollMax;
                }
                else
                {
                    hsSpriteSheet.Maximum = hsSpriteSheet.Minimum;
                }
            }

            pbSheetPreview.Refresh();
        }

        private void saveBuildFile()
        {
            try
            {
                string defaultName = String.Empty;

                bool showDialog = false;
                
                if (String.IsNullOrEmpty(buildFileName_) == true)
                {
                    showDialog = true;
                }
                else
                {
                    defaultName = buildFileName_;
                }

                if (showDialog == true)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog();

                    saveDialog.DefaultExt = "build";
                    saveDialog.AddExtension = true;
                    saveDialog.RestoreDirectory = true;
                    saveDialog.Filter = "Sprite Sheet Build Files (*.build)|*.build";
                    saveDialog.FileName = Path.GetFileName(defaultName);
                    //saveDialog.InitialDirectory = project.ProjectFolderFullPath;

                    // Bring up the save dialog if it has not been saved yet, or if save as was clicked.
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        buildFileName_ = saveDialog.FileName;                        
                    }
                    else
                    {
                        return;
                    }
                }
                
                try
                {
                    string fileContents = string.Empty;

                    int headerFields = 8;
                    int fieldsPerFile = 5;
                    fileContents += headerFields.ToString() + Environment.NewLine;
                    fileContents += fieldsPerFile.ToString() + Environment.NewLine;
                    fileContents += spriteSheetBuilderDto_.CellHeight.ToString() + Environment.NewLine;
                    fileContents += spriteSheetBuilderDto_.CellWidth.ToString() + Environment.NewLine;
                    fileContents += spriteSheetBuilderDto_.Columns.ToString() + Environment.NewLine;
                    fileContents += Convert.ToUInt32(spriteSheetBuilderDto_.VerticalAlignment).ToString() + Environment.NewLine;
                    fileContents += Convert.ToUInt32(spriteSheetBuilderDto_.HorizontalAlignment).ToString() + Environment.NewLine; ;
                    fileContents += spriteSheetBuilderDto_.PaletteMap.Serialize();


                    // Rows can be calculated implicitly using the total images and the columns.

                    foreach (SheetImageSourceDto imageSource in spriteSheetBuilderDto_.ImageSourceList)
                    {
                        fileContents += Environment.NewLine + imageSource.FileName;
                        fileContents += Environment.NewLine + imageSource.CellWidth.ToString();
                        fileContents += Environment.NewLine + imageSource.CellHeight.ToString();
                        fileContents += Environment.NewLine + imageSource.Columns.ToString();
                        fileContents += Environment.NewLine + imageSource.CellCount.ToString();
                    }

                    File.WriteAllText(buildFileName_, fileContents);

                    changesMade_ = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Error Saving Sprite Sheet Build File", MessageBoxButtons.OK);
                }
                finally
                {
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void openBuildFile()
        {
            if (changesMade_ == true)
            {
                DialogResult res = MessageBox.Show("Changes have been made to the current sprite sheet build file. Do you want to save?", "Save Changes?", MessageBoxButtons.YesNoCancel);

                if (res == DialogResult.Yes)
                {
                    saveBuildFile();
                }
                else if (res == DialogResult.Cancel)
                {
                    return;
                }
            }

            OpenFileDialog openDialog = new OpenFileDialog();

            openDialog.CheckFileExists = true;
            openDialog.CheckPathExists = true;
            openDialog.DefaultExt = "build";
            openDialog.Filter = "Sprite Sheet Build Files (*.build)|*.build";
            openDialog.Multiselect = false;
            openDialog.RestoreDirectory = true;

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                string fileConents = File.ReadAllText(openDialog.FileName);

                string[] fileData = fileConents.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                string errorMessage = "Unable to open the sprite sheet build file. It's either missing data or incorrectly formatted.";

                // There should be a minimum of 3 lines, for the height, width, rows, and columns values.
                if (fileData.Length < 3)
                {
                    MessageBox.Show(errorMessage);

                    return;
                }
                
                try
                {
                    newSpriteSheet();
                    
                    int headerSize = Convert.ToInt32(fileData[0]);
                    int fieldsPerFile = Convert.ToInt32(fileData[1]);

                    spriteSheetBuilderDto_.PaletteMap.Clear();

                    spriteSheetBuilderDto_.CellHeight = Convert.ToInt32(fileData[2]);
                    spriteSheetBuilderDto_.CellWidth = Convert.ToInt32(fileData[3]);
                    spriteSheetBuilderDto_.Columns = Convert.ToInt32(fileData[4]);
                    spriteSheetBuilderDto_.VerticalAlignment = (SheetCellVerticalAlignment)Convert.ToUInt32(fileData[5]);
                    spriteSheetBuilderDto_.HorizontalAlignment = (SheetCellHorizontalAlignment)Convert.ToUInt32(fileData[6]);

                    if (headerSize >= 8)
                    {
                        spriteSheetBuilderDto_.PaletteMap.Deserialize(fileData[7]);
                    }

                    bool extractPalette = false;

                    if (headerSize < 8)
                    {
                        // Palette info does not exist. Extract it during the load.
                        extractPalette = true;
                    }
                    
                    for (int i = headerSize; i < fileData.Length; i += fieldsPerFile)
                    {
                        string fileName = fileData[i];

                        SheetImageSourceDto sheetImageSource = new SheetImageSourceDto(fileName);

                        if (fieldsPerFile > 1)
                        {
                            sheetImageSource.CellWidth = Convert.ToInt32(fileData[i + 1]);
                            sheetImageSource.CellHeight = Convert.ToInt32(fileData[i + 2]);
                            sheetImageSource.Columns = Convert.ToInt32(fileData[i + 3]);
                            sheetImageSource.CellCount = Convert.ToInt32(fileData[i + 4]);
                        }

                        spriteSheetBuilderDto_.ImageSourceList.Add(sheetImageSource);

                        lstbxFiles.Items.Add(fileName);

                        if (extractPalette == true)
                        {
                            extractPaletteFromImage(fileName);                                                     
                        }
                        
                        //left to do
                        //4. When exporting, check if a palette map is used and generate a new image with the mapped colors.
                    }

                    GC.Collect();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(errorMessage);
                }
                
                buildFileName_ = openDialog.FileName;

                pgSpriteSheet.SelectedObject = spriteSheetBuilderDto_;
            }
        }

        #endregion

        #region Event Handlers
        
        private void buildAlphaMaskSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Build can only happen if all parameters are set.
            if (spriteSheetBuilderDto_.CellHeight == 0 || spriteSheetBuilderDto_.CellWidth == 0 || spriteSheetBuilderDto_.Columns == 0)
            {
                MessageBox.Show("Insufficient build parameters. Make sure the cell width, cell height, and columns parameters are set.");

                return;
            }

            if (spriteSheetBuilderDto_.ImageSourceList.Count == 0)
            {
                MessageBox.Show("No image sources have been added to the sprite sheet. At least one image source must exist.");

                return;
            }

            // Dispose of the old bitmap object.
            if (bmpSpriteSheet_ != null)
            {
                bmpSpriteSheet_.Dispose();
            }

            // Need to do an initial pass to determine how many images there are in total, from all sources.
            int imageCounter = 0;

            for (int i = 0; i < spriteSheetBuilderDto_.ImageSourceList.Count; i++)
            {
                SheetImageSourceDto imageSource = spriteSheetBuilderDto_.ImageSourceList[i];

                imageCounter += imageSource.CellCount;
            }

            int bitmapWidth = spriteSheetBuilderDto_.Columns * spriteSheetBuilderDto_.CellWidth;

            int rows = Convert.ToInt32(Math.Ceiling(((double)imageCounter / (double)spriteSheetBuilderDto_.Columns)));

            int bitmapHeight = rows * spriteSheetBuilderDto_.CellHeight;

            bmpSpriteSheet_ = new Bitmap(bitmapWidth, bitmapHeight);

            Graphics g = Graphics.FromImage(bmpSpriteSheet_);

            g.FillRectangle(new SolidBrush(Color.FromArgb(255, 0, 0, 0)), new System.Drawing.Rectangle(0, 0, bitmapWidth, bitmapHeight));

            imageCounter = 0;

            for (int i = 0; i < spriteSheetBuilderDto_.ImageSourceList.Count; i++)
            {
                SheetImageSourceDto imageSource = spriteSheetBuilderDto_.ImageSourceList[i];

                Bitmap spriteImage = new Bitmap(imageSource.FileName);

                Bitmap alphaMaskImage = new Bitmap(spriteImage.Size.Width, spriteImage.Size.Height);

                Graphics gAlphaMask = Graphics.FromImage(alphaMaskImage);

                gAlphaMask.FillRegion(new SolidBrush(Color.Magenta), new Region(new System.Drawing.Rectangle(0, 0, spriteImage.Size.Height, spriteImage.Size.Width)));

                gAlphaMask.Dispose();

                // Copy 
                for (int img_y = 0; img_y < spriteImage.Height; img_y++)
                {
                    for (int img_x = 0; img_x < spriteImage.Width; img_x++)
                    {
                        Color spriteColor = spriteImage.GetPixel(img_x, img_y);

                        // Copy the alpha channel from the sprite to the red channel of the mask.
                        Color alphaColor = Color.FromArgb(255, spriteColor.A, 0, 0);

                        // Set the alpha color pixel from the sprite bitmap to the alpha bitmap.
                        alphaMaskImage.SetPixel(img_x, img_y, alphaColor);
                    }
                }
                
                for (int j = 0; j < imageSource.CellCount; j++)
                {
                    int sourceCol = j % imageSource.Columns;

                    int sourceRow = Convert.ToInt32(Math.Floor((float)(j / imageSource.Columns)));

                    int destCol = imageCounter % spriteSheetBuilderDto_.Columns;

                    int destRow = Convert.ToInt32(Math.Floor((float)(imageCounter / spriteSheetBuilderDto_.Columns)));

                    int destX = destCol * spriteSheetBuilderDto_.CellWidth;

                    // Apply a transform for alignment.
                    switch (spriteSheetBuilderDto_.HorizontalAlignment)
                    {
                        case SheetCellHorizontalAlignment.Center:

                            destX += ((spriteSheetBuilderDto_.CellWidth / 2) - (imageSource.CellWidth / 2));

                            break;
                    }

                    int destY = destRow * spriteSheetBuilderDto_.CellHeight;

                    // Apply a transform for alignment.
                    switch (spriteSheetBuilderDto_.VerticalAlignment)
                    {
                        case SheetCellVerticalAlignment.Bottom:

                            destY += (spriteSheetBuilderDto_.CellHeight - imageSource.CellHeight);

                            break;
                    }

                    System.Drawing.Rectangle sourceRect = new System.Drawing.Rectangle(sourceCol * imageSource.CellWidth, 0, imageSource.CellWidth, imageSource.CellHeight);

                    System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(destX, destY, imageSource.CellWidth, imageSource.CellHeight);

                    //g.DrawImage(spriteImage, destRect, 0, 0, destRect.Width, destRect.Height, GraphicsUnit.Pixel, attr);
                    g.DrawImage(alphaMaskImage, destRect, sourceRect, GraphicsUnit.Pixel);

                    imageCounter++;
                }
                
                alphaMaskImage.Dispose();

                spriteImage.Dispose();
            }

            resize();

            pbSheetPreview.Refresh();

            exportSheetToolStripMenuItem.Enabled = true;

            g.Dispose();
        }

        private void buildSpriteSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {          
            // Build can only happen if all parameters are set.
            if (spriteSheetBuilderDto_.CellHeight == 0 || spriteSheetBuilderDto_.CellWidth == 0 || spriteSheetBuilderDto_.Columns == 0)
            {
                MessageBox.Show("Insufficient build parameters. Make sure the cell width, cell height, and columns parameters are set.");

                return;
            }

            if (spriteSheetBuilderDto_.ImageSourceList.Count == 0)
            {
                MessageBox.Show("No image sources have been added to the sprite sheet. At least one image source must exist.");

                return;
            }
            
            // Dispose of the old bitmap object.
            if (bmpSpriteSheet_ != null)
            {
                bmpSpriteSheet_.Dispose();
            }

            int bitmapWidth = spriteSheetBuilderDto_.Columns * spriteSheetBuilderDto_.CellWidth;
            
            // Need to do an initial pass to determine how many images there are in total, from all sources.
            int imageCounter = 0;
            
            for (int i = 0; i < spriteSheetBuilderDto_.ImageSourceList.Count; i++)
            {
                SheetImageSourceDto imageSource = spriteSheetBuilderDto_.ImageSourceList[i];
                
                imageCounter += imageSource.CellCount;
            }
            
            int rows = Convert.ToInt32(Math.Ceiling(((double)imageCounter / (double)spriteSheetBuilderDto_.Columns)));

            int bitmapHeight = rows * spriteSheetBuilderDto_.CellHeight;

            if (bitmapWidth > 0 && bitmapHeight > 0)
            {
                bmpSpriteSheet_ = new Bitmap(bitmapWidth, bitmapHeight);

                Graphics g = Graphics.FromImage(bmpSpriteSheet_);

                g.FillRectangle(new SolidBrush(Color.Magenta), new System.Drawing.Rectangle(0, 0, bitmapWidth, bitmapHeight));

                imageCounter = 0;

                for (int i = 0; i < spriteSheetBuilderDto_.ImageSourceList.Count; i++)
                {
                    SheetImageSourceDto imageSource = spriteSheetBuilderDto_.ImageSourceList[i];

                    Bitmap spriteImage = new Bitmap(imageSource.FileName);

                    // This has ugly results. I'm not sure how PNG does transparent backgrounds, but it's apparently not how I think it does.

                    //// Map a background color to magenta.
                    //ColorMap[] colorMap = new ColorMap[1];

                    //colorMap[0] = new ColorMap();

                    //colorMap[0].OldColor = Color.FromArgb(0, 255, 255, 255);

                    //colorMap[0].NewColor = Color.Magenta;

                    //ImageAttributes attr = new ImageAttributes();

                    //attr.SetRemapTable(colorMap);

                    for (int j = 0; j < imageSource.CellCount; j++)
                    {
                        int sourceCol = j % imageSource.Columns;

                        int sourceRow = Convert.ToInt32(Math.Floor((float)(j / imageSource.Columns)));

                        int destCol = imageCounter % spriteSheetBuilderDto_.Columns;

                        int destRow = Convert.ToInt32(Math.Floor((float)(imageCounter / spriteSheetBuilderDto_.Columns)));

                        int destX = destCol * spriteSheetBuilderDto_.CellWidth;

                        // Apply a transform for alignment.
                        switch (spriteSheetBuilderDto_.HorizontalAlignment)
                        {
                            case SheetCellHorizontalAlignment.Center:

                                destX += ((spriteSheetBuilderDto_.CellWidth / 2) - (imageSource.CellWidth / 2));

                                break;
                        }


                        int destY = destRow * spriteSheetBuilderDto_.CellHeight;

                        // Apply a transform for alignment
                        switch (spriteSheetBuilderDto_.VerticalAlignment)
                        {
                            case SheetCellVerticalAlignment.Bottom:

                                destY += (spriteSheetBuilderDto_.CellHeight - imageSource.CellHeight);

                                break;
                        }

                        System.Drawing.Rectangle sourceRect = new System.Drawing.Rectangle(sourceCol * imageSource.CellWidth, 0, imageSource.CellWidth, imageSource.CellHeight);

                        System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(destX, destY, imageSource.CellWidth, imageSource.CellHeight);

                        //g.DrawImage(spriteImage, destRect, 0, 0, destRect.Width, destRect.Height, GraphicsUnit.Pixel, attr);
                        g.DrawImage(spriteImage, destRect, sourceRect, GraphicsUnit.Pixel);

                        imageCounter++;
                    }

                    spriteImage.Dispose();
                }

                g.Dispose();
            }
            else
            {
                MessageBox.Show("Failed to create bitmap.");
            }

            resize();

            pbSheetPreview.Refresh();

            exportSheetToolStripMenuItem.Enabled = true;
        }

        private void fromSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addImages(ImageSourceType.Sheet);
        }

        private void hsSpriteSheet_Scroll(object sender, ScrollEventArgs e)
        {
            pbSheetPreview.Refresh();
        }

        private void lstbxFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DialogResult res = MessageBox.Show("Remove image source files from the list?", "Remove Image Source Files?", MessageBoxButtons.YesNo);

                if (res == DialogResult.Yes)
                {
                    ListBox.SelectedObjectCollection selectedItems = new ListBox.SelectedObjectCollection(lstbxFiles);

                    selectedItems = lstbxFiles.SelectedItems;

                    if (lstbxFiles.SelectedIndex != -1)
                    {
                        for (int i = selectedItems.Count - 1; i >= 0; i--)
                        {
                            spriteSheetBuilderDto_.ImageSourceList.RemoveAll(imageSource => imageSource.FileName == selectedItems[i].ToString());

                            lstbxFiles.Items.Remove(selectedItems[i]);
                        }
                    }
                }
            }
        }

        private void lstbxFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox.SelectedObjectCollection selectedItems = new ListBox.SelectedObjectCollection(lstbxFiles);

            selectedItems = lstbxFiles.SelectedItems;
            
            List<SheetImageSourceDto> lstImageSources = new List<SheetImageSourceDto>();
            
            for (int i = 0; i < lstbxFiles.SelectedIndices.Count; i++)
            {
                int selectedIndex = lstbxFiles.SelectedIndices[i];

                lstImageSources.Add(spriteSheetBuilderDto_.ImageSourceList[selectedIndex]);
            }
            
            pgImageSource.SelectedObjects = lstImageSources.ToArray();
        }

        private void newSpriteSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newSpriteSheet();
        }

        private void openSpriteSheetBuildFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openBuildFile();
        }

        private void pbSheetPreview_Paint(object sender, PaintEventArgs e)
        {
            if (bmpSpriteSheet_ != null)
            {
                Graphics g = e.Graphics;

                int hscrollOffset = -1 * hsSpriteSheet.Value;

                int vscrollOffset = -1 * vsSpriteSheet.Value;

                g.DrawImageUnscaled(bmpSpriteSheet_, new Point(hscrollOffset, vscrollOffset));                
            }
        }

        private void saveSpriteSheetBuildFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveBuildFile();
        }

        private void exportSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportSheet();
        }

        private void scImages_SplitterMoved(object sender, SplitterEventArgs e)
        {
            resize();
        }

        private void singleImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addImages(ImageSourceType.Single);
        }

        private void SpriteSheetBuilderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            this.Hide();
        }
        
        private void SpriteSheetBuilderForm_Load(object sender, EventArgs e)
        {
            pgSpriteSheet.SelectedObject = spriteSheetBuilderDto_;

            resize();
        }

        private void SpriteSheetBuilderForm_Resize(object sender, EventArgs e)
        {
            resize();
        }

        private void stripToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addImages(ImageSourceType.Strip);
        }
        
        private void vsSpriteSheet_Scroll(object sender, ScrollEventArgs e)
        {
            pbSheetPreview.Refresh();
        }

        #endregion
    }

    public interface ISpriteSheetBuilderForm
    {
        #region Properties

        // Derived from base.
        int Width { get; set; }
        int Height { get; set; }
        int Bottom { get; }
        int Left { get; set; }
        int Top { get; set; }
        bool TopLevel { get; set; }

        #endregion

        #region Public Functions

        void Show(IWin32Window owner);
        void ShowDialog(IWin32Window owner);

        void Hide();
        void Close();

        #endregion
    }
}
