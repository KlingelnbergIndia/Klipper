using Klipper.Desktop.Service.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klipper.Desktop.WPF.Connectors
{
    public class MainConnector
    {
        #region Instance

        static MainConnector _instance = null;

        public static MainConnector Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MainConnector();
                }
                return _instance;
            }
        }

        public static void DeleteInstance()
        {
            _instance = null;
        }

        private MainConnector()
        {
        }

        #endregion

        #region Events

        public event EventHandler<string> LoginSuccessful = null;

        #endregion

        public void Initialize()
        {
            LoginManager.Instance.LoginSuccessful += OnLoginSuccessful;
        }

        #region Event handlers

        private void OnLoginSuccessful(object sender, string e)
        {

            LoginSuccessful?.Invoke(this, e);
        }

        #endregion
    }
}
