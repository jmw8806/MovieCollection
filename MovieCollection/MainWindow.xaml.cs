using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using static DataObjects.DisplayHelpers;
using static DataObjects.MovieSearchHelpers;
using static DataObjects.UtilityHelpers;


namespace MovieCollection
{
   
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
        List<MovieVM> _searchResults = null;
        int _selectedID = 0;
        
        public MainWindow()
        {
            
            InitializeComponent();
        }


        private void mnuTest_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                int currentYear = DateTime.Now.Year;
                MessageBox.Show(currentYear.ToString());
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
                hideMenuItems();
            }
            refreshMovieList();
            resetForms();
            cboAddGenre.ItemsSource = _genres;
            cboAddLanguage.ItemsSource = _languages;
            cboAddRating.ItemsSource = _ratings;
            cboAddFormat.ItemsSource = _formats;
            cboSearchGenre.ItemsSource = _genres;
            cboSearchLanguage.ItemsSource = _languages;
            cboSearchYear.ItemsSource = getYears(1888);
            cboAddYear.ItemsSource = getYears(1888);
            foreach(var movie in _movieVMs)
            {
                lstAdmin.Items.Add(movie.title);
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
                imgProfilePic.Visibility = Visibility.Hidden;
                hideMenuItems();
                hideTabs();
            }
        }





        private void tabAll_GotFocus(object sender, RoutedEventArgs e)
        {
            lblAllMoviesTitle.Text = "Title";
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
                    _selectedID = movie.titleID;
                }
            }

            lblAllMoviesTitle.Text = movie.title;
            lblAllMoviesYear.Content = movie.year;
            lblAllMoviesRating.Content = movie.rating;
            lblAllMoviesRuntime.Content = movie.runtime.ToString() + " mins";
            lblAllMoviesCriterion.Content = "Criterion: " + criterionOutputConverter(movie.isCriterion);
            imgAllMoviesImage.Source = displayImageFromURL(movie.imgName);
            lblAllMoviesFormats.Content = displayList(movie.formats);
            lblAllMoviesGenres.Content = displayList(movie.genres);
            lblAllMoviesLanguages.Content = displayList(movie.languages);
        }

        
        private void btnAddMovie_Click(object sender, RoutedEventArgs e)
        {

            string addTitle = txtAddName.Text;
            


            int addRuntime = 0;
            if(int.TryParse(txtAddRuntime.Text, out int _))
            {
                addRuntime = int.Parse(txtAddRuntime.Text);
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
            if (string.IsNullOrEmpty(addTitle) && string.IsNullOrEmpty(txtAddRuntime.Text)
                && string.IsNullOrEmpty(addLanguage) && string.IsNullOrEmpty(addGenre) && string.IsNullOrEmpty(addFormat) &&
                string.IsNullOrEmpty(addRating))
            {
                MessageBox.Show("Please complete all required fields");
            }
            else
            {
                try
                {
                    int addYear = int.Parse(cboAddYear.SelectedValue.ToString());
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

        private void btnAddCancel_Click(object sender, RoutedEventArgs e)
        {
            resetForms();
        }


        private void btnAddMovieImage_Click(object sender, RoutedEventArgs e)
        {


            imgAddImage.Source = displayImageFromURL(txtAddURL.Text);
        }

        private void btnSearchSubmit_Click(object sender, RoutedEventArgs e)
        {
            List<MovieVM> searchResults = _movieManager.GetAllMovieVMs();
            string searchTitle = txtSearchTitle.Text;
            try
            {

                if (chkSearchTitle.IsChecked == true && searchTitle != null)
                {
                    searchResults = searchByTitle(searchResults, searchTitle);
                }
                if (chkSearchYear.IsChecked == true && cboSearchYear.SelectedValue != null)
                {
                    int searchYear = Convert.ToInt32(cboSearchYear.SelectedValue.ToString());
                    searchResults = searchByYear(searchResults, searchYear);
                }
                if(chkSearchGenre.IsChecked == true && cboSearchGenre.SelectedValue != null) 
                { 
                    searchResults = searchByGenre(searchResults, cboSearchGenre.SelectedValue.ToString());
                }
                if(chkSearchLanguage.IsChecked == true && cboSearchLanguage.SelectedValue != null)
                {
                    searchResults = searchByLanguage(searchResults, cboSearchLanguage.SelectedValue.ToString());
                }
                if(chkSearchRuntime.IsChecked == true && txtSearchRuntime != null)
                {
                    int searchRuntime = Convert.ToInt32(txtSearchRuntime.Text);
                    if(radioSearchLessThan.IsChecked == true)
                    {
                        searchResults = searchByLessThanRuntime(searchResults, searchRuntime);
                    }
                    if(radioSearchGreaterThan.IsChecked == true)
                    {
                        searchResults = searchByGreaterThanRuntime(searchResults, searchRuntime);
                    }
                }
                if(chkSearchIncludeCriterion.IsChecked == true)
                {
                    searchResults = chkSearchCriterion.IsChecked == true ? searchByCriterion(searchResults, true) : searchByCriterion(searchResults, false);
                 
                }

                if(searchResults.Count == 0)
                {
                    MessageBox.Show("No results found");
                }
                
            }
            catch
            {
                MessageBox.Show("Error with your query");
            }
            lstSearchResults.Items.Clear();
            foreach (var result in searchResults)
            {
                lstSearchResults.Items.Add(result.title);
            }
            _searchResults = searchResults;
        }

        private void lstSearchResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MovieVM movie = null;

            foreach (MovieVM movieVM in _searchResults)
            {
                if (lstSearchResults.SelectedValue.ToString() == movieVM.title)
                {
                    movie = movieVM;
                    _selectedID = movieVM.titleID;
                }
            }

            txtResultTitle.Text = movie.title;
            txtResultYear.Text = "Year: "+ movie.year.ToString();
            txtResultRating.Text = "Rating: " + movie.rating;
            txtResultFormats.Text = displayList(movie.formats);
            txtResultLanguages.Text = displayList(movie.languages);
            txtResultGenres.Text = displayList(movie.genres);
            txtResultRuntime.Text = movie.runtime.ToString() +"mins";
            txtResultCriterion.Text = "Criterion: " + criterionOutputConverter(movie.isCriterion);
            imgSearch.Source = displayImageFromURL(movie.imgName);
        }

        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
           this.Close();
        }

        private void mnuAddMovie_Click(object sender, RoutedEventArgs e)
        {
            if(tabAdd.Visibility == Visibility.Visible) 
            { 
                tabAdd.Focus();
            }           
        }


        private void mnuAddCollection_Click(object sender, RoutedEventArgs e)
        {
            tabCollections.Focus();
        }

        private void mnuAll_Click(object sender, RoutedEventArgs e)
        {
            tabAll.Focus();
        }

        private void mnuSearch_Click(object sender, RoutedEventArgs e)
        {
            tabSearch.Focus();
        }

        private void mnuCollections_Click(object sender, RoutedEventArgs e)
        {
            tabCollections.Focus();
        }

        private void mnuProfile_Click(object sender, RoutedEventArgs e)
        {
            tabProfile.Focus();
        }

        private void btnResultEdit_Click(object sender, RoutedEventArgs e)
        {
            
            if (_selectedID != 0)
            {
                var editMovie = new EditMovie(_selectedID);
                blurWindow(10);
                editMovie.ShowDialog();               
                if(editMovie.editResult)
                {
                    refreshMovieList();
                    resetForms();
                }
            }
            else
            {
                MessageBox.Show("Please select a title to edit");
            }
            blurWindow(0);
            
        }

        private void btnResultRemove_Click(object sender, RoutedEventArgs e)
        {
            bool isDeactivated = false;
            if(_selectedID != 0)
            {
                try
                {
                    
                    var warning = MessageBox.Show("Are you sure you want to remove this title?", "!!!", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (warning == MessageBoxResult.OK)
                    {
                        isDeactivated = _movieManager.UpdateMovieIsActive(_selectedID, false);
                        refreshMovieList();
                        resetForms();
                        MessageBox.Show("So long, farewell, auf Wiedersehen, goodbye! \n\n Movie has been removed.", "Success", MessageBoxButton.OK);
                    }
                    else
                    {
                        MessageBox.Show("Whew, that was a close call!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Movie removal failed. \n\n" + ex.InnerException.Message);
                }
            }
        }

        private void btnAllAdd_Click(object sender, RoutedEventArgs e)
        {
            tabAdd.Focus();
        }

        private void btnAllEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedID != 0)
            {
                blurWindow(10);
                var editMovie = new EditMovie(_selectedID);
                editMovie.ShowDialog();
                if (editMovie.editResult)
                {
                    refreshMovieList();
                    resetForms();
                }
            }
            else
            {
                MessageBox.Show("Please select a title to edit");
            }
            blurWindow(0);
        }

        private void btnAllRemove_Click(object sender, RoutedEventArgs e)
        {
            bool isDeactivated = false;
            if (_selectedID != 0)
            {
                try
                {
                    var warning = MessageBox.Show("Are you sure you want to remove this title?", "!!!", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (warning == MessageBoxResult.OK)
                    {
                        isDeactivated = _movieManager.UpdateMovieIsActive(_selectedID, false);
                        refreshMovieList();
                        resetForms();
                        MessageBox.Show("So long, farewell, auf Wiedersehen, goodbye! \n\n Movie has been removed.", "Success", MessageBoxButton.OK);
                    }
                    else
                    {
                        MessageBox.Show("Whew, that was a close call!");
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Movie removal failed. \n\n" + ex.InnerException.Message);
                }
            }
        }



        // utility methods start
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
            imgProfilePic.Visibility = Visibility.Visible;
            imgProfilePic.Source = displayImageFromURL(_loggedInUser.imgURL);
            imgProfileTab.Source = displayImageFromURL(_loggedInUser.imgURL);
            txtProfileFirstName.Text = _loggedInUser.fName;
            txtProfileLastName.Text = _loggedInUser.lName;
            txtProfileEmail.Text = _loggedInUser.email;
            showMenuItems();
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

        //Displays the movie that is shown on home tab
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

        // When called, clears all data from forms and labels.
        private void resetForms()
        {
            txtAddName.Clear();
            cboAddYear.SelectedIndex = 112;
            txtAddRuntime.Clear();
            cboAddLanguage.SelectedIndex = 0;
            cboAddGenre.SelectedIndex = 0;
            cboAddFormat.SelectedIndex = 0;
            cboAddRating.SelectedIndex = 0;
            txtAddNotes.Clear();
            chkAddCriterion.IsChecked = false;
            txtAddURL.Clear();
            imgAddImage.Source = null;
            txtResultTitle.Text = string.Empty;
            txtResultYear.Text = string.Empty;
            txtResultRating.Text = string.Empty;
            txtResultFormats.Text = string.Empty;
            txtResultLanguages.Text = string.Empty;
            txtResultGenres.Text = string.Empty;
            txtResultRuntime.Text = string.Empty;
            txtResultCriterion.Text = string.Empty;
            imgSearch.Source = null;
            lblAllMoviesTitle.Text = string.Empty;
            lblAllMoviesYear.Content = string.Empty;
            lblAllMoviesRating.Content = string.Empty;
            lblAllMoviesFormats.Content = string.Empty;
            lblAllMoviesGenres.Content = string.Empty;
            lblAllMoviesLanguages.Content = string.Empty;
            lblAllMoviesRuntime.Content = string.Empty;
            lblAllMoviesCriterion.Content = string.Empty;
        }

        // clears lists that display all movies, gets all movies from database and adds them to _movieVMs list.
        public void refreshMovieList()
        {
            lstAllMovies.Items.Clear();
            lstSearchResults.Items.Clear();
            _movieVMs = _movieManager.GetAllMovieVMs();
            foreach (var movie in _movieVMs)
            {
                lstAllMovies.Items.Add(movie.title);
            }
        }

        //Takes in a url string, returns the link as a BitmapImage.
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

        private void hideMenuItems()
        {
            mnuAddCollection.Visibility = Visibility.Hidden;
            mnuAddMovie.Visibility = Visibility.Hidden;
            mnuChangePassword.Visibility = Visibility.Hidden;
            mnuAll.Visibility = Visibility.Hidden;
            mnuSearch.Visibility = Visibility.Hidden;
            mnuCollections.Visibility = Visibility.Hidden;
            mnuProfile.Visibility = Visibility.Hidden;
        }

        private void showMenuItems()
        {
            mnuAddCollection.Visibility = Visibility.Visible;
            mnuAddMovie.Visibility = Visibility.Visible;
            mnuChangePassword.Visibility = Visibility.Visible;
            mnuAll.Visibility = Visibility.Visible;
            mnuSearch.Visibility = Visibility.Visible;
            mnuCollections.Visibility = Visibility.Visible;
            mnuProfile.Visibility = Visibility.Visible;
        }

        private void blurWindow(int blurAmnt)
        {         
            BlurEffect blur = new BlurEffect();         
            blur.Radius = blurAmnt;          
            this.Effect = blur;
        }

        private void btnProfileUpdate_Click(object sender, RoutedEventArgs e)
        {
            lblProfileImgURL.Visibility = Visibility.Visible;
            txtProfileImgURL.Visibility = Visibility.Visible;
            btnProfileImgPreview.Visibility = Visibility.Visible;
            txtProfileImgURL.Text = _loggedInUser.imgURL;
            txtProfileImgURL.IsReadOnly = false;

            txtProfileFirstName.ClearValue(Border.BorderBrushProperty);
            txtProfileFirstName.ClearValue(BackgroundProperty);
            txtProfileFirstName.IsReadOnly = false;
    
            txtProfileLastName.ClearValue(Border.BorderBrushProperty);
            txtProfileLastName.ClearValue(BackgroundProperty);
            txtProfileLastName.IsReadOnly = false;

            txtProfileEmail.ClearValue(Border.BorderBrushProperty);
            txtProfileEmail.ClearValue(BackgroundProperty);
            txtProfileEmail.IsReadOnly = false;

            btnProfileUpdate.Visibility = Visibility.Hidden;
            btnProfileDeactivate.Visibility = Visibility.Hidden;

            btnProfileSubmit.Visibility = Visibility.Visible;
            btnProfileCancel.Visibility = Visibility.Visible;
        }

        private void btnProfileCancel_Click(object sender, RoutedEventArgs e)
        {
            txtProfileFirstName.Text = _loggedInUser.fName;
            txtProfileLastName.Text = _loggedInUser.lName;
            txtProfileEmail.Text = _loggedInUser.email;
            txtProfileLastName.Text = _loggedInUser.imgURL;

            txtProfileFirstName.Background = null;
            txtProfileLastName.Background = null;
            txtProfileEmail.Background = null;

            txtProfileFirstName.IsReadOnly = true;
            txtProfileLastName.IsReadOnly = true;
            txtProfileEmail.IsReadOnly = true;
            txtProfileImgURL.IsReadOnly = true;

            lblProfileImgURL.Visibility = Visibility.Hidden;
            txtProfileImgURL.Visibility = Visibility.Hidden;
            btnProfileSubmit.Visibility = Visibility.Hidden;
            btnProfileCancel.Visibility = Visibility.Hidden;
            btnProfileImgPreview.Visibility = Visibility.Hidden;
            btnProfileDeactivate.Visibility = Visibility.Visible;
            btnProfileUpdate.Visibility = Visibility.Visible;
        }

        private void btnProfileSubmit_Click(object sender, RoutedEventArgs e)
        {
            string newFName = txtProfileFirstName.Text;
            string newLName = txtProfileLastName.Text;
            string newEmail = txtProfileEmail.Text;
            string newImgURL = txtProfileImgURL.Text;

            try
            {
                bool result = false;

                result = _userManager.UpdateUser(_loggedInUser, newFName, newLName, newEmail, newImgURL);
                if(result == false)
                {
                    MessageBox.Show("User information not updated");
                }
                else
                {
                    MessageBox.Show("Success! User information is updated");
                    _loggedInUser = _userManager.SelectUserByEmail(newEmail);
                    lblGreeting.Content = "Hello, " + _loggedInUser.fName + ".";
                    txtProfileFirstName.Text = _loggedInUser.fName;
                    txtProfileLastName.Text = _loggedInUser.lName;
                    txtProfileEmail.Text = _loggedInUser.email;
                    txtProfileImgURL.Text = _loggedInUser.imgURL;
                    imgProfileTab.Source = displayImageFromURL(_loggedInUser.imgURL);
                    imgProfilePic.Source = displayImageFromURL(_loggedInUser.imgURL);
                    txtProfileFirstName.Background = null;
                    txtProfileLastName.Background = null;
                    txtProfileEmail.Background = null;

                    txtProfileFirstName.IsReadOnly = true;
                    txtProfileLastName.IsReadOnly = true;
                    txtProfileEmail.IsReadOnly = true;
                    txtProfileImgURL.IsReadOnly = true;

                    lblProfileImgURL.Visibility = Visibility.Hidden;
                    txtProfileImgURL.Visibility = Visibility.Hidden;
                    btnProfileSubmit.Visibility = Visibility.Hidden;
                    btnProfileCancel.Visibility = Visibility.Hidden;
                    btnProfileImgPreview.Visibility = Visibility.Hidden;
                    btnProfileDeactivate.Visibility = Visibility.Visible;
                    btnProfileUpdate.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong \n\n" + ex.InnerException.Message);
            }

        }

        private void btnProfileImgPreview_Click(object sender, RoutedEventArgs e)
        {
            imgProfileTab.Source = displayImageFromURL(txtProfileImgURL.Text);
        }

        private void lstAdmin_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(cboAdminType.SelectedValue == cboAdminTypeMovie)
            {
                MovieVM movie = null;
                foreach (MovieVM movieVM in _movieVMs)
                {
                    if (lstAdmin.SelectedValue.ToString() == movieVM.title)
                    {
                        movie = movieVM;
                        _selectedID = movie.titleID;
                    }
                }
                txtAdminID.Text = movie.titleID.ToString();
                txtAdminOther.Text = movie.title;
                imgAdminMovie.Source = displayImageFromURL(movie.imgName);

            }
        }
    }
}
