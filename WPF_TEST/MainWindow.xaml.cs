using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime;
using System.Runtime.InteropServices;
using AntAlgo;
namespace WPF_TEST
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public class Callback : EventsCallback
    {
        public void Handle(double time, Ant ant, int type)
        {
            
        }
    }
    

    public partial class MainWindow : Window
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int AllocConsole();

        AntColonyControl colony = new AntColonyControl();
        Map map = new Map();
        
        public MainWindow()
        {

            InitializeComponent();
            AllocConsole();
            
            grid.ShowGridLines = true;
            for (int i = 0; i < 10; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(1, GridUnitType.Star);
                ColumnDefinition columnDefinition = new ColumnDefinition();
                columnDefinition.Width = new GridLength(1, GridUnitType.Star);

                grid.RowDefinitions.Add(rowDefinition);
                grid.ColumnDefinitions.Add(columnDefinition);
            }
            map.Init(10, 10, 10);
            for (int i = 0;i< 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.Set(2, i, j, -1);
                    map.Set(4, i, j, -1);
                }
            }
            Callback cb = new Callback();
            Ant nw = new Ant();
            DecisionMakeStandard dm = new DecisionMakeStandard();

            nw.Init(0, 0, 1, Math.PI / 2, 5, map, dm, cb,new Random().Next());
            colony.Add(nw);
            Console.WriteLine(nw.id);

            var rev = new Ant();
            DecisionMakeStandard dm2 = new DecisionMakeStandard();
            dm2.state = 1;
            rev.Init(9.9, 9.9, 1, Math.PI / 2, 5, map, dm2, cb, new Random().Next());
            rev.angle = Math.PI;
            Console.WriteLine(rev.id);

           /* Set1(2, 0);
            Set1(3, 0);
            Set1(2, 1);*/
            colony.Add(rev);

            MakeStep();
            Button btn = new Button();
            Grid.SetRow(btn, 9);
            Grid.SetColumn(btn, 5);
            grid.Children.Add(btn);
            btn.Click += Btn_Click;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            time++;
            colony.Move(time);
            MakeStep();
        }
        void Set1(int x,int y)
        {
            TextBlock tbx = new TextBlock();
            tbx.HorizontalAlignment = HorizontalAlignment.Stretch;
            tbx.VerticalAlignment = VerticalAlignment.Stretch;
            Brush bs = new SolidColorBrush(Colors.Black);
            bs.Opacity = 0.5;
            tbx.Background = bs;
            map.Set(0, x, y, 1);
            grid.Children.Add(tbx);
            Grid.SetColumn(tbx, x);
            Grid.SetRow(tbx, y);

        }
        double time = 0;
        List<object> forClear= new List<object>();
        void MakeStep()
        {
            canvas.Children.Clear();
            foreach (var h in forClear)
            {
                grid.Children.Remove((UIElement)h);
            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    TextBlock tb = new TextBlock();
                    tb.Text = $"{map.Get(2, i, j)}\n{map.Get(4, i, j)}";
                    tb.Foreground = Brushes.Blue;

                    grid.Children.Add(tb);
                    forClear.Add(tb);
                    tb.SetValue(Grid.ColumnProperty, i);
                    tb.SetValue(Grid.RowProperty, j);
                    tb.SetValue(Grid.ZIndexProperty, -1000);
                }
            }

            GeometryGroup gms = new GeometryGroup();
            foreach (var ant in colony.st) {

                Ellipse el = new Ellipse();
                el.Width = 4;
                el.Height = 4;
                el.Stroke = Brushes.Red;
                el.Fill = Brushes.Red;
                Point pt = new Point();
                pt.X = (double)ant.GetRealPos().First;
               
                pt.Y = (double)ant.GetRealPos().Second;

                Line line = new Line();
                Brush bt = new SolidColorBrush(Colors.Black);
                bt.Opacity = 0.2;

                line.X1 = pt.X / 10 * 431;
                line.Y1 = pt.Y / 10 * 431;
                line.X2 = line.X1 + Math.Cos(ant.GetAngle())/10*431;
                line.Y2 = line.Y1 + Math.Sin(ant.GetAngle())/10*431;
                line.StrokeThickness = 2;

                line.Stroke = bt;
               
                canvas.Children.Add(el);
                canvas.Children.Add(line);
                line.SetValue(Canvas.LeftProperty, 0d);
                line.SetValue(Canvas.TopProperty, 0d);
                el.SetValue(Canvas.LeftProperty, pt.X / 10 * 431);
                el.SetValue(Canvas.TopProperty, pt.Y / 10 * 431);

                
            }
            
        }
    }
}
