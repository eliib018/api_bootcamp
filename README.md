\## Funcionalidades



operaciones principales sobre cuentas:

\- Crear una cuenta.
\- Obtener todas las cuentas.
\- Obtener una cuenta por identificador.
\- Actualizar una cuenta.
\- Eliminar una cuenta.
\- Validar los datos recibidos.
\- Detectar conflictos como números de cuenta duplicados.
\- Registrar solicitudes y errores mediante Serilog y Seq.
\- Verificar el estado de la aplicación y de PostgreSQL mediante Health Checks.

\## Arquitectura
El proyecto utiliza Clean Architecture y está dividido en cuatro proyectos:

src/
├── Api/
├── Application/
├── Domain/
└── Infrastructure/

\### Domain
Contiene las entidades y reglas principales del dominio
No depende de las demás capas

\### Application
Contiene los casos de uso de la aplicación
Utiliza:
\- CQRS
\- MediatR
\- FluentValidation
\- DTOs
\- Interfaces de repositorios
Depende de Domain

\### Infrastructure
Contiene la implementación de persistencia
Utiliza:
\- Entity Framework Core
\- PostgreSQL
\- Configuraciones de entidades
\- Repositorios
\- Migraciones
Depende de Application y Domain.

\### ApiBootcamp
Es el punto de entrada de la aplicación
Contiene:
\- Controllers
\- Middleware de manejo de errores
\- Swagger 
\- Health Checks
\- Configuración de Serilog
\- Integración con Seq
\- Inyección de dependencias


\## Tecnologías
\- .NET 10
\- ASP.NET Core
\- Entity Framework Core
\- PostgreSQL
\- MediatR
\- FluentValidation
\- Serilog
\- Seq
\- Swagger / OpenAPI
\- Docker
\- Docker Compose
\- Kubernetes
\- Minikube
\- Helm
\- GitHub Actions



\## Endpoints
| Método | Endpoint | Descripción |

| GET | `/api/v1/cuentas` | Obtener todas las cuentas |

| GET | `/api/v1/cuentas/{id}` | Obtener una cuenta por ID |

| POST | `/api/v1/cuentas` | Crear una cuenta |

| PUT | `/api/v1/cuentas/{id}` | Actualizar una cuenta |

| DELETE | `/api/v1/cuentas/{id}` | Eliminar una cuenta |



La API utiliza códigos HTTP como:



\- `200 OK`

\- `201 Created`

\- `204 No Content`

\- `400 Bad Request`

\- `404 Not Found`

\- `409 Conflict`

\- `500 Internal Server Error`



\## Swagger



Cuando la aplicación se ejecuta en ambiente Development, Swagger se encuentra disponible en:



```text

http://localhost:8080/swagger/index.html

```



\## Health Checks



La aplicación dispone de dos endpoints de salud:





\## Ejecución con Docker Compose



Definir primero la contraseña de PostgreSQL en PowerShell:



```powershell

$env:POSTGRES\_PASSWORD="postgress"

```



Luego ejecutar:



```powershell

docker compose up --build

```



Servicios disponibles:



```text

API:        http://localhost:8080

Swagger:    http://localhost:8080/swagger/index.html

Seq:        http://localhost:5341

PostgreSQL: localhost:5433

```



Para detener los servicios:



```powershell

docker compose down

```



Para eliminar además los volúmenes:



```powershell

docker compose down -v

```



\## Base de datos



La aplicación utiliza PostgreSQL mediante Entity Framework Core.

La migración inicial se encuentra versionada dentro del proyecto Infrastructure.



Cuando:



```text

Database\_\_ApplyMigrations=true

```

la aplicación ejecuta automáticamente las migraciones pendientes al iniciar.



\## Logging y observabilidad



La aplicación utiliza Serilog para generar logs estructurados.



Los eventos son enviados a:



\- Consola.

\- Seq.

Los logs de Swagger, Health Checks y recursos auxiliares se reducen para evitar ruido innecesario en Seq.



\## Kubernetes

Los manifiestos necesarios se encuentran en: k8s/

Incluyen recursos para:

\- Namespace.

\- PostgreSQL.

\- PersistentVolumeClaim de PostgreSQL.

\- Service de PostgreSQL.

\- Seq.

\- PersistentVolumeClaim de Seq.

\- Service de Seq.

\- Secrets.



El namespace utilizado es: api-bootcamp





\## Helm

El chart se encuentra en: helm/api-bootcamp/

Su estructura principal es:





helm/api/

├── Chart.yaml

├── values.yaml

└── templates/

&#x20;   ├── \_helpers.tpl

&#x20;   ├── configmap.yaml

&#x20;   ├── deployment.yaml

&#x20;   ├── secret.yaml

&#x20;   └── service.yaml



Helm permite parametrizar la imagen de la API, puertos, PostgreSQL, Seq, Health Checks y demás configuraciones del despliegue.



Validación del chart:

helm lint helm/api-bootcamp --set-string postgres.password=validacion



\## Minikube

El proyecto puede desplegarse localmente utilizando Minikube.



Ejemplo:

minikube start --driver=docker --cpus=2 --memory=3072



Verificar el clúster:

minikube status

kubectl get nodes



Ver los recursos desplegados:

kubectl get pods -n api-bootcamp

kubectl get services -n api-bootcamp



\## CI/CD

El pipeline está definido en:

.github/workflows/ci.yml



\### Integración continua

El proceso de CI realiza:

1\. Checkout del repositorio.

2\. Configuración de .NET.

3\. Restore.

4\. Build en Release.

5\. Validación del chart Helm.

6\. Inicio de sesión en Docker Hub.

7\. Construcción de la imagen Docker.

8\. Publicación de la imagen versionada.



\### Despliegue continuo

El CD utiliza un runner self-hosted Windows y realiza:

1\. Verificación de Minikube.

2\. Creación del namespace.

3\. Creación dinámica de Secrets.

4\. Despliegue de PostgreSQL.

5\. Despliegue de Seq.

6\. Despliegue de ApiBootcamp mediante Helm.

7\. Verificación de los Deployments y Services.



\## Secrets de GitHub Actions

El workflow utiliza los siguientes Repository Secrets:



DOCKERHUB\_USERNAME

DOCKERHUB\_TOKEN

POSTGRES\_PASSWORD

SEQ\_ADMIN\_PASSWORD



\## Estructura general

api\_bootcamp/

├── .github/

│   └── workflows/

│       └── ci.yml

├── helm/

│   └── api-bootcamp/

├── k8s/

├── src/

│   ├── ApiBootcamp/

│   ├── Application/

│   ├── Domain/

│   └── Infrastructure/

├── .dockerignore

├── .gitignore

├── Dockerfile

├── docker-compose.yml

├── api\_bootcamp.slnx

└── README.md





\## Manejo de errores

La aplicación utiliza un middleware centralizado para manejar excepciones.

Entre las respuestas controladas se encuentran:

400 - errores de validación

409 - conflictos de negocio

500 - errores inesperados



