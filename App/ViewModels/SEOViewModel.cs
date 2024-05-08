using Project.Models;
using Project.Services;
using Project.ViewModels.Commands;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project.ViewModels
{
    /// <summary>
    /// View model responsible for the interactions from the main window.
    /// </summary>
    public class SEOViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _http;
        private SEOModel _input;
        private SEOResponseModel _result;

        public event PropertyChangedEventHandler PropertyChanged;

        public SEOModel Input
        {
            get
            {
                return _input;
            }
            set
            {
                _input = value;
            }
        }

        public SEOResponseModel Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
            }
        }

        public ICommand GetDataCommand { get; private set; }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public SEOViewModel()
        {
            _input = new SEOModel();
            GetDataCommand = new DataCommand(GetData);
            _http = new HttpClient();
        }

        private async void GetData(object info)
        {
            _input = (SEOModel)info;
            _input.IsRequestingData = true;
            OnPropertyChanged(nameof(Input));
            _result = await SEODataService.GetDataAsync(_input.Keywords, _input.Url, _http);
            _input.IsRequestingData = false;
            OnPropertyChanged(nameof(Input));
            OnPropertyChanged(nameof(Result));
            CommandManager.InvalidateRequerySuggested();
        }
    }
}