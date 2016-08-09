using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CTSP路由调度
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public int dim;
        private void label_Click(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if ((this.textBox1.Text == "admin") && (this.textBox2.Text == "123456"))
            {
                this.Hide();
                Form2 form2 = new Form2();
                form2.ShowDialog();
            }
            else
            {
                MessageBox.Show("输入有误，请重新输入", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.textBox1.Text = "";
                this.textBox2.Text = "";
            }    
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}