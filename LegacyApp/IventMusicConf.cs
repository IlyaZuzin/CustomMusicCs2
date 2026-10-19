
using MusicComanderGUI;
using System.Windows.Documents;

namespace CustomMusic
{
    public partial class IventMusicConf : Form
    {
        List<AudioFiles> Left;
        List<AudioFiles> Right;

        public IventMusicConf(List<AudioFiles> type1, string name)
        {
            InitializeComponent();
            LeftIventsName.Text = name;
            Left = type1;
            SingleMode.Visible = true;
            DoubleMode.Visible = false;

            SingleList.DataSource = Left;
            SingleList.DisplayMember = "path";
        }

        public IventMusicConf(List<AudioFiles> type1, string name1, List<AudioFiles> type2, string name2)
        {
            InitializeComponent();
            SingleMode.Visible = false;
            DoubleMode.Visible = true;

            LeftIventsName.Text = name1;
            RightIventsName.Text = name2;

            Left = type1;
            Right = type2;

            LeftList.DataSource = Left;
            LeftList.DisplayMember = "path";

            RightList.DataSource = Right;
            RightList.DisplayMember = "path";
        }

        private void Volume_Scroll(object sender, EventArgs e)
        {
            foreach (int file in LeftList.SelectedIndices)
            {
                Left[file].volume = Volume.Value;
            }
            foreach (int file in RightList.SelectedIndices)
            {
                Right[file].volume = Volume.Value;
            }
            foreach (int file in SingleList.SelectedIndices)
            {
                Left[file].volume = Volume.Value;
            }
            VolumeText.Text = Volume.Value.ToString();
        }

        private void LeftList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AddLeft_Click(object sender, EventArgs e)
        {
            AddFile(ref LeftList,Left);
        }


        private void RightAdd_Click(object sender, EventArgs e)
        {
            AddFile(ref RightList, Right);
        }

        private void LeftDelete_Click(object sender, EventArgs e)
        {
            DeleteFile(ref LeftList, Left);
        }

        private void RightDelete_Click(object sender, EventArgs e)
        {
            DeleteFile(ref RightList, Right);
        }

        private void DeleteFile(ref ListBox listBox, List<AudioFiles> list)
        {
            for (int file = listBox.SelectedIndices.Count - 1; file >= 0; file--)
            {
                list.RemoveAt(listBox.SelectedIndices[file]);
            }
            listBox.DataSource = null;
            listBox.DataSource = list;
            listBox.DisplayMember = "path";
        }
        private void AddFile(ref ListBox listBox, List<AudioFiles> list)
        {
            OpenFileDialog direct = new OpenFileDialog();
            direct.Multiselect = true;
            if (direct.ShowDialog() == DialogResult.OK)
            {
                foreach (string path in direct.FileNames)
                {
                    AudioFiles insert = new AudioFiles();
                    insert.path = path;
                    list.Add(insert);
                } 
                listBox.DataSource = null;
                listBox.DataSource = list;
                listBox.DisplayMember = "path";
            }
        }

        private void keep_Click(object sender, EventArgs e)
        {
            if (RightList.SelectedIndices.Count != 1) return;
            if (LeftList.SelectedIndices.Count != 1) return;
            foreach (int file in LeftList.SelectedIndices)
            {
                Left[file].next_music = LeftList.SelectedIndices[0];
            }
        }

        private void SaveSettings_Click(object sender, EventArgs e)
        {

        }

        private void SingleList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SingleAdd_Click(object sender, EventArgs e)
        {
            AddFile(ref SingleList,Left);
        }

        private void SingleDelete_Click(object sender, EventArgs e)
        {
            DeleteFile(ref SingleList, Left);
        }
    }
}
