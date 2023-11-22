using DataObjects;
using LogicLayer;
using System;
using System.Collections;
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
using System.Windows.Shapes;

using static DataObjects.DisplayHelpers;

namespace MovieCollection
{
    /// <summary>
    /// Interaction logic for EditMovie.xaml
    /// </summary>
    public partial class EditMovie : Window
    {
        private int _movieID;
        private MovieVM _movie;
        public bool editResult = false;
        public EditMovie(int movieID)
        {
            _movieID = movieID;
            
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MovieManager movieManager = new MovieManager();
            _movie = movieManager.GetMovieByTitleID(_movieID);
            txtEditTitle.Text = _movie.title;
            cboEditYear.ItemsSource = getYears(1888);
            cboEditYear.Text = _movie.year.ToString();
            cboEditRating.ItemsSource = movieManager.GetAllRatings();
            cboEditRating.Text = _movie.rating;
            lstEditGenre.ItemsSource = movieManager.GetAllGenres();
            txtEditNotes.Text = _movie.notes;
            foreach(var genre in _movie.genres)
            {
                lstEditGenre.SelectedItem = genre;
            }
            lstEditLanguage.ItemsSource = movieManager.GetAllLanguages();
            foreach(var language in _movie.languages)
            {
                lstEditLanguage.SelectedItem = language;
            }
            txtEditRuntime.Text = _movie.runtime.ToString();
            chkEditCriterion.IsChecked = _movie.isCriterion;
            txtEditURL.Text = _movie.imgName;
            imgEditImage.Source = displayImageFromURL(_movie.imgName);
            
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

        private void btnEditPreviewImage_Click(object sender, RoutedEventArgs e)
        {
            imgEditImage.Source = displayImageFromURL(txtEditURL.Text);
        }

        private void btnEditCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnEditSubmit_Click(object sender, RoutedEventArgs e)
        {
            MovieManager movieManager = new MovieManager();
            bool editMovieSuccess = false;
            try
            {
                string newTitle = txtEditTitle.Text;
                int newYear = int.Parse(cboEditYear.SelectedValue.ToString());
                string newRating = cboEditRating.SelectedValue.ToString();
                int newRuntime = int.Parse(txtEditRuntime.Text);
                bool newCriterion = chkEditCriterion.IsChecked == true;
                string newNotes = txtEditNotes.Text;
                List<string> newGenres = new List<string>();
                foreach(string genre in lstEditGenre.SelectedItems)
                {
                    newGenres.Add(genre);
                }
                List<string> newLanguages = new List<string>();
                foreach(string language in lstEditLanguage.SelectedItems)
                {
                    newLanguages.Add(language);
                }
                string newURL = txtEditURL.Text;

                editMovieSuccess = movieManager.UpdateMovie(_movie, newTitle, newYear, newRating, newRuntime, newCriterion, newNotes,
                    newLanguages, newGenres, newURL);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating title \n\n" + ex.InnerException.Message);
            }

            if(editMovieSuccess ) 
            {
                editResult = true;
                this.Close();                
            }
        }
    }
}
