﻿using Microsoft.AspNetCore.Components.Routing;
using Newtonsoft.Json;
using System.Text;
using Xunit.Abstractions;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ShowerShow.DTO;
using ShowerShow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace ShowerShowIntegrationTest
{
    public class LoginResultDTO
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
    }
    public class ControllerBase
    {
        protected HttpClient client { get; set; }
        protected ITestOutputHelper outputHelper;
        public ControllerBase(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
            this.client = new HttpClient()
            {
                BaseAddress = new Uri($"http://localhost:7177/api/")
                //http://localhost:7071/api/Login"
            };
        }

        protected async Task Authenticate()
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", await GetAuthString());
        }
        //This should work. Contact me if it doesn't
        private async Task<string> GetAuthString()
        {
            Login loginUser = new Login() { Username = "cosmin", Password = "cosmin" };
            string requesturi = "Login";
            HttpContent http = new StringContent(JsonConvert.SerializeObject(loginUser), Encoding.UTF8, "application/json");
            //client.BaseAddress= new Uri("http://localhost:7177/api/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.PostAsync(requesturi, http).Result;

            var authString = (await response.Content.ReadAsAsync<LoginResultDTO>()).AccessToken;
            // client.BaseAddress = new Uri($"http://localhost:7177/api/");
            return authString;
        }

        public async Task FlushUser(string username)
        {
            string requestUri = $"user/{username}";
            await Authenticate();
            var response = await client.DeleteAsync(requestUri);
        }
    }
}