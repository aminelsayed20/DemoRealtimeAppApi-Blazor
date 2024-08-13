using BlazorWebAssemblyApp.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Threading.Tasks;

namespace BlazorWebAssemblyApp
{
    public class UserState
    {

        public string Username { get; private set; }

        public void SetUsername(string username)
        {
            Username = username;
        }
    }
}
