# Foliora CI/CD Pipeline Refactoring

## Agent Implementation Specification

|Property|Value|
|---|---|
|Project|Foliora|
|Task|CI/CD Pipeline Refactoring|
|Platform|GitHub Actions|
|Container Registry|GitHub Container Registry (GHCR)|
|Deployment Runner|Self-hosted GitHub Actions Runner|
|Backend|ASP.NET Core / .NET 10|
|Architecture|Modular Monolith|
|DEV Database|Existing Local Database|
|PROD Database|Supabase PostgreSQL|
|Workflow|CI + Docker Publishing + CD|

---

# 1. Objective

Refactor the existing Foliora GitHub Actions workflow to follow the CI/CD architecture implemented in the IssueTracker project.

The goal is to establish a reliable, maintainable, and environment-isolated deployment pipeline.

The pipeline must support:

- Automated backend CI.
    
- Automated dependency restoration.
    
- Automated solution building.
    
- Automated test execution.
    
- Automated Docker image building.
    
- Automated Docker image publishing to GHCR.
    
- Separate DEV and PROD deployment jobs.
    
- Self-hosted runner for deployments.
    
- Environment-specific secrets and configurations.
    
- Branch-based deployment conditions.
    
- Safe database connectivity and environment isolation.
    

Do not blindly copy the IssueTracker implementation.

Adapt the implementation to Foliora's actual repository structure, application configuration, database providers, Docker setup, and deployment infrastructure.

# 2. Existing Infrastructure

The following architecture is the intended target.

## DEV Environment

- Application: Foliora Backend API.
    
- Docker image: GHCR `develop` tag.
    
- Deployment: Self-hosted GitHub Actions runner.
    
- Database: Existing local DEV database.
    
- Deployment script: `deploy-dev.sh`.
    

## PROD Environment

- Application: Foliora Backend API.
    
- Docker image: GHCR `main` tag.
    
- Deployment: Self-hosted GitHub Actions runner.
    
- Database: Supabase PostgreSQL.
    
- Deployment script: `deploy-prod.sh`.
    

## Important Database Distinction

DEV and PROD do not use the same database infrastructure.

|Component|DEV|PROD|
|---|---|---|
|API Image|`develop`|`main`|
|Database|Existing local database|Supabase PostgreSQL|
|Deployment|Self-hosted runner|Self-hosted runner|
|Configuration|DEV environment|PROD environment|
|Secrets|DEV-specific|PROD-specific|

Do not introduce a Docker-hosted production database.

Do not migrate the existing DEV database to Supabase.

Preserve the existing database infrastructure.

# 3. Target CI/CD Architecture

The expected workflow:

```
                  GitHub Repository
                         |
             Push / Pull Request
                         |
                         v
                  Backend CI
                         |
              +----------+----------+
              |          |          |
              v          v          v
           Restore     Build      Test
                         |
                         v
                  CI Successful
                         |
                         v
                Docker Build & Push
                         |
                         v
                       GHCR
                         |
                 +-------+-------+
                 |               |
                 v               v
            DEV Deployment  PROD Deployment
              develop           main
                 |               |
                 v               v
           Self-hosted       Self-hosted
             Runner            Runner
                 |               |
                 v               v
           deploy-dev.sh    deploy-prod.sh
                 |               |
                 v               v
          Local DEV DB      Supabase PROD DB
```

The pipeline must enforce the correct branch conditions and job dependencies.

# 4. GitHub Actions Workflow Requirements

Create or update the workflow inside:

`.github/workflows/`

Use a descriptive workflow name such as:

`Foliora CI/CD Pipeline`

## 4.1 Workflow Triggers

The workflow must execute on:

```
on:
  push:
    branches:
      - main
      - develop

  pull_request:
    branches:
      - main
      - develop
```

Expected behavior:

|Event|Backend CI|Docker Publish|Deployment|
|---|---|---|---|
|PR to develop|Yes|No|No|
|PR to main|Yes|No|No|
|Push to develop|Yes|Yes|DEV|
|Push to main|Yes|Yes|PROD|

Pull requests must never publish Docker images or execute deployment jobs.

## 4.2 Permissions

Use least-privilege permissions.

The workflow should use:

```
permissions:
  contents: read
```

The Docker publishing job should receive:

```
permissions:
  contents: read
  packages: write
```

Do not grant unnecessary permissions to other jobs.

# 5. Backend CI Job

Job name:

`backend-ci`

Runner:

`ubuntu-latest`

Solution:

`backend/Foliora.slnx`

Required steps:

1. Checkout repository.
    
2. Setup .NET SDK 10.
    
3. Restore dependencies.
    
4. Build solution.
    
5. Run automated tests.
    

Expected command structure:

```
dotnet restore backend/Foliora.slnx

dotnet build backend/Foliora.slnx \
  --no-restore \
  --configuration Release

dotnet test backend/Foliora.slnx \
  --no-build \
  --configuration Release
```

Requirements:

- Use the actual solution path.
    
- Preserve existing project configuration.
    
- Do not skip failing tests.
    
- Do not hide build errors.
    
- Do not modify application business logic to make CI pass.
    
- Verify whether test projects exist.
    
- If tests are missing, report this instead of silently claiming test coverage.
    

The CI job must succeed before Docker publishing can begin.

# 6. Docker Build and GHCR Publishing

Job name:

`backend-cd`

Dependency:

`needs: backend-ci`

Runner:

`ubuntu-latest`

Execution condition:

Only execute when:

- Event is `push`.
    
- Branch is `develop` or `main`.
    

Never publish images during pull requests.

## 6.1 Container Registry

Use:

GitHub Container Registry (GHCR)

Expected image naming convention:

```
ghcr.io/<github-owner>/foliora-api
```

Use the actual repository owner and ensure consistency with deployment scripts.

## 6.2 Authentication

Use:

`docker/login-action`

Authenticate using:

`${{ secrets.GITHUB_TOKEN }}`

Do not hardcode credentials.

## 6.3 Docker Metadata

Use:

`docker/metadata-action`

Generate image tags automatically.

Expected tags:

|Git Branch|Docker Tag|
|---|---|
|develop|develop|
|main|main|
|Any published commit|Short SHA|

Example:

```
ghcr.io/<owner>/foliora-api:develop
ghcr.io/<owner>/foliora-api:main
ghcr.io/<owner>/foliora-api:abc1234
```

## 6.4 Docker Build and Push

Use:

`docker/build-push-action`

Dockerfile:

`backend/Dockerfile`

Build context:

Inspect the repository and determine the correct context before implementation.

Do not assume that the repository root is always the correct build context.

Requirements:

- Build the actual Foliora backend image.
    
- Apply generated metadata tags.
    
- Apply generated labels.
    
- Push only on eligible branch pushes.
    
- Ensure failed builds do not publish incomplete images.
    

The Docker publishing job must complete successfully before deployment begins.

# 7. DEV Deployment Job

Job name:

`deploy-dev`

Runner:

`self-hosted`

Dependency:

`needs: backend-cd`

Execution condition:

```
if: github.event_name == 'push' && github.ref == 'refs/heads/develop'
```

Deployment script:

`deploy-dev.sh`

Required behavior:

1. Checkout repository.
    
2. Load DEV-specific configuration securely.
    
3. Pull the correct GHCR image.
    
4. Use the `develop` image tag.
    
5. Execute the DEV deployment script.
    
6. Verify that the application starts successfully.
    
7. Report deployment failure when any required step fails.
    

## DEV Database Configuration

DEV uses the existing local database infrastructure.

IMPORTANT:

- Preserve the current database engine.
    
- Preserve the existing database name.
    
- Preserve existing ports and container names.
    
- Preserve existing Docker volumes.
    
- Preserve existing connection string configuration.
    
- Preserve existing data.
    
- Do not migrate DEV to Supabase.
    

Inspect the actual network configuration.

If the API runs inside Docker while the DEV database runs on the host, verify the correct host networking and connection string.

Do not blindly use `localhost` inside a Docker container because it refers to the container itself.

Do not modify the database infrastructure unless explicitly required and approved.

# 8. PROD Deployment Job

Job name:

`deploy-prod`

Runner:

`self-hosted`

Dependency:

`needs: backend-cd`

Execution condition:

```
if: github.event_name == 'push' && github.ref == 'refs/heads/main'
```

Deployment script:

`deploy-prod.sh`

Required behavior:

1. Checkout repository.
    
2. Load PROD-specific configuration securely.
    
3. Pull the correct GHCR image.
    
4. Use the `main` image tag.
    
5. Execute the PROD deployment script.
    
6. Verify that the application starts successfully.
    
7. Report deployment failure when any required step fails.
    

## PROD Database Configuration

PROD uses Supabase PostgreSQL.

IMPORTANT:

- Do not create a separate PostgreSQL Docker container for PROD.
    
- Do not provision a separate production database.
    
- Do not replace Supabase with a local database.
    
- Do not commit Supabase credentials.
    
- Do not expose the production connection string in logs.
    

The application must connect to the existing Supabase PostgreSQL instance using secure environment configuration.

Inspect the existing connection string and database provider configuration before making changes.

# 9. Database Provider and EF Core Compatibility

Before implementing database-related configuration, inspect:

1. Existing EF Core packages.
    
2. Current database provider.
    
3. Existing `DbContext` configuration.
    
4. Existing migration files.
    
5. Existing connection string configuration.
    
6. Existing database initialization logic.
    

Determine whether DEV and PROD use compatible database providers.

For example:

- SQL Server DEV.
    
- PostgreSQL PROD through Supabase.
    

If the providers differ, do not assume that changing connection strings is sufficient.

EF Core providers may have different SQL generation behavior, migration compatibility, data types, and database-specific features.

Do not automatically:

- Replace database providers.
    
- Generate new migrations.
    
- Rewrite existing migrations.
    
- Change database schemas.
    
- Reset databases.
    
- Run destructive migrations.
    

If cross-provider compatibility requires architectural changes, report the findings and propose a separate implementation plan.

Do not mix database-provider migration work with CI/CD refactoring without approval.

# 10. Secrets and Environment Configuration

Use GitHub Secrets and GitHub Environments.

Never hardcode sensitive information.

Potential configuration categories:

## DEV

- DEV database connection string or required database credentials.
    
- DEV JWT signing key.
    
- DEV JWT issuer.
    
- DEV JWT audience.
    
- Other existing DEV-specific application secrets.
    

## PROD

- Supabase PostgreSQL connection string.
    
- PROD JWT signing key.
    
- PROD JWT issuer.
    
- PROD JWT audience.
    
- Other existing PROD-specific application secrets.
    

Important:

The exact secret names must be based on the existing application configuration.

Do not invent secret names without checking how the application reads environment variables.

Use separate DEV and PROD configurations.

Never allow the DEV deployment to receive PROD database credentials.

Never allow the PROD deployment to receive DEV database credentials.

Consider configuring GitHub Environments:

- `development`
    
- `production`
    

Production deployment should use appropriate approval protection if configured.

# 11. Deployment Script Requirements

Inspect the following files:

```
deploy-dev.sh
deploy-prod.sh
```

If they already exist, review and adapt them.

If they do not exist, propose their implementation based on the existing Docker Compose configuration and infrastructure.

Do not invent commands or assume the existing infrastructure.

The scripts should:

- Pull the correct GHCR image.
    
- Use the correct image tag.
    
- Load the correct environment configuration.
    
- Restart only the intended application services.
    
- Preserve database containers and volumes.
    
- Verify the application is running.
    
- Return a non-zero exit code when deployment fails.
    

Avoid destructive commands such as:

```
docker compose down -v
docker volume prune
docker system prune -a
```

Do not remove persistent data.

Do not recreate databases as part of ordinary application deployment.

# 12. Docker and Runtime Configuration

Inspect:

- `backend/Dockerfile`
    
- Existing Docker Compose files.
    
- Existing `.env` files.
    
- Existing application configuration.
    
- Existing container networking.
    
- Existing volume definitions.
    
- Existing application ports.
    

Ensure that:

- The API image is built correctly.
    
- The container receives the correct environment variables.
    
- The API can reach its intended database.
    
- DEV and PROD use separate application configurations.
    
- Production secrets are not included in Docker image layers.
    
- Sensitive `.env` files are not committed.
    
- Health checks are used if appropriate.
    

Do not unnecessarily rewrite the existing Dockerfile.

Do not introduce new infrastructure without justification.

# 13. Security Requirements

Follow these rules:

1. Use `GITHUB_TOKEN` for GHCR authentication.
    
2. Apply least-privilege workflow permissions.
    
3. Never hardcode secrets.
    
4. Never print credentials in logs.
    
5. Never expose Supabase connection strings.
    
6. Never execute deployment jobs during pull requests.
    
7. Keep DEV and PROD secrets isolated.
    
8. Protect production deployment where possible.
    
9. Do not execute untrusted pull-request code using privileged deployment credentials.
    
10. Do not use unnecessary administrator permissions.
    

# 14. Repository Inspection Before Implementation

Before modifying any files, inspect:

```
.github/workflows/
backend/
backend/Foliora.slnx
backend/Dockerfile
deploy-dev.sh
deploy-prod.sh
Docker Compose files
Environment configuration files
Database configuration
EF Core migrations
```

Use the actual repository structure.

Identify:

- Existing workflow files.
    
- Existing backend project structure.
    
- Existing test projects.
    
- Existing Docker configuration.
    
- Existing deployment scripts.
    
- Existing DEV database infrastructure.
    
- Existing Supabase configuration.
    
- Existing environment variables.
    
- Existing GHCR image naming.
    
- Existing self-hosted runner assumptions.
    

Do not assume that every listed file exists.

# 15. Implementation Constraints

Strictly follow these constraints:

- Do not modify application business logic.
    
- Do not modify the Modular Monolith architecture.
    
- Do not refactor unrelated backend modules.
    
- Do not modify frontend code.
    
- Do not modify database schemas.
    
- Do not change existing database providers.
    
- Do not reset existing databases.
    
- Do not delete Docker volumes.
    
- Do not expose secrets.
    
- Do not introduce unnecessary dependencies.
    
- Do not change existing production infrastructure without approval.
    

Keep the implementation focused on CI/CD.

# 16. Validation Checklist

After implementation, validate the following:

## Workflow

- YAML syntax is valid.
    
- Workflow triggers are correct.
    
- Backend CI runs on pushes and pull requests.
    
- Docker publishing runs only on eligible pushes.
    
- DEV deployment runs only on `develop`.
    
- PROD deployment runs only on `main`.
    
- Job dependencies are correct.
    
- Failed CI prevents Docker publishing.
    
- Failed publishing prevents deployment.
    

## Docker

- Dockerfile path is correct.
    
- Build context is correct.
    
- GHCR image name is correct.
    
- Branch tags are correct.
    
- SHA tags are generated.
    
- Image publishing uses appropriate permissions.
    

## DEV

- DEV uses the existing local database.
    
- DEV configuration is isolated.
    
- DEV connection string is correct.
    
- Existing database volumes are preserved.
    
- Deployment uses the `develop` image.
    

## PROD

- PROD uses Supabase PostgreSQL.
    
- Supabase credentials are secured.
    
- PROD configuration is isolated.
    
- No unnecessary PostgreSQL container is created.
    
- Deployment uses the `main` image.
    
- Existing production data is preserved.
    

## Security

- No hardcoded credentials.
    
- No secrets exposed in logs.
    
- Pull requests cannot deploy.
    
- Workflow permissions follow least privilege.
    
- Production protection is considered.
    

# 17. Expected Deliverables

The agent must provide:

1. Updated GitHub Actions workflow.
    
2. Updated or newly created deployment scripts, if necessary.
    
3. Documentation of required GitHub Secrets.
    
4. Documentation of required GitHub Environments.
    
5. Explanation of the final CI/CD architecture.
    
6. List of modified files.
    
7. List of newly created files.
    
8. Validation results.
    
9. Remaining manual GitHub configuration steps.
    
10. Any identified infrastructure risks or compatibility issues.
    

# 18. Execution Strategy

## Phase 1: Plan Mode

Start in Plan Mode.

Do not modify files yet.

Inspect the repository and provide:

- Current infrastructure findings.
    
- Existing workflow analysis.
    
- Existing database configuration.
    
- Files that need modification.
    
- Proposed CI/CD architecture.
    
- Required secrets.
    
- Potential risks.
    
- Implementation steps.
    

Wait for explicit user approval.

## Phase 2: Implementation

After approval:

- Implement only the approved changes.
    
- Preserve existing infrastructure.
    
- Keep modifications focused.
    
- Follow all security requirements.
    
- Do not perform destructive database operations.
    

## Phase 3: Validation

After implementation:

- Validate the workflow.
    
- Validate Docker configuration.
    
- Validate deployment scripts.
    
- Verify environment isolation.
    
- Report validation results.
    

Do not claim a successful deployment unless an actual deployment was performed and verified.

---

# FINAL INSTRUCTION

The objective is to create a professional, safe, and maintainable CI/CD pipeline for Foliora.

Follow the existing IssueTracker CI/CD philosophy while adapting it to Foliora's infrastructure.

The most important requirements are:

1. DEV and PROD must remain isolated.
    
2. DEV must continue using its existing local database.
    
3. PROD must use Supabase PostgreSQL.
    
4. Docker images must be published to GHCR.
    
5. Deployments must use separate DEV and PROD jobs.
    
6. Production infrastructure and database data must be protected.
    
7. All implementation decisions must be based on actual repository inspection.
    

**Start with Plan Mode. Inspect first. Explain the plan. Wait for approval.**