using BookZone.API.Manage.DTOs.AccountDtos;
using BookZone.API.Manage.DTOs.AuthorDtos;
using BookZone.API.Manage.DTOs.BookDtos;
using BookZone.API.Manage.DTOs.CategoryDtos;
using BookZone.API.Manage.DTOs.GenreDtos;
using BookZone.API.Manage.DTOs.SettingDtos;
using BookZone.API.Manage.DTOs.SliderDtos;
using BookZone.API.Manage.DTOs.TagDtos;
using BookZone.Data;
using BookZone.Data.Entities;
using BookZone.Repository.Implementations;
using BookZone.Repository.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookZone.API.Client;
using BookZone.API.Client.DTOs.CommentDtos;

namespace BookZone
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddFluentValidation();
            services.AddAutoMapper(typeof(Startup)); 
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            }).AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudience = Configuration.GetSection("JWT:Issuer").Value,
                    ValidIssuer = Configuration.GetSection("JWT:Issuer").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JWT:SecurityKey").Value))
                };
            });


            #region ValidatorServices
            services.AddTransient<IValidator<SliderCreateDto>, SliderCreateDtoValidation>();
            services.AddTransient<IValidator<TagCreateDto>, TagCreateDtoValidator>();
            services.AddTransient<IValidator<GenreCreateDto>, GenreCreateDtoValidator>();
            services.AddTransient<IValidator<AuthorCreateDto>, AuthorCreateDtoValidator>();
            services.AddTransient<IValidator<BookCreateDto>,BookCreateDtoValidator>();
            services.AddTransient<IValidator<CategoryCreateDto>,CategoryCreateDtoValidation>();
            services.AddTransient<IValidator<SettingCreateDto>, SettingCreateDtoValidation>();
            services.AddTransient<IValidator<AccountUpdateDto>, AccountUpdateDtoValidation>();
            services.AddTransient<IValidator<LoginDto>, LoginDtoValidation>();
            services.AddTransient<IValidator<CommentCreateDto>, CommentCreateDtoValidation>();
            #endregion

            #region RepositoryServices
            services.AddScoped<ISliderRepository, SliderRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBookTagRepository, BookTagRepository>();
            services.AddScoped<ISettingRepository, SettingRepository>();
            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
