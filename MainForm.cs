﻿

#region using statements

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataJuggler.PixelDatabase;

#endregion

namespace SubImageCreator
{

    #region class MainForm
    /// <summary>
    /// This is the MainForm of this application
    /// </summary>
    public partial class MainForm : Form
    {
        
        #region Private Variables
        private PixelDatabase pixelDatabase;
        private int subImageSize;
        private List<Bitmap> subImages;
        #endregion
        
        #region Constructor
        /// <summary>
        /// Create a new instance of a 'MainForm' object.
        /// </summary>
        public MainForm()
        {
            // Create Controls
            InitializeComponent();

            // Perform initializations for this object
            Init();
        }
        #endregion
        
        #region Events
            
            #region Canvas_Click(object sender, EventArgs e)
            /// <summary>
            /// event is fired when the 'Canvas' is clicked.
            /// </summary>
            private void Canvas_Click(object sender, EventArgs e)
            {
                // Cast the event args as MouseEventArgs
                MouseEventArgs e2 = (MouseEventArgs) e;

                // if we already have the subimages
                if (SubImagesCount >= 8)
                {
                    // set the value
                    this.StatusLabel.Text = "8 is the maximum sub images";
                }
                else
                {
                    // if the value for HasPixelDatabase is true
                    if (HasPixelDatabase)
                    {
                        // locals
                        int x = e2.X;
                        int y = e2.Y;

                        // for debugging only
                        int originalX = x;
                        int originalY = y;

                        // Get the current image
                        Image image = this.Canvas.BackgroundImage;
                        Bitmap bitmap = new Bitmap(image);

                        // x & y must now be scaled
                        double canvasWidth = this.Canvas.Width;
                        double canvasHeight = this.Canvas.Height;
                        double bitmapWidth = bitmap.Width;
                        double bitmapHeight = bitmap.Height;

                        // get the xScale and the yScale
                        double scaleX = bitmapWidth / canvasWidth;
                        double scaleY = bitmapHeight / canvasHeight;
                        double doubleX = (double) x;
                        double doubleY = (double) y;

                        // reset the values
                        x = (int) (doubleX * scaleX);
                        y = (int) (doubleY * scaleY);

                        // ensure x is in range
                        if (x < 0)
                        {
                            // reset x
                            x = 0;
                        }

                        // ensure x is in range
                        if (x >= bitmap.Width)
                        {
                            // reset x
                            x = bitmap.Width -1;
                        }

                        // ensure y is in range
                        if (y < 0)
                        {
                            // reset y
                            y = 0;
                        }

                        // ensure y is in range
                        if (y >= bitmap.Height)
                        {
                            // reset y
                            y = bitmap.Height - 1;
                        }

                        // here we have (approximately) the x & y clicked. Sometimes rounding makes it off a pixel or two possibly
                        Point topLeft = new Point(x, y);

                        Rectangle size = new Rectangle(0, 0, SubImageSize, SubImageSize);

                        // Create a subImage
                        Bitmap subImage = PixelDatabase.CreateSubImage(topLeft, size);

                        // If the subImage object exists
                        if (subImage != null)
                        {
                            // Add
                            this.SubImages.Add(subImage);
    
                            // Display the SubImages
                            DisplaySubImages();
                        }
                    }
                }
            }
            #endregion
            
            #region Canvas_MouseEnter(object sender, EventArgs e)
            /// <summary>
            /// event is fired when Canvas _ Mouse Enter
            /// </summary>
            private void Canvas_MouseEnter(object sender, EventArgs e)
            {
                // Change the cursor to a hand
                Cursor = Cursors.Hand;
            }
            #endregion
            
            #region Canvas_MouseLeave(object sender, EventArgs e)
            /// <summary>
            /// event is fired when Canvas _ Mouse Leave
            /// </summary>
            private void Canvas_MouseLeave(object sender, EventArgs e)
            {
                // Change the cursor back to the default pointer
                Cursor = Cursors.Default;
            }
            #endregion
            
            #region SearchSubImages(object sender, EventArgs e)
            /// <summary>
            /// event is fired when Search Sub Images
            /// </summary>
            private void SearchSubImages(object sender, EventArgs e)
            {
                
            }
            #endregion
            
            #region SizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
            /// <summary>
            /// event is fired when a selection is made in the 'SizeComboBox_'.
            /// </summary>
            private void SizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
            {
                switch (SizeComboBox.SelectedIndex)
                {
                    case 0:

                        // Set the size
                        SubImageSize = 16;

                        // required
                        break;

                    case 1:

                        // Set the size
                        SubImageSize = 32;

                        // required
                        break;

                    case 2:

                        // Set the size
                        SubImageSize = 48;

                        // required
                        break;

                    case 3:

                        // Set the size
                        SubImageSize = 64;

                        // required
                        break;

                     case 4:

                        // Set the size
                        SubImageSize = 96;

                        // required
                        break;

                    case 5:

                        // Set the size
                        SubImageSize = 128;

                        // required
                        break;

                    case 6:

                        // Set the size
                        SubImageSize = 256;

                        // required
                        break;
                }
            }
            #endregion
            
            #region TakeScreenShotButton_Click(object sender, EventArgs e)
            /// <summary>
            /// event is fired when the 'TakeScreenShotButton' is clicked.
            /// </summary>
            private void TakeScreenShotButton_Click(object sender, EventArgs e)
            {
                // Create a new collection of 'Bitmap' objects.
                this.SubImages = new List<Bitmap>();

                // local
                FormWindowState windowState = this.WindowState;
                
                this.WindowState = FormWindowState.Minimized;
                
                // Give time for system to catch up
                Application.DoEvents();
                
                // Sleep for a second
                System.Threading.Thread.Sleep(1000);
                
                Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                Graphics graphics = Graphics.FromImage(bitmap);
                graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
                
                // Set the CanvasBackground
                Canvas.BackgroundImage = bitmap;
                this.WindowState = windowState;
                
                // Give time for system to catch up
                Application.DoEvents();
                
                // Load the PixelDatabase
                this.PixelDatabase = PixelDatabaseLoader.LoadPixelDatabase(bitmap, Refresh);
            }
            #endregion
            
        #endregion
        
        #region Methods
            
            #region DisplaySubImages()
            /// <summary>
            /// This method Display Sub Images
            /// </summary>
            public void DisplaySubImages()
            {
                // local 
                int count = 0;

                // hide each control
                this.SubImage1.Visible = false;
                this.SubImage2.Visible = false;
                this.SubImage3.Visible = false;
                this.SubImage4.Visible = false;
                this.SubImage5.Visible = false;
                this.SubImage6.Visible = false;
                this.SubImage7.Visible = false;
                this.SubImage8.Visible = false;

                // Display the count
                this.CountLabel.Text = "Sub Images: " + subImages.Count.ToString();

                // if the value for HasSubImages is true
                if (HasSubImages)
                {
                    // Iterate the collection of Bitmap objects
                    foreach (Bitmap bitmap in SubImages)
                    {
                        // Increment the value for count
                        count++;

                        // Determine the action by the count
                        switch (count)
                        {
                            case 1:

                                // Set the BackgroundImage
                                SubImage1.BackgroundImage = bitmap;
                                SubImage1.Visible = true;

                                // required
                                break;

                            case 2:

                                // Set the BackgroundImage
                                SubImage2.BackgroundImage = bitmap;
                                SubImage2.Visible = true;

                                // required
                                break;

                            case 3:

                                // Set the BackgroundImage
                                SubImage3.BackgroundImage = bitmap;
                                SubImage3.Visible = true;

                                // required
                                break;

                            case 4:

                                // Set the BackgroundImage
                                SubImage4.BackgroundImage = bitmap;
                                SubImage4.Visible = true;

                                // required
                                break;

                            case 5:

                                // Set the BackgroundImage
                                SubImage5.BackgroundImage = bitmap;
                                SubImage5.Visible = true;

                                // required
                                break;

                            case 6:

                                // Set the BackgroundImage
                                SubImage6.BackgroundImage = bitmap;
                                SubImage6.Visible = true;

                                // required
                                break;

                            case 7:

                                // Set the BackgroundImage
                                SubImage7.BackgroundImage = bitmap;
                                SubImage7.Visible = true;

                                // required
                                break;

                            case 8:

                                // Set the BackgroundImage
                                SubImage8.BackgroundImage = bitmap;
                                SubImage8.Visible = true;

                                // required
                                break;
                        }
                    }
                }
                
            }
            #endregion
            
            #region Init()
            /// <summary>
            /// This method performs initializations for this object.
            /// </summary>
            public void Init()
            {
                // Add these chioces
                SizeComboBox.Items.Add("16 x 16");
                SizeComboBox.Items.Add("32 x 32");
                SizeComboBox.Items.Add("48 x 48");
                SizeComboBox.Items.Add("64 x 64");
                SizeComboBox.Items.Add("96 x 96");
                SizeComboBox.Items.Add("128 x 128");
                SizeComboBox.Items.Add("256 x 256");

                // Default to 64 x 64
                SizeComboBox.SelectedIndex = 3;

                // Set the SubImageSize
                SubImageSize = 64;

                // Create a new collection of 'Bitmap' objects.
                this.SubImages = new List<Bitmap>();
            }
            #endregion
            
            #region Refresh(string message, int pixelsUpdated)
            /// <summary>
            /// method Refresh
            /// </summary>
            private void Refresh(string message, int pixelsUpdated)
            {
                // do not show SetMaxGraph
                if (message != "SetGraphMax")
                {
                    // Show the text
                    this.StatusLabel.Text = message;
                }
            }
            #endregion
            
        #endregion

        #region Properties()
            
            #region HasPixelDatabase
            /// <summary>
            /// This property returns true if this object has a 'PixelDatabase'.
            /// </summary>
            public bool HasPixelDatabase
            {
                get
                {
                    // initial value
                    bool hasPixelDatabase = (this.PixelDatabase != null);
                    
                    // return value
                    return hasPixelDatabase;
                }
            }
            #endregion
            
            #region HasSubImages
            /// <summary>
            /// This property returns true if this object has a 'SubImages'.
            /// </summary>
            public bool HasSubImages
            {
                get
                {
                    // initial value
                    bool hasSubImages = (this.SubImages != null);
                    
                    // return value
                    return hasSubImages;
                }
            }
            #endregion
            
            #region PixelDatabase
            /// <summary>
            /// This property gets or sets the value for 'PixelDatabase'.
            /// </summary>
            public PixelDatabase PixelDatabase
            {
                get { return pixelDatabase; }
                set { pixelDatabase = value; }
            }
        #endregion

            #region SubImages
            /// <summary>
            /// This property gets or sets the value for 'SubImages'.
            /// </summary>
            public List<Bitmap> SubImages
            {
                get { return subImages; }
                set { subImages = value; }
            }
            #endregion

            #region SubImagesCount
            /// <summary>
            /// This read only property returns the Count of subimages
            /// </summary>
            public int SubImagesCount
            {
                get
                {
                    // initial value
                    int count = 0;

                    // if the value for HasSubImages is true
                    if (HasSubImages)
                    {
                        // set the return value
                        count = subImages.Count;
                    }

                    // return value
                    return count;
                }
            }
            #endregion
            
            #region SubImageSize
            /// <summary>
            /// This property gets or sets the value for 'SubImageSize'.
            /// </summary>
            public int SubImageSize
            {
                get { return subImageSize; }
                set { subImageSize = value; }
            }
        #endregion

        #endregion

    }
    #endregion

}