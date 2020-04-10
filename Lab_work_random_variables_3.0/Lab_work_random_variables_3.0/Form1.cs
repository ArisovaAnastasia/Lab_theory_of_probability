using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace Lab_work_random_variables_3._0
{
    public partial class Form1 : Form
    {
        public int n = 0;
        public double _lmb;
        public double lmb;
        public List<double> rand_val;
        public Form1()
        {
            InitializeComponent();
        }

        private void DrawGraph_1()
        {
            // Получим панель для рисования
            GraphPane pane = zedGraphControl2.GraphPane;

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane.CurveList.Clear();

            int itemscount = 19;

            Random rnd = new Random();

            // Высота столбиков
            double[] values = new double[itemscount];

            // Заполним данные
            for (int i = 0; i < itemscount; i++)
            {
                values[i] = rnd.NextDouble();
            }

            // Создадим кривую-гистограмму
            // Первый параметр - название кривой для легенды
            // Второй параметр - значения для оси X
            // Третий параметр - значения для оси Y
            // Четвертый параметр - цвет
            BarItem bar = pane.AddBar("Гистограмма", null, values, Color.Blue);

            // !!! Расстояния между кластерами (группами столбиков) гистограммы = 0.0
            // У нас в кластере только один столбик.
            pane.BarSettings.MinClusterGap = 0.0f;

            // Вызываем метод AxisChange (), чтобы обновить данные об осях.
            zedGraphControl2.AxisChange();

            // Обновляем график
            zedGraphControl2.Invalidate();
        }

        private void DrawGraph_2()
        {
            // Получим панель для рисования
            GraphPane pane = zedGraphControl2.GraphPane;

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane.CurveList.Clear();

            int itemscount = 19;

            Random rnd = new Random();

            // Высота столбиков
            double[] values = new double[itemscount];
            string[] name = new string[itemscount];

            // Заполним данные
            for (int i = 0; i < itemscount; i++)
            {
                values[i] = rnd.NextDouble();
                name[i] = string.Format("" + (i + 1), i);
            }

            pane.AddBar(null, null, values, Color.Blue);
            pane.XAxis.Type = AxisType.Text;
            pane.XAxis.Scale.TextLabels = name;
            pane.YAxis.Scale.Min = 0.0;
            pane.YAxis.Scale.Max = values.Max() + 0.001;
            pane.BarSettings.MinClusterGap = 0.0f;

            // Вызываем метод AxisChange (), чтобы обновить данные об осях.
            zedGraphControl2.AxisChange();

            // Обновляем график
            zedGraphControl2.Invalidate();
        }

        public static double G(double x, double _lmb)
        {
            return (-_lmb * Math.Log(x));
        }

        public static double F(double x, double lmb)
        {
            if (x <= 0.0)
                return 0.0;
            else
                return (1.0 - Math.Exp(-lmb * x));
        }

        public static double f(double x, double lmb)
        {
            if (x < 0.0)
                return 0.0;
            else
                return (lmb * Math.Exp(-lmb * x));
        }

        public static List<double> Simulate(int n, double _lmb)
        {
            List<double> rand_val = new List<double>();
            double rv = 0.0;
            Random rnd = new Random(DateTime.Now.Second);

            for (int i = 0; i < n; i++)
            {
                rv = rnd.Next();
                rv = (double)rv / Int32.MaxValue;
                rand_val.Add(G(rv, _lmb));
            }
            rand_val.Sort();
            return rand_val;
        } // Проведение экспериментов

        void InitTable1(int n, List<double> rand_val)
        {
            // Если проведено больше 1000 экспериментов, то в таблице отображается только 1000 записей с соответствующим шагом

            if (n <= 1000)
            {
                for (int i = 0; i < n; i++)
                {
                    dataGridView1.Rows.Add();

                    dataGridView1.Rows[i].Cells[0].Value = i;
                    dataGridView1.Rows[i].Cells[1].Value = rand_val[i];
                }
            }
            else
            {
                int h = n / 1000;
                for (int i = 0, j = 0; i < 1000; i++)
                {
                    dataGridView1.Rows.Add();

                    dataGridView1.Rows[i].Cells[0].Value = j;
                    dataGridView1.Rows[i].Cells[1].Value = rand_val[j];
                    j += h;
                }

            }
        } // Полученные значения с.в.

        void InitTable2(int n, List<double> rand_val)
        {
            //Подсчет характеристик

            double M = 1.0 / lmb; // математическое ожидание
            double D = 1.0 / (lmb * lmb); // дисперсия
            double _x_ = 0.0; // выборочное среднее
            double S_2 = 0.0; // выборочная дисперсия
            double R = 0.0; // размах выборки
            double Me = 0.0; // выборочная медиана
            double temp;

            for (int i = n - 1; i >= 0; i--)
            {
                _x_ += rand_val[i];
            }
            _x_ /= (double)n;

            for (int i = n - 1; i >= 0; i--)
            {
                temp = rand_val[i];
                S_2 += (temp - _x_) * (temp - _x_);
            }
            S_2 /= (double)n;

            R = rand_val[n - 1] - rand_val[0];

            int k = n / 2;
            if (n % 2 == 0)
            {
                // если n - четное
                Me = (rand_val[k] + rand_val[k - 1]) / 2.0;
            }
            else
            {
                // если n - нечетное
                Me = rand_val[k];
            }

            dataGridView2.Rows[0].Cells[0].Value = M;
            dataGridView2.Rows[0].Cells[1].Value = _x_;
            dataGridView2.Rows[0].Cells[2].Value = Math.Abs(M - _x_);
            dataGridView2.Rows[0].Cells[3].Value = D;
            dataGridView2.Rows[0].Cells[4].Value = S_2;
            dataGridView2.Rows[0].Cells[5].Value = Math.Abs(D - S_2);
            dataGridView2.Rows[0].Cells[6].Value = Me;
            dataGridView2.Rows[0].Cells[7].Value = R;
        } // Выборочные характеристики
        

        void DrawF(double lmb, List<double> rand_val)
        {
            ZedGraph.PointPairList F_list = new ZedGraph.PointPairList();
            ZedGraph.PointPairList Fn_list = new ZedGraph.PointPairList();
            ZedGraph.PointPairList f_list = new ZedGraph.PointPairList();
            double D = 0.0; // Мера расхождения
            double Fn;
            //for (int i = 0; i < n; i++)
            //{
            //    Fn = (double)i / (double)n;

            //    Fn_list.Add(rand_val[i], Fn);
            //    D = Math.Max(D, Math.Abs(Fn - F(rand_val[i], lmb)));

            //}
            //textBox3.Text = D.ToString();
            double h = rand_val[n-1]/ 1000.0;
            
            int sum = 0;
            for(int i = 0; i < 1000; i++)
            {
                sum = 0;
                for(int j = 0; j< n;j++)
                {
                    double temp = rand_val[0] + h * i;
                    if (rand_val[j] < rand_val[0] + h * i)
                        sum++;
                }
                Fn_list.Add(rand_val[0] + h * i, (double)sum/(double)n);
                F_list.Add(rand_val[0] + h * i, F(h*i, lmb));
                D = Math.Max(D, Math.Abs((double)sum/(double)n - F(rand_val[0] + h * i, lmb)));

            }
            textBox3.Text = D.ToString();
            zedGraphControl1.GraphPane.CurveList.Clear();


            ZedGraph.LineItem CurveF = zedGraphControl1.GraphPane.AddCurve("F", F_list, Color.FromName("Red"), ZedGraph.SymbolType.None);
            ZedGraph.LineItem CurveFn = zedGraphControl1.GraphPane.AddCurve("Fn", Fn_list, Color.FromName("Blue"), ZedGraph.SymbolType.None);


            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();

        } // График функций распределения

        void Drawf(double lmb, List<double> rand_val)
        {

            ZedGraph.PointPairList f_list = new ZedGraph.PointPairList();

            double h = 10.0 / 1000.0;
            for (int i = 0; i < 1000; i++)
            {

                f_list.Add(h * i, f(h * i, lmb));
            }

            zedGraphControl3.GraphPane.CurveList.Clear();

       
            ZedGraph.LineItem Curvef = zedGraphControl3.GraphPane.AddCurve("f", f_list, Color.FromName("Red"), ZedGraph.SymbolType.None);
            zedGraphControl3.GraphPane.YAxis.Scale.Min = 0.0;
            zedGraphControl3.GraphPane.XAxis.Scale.Max = 5.001;
            zedGraphControl3.AxisChange();
            zedGraphControl3.Invalidate();
        } // График функции плотности

        void Gistogramm(int N, List<double> rand_val)
        {

            double[] interval = new double[N + 1];
            double[] value = new double[N];
            string[] name = new string[N];

            double max = 0.0;

            double h = (rand_val[n - 1] - rand_val[0]) / N;

            for (int i = 0; i < N; i++)
            {
                interval[i] = rand_val[0] + (double)i * h;
                name[i] = string.Format("" + (i + 1), i);


            }
            interval[N] = rand_val[n - 1];


            //int sum, index = 0;
            //for (int i = 0; i < N; i++)
            //{
            //    sum = 0;
            //    for(int j = index; j < n; j++)
            //    {
            //        if (rand_val[j] < interval[i + 1])
            //            sum++;
            //        else
            //        {
            //            index = j - 1;
            //            break;
            //        }
            //    }


            //    value[i] = (double)sum/(h*(double)n);
            //}

            int sum;
            for (int i = 0; i < N; i++)
            {
                sum = 0;
                for (int j = 0; j < n; j++)
                {
                    if ((interval[i] < rand_val[j])&&(rand_val[j] <= interval[i + 1]))
                        sum++;
                }
                
                value[i] = (double)sum / (h * (double)n);
            }

            GraphPane pane = zedGraphControl2.GraphPane;
            pane.CurveList.Clear();

            BarItem curve = pane.AddBar(null, null, value, Color.Blue);
            pane.XAxis.Type = AxisType.Text;
            pane.XAxis.Scale.TextLabels = name;
            pane.YAxis.Scale.Min = 0.0;
            pane.YAxis.Scale.Max = value.Max() + 0.001;
            pane.BarSettings.MinClusterGap = 0.0f;

            zedGraphControl2.AxisChange();
            zedGraphControl2.Invalidate();

            //Работа с третьей таблицей
            
            for (int i = 0; i < N; i++)
            {
                dataGridView3.Columns[i].HeaderText = string.Format("z" + (i + 1), i);
                dataGridView3.Rows[0].Cells[i].Value = interval[i] + h * 0.5;
                dataGridView3.Rows[1].Cells[i].Value = f(interval[i] + h * 0.5, lmb);
                dataGridView3.Rows[2].Cells[i].Value = value[i];
                if (Math.Abs(value[i] - f(interval[i] + h * 0.5, lmb)) > max)
                    max = Math.Abs(value[i] - f(interval[i] + h * 0.5, lmb));
             }
            textBox5.Text = max.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            n = System.Convert.ToInt32(textBox1.Text.ToString());
            if (n <= 0)
                n = 10;
            lmb = System.Convert.ToDouble(textBox2.Text.ToString());
            _lmb = 1.0 / lmb;
            if (lmb < 0)
            {
                lmb = 1;
                _lmb = 1;
            }

            rand_val = Simulate(n, _lmb);
            //textBox5.Text = rand_val[0].ToString();
            //textBox6.Text = rand_val[n - 1].ToString();
            dataGridView1.Rows.Clear();
            InitTable1(n, rand_val);
            InitTable2(n, rand_val);
            DrawF(lmb, rand_val);
            Drawf(lmb, rand_val);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int N = System.Convert.ToInt32(textBox4.Text.ToString());

            dataGridView3.ColumnCount = N;
            dataGridView3.RowCount = 3;
            dataGridView3.Rows[0].HeaderCell.Value = "z_j";

            Gistogramm(N, rand_val);

        }
    }
}
