using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IdentityModel.Client;

namespace IdpWpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string _accessToken = string.Empty;
        private string _identyTokenUrl = string.Empty;

        private async void Btn1_OnClick(object sender, RoutedEventArgs e)
        {
            var username = this.UserName.Text;
            var password = this.Password.Text;
            using var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var client = new HttpClient(httpClientHandler); 

            var disco=await client.GetDiscoveryDocumentAsync("http://localhost:5001");
            if (disco.IsError)
            {
                this.rBox1.AppendText(disco.Error);
                return;
            }

            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest()
            {
                Address = disco.TokenEndpoint,
                UserName = username,
                Password = password,
                ClientSecret = "wpf secret",
                ClientId = "wpf client",
                Scope = "scope1 openid profile address phone email"
            });

            if (tokenResponse.IsError)
            {
                this.rBox1.AppendText(tokenResponse.Error);
                return;
            }

            _accessToken = tokenResponse.AccessToken;
            _identyTokenUrl = disco.UserInfoEndpoint;
            this.rBox1.AppendText(tokenResponse.Json.ToString());
        }

        private async void Btn2_OnClick(object sender, RoutedEventArgs e)
        {
            var client = new HttpClient();
            client.SetBearerToken(_accessToken);
            var response = await client.GetAsync("http://localhost:6001/identity");
            if (!response.IsSuccessStatusCode)
            {
                this.rBox2.AppendText(response.StatusCode.ToString());
            }
            else
            {
                this.rBox2.AppendText( await response.Content.ReadAsStringAsync());
            }
        }

        private async void Btn3_OnClick(object sender, RoutedEventArgs e)
        {
            var client = new HttpClient();
            client.SetBearerToken(_accessToken);
            var response = await client.GetAsync(_identyTokenUrl);
            if (!response.IsSuccessStatusCode)
            {
                this.rBox3.AppendText(response.StatusCode.ToString());
            }
            else
            {
                this.rBox3.AppendText( await response.Content.ReadAsStringAsync());
            }
        }
    }
}