# Uroskur
A .NET MAUI app that fetches a weather forecast from OpenWeather, Yr or SMHI and routes from Strava to display the weather conditions along your routes based upon your pace, time of day and start date. The app supports showing a single forecast or multiple forecasts at once.

SMHI are missing the following parameters: feels like temperature, uv index and cloudiness.

## How to use
You will need a Strava account and a OpenWeather account.

### Strava
The process for creating a Strava API application is described on the Strava website. Follow the instructions on [Getting Started]( https://developers.strava.com/docs/getting-started/). To configure Uroskur you need a **Client ID** and **Client Secret**. You will find these on the My API Application page under Settings.

### OpenWeather
Sign up for an account on [OpenWeather](https://openweathermap.org). To configure Uroskur you need a OpenWeather **API key**. You can generate a OpenWeather API key on the OpenWeather website under the "My API keys" page.

### Configure Uroskur
Select "Settings" in the Uroskur app menu. Fill in your Strava Client ID, Strava Client Secret and OpenWeather API key and press "Save Settings". Then press "Connect with Strava". If you aren’t already signed into Strava, you will be prompted for your Strava credentials. Click the Authorize button so that your Strava API Application has permission to read your Strava routes.

## Platforms
The app is tested on the following platforms: Android, iOS and Windows 10/11.

## Screenshots Android
<p float="left">
<img src="https://i.ibb.co/wrxsb1j/Routes-Android.png" width="200" height="400" />
<img src="https://i.ibb.co/H4QP4wF/Route-Android.jpg" width="200" height="400" />
<img src="https://i.ibb.co/Z10gXS6/Options-Android.jpg" width="200" height="400" />
<img src="https://i.ibb.co/VMqHLQS/Forecast-Android.png" width="200" height="400" />
<img src="https://i.ibb.co/WPYwcsj/Forecast2-Android.png" width="200" height="400" />
<img src="https://i.ibb.co/hYWr68V/Combined-Android.jpg" width="200" height="400" />
<img src="https://i.ibb.co/CmZHQGt/Settings-Android.jpg" width="200" height="400" />
<img src="https://i.ibb.co/VCkMtrv/About-Android.jpg" width="200" height="400" />
</p>

## Screenshots iOS
<p float="left">
<img src="https://i.ibb.co/mCRd6XG/Routes-i-OS.png" width="200" height="400" />
<img src="https://i.ibb.co/3R7rJtS/Route-i-OS.png" width="200" height="400" />
<img src="https://i.ibb.co/Jc8D466/Forecast-i-OS.png" width="200" height="400" />
</p>

## Screenshots Windows
<p float="left">
<img src="https://i.ibb.co/prdJJNV/Routes-Win-UI.png" width="200" height="400" />
<img src="https://i.ibb.co/MS9vNkq/Route-Win-UI.png" width="200" height="400" />
<img src="https://i.ibb.co/c6KQDRR/Forecast-Win-UI.png" width="200" height="400" />
</p>
