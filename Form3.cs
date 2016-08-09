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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        public int dim;
        static public int kk = 0;
        static public int creat_kehu = 0;
        static public double[,] data;//= new double[dim,2];
        static public double[] weight;
        static public double[,] testdata;
        Random auto = new Random();
        public void create_data(int t_n)       // 随机生成距离矩阵
        {

            for (int i = 0; i < t_n - 1; i++)
            {
                for (int j = 0; j < 2; j++)
                {


                    data[i, j] = auto.Next(1, 100);


                }
                if (i == 0)
                {
                    weight[i] = 0; 
                }
                else
                {
                    weight[i] = auto.Next(1, 10);
                }
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int num;
            int P_a = 0;
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
                    creat_kehu = 1;
                    dim = int.Parse(textBox1.Text.Trim()) + 2;
                    kk = int.Parse(textBox1.Text.Trim());
                    data = new double[dim - 1, 2];
                    weight = new double[dim - 1];
                    create_data(dim);                  
                    this.dataGridView1.AllowUserToAddRows = true;
                    this.dataGridView1.ColumnCount = dim;
                    this.dataGridView1.RowCount = dim;
                    this.dataGridView1[0, 0].Value = "客户编号";
                    this.dataGridView1[1, 0].Value = "经度";
                    this.dataGridView1[2, 0].Value = "纬度";
                    this.dataGridView1[3, 0].Value = "货物重量";
                    for (int i = 1; i < dim; i++)
                    {
                        this.dataGridView1[0, i].Value = i - 1;
                        this.dataGridView1[1, i].Value = data[i - 1, 0];
                        this.dataGridView1[2, i].Value = data[i - 1, 1];
                        this.dataGridView1[3, i].Value = weight[i - 1];
                    }
                }
            }
            else 
            {
                MessageBox.Show("输入不能为空，请输入整数", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }         
        }
        private void button2_Click(object sender, EventArgs e)
        {           
            this.Close();
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("d:");
            comboBox1.Items.Add("e:");
            comboBox1.Items.Add("f:");
         
        }
        private void button3_Click(object sender, EventArgs e)
        {
                string path;
                MSEXCEL.Application ExcelApp;
                MSEXCEL.Workbook exceldoc;
                if (textBox2.Text == "" || this.comboBox1.Text == "")
                {
                    MessageBox.Show("输入名称和路径", "信息提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
                else
                {
                    path =comboBox1.Text + textBox2.Text;
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
    }
}