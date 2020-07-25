using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;
using SciChart;
using SciChart.Charting.Visuals;

namespace ModelAttemptWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Simulation simulation;
        private Process process = null;

        Random random = new Random();
        public DispatcherTimer Clock { get; set; } = new DispatcherTimer();
        private DispatcherTimer MinClock {get;set;}=new DispatcherTimer();
        OSN twitter;
        private string wheelGraphPath = "C:/Users/Anni/Documents/Uni/Computer Science/Proj/wheel_graph.csv";
        private string realFBEdges = @"C:\Users\Anni\Documents\Uni\Computer Science\Proj\facebook_combined.txt\facebook_combined.csv";

        private int timeSlot = 1;

        public MainWindow()
        {
            this.simulation = new Simulation("SimpleVersion", 10);
            this.simulation.SimplePopulate(500);

            //SciChartSurface.SetRuntimeLicenseKey("9022a0a8 - 41e1 - 43e1 - a680 - 5afb55c964a6");
            Clock.Interval = TimeSpan.FromMilliseconds(150f / simulation.runSpeed);
            Clock.Tick += Update;
            MinClock.Interval = TimeSpan.FromMilliseconds(10f / simulation.runSpeed);
            Clock.Tick += UpdateSimulationTime;



            this.twitter = new OSN(this);

            // Give twitter a small initial population
            this.twitter.PopulateFromPeople(500,simulation.humanPopulation);
            this.twitter.CreateMutualFollowsFromGraph(wheelGraphPath);
            // DisplayOSN(twitter);

            // Create some news to be shared
            for (int i = 0; i < 10; i++)
            {
                twitter.CreateNews("FakeNews", false, twitter.accountList[random.Next(twitter.IDCount)], simulation.time,0.5,0.1);
            }

            for (int i = 0; i < 40; i++)
            {
                twitter.CreateNews("TrueNews", true, twitter.accountList[random.Next(twitter.IDCount)], simulation.time,0.5,1);
            }

            InitializeComponent();
            Clock.Start();
            MinClock.Start();
        }

        private void Update(object sender, EventArgs e)
        {
            simulation.time = 15; // A time slot represents a 15 minute period
            if (this.createFollowsCheckbox.IsChecked==true)
            {
                this.twitter.CreateRandomFollow();
            }
            this.twitter.TimeSlotPasses(simulation.time);
            this.simulation.timeStamps.Add(timeSlot);
            this.twitter.accountList[0].OutputPage(this);
            //this.DisplayOSN(twitter);
            timeSlot++;
        }

        private void UpdateSimulationTime(object sender, EventArgs e)
        {
            this.simulation.time++;
        }

        private void CreateGraphCSV(string n)
        {
            process = new Process();
            process.StartInfo.WorkingDirectory = @"C:\Users\Anni\PycharmProjects\NetworkGraphs";
            process.OutputDataReceived += (sender, e) => Console.WriteLine($"Recieved:\t{e.Data}");
            process.ErrorDataReceived += (sender, e) => Console.WriteLine($"ERROR:\t {e.Data}");
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;

            process.StartInfo.FileName = @"C:\Users\Anni\AppData\Local\Programs\Python\Python37\python.exe";
            /// python exe @"C:\Users\Anni\PycharmProjects\NetworkGraphs\tester_wheel_graph.py";
            process.StartInfo.Arguments = "tester_wheel_graph.py --n " + n;
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit(1000);
        }

        private List<string[]> LoadCsvFile(string filePath)
        {
            var reader = new StreamReader(File.OpenRead(filePath));
            List<string[]> searchList = new List<string[]>();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine(); // ignore the line of labels
                if (line != ",source,target")
                {
                    char[] seperator = new char[] { ',' };
                    string[] lineList = line.Split(seperator);
                    searchList.Add(lineList);
                }
            }
            return searchList;

        }


        public void CreateFollowsFromGraph(string filePath)
        {
            // Change this to create real connections? Not currently needed

            //List<string[]> connections = LoadCsvFile(filePath);
            //foreach (string[] connection in connections)
            //{
            //    // string[0] is the key and isn't necesary
            //    //Console.WriteLine("connection line:" + connection[0] + " " + connection[1] + " " + connection[2]);
            //    int followerID = Convert.ToInt16(connection[1]);
            //    int followeeID = Convert.ToInt16(connection[2]);
            //    this.Follow(accountList[followeeID], accountList[followerID]);
            //}
        }



        // GUI methods

        public void PrepareOSNGrid(OSN osn)
        {
            // Makes the GUI OSN grid big enough for all the accounts in the OSN
            double nAccounts = osn.IDCount;
            int n = OSNGrid.RowDefinitions.Count;
            if (nAccounts > n * n)
            {
                int newN = Convert.ToInt16(Math.Ceiling(Convert.ToDecimal(Math.Sqrt(nAccounts))));
                int nRowsToAdd = newN - n;
                for (int i = 0; i < nRowsToAdd; i++)
                {
                    OSNGrid.RowDefinitions.Add(new RowDefinition());
                    OSNGrid.ColumnDefinitions.Add(new ColumnDefinition());
                }

            }
        }

        public void AddGUIAccount(Account account)
        {
            int n = OSNGrid.RowDefinitions.Count;
            int row = account.ID / n;
            int column = (account.ID % n);


            // nameLabel.Background = new SolidColorBrush(Colors.White);
            ////nameLabel.Name = account.name.ToLower() + "Label";
            //Image newSphere = new Image
            //{
            //    Source = new BitmapImage(new Uri("sphere.png", UriKind.Relative))
            //};
            //newSphere.SetValue(Grid.RowProperty, row);
            //newSphere.SetValue(Grid.ColumnProperty, column);

            //this.OSNGrid.Children.Add(newSphere);
            // sphere actually gets covered by the label but it's still useful to have this code here as ref
            

            // now create following links from this account to people they follow
            foreach (Account followingAccount in account.following)
            {
                int followingRow = followingAccount.ID / n;
                int followingCol = followingAccount.ID % n;
                double squareWidth = OSNGrid.Width/n;

                this.AddConnection(row, column, followingRow, followingCol, squareWidth);
            }

            foreach (Post post in account.page)
            {
                this.AddGUINews(post.news,row,column);
            }
            Label nameLabel = new Label();
            nameLabel.Content = account.person.name;
            nameLabel.SetValue(Label.FontWeightProperty,FontWeights.ExtraBold);
            nameLabel.Margin = new Thickness(0);
            //nameLabel.Background = new SolidColorBrush(Colors.White);
            nameLabel.SetValue(Grid.RowProperty, row);
            nameLabel.SetValue(Grid.ColumnProperty, column);
            nameLabel.SetValue(Panel.ZIndexProperty, OSNGrid.Children.Count);
            this.OSNGrid.Children.Add(nameLabel);
        }

        public void AddConnection(int row1, int col1, int row2, int col2, double squareWidth)
        {
            Line connector = new Line();
            connector.Stroke = new SolidColorBrush(Colors.LightGreen);
            connector.X1 = (col1 + 0.1) * squareWidth;
            connector.Y1 = (row1 + 0.1) * squareWidth;
            connector.X2 = (col2 + 0.1) * squareWidth;
            connector.Y2 = (row2 + 0.1) * squareWidth;
            Grid.SetRowSpan(connector, OSNGrid.RowDefinitions.Count);
            Grid.SetColumnSpan(connector, OSNGrid.ColumnDefinitions.Count);
            OSNGrid.Children.Add(connector);
        }
        
        public void AddGUINews(News news, int row, int column)
        {
            // TODO: make it so that the circle goes in a different place for the second piece of news etc and so that real news is displayed differently
            
            Ellipse newsCircle = new Ellipse();
            newsCircle.Width = 5 * news.viewers.Count;
            newsCircle.Height = 5 * news.viewers.Count;
            if (news.isTrue)
            {
                newsCircle.Fill = new SolidColorBrush(Colors.LawnGreen);
            }
            else
            {
                newsCircle.Fill = new SolidColorBrush(Colors.DarkRed);
            }
            newsCircle.SetValue(Grid.RowProperty, row);
            newsCircle.SetValue(Grid.ColumnProperty, column);
            OSNGrid.Children.Add(newsCircle);
            
        }

        public void DisplayOSN(OSN osn)
        {
            this.OSNGrid.Children.Clear();// will remove all child controls nested in the grid. 
            this.OSNGrid.RowDefinitions.Clear();// will remove all row definitions.
            this.OSNGrid.ColumnDefinitions.Clear();
            this.PrepareOSNGrid(osn);
            foreach(Account account in osn.accountList)
            {
                this.AddGUIAccount(account);
            }
            // go back through and add connections between accounts
        }

        private Brush PickRandomBrush(Random rnd)
        {
            Brush result = Brushes.Transparent;
            Type brushesType = typeof(Brushes);
            System.Reflection.PropertyInfo[] properties = brushesType.GetProperties();
            int random = rnd.Next(properties.Length);
            result = (Brush)properties[random].GetValue(null, null);
            return result;
        }


        // Click event methods

        private void CreateFakeNewsButton_Click(object sender, RoutedEventArgs e)
        {
            twitter.CreateNews("FakeNews", false, twitter.accountList[random.Next(twitter.IDCount)],simulation.time,1,1);
        }
        private void CreateTrueNewsButton_Click(object sender, RoutedEventArgs e)
        {
            twitter.CreateNews("TrueNews", true, twitter.accountList[random.Next(twitter.IDCount)], simulation.time,1,1);
        }
        private void OutputButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Time: " + DateTime.Now.ToString() + ", N shared fake news: " + twitter.nSharedFakeNews+" out of "+ this.twitter.accountList.Count+" users");
            this.outputLabel.Content += this.twitter.nSharedFakeNews + ", ";
            string outputString = "output=[";
            foreach (int value in this.twitter.nSharedFakeNewsList)
            {
                outputString += value.ToString() + ", ";
            }
            outputString += "]";
            Console.WriteLine(outputString);
        }

        private void CreateFollowsClicked(object sender, EventArgs e)
        {
            this.twitter.CreateRandomFollows();
        }

        private void PopulateClicked(object sender, EventArgs e)
        {
            this.twitter.PopulateFromGraph(100);
           // this.DisplayOSN(twitter);
        }

        private void MainWPFWindow_Closed(object sender, EventArgs e)
        {

            File.WriteAllLines(@"C:\Users\Anni\Documents\Uni\Computer Science\Proj\CSVs and text files\nSharedFakeNews.csv", this.twitter.nSharedFakeNewsList.Select(x => string.Join(",", x)));

            foreach(News news in twitter.newsList)
            {
                File.WriteAllLines(@"C:\Users\Anni\Documents\Uni\Computer Science\Proj\CSVs and text files\nShared"+news.ID+".csv", news.nSharedList.Select(x => string.Join(",", x)));
            }
            
            File.WriteAllLines(@"C:\Users\Anni\Documents\Uni\Computer Science\Proj\CSVs and text files\timeStamps.csv", this.simulation.timeStamps.Select(x => string.Join(",", x)));

            string outputString = "output=[";
            foreach (int value in this.twitter.nSharedFakeNewsList)
            {
                outputString += value.ToString() + ", ";
            }
            outputString += "]";
            Console.WriteLine(outputString);


        }
    }
}
