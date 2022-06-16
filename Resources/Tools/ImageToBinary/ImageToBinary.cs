using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace ImageToBinary
{
    public partial class ImageToBinary : Form
    {
        public ImageToBinary()
        {
            InitializeComponent();
            FileDisplay.AutoSize = false;
            FileDisplay.Dock = DockStyle.Fill;
            
        }
        public string[] ImageExtensions = new string[]
        {
            ".bmp",
            ".png",
            ".jpg"
        };
        public List<string> ChosenFiles = new List<string>();
        private void ChooseFile_Click(object sender, EventArgs e)
        {
            string defaultPath = GetType().Assembly.Location;
            defaultPath = defaultPath.Substring(0, defaultPath.IndexOf("Everglow") + 8);
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "选择图片",
                Multiselect = true,
                Filter = "所有文件(*.*)|*.*|" + "bmp图片(*.bmp)|*.bmp|" + "png图片(*.png)|*.png|" + "jpg图片(*.jpg)|*.jpg",
                InitialDirectory = defaultPath
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string[] names = openFileDialog.FileNames;
                foreach (string name in names)
                {
                    if (ImageExtensions.Contains(Path.GetExtension(name)))
                    {
                        ChosenFiles.Add(name);
                    }
                }
                StringBuilder builder = new StringBuilder();
                foreach (var file in ChosenFiles)
                {
                    builder.AppendLine(file);
                }
                //这输入空格了就提前换行了
                FileDisplay.Text = builder.ToString().Replace(' ', '_');
            }
        }
        public bool red = false;
        public bool green = false;
        public bool blue = false;
        public bool alpha = false;
        private void Passes_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var index in Passes.SelectedIndices)
            {
                switch (index)
                {
                    case 0:
                        red = true;
                        break;
                    case 1:
                        green = true;
                        break;
                    case 2:
                        blue = true;
                        break;
                    case 3:
                        alpha = true;
                        break;
                }
            }
        }

        private void Output_Click(object sender, EventArgs e)
        {
            if (!(red || green || blue || alpha) || ChosenFiles.Count == 0)
            {
                MessageBox.Show("未选中任何文件或通道");
                return;
            }

            BitVector32 bits = new BitVector32();
            bits[1] = red;
            bits[1 << 1] = green;
            bits[1 << 2] = blue;
            bits[1 << 3] = alpha;
            try
            {
                foreach (var file in ChosenFiles)
                {
                    using var memoryStream = new MemoryStream(5000000);
                    var writer = new BinaryWriter(memoryStream);

                    using (var fileStream = new FileStream(file, FileMode.Open))
                    {
                        using var bitmap = new Bitmap(fileStream);
                        int width = bitmap.Width;
                        int height = bitmap.Height;
                        writer.Write(width);
                        writer.Write(height);
                        writer.Write(bits.Data);
                        for (int i = 0; i < width; i++)
                        {
                            for (int j = 0; j < height; j++)
                            {
                                var color = bitmap.GetPixel(i, j);
                                if (red)
                                {
                                    writer.Write(color.R);
                                }

                                if (green)
                                {
                                    writer.Write(color.G);
                                }

                                if (blue)
                                {
                                    writer.Write(color.B);
                                }

                                if (alpha)
                                {
                                    writer.Write(color.A);
                                }
                            }
                        }
                    }

                    using var outputFile = new FileStream(Path.ChangeExtension(file, "bin"), FileMode.Create);
                    outputFile.Write(memoryStream.ToArray(), 0, (int)memoryStream.Length);
                }
                MessageBox.Show("All success!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
