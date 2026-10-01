# SaaS Platform

A modern, production-ready **SaaS starter platform** built with **Nuxt, .NET, and PostgreSQL**, with Docker-based deployment and automated CI/CD.

## ✨ Features

* 🔐 Authentication & user management
* 🛡️ RBAC & fine-grained permissions
* 🏢 Multi-organization / multi-tenant support
* ✉️ Organization invitations
* 💳 Subscription billing
* 🐳 Dockerized backend
* 🚀 Vercel + VPS/Droplet deployment
* ⚙️ GitHub Actions CI/CD

## 🏗️ Stack

| Layer      | Technology           |
| ---------- | -------------------- |
| Frontend   | Nuxt                 |
| Backend    | .NET / ASP.NET Core  |
| Database   | PostgreSQL           |
| Deployment | Vercel + VPS/Droplet |
| Containers | Docker               |
| CI/CD      | GitHub Actions       |

## 📁 Structure

```text
├── frontend/       # Nuxt application
├── backend/        # .NET API
├── docker/         # Docker configuration
├── .github/
│   └── workflows/  # CI/CD pipelines
└── README.md
```

## 🚀 Development

```bash
git clone https://github.com/wellington-wong/saas-platform-nuxt-dotnet-docker-ci-cd
cd your-repository

docker compose up -d

# Frontend
cd frontend
npm install
npm run dev

# Backend
cd backend
dotnet restore
dotnet run
```

## 🔄 Deployment

```text
GitHub
  │
  ├──► Vercel ──► Nuxt
  │
  └──► GitHub Actions ──► Docker ──► VPS/Droplet ──► .NET API
                                      │
                                      └── PostgreSQL
```

Production deployments are automated through **GitHub Actions**, with the Nuxt frontend deployed to Vercel and backend services running in Docker on a VPS/Droplet.

## 🔐 Core Domain

```text
User
 └── Organization
      ├── Membership
      ├── Roles
      ├── Permissions
      ├── Invitations
      └── Subscription
```

## 📄 License

MIT
