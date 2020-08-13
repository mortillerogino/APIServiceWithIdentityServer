// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using IdentityServer.Data;
using IdentityServer.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace IdentityServer.Data
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(connectionString, x => x.MigrationsAssembly("IdentityServer.Data")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var maria = userMgr.FindByNameAsync("maria").Result;
                    if (maria == null)
                    {
                        maria = new ApplicationUser
                        {
                            UserName = "maria",
                            Email = "MariaSmith@email.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(maria, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(maria, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Maria Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Maria"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://mariasmith.com"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Log.Debug("maria created");
                    }
                    else
                    {
                        Log.Debug("maria already exists");
                    }

                    var pedro = userMgr.FindByNameAsync("pedro").Result;
                    if (pedro == null)
                    {
                        pedro = new ApplicationUser
                        {
                            UserName = "pedro",
                            Email = "PedroSmith@email.com",
                            EmailConfirmed = true
                        };
                        var result = userMgr.CreateAsync(pedro, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(pedro, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Pedro Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Pedro"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://pedrosmith.com"),
                            new Claim("location", "somewhere")
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Log.Debug("pedro created");
                    }
                    else
                    {
                        Log.Debug("pedro already exists");
                    }
                }
            }
        }
    }
}
