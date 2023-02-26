# :date: .NET Appointment Planner
[![CI](https://github.com/philipp-meier/Edap/actions/workflows/dotnet.yml/badge.svg)](https://github.com/philipp-meier/Edap/actions/workflows/dotnet.yml)
[![MIT License](https://img.shields.io/badge/license-MIT-green.svg)](https://github.com/philipp-meier/Edap/blob/main/LICENSE)

A simple .NET appointment planner with Keycloak integration, that can be hosted within a Docker container.

**Note:** Participation-links like `https://localhost:7298/polls/participate/e84203a2509748d2bcafa209365707bc` do not require an authenticated user.

![Preview](https://static.p-meier.dev/polls/ParticipatePoll.png)

## Use cases
- As an administrator I want to create polls, so that I can find the date, where most of the participants are available.
- As an administrator I want to generate invitation links, so that I can send them to people that will participate in the poll.
- As an administrator I want to close and archive a poll, so that the result can't be changed after a specific date.
- As a user I want to open an invitation link, so that I can participate in a poll.

## Features
- Simple admin interface for creating polls.
- Simple user interface to see the current result and vote.
- Invitation links
- Docker support
- Keycloak integration

## Restrictions (Current version)
- Only the administrator can create polls.
- Every authenticated "Edap"-client user is currently an administrator (no claim checks).

## Tech stack
- ASP.NET Razor pages, C#, EF Core, Sqlite3

## Run
- **Step 1:** Create a poll database in the "Data" directory.  
`mkdir Data && sqlite3 Data/polls.db "VACUUM;"`  

- **Step 2:** Update the database.  
`dotnet ef database update`  

- **Step 3:** Enter your IdP data in the "IdentityProvider"-section of the `appsettings.json`.

- **Step 4:** Run the application  
`dotnet run` or `dotnet watch` for hot reload  

## Deployment
- **Step 1:** Build the image and publish it to your docker registry.  
`docker login <DOCKER_REGISTRY_HOST>`  
`docker build -t <DOCKER_REGISTRY_HOST>/edap .`  
`docker push <DOCKER_REGISTRY_HOST>/edap`  

- **Step 2:** Create a volume on the host to persist data.  
`docker volume create edap_volume`  

- **Step 3:** Copy your polls.db database to the edap_volume.  
Use `docker volume inspect edap_volume` to find the location of the edap_volume.  

- **Step 4:** Download the image on the host machine and run the container.  
`docker login <DOCKER_REGISTRY_HOST>`  
`docker pull <DOCKER_REGISTRY_HOST>/edap`  
`docker run --name edap -d -p 5850:80 --mount source=edap_volume,target=/app/Data <DOCKER_REGISTRY_HOST>/edap`  
