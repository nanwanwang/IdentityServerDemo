﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;

namespace Idp
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Phone(),
                new IdentityResources.Email()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("scope1"),
                new ApiScope("scope2"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "console client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = { "scope1" ,IdentityServerConstants.StandardScopes.OpenId}
                },
                
                new Client()
                {
                    ClientId = "wpf client",
                    ClientName = "",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("wpf secret".Sha256())
                    },
                    AllowedScopes =
                    {
                        "scope1",IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.Address
                    }
                    
                },
                new Client()
                {
                    ClientId = "mvc client",
                    ClientName = "ASP.NET Core MVC Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets = {new Secret("mvc secret".Sha256())},
                    RedirectUris = { "http://localhost:5002/signin-oidc" },
                    FrontChannelLogoutUri = "http://localhost:5002/signout-oidc",
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },
                    
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile" }
                }

                // interactive client using code flow + pkce
                // new Client
                // {
                //     ClientId = "interactive",
                //     ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },
                //     
                //     AllowedGrantTypes = GrantTypes.Code,
                //
                //     RedirectUris = { "https://localhost:44300/signin-oidc" },
                //     FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                //     PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },
                //
                //     AllowOfflineAccess = true,
                //     AllowedScopes = { "openid", "profile", "scope2" }
                // },
            };
    }
}