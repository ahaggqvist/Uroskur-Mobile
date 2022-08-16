﻿global using System.Collections.Immutable;
global using System.Collections.ObjectModel;
global using System.Diagnostics;
global using System.Globalization;
global using System.Net;
global using System.Net.Http.Headers;
global using System.Reflection;
global using System.Text;
global using System.Text.RegularExpressions;
global using System.Xml;
global using System.Xml.Serialization;
global using CommunityToolkit.Maui;
global using CommunityToolkit.Maui.Alerts;
global using CommunityToolkit.Maui.Core;
global using CommunityToolkit.Mvvm.ComponentModel;
global using CommunityToolkit.Mvvm.Input;
global using Microcharts;
global using MonkeyCache.FileStore;
global using Newtonsoft.Json;
global using Newtonsoft.Json.Converters;
global using Polly;
global using SkiaSharp;
global using SkiaSharp.Views.Maui.Controls.Hosting;
global using Uroskur.Models;
global using Uroskur.Models.OpenWeather;
global using Uroskur.Models.Smhi;
global using Uroskur.Models.Strava;
global using Uroskur.Models.Yr;
global using Uroskur.Pages;
global using Uroskur.Services;
global using Uroskur.Utils;
global using Uroskur.Utils.Clients;
global using Uroskur.ViewModels;
global using static Uroskur.Utils.Constants;
global using static Uroskur.Models.Smhi.Name;
global using static Uroskur.Utils.WeatherForecastProvider;
global using Location = Uroskur.Models.Location;
global using Preferences = Uroskur.Models.Preferences;