using System;
using System.ComponentModel;

namespace Project.Models
{
    public class SEOModel : INotifyPropertyChanged
    {
        private string _keywords;
        private string _url;

        public string Keywords
        {
            get => _keywords;
            set { _keywords = value; OnPropertyChanged(nameof(Keywords)); }
        }
        public string Url
        {
            get => _url;
            set { _url = value; OnPropertyChanged(nameof(Url)); }
        }

        public bool IsRequestingData { get; set; }

        public bool HasData => !string.IsNullOrEmpty(Keywords) && !string.IsNullOrEmpty(Url);

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}