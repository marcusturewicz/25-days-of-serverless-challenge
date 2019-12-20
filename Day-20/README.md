# Day 20 of [25 days of serverless](https://25daysofserverless.com)

[IOT WITH COGNITIVE SERVICES](https://25daysofserverless.com/calendar/20)

C# Azure Function, Computer Vision, static website and slack notifications.

Renders your webcam video to a webpage and takes a snapshot image every 2.5 seconds. The image is then sent to the function which uses Azure Cognitive Services Computer Vision to analyse the image and send a notification to Slack if a
gift is detected.

Example:

![](img/gift.png)

Detected:

![](img/slack.png)

-- Created with VS Code.
