﻿using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Atlassian.Jira;
using AzureMonitor.ConfigModels;
using AzureMonitor.Interfaces;
using Microsoft.Identity.Client;

namespace AzureMonitor.Services
{
    public class AuthenticationService: IAuthenticationService
    {
        private IAppConfigService _appConfigService;

        public AuthenticationService(IAppConfigService appConfigService)
        {
            _appConfigService = appConfigService;
        }

        public IConfidentialClientApplication GetAzureConfidentialClientApplication()
        {
            // You can run this sample using ClientSecret or Certificate. The code will differ only when instantiating the IConfidentialClientApplication
            bool isUsingClientSecret = AuthenticationService.AppUsesClientSecret(_appConfigService.Config);

            // Even if this is a console application here, a daemon application is a confidential client application
            IConfidentialClientApplication app;

            if (isUsingClientSecret)
            {
                app = ConfidentialClientApplicationBuilder.Create(_appConfigService.Config.AzureConfig.ClientId)
                    .WithClientSecret(_appConfigService.Config.AzureConfig.ClientSecret)
                    .WithAuthority(new Uri(_appConfigService.Config.AzureConfig.Authority))
                    .Build();
            }
            else
            {
                X509Certificate2 certificate = ReadCertificate(_appConfigService.Config.AzureConfig.CertificateName);
                app = ConfidentialClientApplicationBuilder.Create(_appConfigService.Config.AzureConfig.ClientId)
                    .WithCertificate(certificate)
                    .WithAuthority(new Uri(_appConfigService.Config.AzureConfig.Authority))
                    .Build();
            }

            return app;
        }

        public static Jira GetJiraClientApplication(AppConfig config)
        {
            var jira = Jira.CreateRestClient(config.JiraConfig.Endpoint, config.JiraConfig.Username,
                config.JiraConfig.Password);

            return jira;
        }

        public static async Task<AuthenticationResult> AcquireAzureTokenForClient(IConfidentialClientApplication app, string[] scopes)
        {
            AuthenticationResult result = null;

            try
            {
                result = await app.AcquireTokenForClient(scopes)
                    .ExecuteAsync();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Token acquired for scopes: " + string.Join(',', scopes));
                Console.ResetColor();
            }
            catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
            {
                // Invalid scope. The scope has to be of the form "https://resourceurl/.default"
                // Mitigation: change the scope to be as expected
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Scope provided is not supported");
                Console.ResetColor();
            }

            return result;
        }

        /// <summary>
        /// Checks if the sample is configured for using ClientSecret or Certificate. This method is just for the sake of this sample.
        /// You won't need this verification in your production application since you will be authenticating in AAD using one mechanism only.
        /// </summary>
        /// <param name="config">Configuration from appsettings.json</param>
        /// <returns></returns>
        private static bool AppUsesClientSecret(AppConfig config)
        {
            if (!string.IsNullOrWhiteSpace(config.AzureConfig.ClientSecret))
            {
                return true;
            }

            if (!string.IsNullOrWhiteSpace(config.AzureConfig.CertificateName))
            {
                return false;
            }

            throw new Exception("You must choose between using client secret or certificate. Please update appsettings.json file.");
        }

        private static X509Certificate2 ReadCertificate(string certificateName)
        {
            if (string.IsNullOrWhiteSpace(certificateName))
            {
                throw new ArgumentException("certificateName should not be empty. Please set the CertificateName setting in the appsettings.json", "certificateName");
            }
            X509Certificate2 cert = null;

            using (X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certCollection = store.Certificates;

                // Find unexpired certificates.
                X509Certificate2Collection currentCerts = certCollection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);

                // From the collection of unexpired certificates, find the ones with the correct name.
                X509Certificate2Collection signingCert = currentCerts.Find(X509FindType.FindBySubjectDistinguishedName, certificateName, false);

                // Return the first certificate in the collection, has the right name and is current.
                cert = signingCert.OfType<X509Certificate2>().OrderByDescending(c => c.NotBefore).FirstOrDefault();
            }
            return cert;
        }
    }
}
