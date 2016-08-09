using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ZedGraph;

namespace CTSP路由调度
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        public struct individual
        {

            public int[] chrom;
            public int[] order;
            public double fitness1;
            public int Rout;
        }
        public int dim;
        public int num;
        public int num_0;
        public int Genmax;
        public int popsize;
        public int creat_num = 0;
        public double time_consume;
        public double M1;
        public double[,] city;
        static public double[,] testdata;
        Random auto = new Random(); 
        private double P_100;
        private double P_0;
        private double min;
        private int globle_Rout = 0;
        private int time_start;
        private int min_vehicle = 0;
        private int[] P_gen0;
        private int[] P_gen100;
        private double[] kehuweight;
        public int[] g_min_permutation;
        private double[] Pbest_each_gen;
        public double[, ,] tuo;
        public individual[] newpop;
        public void create_testdata(int t_n)       // 随机生成距离矩阵
        {
            creat_num = 1;
            for (int i = 0; i <= t_n; i++)
            {
                for (int j = 0; j <= t_n; j++)
                {
                    testdata[i, j] = Math.Pow(Math.Pow(city[i, 0] - city[j, 0], 2) + Math.Pow(city[i, 1] - city[j, 1], 2), 0.5);
                }
            }
        }
        public void init_tuo(int t_popsize, int t_n, double[,] t_testdata)        //初始化概率强度矩阵tuo
        {
            int i, j, k;
            for (i = 0; i < t_n; i++)
            {
                for (j = 0; j < t_n; j++)
                {
                    for (k = 1; k <= t_n; k++)
                    {
                        tuo[i, j, k] = 1.0 / t_n;
                    }
                }
            }
        }
        public int P_ij_selected(int t_i, int num_vehicle, int t_n, int t_line, double weight_all, Queue<int> t_Nb)
        {
            int l, result;
            double total = 0;
            double rand_p = 0;
            double sum_p_select = 0;
            double[] P_ij_array = new double[dim + 1];
            for (l = 1; l <= t_n; l++)
            {
                if (t_Nb.Contains(l) == false)
                {
                    if (weight_all + kehuweight[l] > M1)
                    {
                        P_ij_array[l] = 0.0;
                    }
                    else
                    {
                        P_ij_array[l] = tuo[num_vehicle, t_line, l];//车辆K；
                    }
                }
                else
                {
                    P_ij_array[l] = 0;
                }
                total += P_ij_array[l];
            }

            if (total == 0)
            {
                result = 0;
                goto selected;
            }
            for (l = 1; l <= t_n; l++)
            {
                P_ij_array[l] = P_ij_array[l] / total;
            }
            rand_p = auto.NextDouble();
            l = 1;
            sum_p_select = 0.0;
            while ((l <= t_n) && (sum_p_select < rand_p))
            {
                sum_p_select = sum_p_select + P_ij_array[l];
                l = l + 1;
            }
            result = l - 1;
            selected: return result;
        }
        public double calc_length(int t_n, int[] order)
        {
            double C = 0;
            double Z = 0;
            for (int i = 0; i < t_n - 1; i++)
            {
                C = testdata[order[i], order[i + 1]];
                Z = Z + C;

            }
            return Z;
        }
        public void AntMove(int t_popsize, int t_n)///确定移动路径的程序
        {
            int i, j;//t_n,t_m,t_k;
            int vehicle_num = 0;
            int line_num = 0;
            double weight = 0;
            int[] way = new int[100];
            Queue<int> Nb = new Queue<int>();
            for (i = 0; i < t_popsize; i++)
            {
                vehicle_num = 0;
                line_num = 0;
                Nb.Clear();
                weight = 0;
                newpop[i].chrom = new int[2 * dim];
                newpop[i].order = new int[2 * dim];
                newpop[i].fitness1 = 0;
                newpop[i].Rout = 0;
                for (j = 0; j < t_n; j++)
                {
                    if (j == 0)
                    {
                        Nb.Enqueue(0);
                        newpop[i].chrom[j] = P_ij_selected(0, vehicle_num, t_n, line_num, weight, Nb);

                        Nb.Enqueue(newpop[i].chrom[j]);
                        weight = kehuweight[newpop[i].chrom[j]];

                    }
                    else
                    {
                        line_num++;
                        newpop[i].chrom[j] = P_ij_selected(newpop[i].chrom[j - 1], vehicle_num, t_n, line_num, weight, Nb);

                        if (newpop[i].chrom[j] == 0)
                        {
                            vehicle_num++;
                            line_num = 0;
                            Nb.Enqueue(0);
                            weight = 0;
                            newpop[i].chrom[j] = P_ij_selected(0, vehicle_num, t_n, line_num, weight, Nb);
                            Nb.Enqueue(newpop[i].chrom[j]);
                            weight = kehuweight[newpop[i].chrom[j]];
                        }
                        else
                        {
                            weight += kehuweight[newpop[i].chrom[j]];
                            Nb.Enqueue(newpop[i].chrom[j]);
                        }
                    }
                }
                Nb.Enqueue(0);
                newpop[i].order = Nb.ToArray();
                for (int m = 1; m < newpop[i].order.Length; m++)
                {
                    if (newpop[i].order[m] == 0)
                    {
                        newpop[i].Rout++;
                    }
                }
                newpop[i].fitness1 = calc_length(Nb.Count, newpop[i].order);
            }
        }
        public void two_opt(int[] Jiagong)//更新轨迹强度
        {
            int begin = 0;
            int a0 = 0, b0 = 0;
            int Change = 0;
            int B_index = 0;
            int end = 0;
            int index_a0 = 0, index_a1 = 0, index_b0 = 0, index_b1 = 0;
            int[] array = new int[30];
            int[] jiagong_order = new int[200];
            int[] way_0 = new int[30];
            int[] way_1 = new int[30];
            double distinction;
            double sum_0 = 0, sum_1 = 0;
            Queue<int> OPT = new Queue<int>();
            Queue<int> ENQ = new Queue<int>();
            Queue<int> ENP = new Queue<int>();
            Queue<int> EN_all = new Queue<int>();
            ArrayList IN_way = new ArrayList();
            ArrayList IN_good = new ArrayList();
            for (int k = 0; k < Jiagong.Length; k++)
            {
                IN_way.Add(Jiagong[k]);
                if (Jiagong[k] == 0)
                {
                    OPT.Enqueue(k);
                    end++;
                }
            }
            array = OPT.ToArray();
            OPT.Clear();
            jiagong_order = Jiagong;
            for (int i = array[begin] + 1; i <= array[end - 2]; i++)
            {
                if (i == array[begin + 1])
                {
                    begin++;
                    i = array[begin] + 1;
                }
                for (int l = array[begin + 1] + 1; l < array[end - 1]; l++)
                {
                    if (l < array[begin + 1])
                    {
                        l = array[begin + 1] + 1;
                    }
                    if (Change == 1)
                    {
                        Change = 0;
                        l = IN_way.IndexOf(B_index);
                    }
                    distinction = testdata[jiagong_order[i], jiagong_order[i + 1]] + testdata[jiagong_order[l], jiagong_order[l + 1]] - (testdata[jiagong_order[i], jiagong_order[l + 1]] + testdata[jiagong_order[l], jiagong_order[i + 1]]);
                    if (distinction > 0)
                    {
                        a0 = i;
                        b0 = l;
                        if (jiagong_order[a0] == 0 || jiagong_order[b0] == 0)
                        {
                            continue;
                        }
                        for (int k = 0; k < array.Length; k++)
                        {
                            if ((a0 > array[k]) && (a0 < array[k + 1]))
                            {
                                index_a0 = array[k];
                                index_a1 = array[k + 1];
                            }
                            if ((b0 > array[k]) && (b0 < array[k + 1]))
                            {
                                index_b0 = array[k];
                                index_b1 = array[k + 1];
                            }
                        }
                        for (int k = index_a0; k <= a0; k++)
                        {
                            ENQ.Enqueue(jiagong_order[k]);
                        }
                        for (int k = b0 + 1; k <= index_b1; k++)
                        {
                            ENQ.Enqueue(jiagong_order[k]);
                        }
                        for (int k = index_b0; k <= b0; k++)
                        {
                            ENP.Enqueue(jiagong_order[k]);
                        }
                        for (int k = a0 + 1; k <= index_a1; k++)
                        {
                            ENP.Enqueue(jiagong_order[k]);
                        }
                        way_0 = ENQ.ToArray();
                        way_1 = ENP.ToArray();
                        ENQ.Clear();
                        ENP.Clear();
                        if (sum_0 > M1 || sum_1 > M1)
                        {
                            continue;
                        }
                        else
                        {
                            Change = 1;
                            B_index = jiagong_order[b0];
                            for (int k = 0; k <= a0; k++)
                            {
                                IN_good.Add(IN_way[k]);
                            }
                            for (int k = b0 + 1; k <= index_b1; k++)
                            {
                                IN_good.Add(IN_way[k]);
                            }
                            if (index_a1 == index_b0)
                            {
                                for (int k = index_b0 + 1; k <= b0; k++)
                                {
                                    IN_good.Add(IN_way[k]);
                                }
                                for (int k = a0 + 1; k <= index_a1; k++)
                                {
                                    IN_good.Add(IN_way[k]);
                                }
                                if (index_b1 != IN_way.Count - 1)
                                {
                                    for (int k = index_b1 + 1; k <= IN_way.Count - 1; k++)
                                    {
                                        IN_good.Add(IN_way[k]);
                                    }
                                }
                            }
                            else
                            {
                                for (int k = index_a1 + 1; k <= index_b0; k++)
                                {
                                    IN_good.Add(IN_way[k]);
                                }
                                for (int k = index_b0 + 1; k <= b0; k++)
                                {
                                    IN_good.Add(IN_way[k]);
                                }
                                for (int k = a0 + 1; k <= index_a1; k++)
                                {
                                    IN_good.Add(IN_way[k]);
                                }
                                if (index_b1 != IN_way.Count - 1)
                                {
                                    for (int k = index_b1 + 1; k <= IN_way.Count - 1; k++)
                                    {
                                        IN_good.Add(IN_way[k]);
                                    }
                                }
                            }
                            IN_way.Clear();
                            for (int k = 0; k < IN_good.Count; k++)
                            {
                                IN_way.Add(IN_good[k]);
                            }
                            IN_good.Clear();
                            jiagong_order = (Int32[])IN_way.ToArray(typeof(Int32));
                            for (int k = 0; k < jiagong_order.Length; k++)
                            {
                                Jiagong[k] = jiagong_order[k];
                            }
                            for (int k = 0; k < jiagong_order.Length; k++)
                            {

                                if (jiagong_order[k] == 0)
                                {
                                    OPT.Enqueue(k);
                                    // end++;
                                }
                            }
                            array = OPT.ToArray();
                            OPT.Clear();
                        }
                    }
                    sum_0 = 0.0;
                    sum_1 = 0.0;
                }
            }
        }       
        public void inherent_2opt(int[] Jiagong)
        {
            int end = 0;
            int a0 = 0, b0 = 0;
            int[] array = new int[dim];
            int[] way = new int[dim];
            int[] single = new int[dim];
            double distance = 0;
            Queue<int> OPT = new Queue<int>();
            Queue<int> OQ_T = new Queue<int>();
            Queue<int> ENQ = new Queue<int>();
            Queue<int> EQQ = new Queue<int>();
            for (int k = 0; k < Jiagong.Length; k++)
            {
                if (Jiagong[k] == 0)
                {
                    OPT.Enqueue(k);
                    end++;
                }
            }
            array = OPT.ToArray();
            OPT.Clear();
            OQ_T.Clear();
            for (int k = 0; k < array.Length - 1; k++)
            {
                ENQ.Clear();
                for (int i = array[k]; i <= array[k + 1]; i++)
                {
                    ENQ.Enqueue(Jiagong[i]);
                }
                single = ENQ.ToArray();
                ENQ.Clear();
                if (single.Length < 4)
                {
                    continue;
                }
                for (int m = 0; m < single.Length - 3; m++)
                {
                    for (int n = m + 2; n < single.Length - 1; n++)
                    {
                        distance = testdata[single[m], single[m + 1]] + testdata[single[n], single[n + 1]] - (testdata[single[m], single[n]] + testdata[single[m + 1], single[n + 1]]);
                        if (distance > 0)
                        {
                            a0 = m;
                            b0 = n;
                            for (int t = 0; t <= a0; t++)
                            {
                                EQQ.Enqueue(single[t]);
                            }
                            for (int t = b0; t > a0; t--)
                            {
                                EQQ.Enqueue(single[t]);
                            }
                            for (int t = b0 + 1; t < single.Length; t++)
                            {
                                EQQ.Enqueue(single[t]);
                            }
                            way = EQQ.ToArray();
                            EQQ.Clear();
                            int[] way_0 = new int[way.Length - 2];
                            for (int t = 0; t < way_0.Length; t++)
                            {
                                way_0[t] = way[t + 1];
                            }
                            for (int i = array[k]; i <= array[k + 1]; i++)
                            {
                                Jiagong[i] = way[i - array[k]];
                            }
                            ENQ.Clear();
                            for (int i = array[k]; i <= array[k + 1]; i++)
                            {
                                ENQ.Enqueue(Jiagong[i]);
                            }
                            single = ENQ.ToArray();
                        }
                    }
                }
            }
        }
        public void Update_tuo(int t_popsize, int[] Jiagong, int t_n)//更新轨迹强度
        {
            int line_num = 0;
            int t_order = 0;
            double sum = 0;
            int vehicle_num = 0;
            for (int j = 1; j < Jiagong.Length; j++)
            {
                if (Jiagong[j] != 0)
                {

                    tuo[vehicle_num, line_num, Jiagong[j]] = tuo[vehicle_num, line_num, Jiagong[j]] + 1.0 / dim;
                    line_num++;
                    t_order++;
                }
                else
                {
                    vehicle_num++;
                    line_num = 0;
                }
            }
            for (int i = 0; i < vehicle_num; i++)
            {
                for (int j = 0; j < t_n; j++)
                {
                    sum = 0.0;
                    for (int k = 1; k <= t_n; k++)
                    {
                        sum += tuo[i, j, k];
                    }
                    for (int k = 0; k <= t_n; k++)
                    {
                        tuo[i, j, k] = tuo[i, j, k] / (sum);
                    }
                }
            }
        }
        public void generation(int t_n, int t_popsize, double[,] t_testdata, int gen,int Search_Local)//每代进化程序
        {
            double fitness = 0;
            AntMove(t_popsize, t_n);
            for (int i = 0; i < t_popsize - 1; i++)
            {
                for (int j = i + 1; j < t_popsize; j++)
                {
                    if (newpop[i].fitness1 > newpop[j].fitness1)/////交换
                    {
                        int[] t_individual2 = new int[100];
                        double fitness2 = 0;
                        t_individual2 = newpop[i].chrom;
                        fitness2 = newpop[i].fitness1;
                        newpop[i] = newpop[j];
                        newpop[j].chrom = t_individual2;
                        newpop[j].fitness1 = fitness2;
                    }
                }
            }
            if (gen == 1)
            {
                globle_Rout = newpop[0].Rout;
            }
            else
            {
                if (globle_Rout > newpop[0].Rout)
                {
                    globle_Rout = newpop[0].Rout;
                }
            }           
            if (Search_Local != 0)
            {
                int[] good_order = new int[newpop[0].order.Length];
                for (int k = 0; k < newpop[0].order.Length; k++)
                {
                    good_order[k] = newpop[0].order[k];
                }
                for (int k = 0; k < 6; k++)
                {
                    two_opt(good_order);
                    inherent_2opt(good_order);
                }
                fitness = calc_length(good_order.Length, good_order);
                if (fitness < newpop[0].fitness1)
                {
                    for (int k = 0; k < newpop[0].order.Length; k++)
                    {
                        newpop[0].order[k] = good_order[k];
                    }
                    newpop[0].fitness1 = fitness;
                }
            }
            Update_tuo(t_popsize, newpop[0].order, t_n);
        }
        public void Main(int S_search)
        {
            create_testdata(dim);
            int i;
            int gen = 0;
            double g_min = 0;
            Queue<int> OPT = new Queue<int>();
            Queue<int> OPQ = new Queue<int>();
            init_tuo(popsize, dim, testdata);
            do
            {
                gen++;
                generation(dim, popsize, testdata, gen,S_search);
                min = newpop[0].fitness1;
                OPT.Clear();
                for (int j = 0; j < newpop[0].order.Length; j++)
                {
                    OPT.Enqueue(newpop[0].order[j]);
                }
                if (gen == 1)
                {
                    g_min = min;
                    min_vehicle = newpop[0].Rout;
                    g_min_permutation = OPT.ToArray();
                }
                else
                {
                    if ((min_vehicle > newpop[0].Rout) || (min_vehicle == newpop[0].Rout && g_min > min))
                    {
                        min_vehicle = newpop[0].Rout;
                        g_min = min;
                        g_min_permutation = OPT.ToArray();
                    }
                }
                OPQ.Clear();
                for (int l = 0; l < g_min_permutation.Length; l++)
                {
                    OPQ.Enqueue(g_min_permutation[l]);
                }
                Pbest_each_gen[gen] = g_min;
                if (gen == 1)
                {
                    P_0 = g_min;
                    P_gen0 = OPT.ToArray();
                }
                else
                {
                    P_gen100 = OPQ.ToArray();
                    P_100 = g_min;
                }
            }
            while (gen < Genmax);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (creat_num == 1)
            {
                string ii, jj;
                PointPairList list1 = new PointPairList();
                int time_end, time_consume1;
                int search = 0;
                double time_consume;
                time_start = Environment.TickCount;
                Main(search);
                time_end = Environment.TickCount;
                time_consume1 = time_end - time_start;
                time_consume = (double)time_consume1;
                this.zedGraphControl1.GraphPane.CurveList.Clear();
                this.zedGraphControl1.GraphPane.GraphObjList.Clear();
                this.zedGraphControl1.Refresh();
                for (int i = 0; i < P_gen100.Length; i++)
                {
                    list1.Add(city[P_gen100[i], 0], city[P_gen100[i], 1]);
                    this.listBox1.Items.Add("第" + i.ToString() + "代的最优值为：" + Pbest_each_gen[i].ToString());
                }
                this.listBox1.Items.Add("运行时间：" + (time_consume / 1000).ToString() + "秒");
                this.zedGraphControl1.GraphPane.AddCurve("无保优", list1, Color.Red, SymbolType.Diamond);
                this.zedGraphControl1.AxisChange();
                this.zedGraphControl1.Refresh();
                ii = "无局部"+"  " + P_0.ToString() + "  " + "第1代的最优个体位置:" + "  ";
                for (int i = 0; i < P_gen0.Length; i++)
                {
                    ii += P_gen0[i].ToString() + "  ";
                }
                this.listBox2.Items.Add(ii);
                jj = "无局部" + "  " + P_100.ToString() + "  " + "第" + Genmax.ToString() + "代的最优个体位置：" + "  ";
                for (int i = 0; i < P_gen100.Length; i++)
                {
                    jj += P_gen100[i].ToString() + "  ";
                }
                this.listBox2.Items.Add(jj);
            }
            else 
            {
                MessageBox.Show("请生成问题", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } 
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (creat_num == 1)
            {
                string ii, jj;
                PointPairList list1 = new PointPairList();
                int time_end, time_consume1;
                int search = 1;
                time_start = Environment.TickCount;
                Main(search);
                time_end = Environment.TickCount;
                time_consume1 = time_end - time_start;
                time_consume = (double)time_consume1;
                this.zedGraphControl1.GraphPane.CurveList.Clear();
                this.zedGraphControl1.GraphPane.GraphObjList.Clear();
                this.zedGraphControl1.Refresh();
                for (int i = 0; i < P_gen100.Length; i++)
                {
                    list1.Add(city[P_gen100[i], 0], city[P_gen100[i], 1]);
                    this.listBox1.Items.Add("第" + i.ToString() + "代的最优值为：" + Pbest_each_gen[i].ToString());
                }
                this.listBox1.Items.Add("运行时间：" + (time_consume / 1000).ToString() + "秒");
                this.zedGraphControl1.GraphPane.AddCurve("无保优", list1, Color.Red, SymbolType.Diamond);
                this.zedGraphControl1.AxisChange();
                this.zedGraphControl1.Refresh();
                ii = "带局部" + "  " + P_0.ToString() + "  " + "第1代的最优个体位置:" + "  ";
                for (int i = 0; i < P_gen0.Length; i++)
                {
                    ii += P_gen0[i].ToString() + "  ";
                }
                this.listBox2.Items.Add(ii);
                jj = "带局部" + "  " + P_100.ToString() + "  " + "第" + Genmax.ToString() + "代的最优个体位置：" + "  ";
                for (int i = 0; i < P_gen100.Length; i++)
                {
                    jj += P_gen100[i].ToString() + "  ";
                }
                this.listBox2.Items.Add(jj);
            }
            else 
            {
                MessageBox.Show("请生成问题", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void Form4_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("d:");
            comboBox1.Items.Add("e:");
            comboBox1.Items.Add("f:");
            this.zedGraphControl1.GraphPane.Title.Text = "PBIL";
            this.zedGraphControl1.GraphPane.XAxis.Title.Text = "纬度坐标";
            this.zedGraphControl1.GraphPane.YAxis.Title.Text = "经度坐标";
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if ((this.textBox1.Text != "")&&(this.textBox2.Text != ""))
            {
                num = textBox1.Text.Length;
                num_0 = textBox2.Text.Length;
                int a = num;
                int b = num_0;
                char[] length = new char[num];
                char[] length_0 = new char[num_0];
                length = textBox1.Text.ToCharArray();
                length_0 = textBox2.Text.ToCharArray();
                for (int i = 0; i < num; i++)
                {
                    if (length[i] < '0' || length[i] > '9')
                    {

                        MessageBox.Show("输入有误，请输入整数", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.textBox1.Text = "";
                        break;
                    }
                }
                for (int i = 0; i < num_0; i++)
                {
                    if (length_0[i] < '0' || length_0[i] > '9')
                    {

                        MessageBox.Show("输入有误，请输入整数", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.textBox2.Text = "";
                        break;
                    }
                }
                if ((length[num - 1] >= '0' && length[num - 1] <= '9' && length_0[num_0 - 1] >= '0' && length_0[num_0 - 1] <= '9') && (Form3.creat_kehu ==1)&&(Form5.creat_vehicle==1))
                {
                    dim = Form3.kk;
                    Genmax = int.Parse(textBox1.Text.Trim());
                    popsize = int.Parse(textBox2.Text.Trim());
                    city = Form3.data;
                    M1 = Form5.M1;
                    if (M1 == 0)
                    {                       
                        this.Hide();
                        MessageBox.Show("请生成车辆信息", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Form5 form5 = new Form5();
                        form5.Show(); 
                    }
                    testdata = new double[dim + 1, dim + 1];
                    kehuweight = new double[dim + 1];
                    kehuweight = Form3.weight;
                    P_gen0 = new int[dim];
                    P_gen100 = new int[dim];
                    g_min_permutation = new int[dim];
                    Pbest_each_gen = new double[Genmax + 1];
                    tuo = new double[dim, dim, dim + 1];
                    newpop = new individual[popsize];
                    create_testdata(dim);
                }
                else
                {
                    if (Form5.creat_vehicle == 0)
                    {
                        this.Hide();
                        MessageBox.Show("请生成车辆信息", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Form5 form5 = new Form5();
                        form5.Show();
                    }                  
                    else
                    {
                        if (Form3.creat_kehu == 0)
                        {
                            this.Hide();
                            MessageBox.Show("请生成客户信息", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Form3 form3 = new Form3();
                            form3.Show();
                        }                       
                    }
                }
            }
            else
            {
                MessageBox.Show("循环代数和种群数量不能为空，请输入整数", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }         
        }

        private void button3_Click(object sender, EventArgs e)
        {           
            if (textBox3.Text == "" || this.comboBox1.Text == "")
            {
                MessageBox.Show("输入名称和路径", "信息提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            else
            {
                string fileName = comboBox1.Text +"\\"+ textBox3.Text+".txt";
                FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None);
                // FileStream fa = new FileStream(Name, FileMode.Append, FileAccess.Write, FileShare.None);
                byte[] byteText = System.Text.Encoding.ASCII.GetBytes("length" + "\r\0" + P_100.ToString() + "\r\0" + "\r\0" + "time" + "\r\0" + (time_consume / 1000).ToString() + "\r\0" + "\r\0" + "vehicle" + "\r\0" + min_vehicle.ToString() + "\r\n");// "\r\0"
                fs.Write(byteText, 0, byteText.Length);
                for (int i = 0; i < P_gen100.Length; i++)
                {
                    byte[] byteText0 = System.Text.Encoding.ASCII.GetBytes(P_gen100[i].ToString() + "\r\0");// "\r\0"
                    fs.Write(byteText0, 0, byteText0.Length);
                    if (i == P_gen100.Length - 1)
                    {
                        byte[] byteText1 = System.Text.Encoding.ASCII.GetBytes("over" + "\r\n");// "\r\0"
                        fs.Write(byteText1, 0, byteText1.Length);
                    }
                }
                fs.Close();
                MessageBox.Show("保存成功", "信息提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }          
        }
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }
    }
}