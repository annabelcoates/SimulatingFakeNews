using System;
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

namespace ModelAttemptWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random random = new Random();
        List<string> nameList = new List<string>() { "David","Patrick", "Alastair", "Bryan", "Marc", "Jamie", "Hugh", "Euan", "Gerard", "Sean", "Wayne", "Adam", "Calum", "Alasdair", "Robin", "Greig", "Angus", "Russell", "Cameron", "Roderick", "Norman", "Murray", "Gareth" };
        public DispatcherTimer Clock { get; set; } = new DispatcherTimer();
        public Account myAccount;
        public MainWindow()
        {
            Clock.Interval = TimeSpan.FromMilliseconds(500f);
            Clock.Tick += Update;
            InitializeComponent();
            Clock.Start();

            OSN facebook = new OSN(this);
            Account tedsAccount = facebook.NewAccount("Ted Cruz", 100);// change to decimal;
            List<Account> followingList = new List<Account>() { tedsAccount };
            foreach(string name in nameList)
            {
                Account tempAccount = facebook.NewAccount(name, 100);
                tempAccount.Follow(tedsAccount);
            }
            Account myAccount = facebook.NewAccount("Annabel Coates", 100);
            myAccount.Follow(tedsAccount);
            News tedsFakeNews = new News("FakeNews1", false);
            News tedsRealNews = new News("RealNews1", true);
            tedsAccount.CreateFakeNews("FakeNews1",0);
            tedsAccount.CreateTrueNews("RealNews1", 0);
            tedsAccount.ViewFeed();
            myAccount.ViewFeed();
            myAccount.OutputPage();
            DisplayOSN(facebook);
            }
        private void Update(object sender, EventArgs e)
        {
            this.date.Content = DateTime.Now;
        }
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

            Label nameLabel = new Label();
            nameLabel.Content = account.name;
            nameLabel.SetValue(Grid.RowProperty, row);
            nameLabel.SetValue(Grid.ColumnProperty, column);
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
            this.OSNGrid.Children.Add(nameLabel);

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
        }

        public void AddConnection(int row1, int col1, int row2, int col2, double squareWidth)
        {
            Line connector = new Line();
            connector.Stroke = PickRandomBrush(random);
            connector.X1 = (col1 + 0.5) * squareWidth;
            connector.Y1 = (row1 + 0.5) * squareWidth;
            connector.X2 = (col2 + 0.5) * squareWidth;
            connector.Y2 = (row2 + 0.5) * squareWidth;
            Grid.SetRowSpan(connector, OSNGrid.RowDefinitions.Count);
            Grid.SetColumnSpan(connector, OSNGrid.ColumnDefinitions.Count);
            OSNGrid.Children.Add(connector);
        }
        
        public void AddGUINews(News news, int row, int column)
        {
            // TODO: make it so that the circle goes in a different place for the second piece of news etc and so that real news is displayed differently
            
            Ellipse newsCircle = new Ellipse();
            newsCircle.Width = 25;
            newsCircle.Height = 25;
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

    }
}
