using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Runtime.Serialization;
using System.Net;
using System.Runtime.Serialization.Json;
using BingMapsRESTService.Common.JSON;
using Microsoft.Maps.MapControl.WPF;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace WpfMapControls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    
    {
        private const string KEY = "Au1qF3-RmEKTs2aAiGvmGvpDVBcg-KD5tp1pdbRUM4qGMvBdMAHccJgFOyddKMVr";
        public const string FILE_NAME = "lineToStopsListData";

        /*private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);*/
        /////////////////////////////////

        public Dictionary<int, List<BusStop>> lineToStopsList = new();

        public string? sessionKey;
        public MainWindow()
        {
            InitializeComponent();

            



            ///Get data from JSON
            try
            {
                FileStream stream = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
                IFormatter formatter = new BinaryFormatter();
                lineToStopsList = (Dictionary<int, List<BusStop>>)formatter.Deserialize(stream);
                //lineToStopsList = JsonSerializer.Deserialize<Dictionary<int, List<BusStop>>>(stream);
                stream.Close();
                //DemoTextBlock.Text = lineToStopsList.Count.ToString();
            }
            catch (Exception e)
            {
                //DemoTextBlock.Text = e.Message;
            }
            //Pushpin.Template = FindResource("PushpinControlTemplate") as System.Windows.Controls.ControlTemplate;

/*
            string sessionKey = "f";
            MainMap.CredentialsProvider.GetCredentials((c) =>
            {
                sessionKey = c.ApplicationId;


                

            });
            */
            //BusStopList.Items.Add(sessionKey);
            
            string from = "Varna";
            string to = "Dobrich";

            //MainMap.Children.Add(new Pushpin() { Location = from }) ;

            
            



        }
        /// <summary>
        /// Auto method for GET requests
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="callback"></param>
      

        /// <summary>
        /// Moving the PushPoint on MouseRightClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       

        /// <summary>
        /// Removing the Close Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            //SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        /// <summary>
        /// Adding stops to the List Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddStopButtonClick(object sender, RoutedEventArgs e)
        {
            int BusLine = int.Parse(BusLaneTextBox.Text);
            string programName = ProgramNameTextBox.Text;
            string label = LabelTextBox.Text;
            if (label == "")
            {
                foreach(var v in lineToStopsList)
                {
                    foreach(var a in v.Value)
                    {
                        if(a.ProgramName == programName)
                        {
                            label = a.Label;
                            Pushpin_A.Location = (Microsoft.Maps.MapControl.WPF.Location)a.location;
                        }
                    }
                }

            }

            if (!lineToStopsList.ContainsKey(BusLine))
            {
                

                List<BusStop> stops = new();
                stops.Add(new BusStop((SerializableLocation)Pushpin_A.Location,programName,label));
                lineToStopsList.Add(BusLine,stops);
            }
            else
            {
                lineToStopsList[BusLine].Add(new BusStop((SerializableLocation)Pushpin_A.Location, programName, label));
            }
        }


        /// <summary>
        /// Saving data to JSON
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            FileStream stream = new FileStream(FILE_NAME, FileMode.Create,FileAccess.ReadWrite);
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, lineToStopsList);
            
            //JsonSerializer.Serialize(stream,lineToStopsList, typeof(Dictionary<int, List<BusStop>>));
            stream.Close();
        }
        bool sw = false; List<Pushpin> dataList = new();
        public string checkstring;
        private void CheckButtonClick(object sender, RoutedEventArgs e)
        {
            
            if (sw == false)
            {
                sw = !sw;
                Pushpin a;

                foreach (var liniq in lineToStopsList)
                {

                    
                    int s = liniq.Key;

                    foreach (var spirka in liniq.Value)
                    {
                        a = new Pushpin();
                        a.Location = (Microsoft.Maps.MapControl.WPF.Location)spirka.location;
                        a.Content = liniq.Key;
                        a.MouseEnter += (s, e) => { Check1.Content = spirka.ProgramName; Check2.Content = spirka.Label; };
                        
                        dataList.Add(a);
                        MainMap.Children.Add(a/*new Pushpin { MouseDoubleClick += (x,y) => Console.WriteLine(), Location = (Microsoft.Maps.MapControl.WPF.Location)spirka.location, Content = liniq.Key }*/);


                    }
                    
                }
                return;
            }
            else
            {
                
                sw = !sw;
                foreach (var v in dataList)
                {
                    MainMap.Children.Remove(v);
                }
                return;
            }
                
            
        
    }

       

        private void Minimize(object sender, RoutedEventArgs e)
        {
            if(this.WindowState == WindowState.Normal)
            {
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
                return;
            }
            this.WindowStyle = WindowStyle.SingleBorderWindow;
            this.WindowState = WindowState.Normal;
        }

        private void MainMap_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            System.Windows.Point mousePosition = e.GetPosition(this);
            mousePosition.X -= Cl.Width.Value;
            Microsoft.Maps.MapControl.WPF.Location location = MainMap.ViewportPointToLocation(mousePosition);
            Pushpin_B.Location = location;
        }
        private void MainMap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point mousePosition = e.GetPosition(this);
            mousePosition.X -= Cl.Width.Value;
            Microsoft.Maps.MapControl.WPF.Location location = MainMap.ViewportPointToLocation(mousePosition);
            Pushpin_A.Location = location;
        }
        List<Pushpin> activePushpins = new();
        List<MapPolyline> activePolylines = new();
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            activePushpins = new();
            int line;
            List<BusStop> f = new();
            LocationCollection A;
            f = Route.GetLocationsList(this.lineToStopsList, Pushpin_A.Location, Pushpin_B.Location, out line);
            /* if (line == 148)
             {
                 A = RouteProvider.GetLocations("Au1qF3-RmEKTs2aAiGvmGvpDVBcg-KD5tp1pdbRUM4qGMvBdMAHccJgFOyddKMVr", Route.GetLocationsList(this.lineToStopsList, Pushpin_B.Location, Pushpin_A.Location, out line), out _);
             }*/
            string key = KEY;
            if (line == 148)
            {
                if (Pushpin_A.Location.Latitude > Pushpin_B.Location.Latitude)
                    A = RouteProvider.GetLocations(key, f = Route.GetLocationsList(this.lineToStopsList, Pushpin_A.Location, Pushpin_B.Location, out line), out _);
                else { A = RouteProvider.GetLocations(key,  Route.GetLocationsList(this.lineToStopsList, Pushpin_B.Location, Pushpin_A.Location, out line), out _);f.Reverse(); }
            }
            else
            {
                if (Pushpin_A.Location.Latitude < Pushpin_B.Location.Latitude)
                    A = RouteProvider.GetLocations(key, f = Route.GetLocationsList(this.lineToStopsList, Pushpin_A.Location, Pushpin_B.Location, out line), out _);
                else { A = RouteProvider.GetLocations(key, f = Route.GetLocationsList(this.lineToStopsList, Pushpin_B.Location, Pushpin_A.Location, out line), out _);f.Reverse(); }

            }
            MapPolyline routeLine = new();
            routeLine.StrokeThickness = 5;
            routeLine.Stroke =new SolidColorBrush(Colors.Blue);
            routeLine.Locations = A;



            MainMap.Children.Add(routeLine);
            activePolylines.Add(routeLine);

            FinalLabel.Content = f[^1].Label;
            StartLabel.Content = f[0].Label;
            

            foreach (var p in f)
            {
                activePushpins.Add(new Pushpin()
                {
                    Location = (Microsoft.Maps.MapControl.WPF.Location)p.location,
                    Content = line,
                    
                    
                });;
                activePushpins[^1].MouseEnter += (o, e) => DataLabel.Content = p.Label;
                BusStopList.Items.Add(p.Label);
            }
            activePushpins[0].Background = new SolidColorBrush(Colors.Green);
            activePushpins[^1].Background = new SolidColorBrush(Colors.Green);

            foreach (var p in activePushpins)
            {
                MainMap.Children.Add(p);
            }
            LineLabel.Content = line.ToString();

            //foreach (var v in c)

            //BusStopList.Items.Add((string)v);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataLabel.Content = "";
            LineLabel.Content = "";
            StartLabel.Content = "";
            FinalLabel.Content = "";
            BusStopList.Items.Clear();

           foreach(var p in activePushpins)
            {
                MainMap.Children.Remove(p);
            }
           foreach(var p in activePolylines)
            {
                MainMap.Children.Remove(p);
            }

            activePolylines = new();
            activePushpins = new();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (MainMap.Mode.ToString() == "Microsoft.Maps.MapControl.WPF.RoadMode")
                MainMap.Mode = new AerialMode(true);
            else MainMap.Mode = new RoadMode();
        }
    }
}
