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
        CollectionManager _collectionManager = null;
        int _random_id = 0;
        UserVM _loggedInUser = null;
        MovieVM _homeMovie = null;
        List<Movie> _movies = null;
        List<MovieVM> _movieVMs = null;
        MovieVM _selectedMovie = null;
        List<Movie> _inactiveMovies = null;
        List<string> _genres = null;
        List<string> _languages = null;
        List<string> _ratings = null;
        List<string> _formats = null;
        List<MovieVM> _searchResults = null;
        int _selectedID = 0;
        List<int> _movieYears = null;
        List<string> _searchLanguages = null;
        List<string> _searchGenres = null;
        List<User> _activeUsers = null;
        List<User> _inactiveUsers = null;
        List<CollectionVM> _collectionVMs = null;
        int _selectedUserID = 0;

        public MainWindow()
        {

            InitializeComponent();
        }


        private void mnuTest_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (_loggedInUser != null)
                {
                    _collectionVMs = _collectionManager.GetCollectionsByUserID(_loggedInUser.userID);
                    string names = "";
                    foreach (CollectionVM collection in _collectionVMs)
                    {
                        names += collection.collectionName + " ";
                    }
                    MessageBox.Show(names);
                }
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
            _collectionManager = new CollectionManager();
            _movies = new List<Movie>();
            _movieVMs = new List<MovieVM>();
            _genres = new List<string>();
            _languages = new List<string>();
            _ratings = new List<string>();
            _movieYears = new List<int>();
            _searchLanguages = new List<string>();
            _searchGenres = new List<string>();

            try
            {

                _movies = _movieManager.GetAllMovies();
                _movieVMs = _movieManager.GetAllMovieVMs();
                _genres = _movieManager.GetAllGenres();
                _languages = _movieManager.GetAllLanguages();
                _ratings = _movieManager.GetAllRatings();
                _formats = _movieManager.GetAllFormats();
                _movieYears = getMovieYears(_movieVMs);
                _searchLanguages = getCurrentMovieLanguages(_movieVMs);
                _searchGenres = getCurrentMovieGenres(_movieVMs);
                _homeMovie = _movieManager.GetMovieByTitleID(_movieManager.randomMovieID(_movieVMs));
                _activeUsers = _userManager.GetActiveUsers();
                _inactiveUsers = _userManager.GetInactiveUsers();
                _inactiveMovies = _movieManager.GetAllInactiveMovies();
                display_home_movie(_homeMovie);
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
            _movieYears = getMovieYears(_movieVMs);
            cboAddGenre.ItemsSource = _genres;
            cboAddLanguage.ItemsSource = _languages;
            cboAddRating.ItemsSource = _ratings;
            cboAddFormat.ItemsSource = _formats;
            cboSearchGenre.ItemsSource = _searchGenres;
            cboSearchLanguage.ItemsSource = _searchLanguages;
            cboSearchYear.ItemsSource = _movieYears;
            cboAddYear.ItemsSource = getYears(1888);

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
                if (pwdPassword.Password.ToString() == "password")
                {

                    UpdatePassword passwordUpdate = new UpdatePassword(_loggedInUser.email);

                    var result = passwordUpdate.ShowDialog();
                    if (result == true)
                    {
                        MessageBox.Show("Password Updated.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    else
                    {
                        MessageBox.Show("Update Canceled. Logging Out", "Password Not Changed", MessageBoxButton.OK, MessageBoxImage.Hand);
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
                        return;
                    }
                }


                updateUIForLogin();
                showTabs();
                _collectionVMs = _collectionManager.GetCollectionsByUserID(_loggedInUser.userID);
                foreach (CollectionVM collection in _collectionVMs)
                {
                    collection.movieIDs = _collectionManager.GetMovieIDsByCollectionID(collection.collectionID);
                    cboCollectionsSelect.Items.Add(collection.collectionName);
                    cboResultAddToCollection.Items.Add(collection.collectionName);
                    cboCollectionsAddMoveCollection.Items.Add(collection.collectionName);
                }
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
                cboCollectionsSelect.Items.Clear();
                cboCollectionsAddMoveCollection.Items.Clear();
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
            try
            {

                foreach (MovieVM movieVM in _movieVMs)
                {
                    if (lstAllMovies.SelectedItem != null && lstAllMovies.SelectedValue.ToString() == movieVM.title)
                    {
                        movie = movieVM;
                        _selectedID = movie.titleID;
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
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("No Movie Selected");
            }

        }


        private void btnAddMovie_Click(object sender, RoutedEventArgs e)
        {

            string addTitle = txtAddName.Text;



            int addRuntime = 0;
            if (int.TryParse(txtAddRuntime.Text, out int _))
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
            if (string.IsNullOrEmpty(addImage))
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
                        refreshSearchCBOs();
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
                if (chkSearchGenre.IsChecked == true && cboSearchGenre.SelectedValue != null)
                {
                    searchResults = searchByGenre(searchResults, cboSearchGenre.SelectedValue.ToString());
                }
                if (chkSearchLanguage.IsChecked == true && cboSearchLanguage.SelectedValue != null)
                {
                    searchResults = searchByLanguage(searchResults, cboSearchLanguage.SelectedValue.ToString());
                }
                if (chkSearchRuntime.IsChecked == true && txtSearchRuntime != null)
                {
                    int searchRuntime = Convert.ToInt32(txtSearchRuntime.Text);
                    if (radioSearchLessThan.IsChecked == true)
                    {
                        searchResults = searchByLessThanRuntime(searchResults, searchRuntime);
                    }
                    if (radioSearchGreaterThan.IsChecked == true)
                    {
                        searchResults = searchByGreaterThanRuntime(searchResults, searchRuntime);
                    }
                }
                if (chkSearchIncludeCriterion.IsChecked == true)
                {
                    searchResults = chkSearchCriterion.IsChecked == true ? searchByCriterion(searchResults, true) : searchByCriterion(searchResults, false);

                }

                if (searchResults.Count == 0)
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
                if (lstSearchResults.SelectedItem != null && lstSearchResults.SelectedValue.ToString() == movieVM.title)
                {
                    movie = movieVM;
                    _selectedID = movieVM.titleID;
                    txtResultTitle.Text = movie.title;
                    txtResultYear.Text = "Year: " + movie.year.ToString();
                    txtResultRating.Text = "Rating: " + movie.rating;
                    txtResultFormats.Text = displayList(movie.formats);
                    txtResultLanguages.Text = displayList(movie.languages);
                    txtResultGenres.Text = displayList(movie.genres);
                    txtResultRuntime.Text = movie.runtime.ToString() + "mins";
                    txtResultCriterion.Text = "Criterion: " + criterionOutputConverter(movie.isCriterion);
                    imgSearch.Source = displayImageFromURL(movie.imgName);
                }
            }


        }

        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void mnuAddMovie_Click(object sender, RoutedEventArgs e)
        {
            if (tabAdd.Visibility == Visibility.Visible)
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
                if (editMovie.editResult)
                {
                    refreshMovieList();
                    resetForms();
                    refreshSearchCBOs();
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
                        refreshSearchCBOs();




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
                    refreshSearchCBOs();
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
                        refreshSearchCBOs();
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
            cboAdminType.SelectedIndex = -1;
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
            btnProfilePassword.Visibility = Visibility.Hidden;

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
            btnProfilePassword.Visibility = Visibility.Visible;
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
                if (result == false)
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
                    btnProfilePassword.Visibility = Visibility.Visible;
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
            try
            {
                if (cboAdminType.SelectedValue == cboAdminTypeMovie && cboAdminStatus.SelectedValue == cboAdminStatusActive)
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
                if (cboAdminType.SelectedValue == cboAdminTypeMovie && cboAdminStatus.SelectedValue == cboAdminStatusInactive)
                {
                    Movie movie = null;

                    foreach (Movie inactiveMovie in _inactiveMovies)
                    {
                        if (lstAdmin.SelectedValue.ToString() == inactiveMovie.title)
                        {
                            movie = inactiveMovie;
                            _selectedID = inactiveMovie.titleID;
                        }
                    }
                    if (movie != null)
                    {
                        txtAdminID.Text = movie.titleID.ToString();
                        txtAdminOther.Text = movie.title;
                        string imgURL = _movieManager.GetImageURLByMovieID(movie.titleID);
                        imgAdminMovie.Source = displayImageFromURL(imgURL);
                    }
                }
                if (cboAdminType.SelectedValue == cboAdminTypeUser)
                {
                    if (cboAdminStatus.SelectedValue == cboAdminStatusActive)
                    {
                        User selectedUser = null;
                        foreach (User user in _activeUsers)
                        {
                            if (lstAdmin.SelectedValue.ToString() == user.email)
                            {
                                selectedUser = user;
                            }
                        }
                        if (selectedUser != null)
                        {
                            txtAdminID.Text = selectedUser.userID.ToString();
                            txtAdminOther.Text = selectedUser.email;
                            imgAdminUser.Source = displayImageFromURL(selectedUser.imgURL);
                            _selectedUserID = selectedUser.userID;
                        }
                    }
                    if (cboAdminStatus.SelectedValue == cboAdminStatusInactive)
                    {
                        User selectedUser = null;
                        foreach (User user in _inactiveUsers)
                        {
                            if (lstAdmin.SelectedValue.ToString() == user.email)
                            {
                                selectedUser = user;
                            }
                        }
                        if (selectedUser != null)
                        {
                            txtAdminID.Text = selectedUser.userID.ToString();
                            txtAdminOther.Text = selectedUser.email;
                            imgAdminUser.Source = displayImageFromURL(selectedUser.imgURL);
                            _selectedUserID = selectedUser.userID;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error with your request /n/n" + ex.InnerException.Message);
            }

        }

        private void refreshSearchCBOs()
        {
            _movieYears.Clear();
            _movieYears = new List<int>();
            _movieYears = getMovieYears(_movieVMs);
            cboSearchYear.ItemsSource = _movieYears;

            _searchLanguages.Clear();
            _searchLanguages = new List<string>();
            _searchLanguages = getCurrentMovieLanguages(_movieVMs);
            cboSearchLanguage.ItemsSource = _searchLanguages;

            _searchGenres.Clear();
            _searchGenres = new List<string>();
            _searchGenres = getCurrentMovieGenres(_movieVMs);
            cboSearchGenre.ItemsSource = _searchGenres;
        }



        private void cboAdminType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstAdmin != null)
                {
                    lstAdmin.Items.Clear();

                    txtAdminID.Text = "";
                    txtAdminOther.Text = "";
                    imgAdminMovie.Source = null;
                    imgAdminUser.Source = null;

                }
                if (cboAdminType.SelectedValue == cboAdminTypeDefault)
                {

                }
                if (cboAdminType.SelectedValue == cboAdminTypeMovie)
                {
                    lblAdminOther.Content = "Name:";
                    imgAdminMovie.Visibility = Visibility.Visible;
                    imgAdminUser.Visibility = Visibility.Hidden;
                    btnAdminRight.Visibility = Visibility.Hidden;

                }
                if (cboAdminType.SelectedValue == cboAdminTypeUser)
                {
                    lblAdminOther.Content = "Email:";
                    btnAdminRight.Content = "Reset Password";
                    imgAdminMovie.Visibility = Visibility.Hidden;
                    imgAdminUser.Visibility = Visibility.Visible;
                    btnAdminRight.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing your request\n\n" + ex.InnerException.Message);
            }
        }

        private void cboAdminStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstAdmin != null)
                {
                    lstAdmin.Items.Clear();

                    txtAdminID.Text = "";
                    txtAdminOther.Text = "";
                    imgAdminMovie.Source = null;
                    imgAdminUser.Source = null;

                }
                if (cboAdminStatus.SelectedValue == cboAdminStatusDefault)
                {

                }
                if (cboAdminType.SelectedValue == cboAdminTypeMovie)
                {

                    if (cboAdminStatus.SelectedValue == cboAdminStatusActive)
                    {
                        txtAdminID.Text = "";
                        txtAdminOther.Text = "";
                        imgAdminMovie.Source = null;
                        imgAdminUser.Source = null;
                        btnAdminLeft.Content = "Deactivate";

                        if (lstAdmin != null)
                        {
                            lstAdmin.Items.Clear();
                        }
                        foreach (MovieVM movie in _movieVMs)
                        {
                            lstAdmin.Items.Add(movie.title);
                        }
                    }
                    if (cboAdminStatus.SelectedValue == cboAdminStatusInactive)
                    {
                        txtAdminID.Text = "";
                        txtAdminOther.Text = "";
                        imgAdminMovie.Source = null;
                        imgAdminUser.Source = null;
                        btnAdminLeft.Content = "Reactivate";

                        if (lstAdmin != null)
                        {
                            lstAdmin.Items.Clear();
                        }
                        if (_inactiveMovies != null)
                        {
                            foreach (Movie movie in _inactiveMovies)
                            {
                                lstAdmin.Items.Add(movie.title);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No inactive movies found");
                        }
                    }
                }
                if (cboAdminType.SelectedValue == cboAdminTypeUser)
                {

                    btnAdminRight.Content = "Reset Password";
                    if (cboAdminStatus.SelectedValue == cboAdminStatusActive)
                    {
                        btnAdminLeft.Content = "Deactivate";
                        if (_activeUsers != null)
                        {
                            foreach (User user in _activeUsers)
                            {
                                lstAdmin.Items.Add(user.email);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No active users found");
                        }

                    }
                    if (cboAdminStatus.SelectedValue == cboAdminStatusInactive)
                    {
                        btnAdminLeft.Content = "Reactivate";
                        if (_inactiveUsers != null)
                        {
                            foreach (User user in _inactiveUsers)
                            {
                                lstAdmin.Items.Add(user.email);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No inactive users found");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No info found \n\n" + ex.InnerException.Message);
            }


        }

        private void btnAdminLeft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cboAdminType.SelectedValue == cboAdminTypeMovie)
                {
                    if (cboAdminStatus.SelectedValue == cboAdminStatusActive && lblAdminID.Content.ToString() != "")
                    {
                        _movieManager.UpdateMovieIsActive(_selectedID, false);
                        MessageBox.Show("Movie has been deactivated");
                        _inactiveMovies.Clear();
                        _movieVMs.Clear();
                        _movieVMs = _movieManager.GetAllMovieVMs();
                        _inactiveMovies = _movieManager.GetAllInactiveMovies();
                    }

                    if (cboAdminStatus.SelectedValue == cboAdminStatusInactive && lblAdminID.Content.ToString() != "")
                    {
                        _movieManager.UpdateMovieIsActive(_selectedID, true);
                        MessageBox.Show("Movie has been reactivated");
                        _inactiveMovies.Clear();
                        _movieVMs.Clear();
                        _movieVMs = _movieManager.GetAllMovieVMs();
                        _inactiveMovies = _movieManager.GetAllInactiveMovies();
                    }
                }
                if (cboAdminType.SelectedValue == cboAdminTypeUser)
                {

                    if (cboAdminStatus.SelectedValue == cboAdminStatusActive)
                    {
                        _userManager.UpdateUserIsActive(_selectedUserID, false);
                        MessageBox.Show("User has been deactivated");
                        _activeUsers.Clear();
                        _inactiveUsers.Clear();
                        _activeUsers = _userManager.GetActiveUsers();
                        _inactiveUsers = _userManager.GetInactiveUsers();
                        lstAdmin.Items.Clear();
                        foreach (var user in _activeUsers)
                        {
                            lstAdmin.Items.Add(user.email);
                        }
                    }
                    if (cboAdminStatus.SelectedValue == cboAdminStatusInactive)
                    {
                        _userManager.UpdateUserIsActive(_selectedUserID, true);
                        MessageBox.Show("User has been reactivated");
                        _activeUsers.Clear();
                        _inactiveUsers.Clear();
                        _activeUsers = _userManager.GetActiveUsers();
                        _inactiveUsers = _userManager.GetInactiveUsers();
                        lstAdmin.Items.Clear();
                        foreach (var user in _inactiveUsers)
                        {
                            lstAdmin.Items.Add(user.email);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error executing action \n\n" + ex.InnerException.Message);
            }
        }

        private void btnProfilePassword_Click(object sender, RoutedEventArgs e)
        {
            UpdatePassword updatePassword = new UpdatePassword(_loggedInUser.email);
            blurWindow(10);
            updatePassword.ShowDialog();
            if (updatePassword.DialogResult == true)
            {
                MessageBox.Show("Password Successfully Changed");
            }
            else
            {
                MessageBox.Show("Password not changed, please try again");
            }
            blurWindow(0);
        }

        private void btnProfileDeactivate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Are you sure?", "Deactivation Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _userManager.UpdateUserIsActive(_loggedInUser.userID, false);
                    _inactiveUsers.Clear();
                    _activeUsers.Clear();
                    _inactiveUsers = _userManager.GetInactiveUsers();
                    _activeUsers = _userManager.GetActiveUsers();
                    MessageBox.Show("Your account has been deactivated. Please contact james@gmail.com for reactivation");
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
                else
                {
                    MessageBox.Show("Account not deactivated. Please try again later");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error with account deactivation. Try again later. \n\n");
            }
        }

        private void btnAdminRight_Click(object sender, RoutedEventArgs e)
        {
            if (btnAdminRight.Content.ToString() == "Reset Password")
            {
                try
                {

                    if (txtAdminOther.Text != "" && _userManager.ResetPasswordAdmin(txtAdminOther.Text))
                    {
                        MessageBox.Show("Password reset");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error resetting password", ex.InnerException.Message);
                }
            }
        }

        private void mnuChangePassword_Click(object sender, RoutedEventArgs e)
        {
            UpdatePassword updatePassword = new UpdatePassword(_loggedInUser.email);
            blurWindow(10);
            updatePassword.ShowDialog();
            if (updatePassword.DialogResult == true)
            {
                MessageBox.Show("Password Successfully Changed");
            }
            else
            {
                MessageBox.Show("Password not changed, please try again");
            }
            blurWindow(0);
        }

        private void cboCollectionsSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstCollectionContents.Items.Clear();
            try
            {
                if (_collectionVMs != null)
                {
                    List<MovieVM> movies = new List<MovieVM>();
                    foreach (CollectionVM collection in _collectionVMs)
                    {
                        if (collection != null && cboCollectionsSelect.SelectedValue != null &&
                            collection.collectionName == cboCollectionsSelect.SelectedValue.ToString())
                        {

                            foreach (int movieID in collection.movieIDs)
                            {
                                movies.Add(_movieManager.GetMovieByTitleID(movieID));
                            }

                        }
                    }
                    if (movies != null)
                    {
                        foreach (MovieVM movie in movies)
                        {
                            lstCollectionContents.Items.Add(movie.title);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void lstCollectionContents_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MovieVM movie = null;
            try
            {

                foreach (MovieVM movieVM in _movieVMs)
                {
                    if (lstCollectionContents.SelectedItem != null && lstCollectionContents.SelectedValue.ToString() == movieVM.title)
                    {
                        movie = movieVM;
                        txtCollectionsTitle.Text = movie.title;
                        txtCollectionsYear.Text = movie.year.ToString();
                        txtCollectionsRating.Text = movie.rating;
                        txtCollectionsRuntime.Text = movie.runtime.ToString();
                        txtCollectionsCriterion.Text = criterionOutputConverter(movie.isCriterion);
                        txtCollectionsGenres.Text = displayList(movie.genres);
                        txtCollectionsLanguages.Text = displayList(movie.languages);
                        imgCollectionsImage.Source = displayImageFromURL(movie.imgName);
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("No Movie Selected");
            }
        }

        private void btnCollectionNew_Click(object sender, RoutedEventArgs e)
        {
            if (txtCollectionNew.Text == "")
            {
                MessageBox.Show("Please give your collection a name");
            }
            else
            {
                try
                {
                    foreach (CollectionVM collection in _collectionVMs)
                    {
                        if (txtCollectionNew.Text.ToLower() == collection.collectionName.ToLower())
                        {
                            MessageBox.Show("You already have a collection by that name.");
                        }
                    }

                    bool addCollectionResult = _collectionManager.AddUserCollection(_loggedInUser.userID, txtCollectionNew.Text);
                    
                    if (addCollectionResult)
                    {
                        MessageBox.Show("Success! Collection Added");
                        _collectionVMs = _collectionManager.GetCollectionsByUserID(_loggedInUser.userID);
                        cboCollectionsSelect.Items.Clear();
                        cboCollectionsAddMoveCollection.Items.Clear();
                        cboResultAddToCollection.Items.Clear();
                        txtCollectionNew.Text = "";
                        foreach (CollectionVM collectionVM in _collectionVMs)
                        {
                            cboCollectionsSelect.Items.Add(collectionVM.collectionName);
                            cboCollectionsAddMoveCollection.Items.Add(collectionVM.collectionName);
                            cboResultAddToCollection.Items.Add(collectionVM.collectionName);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Collection addition failed");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not add collection\n\n" + ex.InnerException.Message);
                }

            }
        }

        private void btnCollectionDelete_Click(object sender, RoutedEventArgs e)
        {
            if(cboCollectionsSelect.SelectedValue == null)
            {
                MessageBox.Show("You must choose a collection to remove.");
            } 
            else if (cboCollectionsSelect.SelectedValue.ToString() == "Favorites")
            {
                MessageBox.Show("You can not remove the collection 'Favorites'");
            }            
            else
            {
                try
                {
                    CollectionVM selectedCollection = new CollectionVM();
                    
                    foreach(var collection in _collectionVMs)
                    {
                        if(collection.collectionName ==  cboCollectionsSelect.SelectedValue.ToString())
                        {
                            selectedCollection = collection;
                            break;
                        }
                    }

                    var confirmation = MessageBox.Show("Are you sure you want to remove " + 
                        cboCollectionsSelect.SelectedValue.ToString() + "?", "Confirm Removal", MessageBoxButton.YesNo);
                    if(confirmation == MessageBoxResult.No) 
                    {
                        MessageBox.Show("Whew, close call! We'll save this collection for later!");
                    } else
                    {
                        bool result = _collectionManager.RemoveUserCollection(_loggedInUser.userID, selectedCollection.collectionID);
                        if(result)
                        {
                            MessageBox.Show("This collection has been removed");
                            _collectionVMs = _collectionManager.GetCollectionsByUserID(_loggedInUser.userID);
                            cboCollectionsSelect.Items.Clear();
                            cboCollectionsAddMoveCollection.Items.Clear();
                            cboResultAddToCollection.Items.Clear();
                            foreach(var collection in _collectionVMs)
                            {
                                cboCollectionsSelect.Items.Add(collection.collectionName);
                                cboCollectionsAddMoveCollection.Items.Add(collection.collectionName);
                                cboResultAddToCollection.Items.Add(collection.collectionName);
                                
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error removing collection \n\n" + ex.InnerException.Message);
                }
            }
        }

        private void btnCollectionRemoveTitle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CollectionVM selectedCollection = new CollectionVM();
                MovieVM movie = null;
                foreach (var collection in _collectionVMs)
                {
                    if (collection.collectionName == cboCollectionsSelect.SelectedValue.ToString())
                    {
                        selectedCollection = collection;
                        break;
                    }
                }

                foreach (MovieVM movieVM in _movieVMs)
                {
                    if (lstCollectionContents.SelectedItem != null && lstCollectionContents.SelectedValue.ToString() == movieVM.title)
                    {
                        movie = movieVM; 
                    }
                }

                var confirmation = MessageBox.Show("Are you sure you want to remove this movie?", "Confirm Removal", MessageBoxButton.YesNo);
                if (confirmation == MessageBoxResult.No)
                {
                    MessageBox.Show("Woah, that was a close call! We'll keep this movie right where it is!");
                }
                else
                {
                    bool result = _collectionManager.RemoveMovieFromCollection(movie.titleID, selectedCollection.collectionID);
                    if(result)
                    {
                        MessageBox.Show("Success! Movie removed from your collection!");
                        txtCollectionsTitle.Text = "";
                        txtCollectionsYear.Text = "";
                        txtCollectionsRating.Text = "";
                        txtCollectionsRuntime.Text = "";
                        txtCollectionsCriterion.Text = "";
                        txtCollectionsGenres.Text = "";
                        txtCollectionsLanguages.Text = "";
                        imgCollectionsImage.Source = null;

                        txtCollectionsYear.Text = "";
                        foreach (var collection in _collectionVMs)
                        {
                            collection.movieIDs = _collectionManager.GetMovieIDsByCollectionID(collection.collectionID);
                            lstCollectionContents.Items.Clear();

                            foreach(var movieVM in _movieVMs)
                            {
                                foreach(int movieID in collection.movieIDs)
                                {
                                    if(movieVM.titleID ==  movieID)
                                    {
                                        lstCollectionContents.Items.Add(movieVM.title);
                                    }
                                }
                            }
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("Failure. The movie remains");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("No Movie Selected");
            }
        }

        private void btnResultAdd_Click(object sender, RoutedEventArgs e)
        {
            if(cboResultAddToCollection.SelectedValue == null)
            {
                MessageBox.Show("Please select a collection to add to");
            }
            else if(_selectedID == 0)
            {
                MessageBox.Show("Please select a movie to add to the collection");
            }
            else
            {
                try
                {
                    
                    foreach(var collection in _collectionVMs)
                    {
                        if(collection.collectionName == cboResultAddToCollection.SelectedValue.ToString())
                        {
                            foreach(int movie in collection.movieIDs) 
                            { 
                               if(movie == _selectedID)
                                {
                                    MessageBox.Show("This movie already exists in this collection");
                                    break;
                                }
                               else
                                {
                                    bool result = _collectionManager.AddMovieToCollection(_selectedID, collection.collectionID);
                                    if (!result)
                                    {
                                        MessageBox.Show("Movie may already exist in collection.");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Movie added to collection");
                                        collection.movieIDs = _collectionManager.GetMovieIDsByCollectionID(collection.collectionID);
                                        break;
                                    }
                                }
                            }
                        }

                    }
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Movie already exists in the collection");
                }
            }
        }
    }
}
