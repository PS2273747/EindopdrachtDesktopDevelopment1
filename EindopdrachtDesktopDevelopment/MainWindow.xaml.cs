using EindopdrachtDesktopDevelopment.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace EindopdrachtDesktopDevelopment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged

    {

       
        #region        INotifyPropertyChanged 
            public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        
        #endregion
        //connection to database class
        DbConnection db = new DbConnection();

        // an ObservableCollection list with all gamers and games trough the classes
        private ObservableCollection<Gamer> gamers = new ObservableCollection<Gamer>() ;
        private ObservableCollection<Game> games = new ObservableCollection<Game>();

        public ObservableCollection<Gamer> Gamers
        {
            get { return gamers; }
            set { gamers = value; }
        }
        private Gamer selectedGamer;


        public Gamer SelectedGamer
        {
            get { return selectedGamer; }
            set { selectedGamer = value;OnPropertyChanged(); }
        }


    public  ObservableCollection<Game> Games
        {
            get { return games; }
            set { games = value; }
        }
        private Game selectedGame;
        public Game SelectedGame
        {

            get { return selectedGame; }  
            set { selectedGame = value;OnPropertyChanged(); }
        }
    


        // lists with Gamers, their wishlisted games and all games trough the classes
        public List<WishlistedGames> wishlistedGames = new List<WishlistedGames>();
        public List<Game> lstGame = new List<Game>(); 
        public List<Gamer> lstGamer = new List<Gamer>(); 
 

        public MainWindow()
        {
            InitializeComponent();
            PopulateGamers();
            PopulateGames();
            DataContext = this;
        }

        private void PopulateGamers()
            //fills listview with Gamers
        {
           string resultaat = db.GetGamers(gamers);
            if (resultaat != "ok")
            {
                MessageBox.Show(resultaat);
                return;
            }
        }

        private void PopulateGames()
        //fills listview with Games
        {
            string resultaat = db.GetGames(games);
            if (resultaat != "ok")
            {
                MessageBox.Show(resultaat);
                return;
            }
        }
        private void BtnGamerAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnGamerDel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FavDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FavGame_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
