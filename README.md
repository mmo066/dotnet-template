# Dotnet Template
Template repository for Web APIs using .Net 6 with a workflow that automatically configures the project files, Kubernetes deployment configurations and GitHub actions.

## How to use
1. **Create your repository**
   1. Click `Use this template` -> `Create a new repository` 
   2. Fill in the name for your repository and submit

## What's included?
* Fully configured Kubernetes deployment files for each environment
* GitHub Actions that cover the build & deploy flow while also including secrets injection, version release, rollback and package cleaning
* Whitelist for IP addresses from BKS/VPN
* Swagger for development environments
* Health check with Liveness and Startup probes
* JSON logger for production environment
* Error controller
* CORS policy for each environment
* Bearer authentication scheme
