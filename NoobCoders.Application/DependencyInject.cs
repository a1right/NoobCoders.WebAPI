﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NoobCoders.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using NoobCoders.Application.Services;

namespace NoobCoders.Application
{
    public static class DependencyInject
    {
        public static IServiceCollection AddCsvReader(this IServiceCollection services)
        {
            services.AddSingleton<ICsvReader, CsvReaderService>();
            return services;
        }

        public static IServiceCollection AddRecordsService(this IServiceCollection services)
        {
            services.AddScoped<IRecordsService, RecordsService>();
            return services;
        }
    }
}
