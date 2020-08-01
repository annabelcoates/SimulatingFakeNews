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
        OSN facebook;
        private string wheelGraphPath = "C:/Users/Anni/Documents/Uni/Computer Science/Proj/wheel_graph.csv";
        private string smallWorldPath = "C:/Users/Anni/Documents/Uni/Computer Science/Proj/small_world_graph.csv";
        private string realFBEdges = @"C:\Users\Anni\Documents\Uni\Computer Science\Proj\facebook_combined.txt\facebook_combined.csv";

        private int timeSlot = 1;

        public MainWindow()
        {
            this.UKDistributionSimulation(1000);

            Clock.Interval = TimeSpan.FromMilliseconds(150f / simulation.runSpeed);
            Clock.Tick += StandardFBUpdate;
            MinClock.Interval = TimeSpan.FromMilliseconds(10f / simulation.runSpeed);
            Clock.Tick += UpdateSimulationTime;


            InitializeComponent();
            Clock.Start();
            MinClock.Start();
        }
        private void StandardFacebook(int n)
        {
            this.simulation = new Simulation("SimpleVersion", 10);
            this.simulation.RandomPopulate(n);

            //SciChartSurface.SetRuntimeLicenseKey("9022a0a8 - 41e1 - 43e1 - a680 - 5afb55c964a6");

            
            this.facebook = new OSN(this);

            // Create a population of people and have them all be following n/10 other people
            this.facebook.PopulateFromPeople(n, n/10, simulation.humanPopulation);
            this.facebook.CreateMutualFollowsFromGraph(wheelGraphPath);

            // Now give people more connections based on their personality traits
            foreach(Account account in facebook.accountList)
            {

            }

            // Create some news to be shared
            for (int i = 0; i < 20; i++)
            {
                facebook.CreateNews("FakeNews", false, facebook.accountList[random.Next(facebook.IDCount)], simulation.time, 1, 0.1);
            }

            for (int i = 0; i < 20; i++)
            {
                facebook.CreateNews("TrueNews", true, facebook.accountList[random.Next(facebook.IDCount)], simulation.time, 0.5, 1);
            }

        }

        private void StandardFBUpdate(object sender, EventArgs e)
        {
            simulation.time = 15; // A time slot represents a 15 minute period
            if (this.createFollowsCheckbox.IsChecked==true)
            {
                this.facebook.CreateRandomFollow();
            }
            this.facebook.TimeSlotPasses(simulation.time);
            this.simulation.timeStamps.Add(timeSlot);
            this.facebook.accountList[0].OutputPage(this);
            //this.DisplayOSN(twitter);
            timeSlot++;
        }


        private void HighEFacebookSimulation(int n)
        {
            this.simulation = new Simulation("High extroversion facebook", 10);
            this.simulation.RandomPopulate(n);
            this.facebook = new OSN(this);

            // Give twitter a small initial population
            this.facebook.PopulateFromPeople(n, n/5, simulation.humanPopulation);
            this.facebook.CreateMutualFollowsFromGraph(wheelGraphPath);
            // DisplayOSN(twitter);

            // Create some news to be shared
            for (int i = 0; i < 20; i++)
            {
                facebook.CreateNews("FakeNews", false, facebook.accountList[random.Next(facebook.IDCount)], simulation.time, 1, 0.1);
            }

            for (int i = 0; i < 20; i++)
            {
                facebook.CreateNews("TrueNews", true, facebook.accountList[random.Next(facebook.IDCount)], simulation.time, 0.5, 1);
            }
        }
        private void UKDistributionSimulation(int n)
        {
            this.simulation = new Simulation("UK Distribution facebook", 10);
            this.simulation.DistributionPopulate(n);
            this.facebook = new OSN(this);

            // Give twitter a small initial population
            int k = 10; // make k twice as small for a mutual follow system
            this.facebook.PopulateFromPeople(n,k, simulation.humanPopulation);
            this.facebook.CreateMutualFollowsFromGraph(smallWorldPath);
            this.facebook.CreateFollowsBasedOnPersonality(n / 2);
            // DisplayOSN(twitter);

            // Create some news to be shared
            for (int i = 0; i < 20; i++)
            {
                facebook.CreateNews("FakeNews", false, facebook.accountList[random.Next(facebook.IDCount)], simulation.time, 1, 0.1);
            }

            for (int i = 0; i < 20; i++)
            {
                facebook.CreateNews("TrueNews", true, facebook.accountList[random.Next(facebook.IDCount)], simulation.time, 0.5, 1);
            }

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
            facebook.CreateNews("FakeNews", false, facebook.accountList[random.Next(facebook.IDCount)],simulation.time,1,1);
        }
        private void CreateTrueNewsButton_Click(object sender, RoutedEventArgs e)
        {
            facebook.CreateNews("TrueNews", true, facebook.accountList[random.Next(facebook.IDCount)], simulation.time,1,1);
        }
        private void OutputButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Time: " + DateTime.Now.ToString() + ", N shared fake news: " + facebook.nSharedFakeNews+" out of "+ this.facebook.accountList.Count+" users");
            this.outputLabel.Content += this.facebook.nSharedFakeNews + ", ";
            string outputString = "output=[";
            foreach (int value in this.facebook.nSharedFakeNewsList)
            {
                outputString += value.ToString() + ", ";
            }
            outputString += "]";
            Console.WriteLine(outputString);
        }



        private void PopulateClicked(object sender, EventArgs e)
        {
            //this.facebook.PopulateFromGraph(100,10);
           // this.DisplayOSN(twitter);
        }

        private void MainWPFWindow_Closed(object sender, EventArgs eA)
        {

            File.WriteAllLines(@"C:\Users\Anni\Documents\Uni\Computer Science\Proj\CSVs and text files\nSharedFakeNews.csv", this.facebook.nSharedFakeNewsList.Select(x => string.Join(",", x)));

            var csv = new StringBuilder();
            List<double> populationAverages = simulation.CalculateAverages();
            var firstLine = string.Format("{0},{1},{2},{3},{4},{5},{6}", populationAverages[0], populationAverages[1], populationAverages[2], populationAverages[3], populationAverages[4], populationAverages[5], populationAverages[6]);
            csv.AppendLine(firstLine);
            foreach (News news in facebook.newsList)
            {
                File.WriteAllLines(@"C:\Users\Anni\Documents\Uni\Computer Science\Proj\CSVs and text files\nShared"+news.ID+".csv", news.nSharedList.Select(x => string.Join(",", x)));


                List<double> personalityAverages = news.CalculateSharerAverages();

                var newLine = string.Format("{0},{1},{2},{3},{4},{5},{6}", personalityAverages[0], personalityAverages[1], personalityAverages[2], personalityAverages[3], personalityAverages[4], personalityAverages[5], personalityAverages[6]);
                csv.AppendLine(newLine);

            }
            File.WriteAllText(@"C:\Users\Anni\Documents\Uni\Computer Science\Proj\CSVs and text files\personalityAverages.csv", csv.ToString());




            File.WriteAllLines(@"C:\Users\Anni\Documents\Uni\Computer Science\Proj\CSVs and text files\timeStamps.csv", this.simulation.timeStamps.Select(x => string.Join(",", x)));

            string outputString = "output=[";
            foreach (int value in this.facebook.nSharedFakeNewsList)
            {
                outputString += value.ToString() + ", ";
            }
            outputString += "]";
            Console.WriteLine(outputString);

        }
    }
}
