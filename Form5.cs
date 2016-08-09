using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using MSEXCEL = Microsoft.Office.Interop.Excel;


namespace CTSP路由调度
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }
        static public double M1;
        static public int creat_vehicle = 0;
        public int num = 0;
        private void Form5_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("d:");
            comboBox1.Items.Add("e:");
            comboBox1.Items.Add("f:");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text != "")
            {
                num = textBox1.Text.Length;//=// int.Parse(textBox1.Text.Trim());
                int a = num;
                char[] length = new char[num];
                length = textBox1.Text.ToCharArray();
                for (int i = 0; i < num; i++)
                {
                    if (length[i] < '0' || length[i] > '9')
                    {
                        MessageBox.Show("输入有误，请输入整数", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.textBox1.Text = "";
                        break;
                    }
                }
                if (length[num - 1] >= '0' && length[num - 1] <= '9')
                {
                    M1 = double.Parse(textBox1.Text.ToString());
                    if(M1<10.0)
                    {
                        MessageBox.Show("输入有误，请输入大于10的整数", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.textBox1.Text = "";
                    }
                }
            }
            else
            {
                MessageBox.Show("输入不能为空，请输入整数", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } 
            creat_vehicle = 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path;
            MSEXCEL.Application ExcelApp;
            MSEXCEL.Workbook exceldoc;
            if (textBox2.Text == "" || this.comboBox1.Text == "")
            {
                MessageBox.Show("输入名称和保存地址", "信息提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            else
            {
                path = comboBox1.Text + textBox2.Text;
                ExcelApp = new MSEXCEL.ApplicationClass();
                if (File.Exists((string)path))
                {
                    File.Delete((string)path);
                }
                Missing Nothing = Missing.Value;
                exceldoc = ExcelApp.Workbooks.Add(Nothing);
                MSEXCEL.Worksheet worksheet = (MSEXCEL.Worksheet)exceldoc.Sheets[1];
                for (int i = 0; i < this.dataGridView1.ColumnCount; i++)
                {
                    for (int j = 0; j < this.dataGridView1.RowCount; j++)
                    {
                        ExcelApp.Cells[j + 1, i + 1] = this.dataGridView1[i, j].Value;
                    }
                }
                worksheet.SaveAs(path, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing);
                exceldoc.Close(Nothing, Nothing, Nothing);
                ExcelApp.Quit();
                MessageBox.Show("保存成功", "信息提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }   
        }
        private void button4_Click(object sender, EventArgs e)
        {
            this.dataGridView1.AllowUserToAddRows = true;
            this.dataGridView1.ColumnCount = 8;
            this.dataGridView1.RowCount = 20;
            this.dataGridView1[0, 0].Value = "编号";
            this.dataGridView1[1, 0].Value = "姓名";
            this.dataGridView1[2, 0].Value = "年龄";
            this.dataGridView1[3, 0].Value = "电话";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}