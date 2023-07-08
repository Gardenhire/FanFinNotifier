using System;
using FirebaseAdmin;
using Google.Cloud.Functions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace FanFinNotifier
{
	public class Startup: FunctionsStartup
	{
        public override void Configure(WebHostBuilderContext context, IApplicationBuilder app)
        {
            FirebaseApp.Create();
        }
    }
}

