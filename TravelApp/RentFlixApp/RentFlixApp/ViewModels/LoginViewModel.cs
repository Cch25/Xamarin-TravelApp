using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using RentFlixApp.Annotations;
using RentFlixApp.Helpers;
using RentFlixApp.Services;
using Xamarin.Forms;

namespace RentFlixApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly CredentialServices _loginService = new CredentialServices();
        private bool _isBusy;
        public string UserName { get; set; }
        public string Password { get; set; }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var accessToken = await _loginService.LoginAsync(UserName, Password);
                    Settings.AccessToken = accessToken;
                    Settings.Username = UserName;
                    Settings.Password = Password;
                });
            }
        }
        public async Task LoginUserAsync()
        {
            IsBusy = true;
            var accessToken = await _loginService.LoginAsync(UserName, Password);
            Settings.AccessToken = accessToken;
            Settings.Username = UserName;
            Settings.Password = Password;
            IsBusy = false;
        }

        public LoginViewModel()
        {
            UserName = Settings.Username;
            Password = Settings.Password;
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
