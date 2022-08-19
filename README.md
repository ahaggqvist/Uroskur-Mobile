# Uroskur
[![Windows Builds](https://github.com/ahaggqvist/Uroskur-Maui/actions/workflows/dotnet-windows.yml/badge.svg?branch=main)](https://github.com/ahaggqvist/Uroskur-Maui/actions/workflows/dotnet-windows.yml) [![Android Builds](https://github.com/ahaggqvist/Uroskur-Maui/actions/workflows/dotnet-android.yml/badge.svg?branch=main)](https://github.com/ahaggqvist/Uroskur-Maui/actions/workflows/dotnet-android.yml) [![Mac Builds](https://github.com/ahaggqvist/Uroskur-Maui/actions/workflows/dotnet-macos.yml/badge.svg?branch=main)](https://github.com/ahaggqvist/Uroskur-Maui/actions/workflows/dotnet-macos.yml)

A .NET MAUI app that fetches a weather forecast from OpenWeather, Yr or SMHI and routes from Strava to show weather conditions along your routes based upon your pace, time of day and start date. The app supports showing a single forecast or multiple forecasts at once.

SMHI are missing the following parameters: feels like temperature, uv index and cloudiness.

## How to use
You will need a Strava account and a OpenWeather account.

### Strava
The process for creating a Strava API application is described on the Strava website. Follow the instructions on [Getting Started]( https://developers.strava.com/docs/getting-started/). To configure Uroskur you need a **Client ID** and **Client Secret**. You will find these on the "My API Application" page under Settings.

### OpenWeather
Sign up for an account on [OpenWeather](https://openweathermap.org). To configure Uroskur you need a OpenWeather **API key**. You can generate a OpenWeather API key on the OpenWeather website under the "My API keys" page.

### Setup Uroskur
Select "Settings" in the Uroskur app menu. Fill in your Strava Client ID, Strava Client Secret and OpenWeather API key and press "Save Settings". Then press "Connect with Strava". If you aren’t already signed into Strava, you will be prompted for your Strava credentials. Click the Authorize button so that your Strava API Application has permission to read your Strava routes.

## Platforms
The app is tested on the following platforms: Android, iOS, Windows 10/11 and macOS.

## Screenshots Android
<p float="left">
<img src="https://i.ibb.co/wRSzC2D/Routes.jpg" width="200" />
<img src="https://i.ibb.co/0y1GRN6/Route.jpg" width="200" />
<img src="https://i.ibb.co/pny4Wpj/Forecast1.jpg" width="200" />
<img src="https://i.ibb.co/HNBq2Hk/Forecast-combo.jpg" width="200" />
<img src="https://i.ibb.co/ZgSK9wM/Settings.jpg" width="200" />
<img src="https://i.ibb.co/Z1hr4kM/About.jpg" width="200" />
</p>
