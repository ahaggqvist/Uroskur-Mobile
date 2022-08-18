# Uroskur
[![Build .NET MAUI Android](https://github.com/ahaggqvist/Uroskur-Maui/actions/workflows/dotnet-android.yml/badge.svg?branch=main)](https://github.com/ahaggqvist/Uroskur-Maui/actions/workflows/dotnet-android.yml) [![Build .NET MAUI Windows](https://github.com/ahaggqvist/Uroskur-Maui/actions/workflows/dotnet-windows.yml/badge.svg?branch=main)](https://github.com/ahaggqvist/Uroskur-Maui/actions/workflows/dotnet-windows.yml)

A .NET MAUI app that fetches a weather forecast from OpenWeather, Yr or SMHI and routes from Strava to display the weather conditions along your routes based upon your pace, time of day and start date. The app supports showing a single forecast or multiple forecasts at once.

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
<img src="https://i.ibb.co/Sr5sKJy/Routes.jpg" width="200" />
<img src="https://i.ibb.co/P1fN6Tv/Route.jpg" width="200" />
<img src="https://i.ibb.co/kMS8nd8/Forecast1.jpg" width="200" />
<img src="https://i.ibb.co/Y2Sr9hs/Forecast2.jpg" width="200" />
<img src="https://i.ibb.co/MZVFtQT/Forecast-combined.jpg" width="200" />
<img src="https://i.ibb.co/vJYdnDb/Settings.jpg" width="200" />
<img src="https://i.ibb.co/Z1hr4kM/About.jpg" width="200" />
</p>
