using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using RentFlixApp.Annotations;
using RentFlixApp.Helpers;
using RentFlixApp.Services;

namespace RentFlixApp.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private readonly CredentialServices _registerServices = new CredentialServices();
        public string Username { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public async Task<bool> RegisterUser()
        {
            IsBusy = true;
            var isSuccess = await _registerServices.RegisterAsync(NickName, Username, Password, ConfirmPassword);
            Settings.Username = Username;
            Settings.Password = Password;
            Settings.NickName = NickName;
            IsBusy = false;
            return isSuccess;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
