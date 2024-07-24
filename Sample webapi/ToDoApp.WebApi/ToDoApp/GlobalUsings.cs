global using Microsoft.AspNetCore.Mvc;
global using ToDoApp.BAL.Interfaces;
global using ToDoApp.DAL.DTO;
global using ToDoApp.ResponseModel.Enums;
global using ToDoApp.ResponseModel;
global using Asp.Versioning;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.IdentityModel.Tokens;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Text;
global using ToDoApp.BAL.Service;
global using ToDoApp.DAL.Interfaces;
global using ToDoApp.DAL.Mapper;
global using ToDoApp.DAL.Repository;
global using Newtonsoft.Json;
global using Newtonsoft.Json.Converters;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.OpenApi.Models;
global using Serilog;
global using ToDoApp.DB.Models;
global using ToDoApp.Helpers;