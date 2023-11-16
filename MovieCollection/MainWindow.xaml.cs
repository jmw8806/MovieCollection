using DataAccessFakes;
using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Drawing;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Resources;
using static DataObjects.DisplayHelpers;
using System.Reflection;
using System.Threading;
using System.Drawing;
using System.IO;

namespace MovieCollection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UserManager _userManager = null;
        MovieManager _movieManager = null;
        int _random_id = 0;
        UserVM _loggedInUser = null;
        MovieVM _homeMovie = null;
        List<Movie> _movies = null;
        List<MovieVM> _movieVMs = null;
        MovieVM _selectedMovie = null;
        ResourceManager _rm = new ResourceManager("MovieCollection.Resource", Assembly.GetExecutingAssembly());

        public MainWindow()
        {
            InitializeComponent();
        }


        private void mnuTest_Click(object sender, RoutedEventArgs e)
        {
            string image = "";
            try
            {
                image = _movieManager.GetImageNameByMovieID(_homeMovie.titleID);
                MessageBox.Show(image);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error \n\n" + ex);
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _userManager = new UserManager();
            _movieManager = new MovieManager();
            _random_id = generate_random_id(_movieManager.count_all_titles());
            _movies = new List<Movie>();
            _movieVMs = new List<MovieVM>();
            try
            {
                _homeMovie = _movieManager.GetMovieByTitleID(_random_id);
                display_home_movie(_homeMovie);
                _movies = _movieManager.GetAllMovies();
                _movieVMs = _movieManager.GetAllMovieVMs();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error \n" + _random_id.ToString() + "\n" + ex);
            }

            txtEmail.Focus();
            btnLoginSubmit.IsDefault = true;
            if (_loggedInUser == null)
            {
                hideTabs();
            }


        }

        private void btnLoginSubmit_Click(object sender, RoutedEventArgs e)
        {

            string email = txtEmail.Text;
            string password = pwdPassword.Password;
            if (!email.IsValidEmail())
            {
                MessageBox.Show("Invalid Email", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                txtEmail.SelectAll();
                txtEmail.Focus();
                return;
            }
            if (!password.IsValidPassword())
            {
                MessageBox.Show("Invalid Password", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                pwdPassword.Clear();
                pwdPassword.Focus();
            }

            try
            {
                _loggedInUser = _userManager.LoginUser(email, password);

                updateUIForLogin();
                showTabs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Login Failed ", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void updateUIForLogin()
        {
            if (_loggedInUser.roles.ToString() == "User")
            {
                lblGreeting.Content = "Hello, " + _loggedInUser.fName.ToString() + ".";
            }
            else if (_loggedInUser.roles.ToString() == "Administrator")
            {
                lblGreeting.Content = "Welcome back boss!";
            };
            btnLoginCancel.Content = "Log Out";
            btnLoginSubmit.Visibility = Visibility.Hidden;
            txtEmail.Clear();
            pwdPassword.Clear();
            lblEmail.Visibility = Visibility.Hidden;
            lblPassword.Visibility = Visibility.Hidden;
            txtEmail.Visibility = Visibility.Hidden;
            pwdPassword.Visibility = Visibility.Hidden;
            btnLoginSubmit.IsDefault = false;
        }

        private void btnLoginCancel_Click(object sender, RoutedEventArgs e)
        {
            if (btnLoginCancel.Content.ToString() == "Cancel")
            {
                txtEmail.Clear();
                txtEmail.Focus();
                pwdPassword.Clear();
            }
            if (btnLoginCancel.Content.ToString() == "Log Out")
            {
                _loggedInUser = null;
                lblGreeting.Content = "Please Log In to Continue to the Movie Collection.";
                txtEmail.Focus();
                txtEmail.Visibility = Visibility.Visible;
                pwdPassword.Visibility = Visibility.Visible;
                btnLoginSubmit.Visibility = Visibility.Visible;
                btnLoginSubmit.IsDefault = true;
                btnLoginCancel.Content = "Cancel";
                lblEmail.Visibility = Visibility.Visible;
                lblPassword.Visibility = Visibility.Visible;
                hideTabs();
            }
        }

        //hides all tabs except home
        private void hideTabs()
        {
            tabsetMain.SelectedIndex = 0;

            foreach (var tab in tabsetMain.Items)
            {
                if (tab != tabHome)
                {

                    ((TabItem)tab).Visibility = Visibility.Collapsed;
                }
            }
        }

        private void showTabs()
        {
            if (_loggedInUser != null || _loggedInUser.roles != "Guest")
            {
                string role = _loggedInUser.roles;
                foreach (var tab in tabsetMain.Items)
                {
                    if (role != "Administrator")
                    {
                        if (tab != tabAdministration)
                        {
                            ((TabItem)tab).Visibility = Visibility.Visible;
                        }
                    }
                    if (role == "Administrator")
                    {
                        ((TabItem)tab).Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private int generate_random_id(int count)
        {
            int num = 0;
            var random = new Random();
            num = random.Next(0, count) + 10000;
            return num;
        }

        private void display_home_movie(MovieVM movie)
        {
            lblHomeTitle.Content = movie.title.ToString();
            txtHomeRuntime.Text = movie.runtime.ToString();
            txtHomeRating.Text = movie.rating.ToString();
            txtHomeLanguage.Text = displayList(movie.languages);
            txtHomeGenres.Text = displayList(movie.genres);
            txtHomeFormat.Text = displayList(movie.formats);
            
            
            imgHome.Source = getImage(movie.imgName);
            
        }


      
        private void tabAll_GotFocus(object sender, RoutedEventArgs e)
        {
            lblAllMoviesTitle.Content = "Title";
            lblAllMoviesYear.Content = "Year";
            lblAllMoviesRating.Content = "Rating";
            lblAllMoviesRuntime.Content = "Runtime";
            lblAllMoviesCriterion.Content = "Criterion";
            imgAllMoviesImage.Source = getImage("blank.png");
            lblAllMoviesFormats.Content = "Formats";
            lblAllMoviesGenres.Content = "Genres";
            lblAllMoviesLanguages.Content = "Languages";
            
            foreach(var movie in _movieVMs)
            {
                lstAllMovies.Items.Add(movie.title);
            }
            
        }

        private void lstAllMovies_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
            MovieVM movie = null;
            
            foreach(MovieVM movieVM in _movieVMs)
            {
                if(lstAllMovies.SelectedValue.ToString() == movieVM.title)
                {
                    movie = movieVM;
                }
            }
            lstAllMovies.Items.Clear();
            lblAllMoviesTitle.Content = movie.title;
            lblAllMoviesYear.Content = movie.year;
            lblAllMoviesRating.Content = movie.rating;
            lblAllMoviesRuntime.Content = movie.runtime.ToString() + " mins";
            lblAllMoviesCriterion.Content = criterionOutputConverter(movie.isCriterion);
            imgAllMoviesImage.Source = getImage(movie.imgName);
            lblAllMoviesFormats.Content = displayList(movie.formats);
            lblAllMoviesGenres.Content = displayList(movie.genres);
            lblAllMoviesLanguages.Content = displayList(movie.languages);
        }

        private BitmapSource getImage(string name)
        {
            return LoadImage("C:\\Users\\jmw66\\source\\repos\\MovieCollection\\MovieCollection\\Resources\\" + name);
        }
        private static BitmapSource LoadImage(string path)
        {
            var bitmap = new BitmapImage();

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
            }

            return bitmap;
        }

       
    }
}
