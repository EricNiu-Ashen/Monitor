using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;


namespace Test
{
    public partial class MainForm : Form
    {

        private List<ClientMonitor> clients = new List<ClientMonitor>();
        private List<PictureBox> imgs = new List<PictureBox>();
        private List<Label> labels = new List<Label>();
        private Point[] points = { new Point(5, 5), new Point(846, 5),new Point(5,511),new Point(846,511) };
        private Size size = new Size(836, 501);


        public MainForm()
        {
            InitializeComponent();
        }


        private void MainForm_Shown(object sender, EventArgs e)
        {

            imgs.Add(client1Picture);
            imgs.Add(client2Picture);
            imgs.Add(client3Picture);
            imgs.Add(client4Picture);
            labels.Add(idLabel1);
            labels.Add(idLabel2);
            labels.Add(idLabel3);
            labels.Add(idLabel4);
            labels.Add(handleLable1);
            labels.Add(handleLable2);
            labels.Add(handleLable3);
            labels.Add(handleLable4);
            labels.Add(posLabel1);
            labels.Add(posLabel2);
            labels.Add(posLabel3);
            labels.Add(posLabel4);
            labels.Add(sizeLabel1);
            labels.Add(sizeLabel2);
            labels.Add(sizeLabel3);
            labels.Add(sizeLabel4);

            foreach (Process proInfo in Process.GetProcesses())
            {
                if (proInfo.ProcessName == "onmyoji")
                {

                    clients.Add(new ClientMonitor(proInfo));
                }
            }

            for (int i = 0; i < clients.Count; i++)
            {
                labels[i ].Text = clients[i].client.ProcessName;
                labels[i + 4].Text = clients[i].client.Id.ToString();
                clients[i].ChangeWindow(points[i], size);
            }

           img_fresh.Start();

        }

        private void add_button_Click(object sender, EventArgs e)
        {

        }

        private void getImg_Click(object sender, EventArgs e)
        {
            foreach (ClientMonitor client in clients)
            {
                Image bmp = client.GetWindowCapture();
                
                bmp.Save("C:/Users/Tazi/Desktop/" + client.GetId() + ".bmp");
                bmp.Dispose();
            }
        }

        private void img_fresh_Tick(object sender, EventArgs e)
        {

            for (int i = 0; i < clients.Count; i++)
            {
                Image current_img = clients[i].PrintWindow();
                Image past_img = imgs[i].Image;
                imgs[i].Image = current_img;
                if(past_img!=null)
                {
                    past_img.Dispose();
                }


                labels[i + 8].Text = clients[i].pos.ToString();
                labels[i + 12].Text = clients[i].size.ToString();
            }

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void resetWindows_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].ChangeWindow(points[i], size);
            }
        }
    }
}
