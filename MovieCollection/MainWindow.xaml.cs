using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static DataObjects.DisplayHelpers;


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
        List<string> _genres = null;
        List<string> _languages = null;
        List<string> _ratings = null;
        List<string> _formats = null;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void mnuTest_Click(object sender, RoutedEventArgs e)
        {

            try
            {
               
                
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
            _genres = new List<string>();
            _languages = new List<string>();
            _ratings = new List<string>();
            try
            {
                _homeMovie = _movieManager.GetMovieByTitleID(_random_id);
                display_home_movie(_homeMovie);
                _movies = _movieManager.GetAllMovies();
                _movieVMs = _movieManager.GetAllMovieVMs();
                _genres = _movieManager.GetAllGenres();
                _languages = _movieManager.GetAllLanguages();
                _ratings = _movieManager.GetAllRatings();
                _formats = _movieManager.GetAllFormats();

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
            refreshMovieList();
            resetForms();
            cboAddGenre.ItemsSource = _genres;
            cboAddLanguage.ItemsSource = _languages;
            cboAddRating.ItemsSource = _ratings;
            cboAddFormat.ItemsSource = _formats;


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
            BitmapImage bitmap = displayImageFromURL(movie.imgName);
            imgHome.Source = bitmap;
           

        }



        private void tabAll_GotFocus(object sender, RoutedEventArgs e)
        {
            lblAllMoviesTitle.Content = "Title";
            lblAllMoviesYear.Content = "Year";
            lblAllMoviesRating.Content = "Rating";
            lblAllMoviesRuntime.Content = "Runtime";
            lblAllMoviesCriterion.Content = "Criterion";
            lblAllMoviesFormats.Content = "Formats";
            lblAllMoviesGenres.Content = "Genres";
            lblAllMoviesLanguages.Content = "Languages";

        }

        private void lstAllMovies_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            MovieVM movie = null;

            foreach (MovieVM movieVM in _movieVMs)
            {
                if (lstAllMovies.SelectedValue.ToString() == movieVM.title)
                {
                    movie = movieVM;
                }
            }

            lblAllMoviesTitle.Content = movie.title;
            lblAllMoviesYear.Content = movie.year;
            lblAllMoviesRating.Content = movie.rating;
            lblAllMoviesRuntime.Content = movie.runtime.ToString() + " mins";
            lblAllMoviesCriterion.Content = criterionOutputConverter(movie.isCriterion);
            imgAllMoviesImage.Source = displayImageFromURL(movie.imgName);
            lblAllMoviesFormats.Content = displayList(movie.formats);
            lblAllMoviesGenres.Content = displayList(movie.genres);
            lblAllMoviesLanguages.Content = displayList(movie.languages);
        }

        private void tabAdd_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }

        private void btnAddMovie_Click(object sender, RoutedEventArgs e)
        {

            string addTitle = txtAddName.Text;
            int addYear = 0;
            if(int.TryParse(txtAddYear.Text, out int _))
            {
                addYear = int.Parse(txtAddYear.Text);
            }
            else
            {
                MessageBox.Show("Year is in incorrect format");
                txtAddYear.Clear();
            }


            int addRuntime = 0;
            if(int.TryParse(txtAddRuntime.Text, out int _))
            {
                addRuntime = int.Parse(txtAddName.Text);
            }
            else
            {
                MessageBox.Show("Runtime is in incorrect format");
                txtAddRuntime.Clear();
            }
            
            string addLanguage = cboAddLanguage.SelectedValue.ToString();
            string addGenre = cboAddGenre.SelectedValue.ToString();
            string addFormat = cboAddFormat.SelectedValue.ToString();
            string addRating = cboAddRating.SelectedValue.ToString();
            string addNotes = txtAddNotes.Text;
            if (string.IsNullOrEmpty(addNotes))
            {
                addNotes = "";
            }
            bool addIsCriterion = false;
            string addImage = txtAddURL.Text;
            if(string.IsNullOrEmpty(addImage))
            {
                addImage = "https://cdn4.iconfinder.com/data/icons/picture-sharing-sites/32/No_Image-1024.png";
            }
            if (chkAddCriterion.IsChecked == true)
            {
                addIsCriterion = true;
            }
            if (string.IsNullOrEmpty(addTitle) && string.IsNullOrEmpty(txtAddYear.Text) && string.IsNullOrEmpty(txtAddRuntime.Text)
                && string.IsNullOrEmpty(addLanguage) && string.IsNullOrEmpty(addGenre) && string.IsNullOrEmpty(addFormat) &&
                string.IsNullOrEmpty(addRating))
            {
                MessageBox.Show("Please complete all required fields");
            }
            else
            {
                try
                {
                    bool newMovie = _movieManager.AddMovie(addTitle, addYear, addRating, addRuntime, addIsCriterion, addNotes, addLanguage, addGenre, addImage, addFormat);
                    if (newMovie)
                    {
                        MessageBox.Show("Success! " + addTitle + " was added.");
                        refreshMovieList();
                        resetForms();
                    }
                    else
                    {
                        MessageBox.Show("Error. Your title was not added");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Application Error \n\n" + ex.Message);
                }
            }

        }

        private void resetForms()
        {

            txtAddName.Clear();
            txtAddYear.Clear();
            txtAddRuntime.Clear();
            cboAddLanguage.SelectedIndex = 0;
            cboAddGenre.SelectedIndex = 0;
            cboAddFormat.SelectedIndex = 0;
            cboAddRating.SelectedIndex = 0;
            txtAddNotes.Clear();
            chkAddCriterion.IsChecked = false;
            txtAddURL.Clear();
        }

        private void refreshMovieList()
        {
            lstAllMovies.Items.Clear();
            _movieVMs = _movieManager.GetAllMovieVMs();
            foreach (var movie in _movieVMs)
            {
                lstAllMovies.Items.Add(movie.title);
            }
        }

        private void btnAddCancel_Click(object sender, RoutedEventArgs e)
        {
            resetForms();
        }

        private BitmapImage displayImageFromURL(string url)
        {
            string imageUrl = url;
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            try
            {
                bitmap.UriSource = new Uri(imageUrl);
                bitmap.EndInit();
            }
            catch
            {
                bitmap = null;
            }
            return bitmap;
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

        private void btnAddMovieImage_Click(object sender, RoutedEventArgs e)
        {
            BitmapSource bitmap = displayImageFromURL(txtAddURL.Text);
            if(bitmap != null) 
            { 
                imgAddImage.Source = bitmap;
            }
        }
    }
}
