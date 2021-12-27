using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SpriteSheetBuilder
{
    public partial class SpriteSheetBuilderControl : UserControl, ISpriteSheetBuilderControl
    {
        public SpriteSheetBuilderControl()
        {
            InitializeComponent();
        }

        #region Private Variables

        private string buildFileName_ = string.Empty;

        private bool changesMade_ = false;

        private Bitmap bmpSpriteSheet_;

        private SpriteSheetBuilderDto spriteSheetBuilderDto_ = new SpriteSheetBuilderDto();

        #endregion

        #region Properties

        public bool ChangesMade
        {
            get { return changesMade_; }
        }

        #endregion

        #region Public Functions

        public void AddImages(ImageSourceType imageSourceType, string[] filenames)
        {
            addImages(imageSourceType, filenames);
        }

        public void BuildAlphaMask()
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

            //if (spriteSheetBuilderDto_.AlphaChannel == AlphaChannel.Red)
            //{
            //    g.FillRectangle(new SolidBrush(Color.FromArgb(255, 0, 0, 0)), new System.Drawing.Rectangle(0, 0, bitmapWidth, bitmapHeight));
            //}
            //else if (spriteSheetBuilderDto_.AlphaChannel == AlphaChannel.Green)
            //{
            //    g.FillRectangle(new SolidBrush(Color.FromArgb(255, 0, 0, 0)), new System.Drawing.Rectangle(0, 0, bitmapWidth, bitmapHeight));
            //}
            //else if (spriteSheetBuilderDto_.AlphaChannel == AlphaChannel.Blue)
            //{
            //    g.FillRectangle(new SolidBrush(Color.FromArgb(255, 0, 0, 0)), new System.Drawing.Rectangle(0, 0, bitmapWidth, bitmapHeight));
            //}
            //else if (spriteSheetBuilderDto_.AlphaChannel == AlphaChannel.Grayscale)
            //{
            //    g.FillRectangle(new SolidBrush(Color.FromArgb(255, 0, 0, 0)), new System.Drawing.Rectangle(0, 0, bitmapWidth, bitmapHeight));
            //}

            imageCounter = 0;

            for (int i = 0; i < spriteSheetBuilderDto_.ImageSourceList.Count; i++)
            {
                SheetImageSourceDto imageSource = spriteSheetBuilderDto_.ImageSourceList[i];

                Bitmap spriteImage = new Bitmap(imageSource.FileName);

                Bitmap alphaMaskImage = new Bitmap(spriteImage.Size.Width, spriteImage.Size.Height);

                Graphics gAlphaMask = Graphics.FromImage(alphaMaskImage);

                gAlphaMask.FillRegion(new SolidBrush(spriteSheetBuilderDto_.BackgroundColor), new Region(new System.Drawing.Rectangle(0, 0, spriteImage.Size.Height, spriteImage.Size.Width)));

                gAlphaMask.Dispose();

                // Copy 
                for (int img_y = 0; img_y < spriteImage.Height; img_y++)
                {
                    for (int img_x = 0; img_x < spriteImage.Width; img_x++)
                    {
                        Color spriteColor = spriteImage.GetPixel(img_x, img_y);

                        // Copy the alpha channel from the sprite to the alpha channel of the mask.
                        Color alphaColor = Color.Transparent;

                        if (spriteSheetBuilderDto_.AlphaChannel == AlphaChannel.Red)
                        {
                            alphaColor = Color.FromArgb(255, spriteColor.A, 0, 0);
                        }
                        else if (spriteSheetBuilderDto_.AlphaChannel == AlphaChannel.Green)
                        {
                            alphaColor = Color.FromArgb(255, 0, spriteColor.A, 0);
                        }
                        else if (spriteSheetBuilderDto_.AlphaChannel == AlphaChannel.Blue)
                        {
                            alphaColor = Color.FromArgb(255, 0, 0, spriteColor.A);
                        }
                        else if (spriteSheetBuilderDto_.AlphaChannel == AlphaChannel.Grayscale)
                        {
                            alphaColor = Color.FromArgb(255, spriteColor.A, spriteColor.A, spriteColor.A);
                        }

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

            lblInitialized.Visible = false;

            pbSheetPreview.Refresh();
            
            g.Dispose();
        }

        public void BuildSheet()
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

            int bitmapWidth = (spriteSheetBuilderDto_.Columns * spriteSheetBuilderDto_.CellWidth) + ((spriteSheetBuilderDto_.Columns - 1) * spriteSheetBuilderDto_.Padding);

            // Need to do an initial pass to determine how many images there are in total, from all sources.
            int imageCounter = 0;

            for (int i = 0; i < spriteSheetBuilderDto_.ImageSourceList.Count; i++)
            {
                SheetImageSourceDto imageSource = spriteSheetBuilderDto_.ImageSourceList[i];

                imageCounter += imageSource.CellCount;
            }

            int rows = Convert.ToInt32(Math.Ceiling(((double)imageCounter / (double)spriteSheetBuilderDto_.Columns)));

            int bitmapHeight = (rows * spriteSheetBuilderDto_.CellHeight) + ((rows - 1) * spriteSheetBuilderDto_.Padding);

            if (bitmapWidth > 0 && bitmapHeight > 0)
            {
                bmpSpriteSheet_ = new Bitmap(bitmapWidth, bitmapHeight);

                Graphics g = Graphics.FromImage(bmpSpriteSheet_);

                g.FillRectangle(new SolidBrush(spriteSheetBuilderDto_.BackgroundColor), new System.Drawing.Rectangle(0, 0, bitmapWidth, bitmapHeight));

                imageCounter = 0;

                for (int i = 0; i < spriteSheetBuilderDto_.ImageSourceList.Count; i++)
                {
                    SheetImageSourceDto imageSource = spriteSheetBuilderDto_.ImageSourceList[i];

                    Bitmap spriteImageSource = new Bitmap(imageSource.FileName);

                    Bitmap spriteImageResult = new Bitmap(imageSource.FileName);

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

                            case SheetCellHorizontalAlignment.Right:

                                destX += (spriteSheetBuilderDto_.CellWidth - imageSource.CellWidth);

                                break;
                        }

                        // Adjust by the padding value if this is not the first column.
                        if (destCol > 0)
                        {
                            destX += (spriteSheetBuilderDto_.Padding * destCol);
                        }


                        int destY = destRow * spriteSheetBuilderDto_.CellHeight;

                        // Apply a transform for alignment
                        switch (spriteSheetBuilderDto_.VerticalAlignment)
                        {
                            case SheetCellVerticalAlignment.Center:

                                destY += ((spriteSheetBuilderDto_.CellHeight / 2) - (imageSource.CellHeight / 2));

                                break;

                            case SheetCellVerticalAlignment.Bottom:

                                destY += (spriteSheetBuilderDto_.CellHeight - imageSource.CellHeight);

                                break;
                        }

                        // Adjust by the padding value if this is not the first row.
                        if (destRow > 0)
                        {
                            destY += (spriteSheetBuilderDto_.Padding * destRow);
                        }

                        System.Drawing.Rectangle sourceRect = new System.Drawing.Rectangle(sourceCol * imageSource.CellWidth, sourceRow * imageSource.CellHeight, imageSource.CellWidth, imageSource.CellHeight);

                        System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(destX, destY, imageSource.CellWidth, imageSource.CellHeight);

                        // Perform the palette mapping.
                        for (int y = 0; y < spriteImageSource.Height; y++)
                        {
                            for (int x = 0; x < spriteImageSource.Width; x++)
                            {
                                Color sourcePixel = spriteImageSource.GetPixel(x, y);

                                Color sourcePixelFullAlpha = Color.FromArgb(255, sourcePixel.R, sourcePixel.G, sourcePixel.G);
                                                                
                                if (spriteSheetBuilderDto_.PaletteMap.ColorMap.ContainsKey(sourcePixelFullAlpha))
                                {
                                    Color mappedPixel = spriteSheetBuilderDto_.PaletteMap.ColorMap[sourcePixelFullAlpha];

                                    if (mappedPixel != sourcePixelFullAlpha)
                                    {
                                        spriteImageResult.SetPixel(x, y, mappedPixel);
                                    }
                                    else
                                    {
                                        // Mapping to itself.
                                        spriteImageResult.SetPixel(x, y, sourcePixel);
                                    }
                                }
                                else
                                {
                                    spriteImageResult.SetPixel(x, y, sourcePixel);
                                }
                            }
                        }                        

                        //g.DrawImage(spriteImage, destRect, 0, 0, destRect.Width, destRect.Height, GraphicsUnit.Pixel, attr);
                        g.DrawImage(spriteImageResult, destRect, sourceRect, GraphicsUnit.Pixel);

                        imageCounter++;
                    }

                    spriteImageSource.Dispose();

                    spriteImageResult.Dispose();
                }

                lblInitialized.Visible = false;

                g.Dispose();
            }
            else
            {
                MessageBox.Show("Failed to create bitmap.");
            }

            resize();

            pbSheetPreview.Refresh();
        }


        public void ExportSheet()
        {
            exportSheet();
        }

        public void NewSpriteSheet()
        {
            newSpriteSheet();
        }

        public void OpenBuildFile(string filename)
        {
            openBuildFile(filename);
        }

        public void SaveBuildFile()
        {
            saveBuildFile();
        }

        #endregion

        #region Private Functions

        private void addImages(ImageSourceType imageSourceType, string[] filenames)
        {
            foreach (string filename in filenames)
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

                    // If this is full transparency, ignore it.
                    if (pixelColor.A > 0)
                    {
                        // Ignore transparency, just use the base color.
                        Color pixelColorFullAlpha = Color.FromArgb(255, pixelColor.R, pixelColor.G, pixelColor.B);

                        if (spriteSheetBuilderDto_.PaletteMap.ContainsKey(pixelColorFullAlpha) == false)
                        {
                            spriteSheetBuilderDto_.PaletteMap.Add(pixelColorFullAlpha, pixelColorFullAlpha);
                        }
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

            pgSpriteSheet.Enabled = true;

            changesMade_ = false;

            buildFileName_ = string.Empty;

            lblInitialized.Visible = true;

            pbSheetPreview.BackColor = SystemColors.ControlDarkDark;
        }

        private void openBuildFile(string filename)
        {
            string fileConents = File.ReadAllText(filename);

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

                if (headerSize >= 9)
                {
                    var colorStr = fileData[8].Split(',');

                    Color bgColor = Color.FromArgb(Convert.ToInt32(colorStr[3]), Convert.ToInt32(colorStr[0]), Convert.ToInt32(colorStr[1]), Convert.ToInt32(colorStr[2]));

                    spriteSheetBuilderDto_.BackgroundColor = bgColor;
                }

                if (headerSize >= 10)
                {
                    var alphaChannel = fileData[9];
                                       
                    spriteSheetBuilderDto_.AlphaChannel = (AlphaChannel)Convert.ToUInt32(alphaChannel);
                }

                if (headerSize >= 11)
                {
                    spriteSheetBuilderDto_.Padding = Convert.ToInt32(fileData[10]);
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
                }

                GC.Collect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(errorMessage);
            }

            buildFileName_ = filename;

            pgSpriteSheet.SelectedObject = spriteSheetBuilderDto_;
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

                    int headerFields = 11;
                    int fieldsPerFile = 5;
                    fileContents += headerFields.ToString() + Environment.NewLine;
                    fileContents += fieldsPerFile.ToString() + Environment.NewLine;
                    fileContents += spriteSheetBuilderDto_.CellHeight.ToString() + Environment.NewLine;
                    fileContents += spriteSheetBuilderDto_.CellWidth.ToString() + Environment.NewLine;
                    fileContents += spriteSheetBuilderDto_.Columns.ToString() + Environment.NewLine;
                    fileContents += Convert.ToUInt32(spriteSheetBuilderDto_.VerticalAlignment).ToString() + Environment.NewLine;
                    fileContents += Convert.ToUInt32(spriteSheetBuilderDto_.HorizontalAlignment).ToString() + Environment.NewLine;
                    fileContents += spriteSheetBuilderDto_.PaletteMap.Serialize() + Environment.NewLine;

                    Color bgColor = spriteSheetBuilderDto_.BackgroundColor;

                    fileContents += bgColor.R.ToString() + "," + bgColor.G.ToString() + "," + bgColor.B.ToString() + "," + bgColor.A.ToString() + Environment.NewLine;

                    fileContents += Convert.ToUInt32(spriteSheetBuilderDto_.AlphaChannel).ToString() + Environment.NewLine;

                    fileContents += spriteSheetBuilderDto_.Padding;

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

        #endregion

        #region Event Handlers
        
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

        private void vsSpriteSheet_Scroll(object sender, ScrollEventArgs e)
        {
            pbSheetPreview.Refresh();
        }

        private void hsSpriteSheet_Scroll(object sender, ScrollEventArgs e)
        {
            pbSheetPreview.Refresh();
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

        private void scImages_SplitterMoved(object sender, SplitterEventArgs e)
        {
            resize();
        }

        private void SpriteSheetBuilderControl_Load(object sender, EventArgs e)
        {
            pgSpriteSheet.SelectedObject = spriteSheetBuilderDto_;

            resize();
        }

        private void SpriteSheetBuilderControl_Resize(object sender, EventArgs e)
        {
            resize();
        }

        #endregion

    }

    public interface ISpriteSheetBuilderControl
    {
        #region Properties

        bool ChangesMade { get; }

        #endregion

        #region Public Functions

        void AddImages(ImageSourceType imageSourceType, string[] filenames);

        void BuildAlphaMask();

        void BuildSheet();

        void ExportSheet();

        void NewSpriteSheet();

        void OpenBuildFile(string filename);

        void SaveBuildFile();

        #endregion
    }
}
