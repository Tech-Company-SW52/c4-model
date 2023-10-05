using Structurizr;
using Structurizr.Api;

namespace c4_model_design
{
    class Program
    {
        private const string notString = "";

        static void Main(string[] args)
        {
            RenderModels();
        }

        static void RenderModels()
        {
            const long workspaceId = 86156;
            const string apiKey = "54ba985e-1536-4856-9c02-01b75136f29c";
            const string apiSecret = "7bab3af8-2cd8-4cf2-bba9-e1e4044f6bc9";

            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);

            Workspace workspace = new Workspace("C4-Structurizer Model", "FastPorte");

            ViewSet viewSet = workspace.Views;

            Model model = workspace.Model;

            // Diagrama de Contexto para la Arquitectura de Microblogging

            SoftwareSystem microbloggingPlatform = model.AddSoftwareSystem(
                "Microblogging Platform",
                "Plataforma descentralizada de microblogging para compartir conocimiento en grupos empresariales o comunidades."
            );

            SoftwareSystem apiGateway = model.AddSoftwareSystem(
                "API Gateway",
                "Interfaz para el acceso a la plataforma mediante diferentes tipos de usuarios y conexiones remotas.");

            SoftwareSystem rubyBackend = model.AddSoftwareSystem(
                "Ruby Backend",
                "Módulos en Ruby para la lógica de negocio, respuestas, etiquetado, moderación, etc.");

            SoftwareSystem postgresDatabase = model.AddSoftwareSystem(
                "PostgreSQL Database",
                "Base de datos principal para almacenar usuarios y posts.");

            SoftwareSystem redisServer = model.AddSoftwareSystem(
                "Redis Server",
                "Servidor de cache y almacenamiento de datos para los jobs del sistema.");

            SoftwareSystem sideKiq = model.AddSoftwareSystem(
                "SideKiq",
                "Framework de Ruby para manejar jobs del sistema.");

            SoftwareSystem elasticsearch = model.AddSoftwareSystem(
                "Amazon ElasticSearch",
                "Servicio para indexar y buscar posts.");

            SoftwareSystem syncServer = model.AddSoftwareSystem(
                "Sync Server",
                "Servidor para manejar pedidos de Stream de sincronización de datos.");

            SoftwareSystem fileStorage = model.AddSoftwareSystem(
                "File Storage",
                "Almacenamiento de archivos anexados a los posts.");

            Person webUser = model.AddPerson("Web User", "Usuario que accede a la plataforma a través de la interfase web.");
            Person mobileUser = model.AddPerson("Mobile User", "Usuario que accede a la plataforma a través de la interfase móvil.");
            Person apiUser = model.AddPerson("API User", "Usuario con conexión de API.");
            Person remoteInstanceUser = model.AddPerson("Remote Instance User", "Usuario en una instancia remota de Microblogging.");

            webUser.Uses(apiGateway, "Accede a la plataforma");
            mobileUser.Uses(apiGateway, "Accede a la plataforma");
            apiUser.Uses(apiGateway, "Accede a la plataforma");
            remoteInstanceUser.Uses(apiGateway, "Interactúa con otras instancias");

            apiGateway.Uses(rubyBackend, "Procesa peticiones");
            rubyBackend.Uses(postgresDatabase, "Almacena y recupera datos");
            rubyBackend.Uses(redisServer, "Cachea datos y maneja jobs");
            rubyBackend.Uses(sideKiq, "Programa y maneja jobs");
            rubyBackend.Uses(elasticsearch, "Indexa y busca posts");
            rubyBackend.Uses(syncServer, "Sincroniza datos con otras instancias");
            rubyBackend.Uses(fileStorage, "Almacena archivos");

            // Tags
            webUser.AddTags("Web User");
            mobileUser.AddTags("Mobile User");
            apiUser.AddTags("API User");
            remoteInstanceUser.AddTags("Remote Instance User");
            microbloggingPlatform.AddTags("Microblogging Platform");
            apiGateway.AddTags("API Gateway");
            rubyBackend.AddTags("Ruby Backend");
            postgresDatabase.AddTags("PostgreSQL Database");
            redisServer.AddTags("Redis Server");
            sideKiq.AddTags("SideKiq");
            elasticsearch.AddTags("Amazon ElasticSearch");
            syncServer.AddTags("Sync Server");
            fileStorage.AddTags("File Storage");

            Styles styles = viewSet.Configuration.Styles;

            styles.Add(new ElementStyle("Web User") { Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("Mobile User") { Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("API User") { Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("Remote Instance User") { Background = "#aa60af", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("Microblogging Platform") { Background = "#008f39", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("API Gateway") { Background = "#90714c", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("Ruby Backend") { Background = "#7971eb", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("PostgreSQL Database") { Background = "#2f95c7", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("Redis Server") { Background = "#f29549", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("SideKiq") { Background = "#f2d349", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("Amazon ElasticSearch") { Background = "#7f8c8d", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("Sync Server") { Background = "#c0392b", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("File Storage") { Background = "#d35400", Color = "#ffffff", Shape = Shape.RoundedBox });
            SystemContextView contextView =
                viewSet.CreateSystemContextView(microbloggingPlatform, "Contexto", "Diagrama de contexto de la Arquitectura de Microblogging");
            contextView.PaperSize = PaperSize.A4_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();
            // Diagrama de Contenedores
            Container mobileApplication = microbloggingPlatform.AddContainer(
                "Mobile App",
                "Los usuarios podrán interactuar con la plataforma de microblogging desde su celular",
                "Flutter/Dart");
            Container webApplication = microbloggingPlatform.AddContainer(
                "Web App",
                "Los usuarios podrán interactuar con la plataforma de microblogging desde la web",
                "Angular");
            Container apiGatewayContainer = microbloggingPlatform.AddContainer(
                "API Gateway",
                "API para manejar las peticiones de los clientes y comunicarse con los módulos del backend",
                "Java - SpringBoot");

            Container postsModule = microbloggingPlatform.AddContainer(
                "Módulo de Posts",
                "Gestión de creación, edición y eliminación de posts",
                "Ruby");
            Container responsesModule = microbloggingPlatform.AddContainer(
                "Módulo de Respuestas",
                "Gestión de respuestas a posts y a otras respuestas",
                "Ruby");
            Container usersModule = microbloggingPlatform.AddContainer(
                "Módulo de Usuarios",
                "Gestión de información de usuarios",
                "Ruby");
            Container notificationsModule = microbloggingPlatform.AddContainer(
                "Módulo de Notificaciones",
                "Gestión de notificaciones",
                "Ruby");
            Container moderationModule = microbloggingPlatform.AddContainer(
                "Módulo de Moderación",
                "Gestión de reglas de moderación y reportes",
                "Ruby");

            Container database = microbloggingPlatform.AddContainer("Database", "Almacenamiento de datos", "PostgreSQL");
            Container cacheServer = microbloggingPlatform.AddContainer("Cache Server", "Servidor de cache y jobs", "Redis");
            Container searchService = microbloggingPlatform.AddContainer("Search Service", "Servicio de búsqueda de posts", "Amazon ElasticSearch");
            Container syncService = microbloggingPlatform.AddContainer("Sync Service", "Servicio de sincronización de datos", "Node.js");
            Container fileStorageContainer = microbloggingPlatform.AddContainer(
    "File Storage",
    "Almacenamiento de archivos multimedia",
    "Amazon S3");
            webUser.Uses(mobileApplication, "Interactúa");
            webUser.Uses(webApplication, "Interactúa");

            mobileApplication.Uses(apiGateway, "API Request", "JSON/HTTPS");
            webApplication.Uses(apiGateway, "API Request", "JSON/HTTPS");

            apiGateway.Uses(postsModule, "API Request", "JSON/HTTPS");
            apiGateway.Uses(responsesModule, "API Request", "JSON/HTTPS");
            apiGateway.Uses(usersModule, "API Request", "JSON/HTTPS");
            apiGateway.Uses(notificationsModule, "API Request", "JSON/HTTPS");
            apiGateway.Uses(moderationModule, "API Request", "JSON/HTTPS");

            postsModule.Uses(database, "CRUD Operations");
            responsesModule.Uses(database, "CRUD Operations");
            usersModule.Uses(database, "CRUD Operations");
            notificationsModule.Uses(database, "CRUD Operations");
            moderationModule.Uses(database, "CRUD Operations");

            postsModule.Uses(cacheServer, "Cache Operations");
            responsesModule.Uses(cacheServer, "Cache Operations");
            notificationsModule.Uses(cacheServer, "Cache Operations");

            postsModule.Uses(searchService, "Index/Query", "JSON/HTTPS");
            searchService.Uses(database, "Query");

            postsModule.Uses(syncService, "Sync Data", "Websockets/HTTPS");
            responsesModule.Uses(syncService, "Sync Data", "Websockets/HTTPS");

            postsModule.Uses(fileStorage, "Store/Retrieve Files", "HTTPS");
            responsesModule.Uses(fileStorage, "Store/Retrieve Files", "HTTPS");
            // Tags
            mobileApplication.AddTags("MobileApp");
            webApplication.AddTags("WebApp");
            apiGateway.AddTags("APIGateway");
            postsModule.AddTags("PostsModule");
            responsesModule.AddTags("ResponsesModule");
            usersModule.AddTags("UsersModule");
            notificationsModule.AddTags("NotificationsModule");
            moderationModule.AddTags("ModerationModule");
            database.AddTags("Database");
            cacheServer.AddTags("CacheServer");
            searchService.AddTags("SearchService");
            syncService.AddTags("SyncService");
            fileStorage.AddTags("FileStorage");

            string contextTag = "Context";

            postsModule.AddTags(contextTag);
            responsesModule.AddTags(contextTag);
            usersModule.AddTags(contextTag);
            notificationsModule.AddTags(contextTag);
            moderationModule.AddTags(contextTag);

            // Estilos
            styles.Add(new ElementStyle("MobileApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.MobileDevicePortrait, Icon = notString });
            styles.Add(new ElementStyle("WebApp") { Background = "#7be3af", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = notString });
            styles.Add(new ElementStyle("APIGateway") { Background = "#0000ff", Color = "#ffffff", Shape = Shape.RoundedBox, Icon = notString });
            styles.Add(new ElementStyle("Database") { Background = "#ff0000", Color = "#ffffff", Shape = Shape.Cylinder, Icon = notString });
            styles.Add(new ElementStyle("CacheServer") { Background = "#f29549", Color = "#ffffff", Shape = Shape.Cylinder, Icon = notString });
            styles.Add(new ElementStyle("SearchService") { Background = "#7f8c8d", Color = "#ffffff", Shape = Shape.RoundedBox, Icon = notString });
            styles.Add(new ElementStyle("SyncService") { Background = "#c0392b", Color = "#ffffff", Shape = Shape.RoundedBox, Icon = notString });
            styles.Add(new ElementStyle("FileStorage") { Background = "#d35400", Color = "#ffffff", Shape = Shape.RoundedBox, Icon = notString });
            styles.Add(new ElementStyle("PostsModule") { Background = "#facc2e", Shape = Shape.Hexagon, Icon = notString });
            styles.Add(new ElementStyle("ResponsesModule") { Background = "#facc2e", Shape = Shape.Hexagon, Icon = notString });
            styles.Add(new ElementStyle("UsersModule") { Background = "#facc2e", Shape = Shape.Hexagon, Icon = notString });
            styles.Add(new ElementStyle("NotificationsModule") { Background = "#facc2e", Shape = Shape.Hexagon, Icon = notString });
            styles.Add(new ElementStyle("ModerationModule") { Background = "#facc2e", Shape = Shape.Hexagon, Icon = notString });

            ContainerView containerView =
                viewSet.CreateContainerView(microbloggingPlatform, "Contenedor", "Diagrama de contenedores para la plataforma de Microblogging");
            containerView.PaperSize = PaperSize.A4_Landscape;
            containerView.AddAllElements();

            // 3. Diagrama de Componentes

            // Posts Module
            Component domainLayerP = postsModule.AddComponent("Domain Layer", "", "Ruby");
            Component postsController = postsModule.AddComponent("PostsController", "REST API endpoints de posts.", "REST Controller");
            Component postsApplicationService = postsModule.AddComponent("PostsApplicationService", "Provee métodos para crear y gestionar posts.", "Component");
            Component postsRepository = postsModule.AddComponent("PostsRepository", "Información de los posts", "Component");

            apiGateway.Uses(postsController, "", "JSON/HTTPS");
            postsController.Uses(postsApplicationService, "Invoca métodos de gestión de posts");
            postsApplicationService.Uses(domainLayerP, "Usa", "");
            postsApplicationService.Uses(postsRepository, "", "");
            postsRepository.Uses(database, "", "");

            // Tags y Styles para Posts Module
            domainLayerP.AddTags("DomainLayerP");
            postsController.AddTags("PostsController");
            postsApplicationService.AddTags("PostsApplicationService");
            postsRepository.AddTags("PostsRepository");

            styles.Add(new ElementStyle("DomainLayerP") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PostsController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PostsApplicationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PostsRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            // Responses Module
            Component domainLayerR = responsesModule.AddComponent("Domain Layer", "", "Ruby");
            Component responsesController = responsesModule.AddComponent("ResponsesController", "REST API endpoints de respuestas.", "REST Controller");
            Component responsesApplicationService = responsesModule.AddComponent("ResponsesApplicationService", "Provee métodos para crear y gestionar respuestas.", "Component");
            Component responsesRepository = responsesModule.AddComponent("ResponsesRepository", "Información de las respuestas", "Component");

            apiGateway.Uses(responsesController, "", "JSON/HTTPS");
            responsesController.Uses(responsesApplicationService, "Invoca métodos de gestión de respuestas");
            responsesApplicationService.Uses(domainLayerR, "Usa", "");
            responsesApplicationService.Uses(responsesRepository, "", "");
            responsesRepository.Uses(database, "", "");

            // Tags y Styles para Responses Module
            domainLayerR.AddTags("DomainLayerR");
            responsesController.AddTags("ResponsesController");
            responsesApplicationService.AddTags("ResponsesApplicationService");
            responsesRepository.AddTags("ResponsesRepository");

            styles.Add(new ElementStyle("DomainLayerR") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ResponsesController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ResponsesApplicationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ResponsesRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            // Users Module
            Component domainLayerU = usersModule.AddComponent("Domain Layer", "", "Ruby");
            Component usersController = usersModule.AddComponent("UsersController", "REST API endpoints de usuarios.", "REST Controller");
            Component usersApplicationService = usersModule.AddComponent("UsersApplicationService", "Provee métodos para crear y gestionar usuarios.", "Component");
            Component usersRepository = usersModule.AddComponent("UsersRepository", "Información de los usuarios", "Component");

            apiGateway.Uses(usersController, "", "JSON/HTTPS");
            usersController.Uses(usersApplicationService, "Invoca métodos de gestión de usuarios");
            usersApplicationService.Uses(domainLayerU, "Usa", "");
            usersApplicationService.Uses(usersRepository, "", "");
            usersRepository.Uses(database, "", "");

            // Notifications Module
            Component domainLayerN = notificationsModule.AddComponent("Domain Layer", "", "Ruby");
            Component notificationsController = notificationsModule.AddComponent("NotificationsController", "REST API endpoints de notificaciones.", "REST Controller");
            Component notificationsApplicationService = notificationsModule.AddComponent("NotificationsApplicationService", "Provee métodos para gestionar notificaciones.", "Component");
            Component notificationsRepository = notificationsModule.AddComponent("NotificationsRepository", "Información de las notificaciones", "Component");

            apiGateway.Uses(notificationsController, "", "JSON/HTTPS");
            notificationsController.Uses(notificationsApplicationService, "Invoca métodos de gestión de notificaciones");
            notificationsApplicationService.Uses(domainLayerN, "Usa", "");
            notificationsApplicationService.Uses(notificationsRepository, "", "");
            notificationsRepository.Uses(database, "", "");

            // Moderation Module
            Component domainLayerM = moderationModule.AddComponent("Domain Layer", "", "Ruby");
            Component moderationController = moderationModule.AddComponent("ModerationController", "REST API endpoints de moderación.", "REST Controller");
            Component moderationApplicationService = moderationModule.AddComponent("ModerationApplicationService", "Provee métodos para moderar contenido.", "Component");
            Component moderationRepository = moderationModule.AddComponent("ModerationRepository", "Información de moderación", "Component");

            apiGateway.Uses(moderationController, "", "JSON/HTTPS");
            moderationController.Uses(moderationApplicationService, "Invoca métodos de moderación");
            moderationApplicationService.Uses(domainLayerM, "Usa", "");
            moderationApplicationService.Uses(moderationRepository, "", "");
            moderationRepository.Uses(database, "", "");

            // Interacciones con otros contenedores
            searchService.Uses(postsRepository, "Busca posts");
            searchService.Uses(usersRepository, "Busca usuarios");
            syncService.Uses(database, "Sincroniza datos");
            cacheServer.Uses(database, "Cachea datos");
            fileStorage.Uses(apiGateway, "Almacena archivos");

            // Tags
            searchService.AddTags("SearchService");
            syncService.AddTags("SyncService");
            cacheServer.AddTags("CacheServer");
            fileStorage.AddTags("FileStorage");

            // Component Views
            ComponentView componentViewPosts = viewSet.CreateComponentView(postsModule, "Posts", "Component Diagram");
            componentViewPosts.PaperSize = PaperSize.A4_Landscape;
            componentViewPosts.Add(apiGateway);
            componentViewPosts.Add(database);
            componentViewPosts.AddAllComponents();

            ComponentView componentViewResponses = viewSet.CreateComponentView(responsesModule, "Responses", "Component Diagram");
            componentViewResponses.PaperSize = PaperSize.A4_Landscape;
            componentViewResponses.Add(apiGateway);
            componentViewResponses.Add(database);
            componentViewResponses.AddAllComponents();

            ComponentView componentViewUsers = viewSet.CreateComponentView(usersModule, "Users", "Component Diagram");
            componentViewUsers.PaperSize = PaperSize.A4_Landscape;
            componentViewUsers.Add(apiGateway);
            componentViewUsers.Add(database);
            componentViewUsers.AddAllComponents();

            ComponentView componentViewNotifications = viewSet.CreateComponentView(notificationsModule, "Notifications", "Component Diagram");
            componentViewNotifications.PaperSize = PaperSize.A4_Landscape;
            componentViewNotifications.Add(apiGateway);
            componentViewNotifications.Add(database);
            componentViewNotifications.AddAllComponents();

            ComponentView componentViewModeration = viewSet.CreateComponentView(moderationModule, "Moderation", "Component Diagram");
            componentViewModeration.PaperSize = PaperSize.A4_Landscape;
            componentViewModeration.Add(apiGateway);
            componentViewModeration.Add(database);
            componentViewModeration.AddAllComponents();

            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}
