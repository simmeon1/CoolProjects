using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary;
using Newtonsoft.Json;

namespace MusicUI
{
    public partial class Form1 : Form
    {
        private List<Song> songList = new List<Song>();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string file = openFileDialog1.FileName;
            try
            {
                List<Song> list = JsonConvert.DeserializeObject<List<Song>>(File.ReadAllText(file));
                songList = list;
                RefreshSongGrid();
                lblStatus.Text = $"{songList.Count} songs loaded.";
            }
            catch (Exception)
            {

                lblStatus.Text = "Songs could not be loaded.";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshSongGrid();
        }

        private void RefreshSongGrid()
        {
            bindingSource1.Clear();
            foreach (Song song in songList) bindingSource1.Add(song);
            dataGridView1.DataSource = bindingSource1;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                if (column.Name.Equals(nameof(Song.ListType)) || column.Name.Equals(nameof(Song.ListTypeName))) column.Visible = false;
            }
        }
    }
}
