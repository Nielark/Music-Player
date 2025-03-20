using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicPlayer
{
    public partial class MusicProperties : Form
    {
        private readonly PlayerControls playerControls;
        private readonly Form formMain;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );

        public MusicProperties(Form formMain, PlayerControls playerControls)
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));   // Set the rounded corners
            MyModule.SetRoundedCorners(pnlMain, 10, 10, 10, 10);    // Set the rounded corners of the main panel

            // Set the form and player controls
            this.formMain = formMain;
            this.playerControls = playerControls;
        }

        private void MusicProperties_Load(object sender, EventArgs e)
        {
            CenterPropertyForm();       // Center the property form
            DisplayMusicProperties();   // Display the music properties
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();   // Close the form
        }

        private void BtnOpenFileLocation_Click(object sender, EventArgs e)
        {
            // Open the file location of the current music
            try
            {
                // Get the file path of the current music
                string filePath = playerControls.playMusicQueue[playerControls.CurrentMusicIndex].File;

                // Check if the file exists
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("File not found! Please check if it was moved or deleted.",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Process.Start("explorer.exe", $"/select, \"{filePath}\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to open the file location:\n{ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CenterPropertyForm()
        {
            // Center the property form with respect to the main form
            var centerPositionX = formMain.Location.X + (formMain.Width / 2) - (Width / 2);     // Center horizontally
            var centerPositionY = formMain.Location.Y + (formMain.Height / 2) - (Height / 2);   // Center vertically
            this.Location = new Point(centerPositionX, centerPositionY);    // Set the location
        }

        private void DisplayMusicProperties()
        {
            // Get the current music properties
            var playMusicQueue = playerControls.playMusicQueue;         // Get the play music queue
            var currentMusicIndex = playerControls.CurrentMusicIndex;   // Get the current music index

            // Display the music properties
            lblTitleValue.Text = playMusicQueue[currentMusicIndex].Title;
            lblArtistValue.Text = playMusicQueue[currentMusicIndex].Artist;
            lblAlbumTitleValue.Text = playMusicQueue[currentMusicIndex].Title;
            //lblYearValue.Text = playMusicQueue[currentMusicIndex].Year;
            //lblGenreValue.Text = playMusicQueue[currentMusicIndex].Genre;
            lblLengthValue.Text = playMusicQueue[currentMusicIndex].Duration;
            //lblBitRateValue.Text = playMusicQueue[currentMusicIndex].Bitrate;
            //lblItemTypeValue.Text = playMusicQueue[currentMusicIndex].Type;
            lblFileLocationValue.Text = playMusicQueue[currentMusicIndex].File;
        }
    }
}
