using Structurizr;
using Structurizr.Api;

namespace c4_model_design
{
    class Program
    {
        static void Main(string[] args)
        {
            RenderModels();
        }

        static void RenderModels()
        {
            const long workspaceId = 85947;
            const string apiKey = "b57c77e3-b8c6-4611-97f7-658da42d0a69";
            const string apiSecret = "1da66943-8509-48d0-9d34-5efe249f2dd3";

            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);

            Workspace workspace = new Workspace("C4 Model", "FastPorte");

            ViewSet viewSet = workspace.Views;

            Model model = workspace.Model;

            // 1. Diagrama de Contexto
            SoftwareSystem fastPorte = model.AddSoftwareSystem(
                "FastPorte",
                "Software que conectará transportistas de diferentes rubros (independientes o empresas) con personas (clientes o personas naturales) que requieren de sus servicios.");
            SoftwareSystem googleMaps = model.AddSoftwareSystem(
                "Google Maps", "Plataforma que ofrece una REST API de información geo referencial.");
            SoftwareSystem reniec = model.AddSoftwareSystem(
                "RENIEC System",
                "Organización autónoma del Estado Peruano encargada de la validación de datos.");
            SoftwareSystem email = model.AddSoftwareSystem(
                "E-mail System",
                "Sistema de confirmación de servicios mediante correo.");
            SoftwareSystem payment = model.AddSoftwareSystem(
                "Payment System",
                "Intermediario de pagos.");

            Person client =
                model.AddPerson("Client", "Persona que requiere de algún servicio de transporte.");
            Person carrier =
                model.AddPerson("Carrier", "Trabajadores el sector transporte que quieran brindar sus servicios.");
            Person admin = model.AddPerson("Admin", "Usuario administrador.");

            client.Uses(fastPorte, "Contacta con transportistas.");
            carrier.Uses(fastPorte, "Promociona sus servicios de transporte.");
            admin.Uses(fastPorte, "Usuario administrador del sistema.");

            fastPorte.Uses(googleMaps, "Usa la API de google maps");
            fastPorte.Uses(reniec, "Verifica datos de los usuarios");
            fastPorte.Uses(email, "Envia correos de confirmación de servicios");
            fastPorte.Uses(payment, "Realiza pagos mediante la aplicación");

            // Tags
            client.AddTags("Client");
            carrier.AddTags("Carrier");
            admin.AddTags("Admin");
            fastPorte.AddTags("FastPorte");
            googleMaps.AddTags("GoogleMaps");
            reniec.AddTags("RENIEC System");
            email.AddTags("E-mail System");
            payment.AddTags("Payment System");

            Styles styles = viewSet.Configuration.Styles;

            styles.Add(new ElementStyle("Client")
            {
                Background = "#0a60ff",
                Color = "#ffffff",
                Shape = Shape.Person
            });
            styles.Add(new ElementStyle("Carrier")
            {
                Background = "#0a60ff",
                Color = "#ffffff",
                Shape = Shape.Person
            });
            styles.Add(new ElementStyle("Admin")
            {
                Background = "#aa60af",
                Color = "#ffffff",
                Shape = Shape.Person
            });
            styles.Add(new ElementStyle("FastPorte")
            {
                Background = "#008f39",
                Color = "#ffffff",
                Shape = Shape.RoundedBox
            });
            styles.Add(new ElementStyle("GoogleMaps")
            {
                Background = "#90714c",
                Color = "#ffffff",
                Shape = Shape.RoundedBox
            });
            styles.Add(new ElementStyle("RENIEC System")
            {
                Background = "#7971eb",
                Color = "#ffffff",
                Shape = Shape.RoundedBox
            });
            styles.Add(new ElementStyle("E-mail System")
            {
                Background = "#2f95c7",
                Color = "#ffffff",
                Shape = Shape.RoundedBox
            });
            styles.Add(new ElementStyle("Payment System")
            {
                Background = "#f29549",
                Color = "#ffffff",
                Shape = Shape.RoundedBox
            });

            SystemContextView contextView =
                viewSet.CreateSystemContextView(fastPorte, "Contexto", "Diagrama de contexto");
            contextView.PaperSize = PaperSize.A4_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            // 2. Diagrama de Contenedores

            Container mobileApplication = fastPorte.AddContainer(
                "Mobile App",
                "los clientes podrán contratar los servicios de transporte desde su celular",
                "Flutter/Dart");
            Container webApplication = fastPorte.AddContainer(
                "Web App",
                "los clientes podrán contratar los servicios de transporte desde la web",
                "Angular");
            Container landingPage = fastPorte.AddContainer(
                "Landing Page",
                "los usuarios podrán visualizar las características de FastPorte",
                "Bootstrap");
            Container apiRest = fastPorte.AddContainer(
                "API REST",
                "API Rest",
                "Java - SpringBoot");

            Container personalData = fastPorte.AddContainer(
                "Información personal",
                "Informacion de usuario (mombre, edad, fotos, etc.)",
                "Angular");
            Container search = fastPorte.AddContainer(
                "Búsqueda de transportistas",
                "Búsqueda de transportistas según las características requeridas por el cliente",
                "Angular");
            Container hiring = fastPorte.AddContainer(
                "Contratación del servicio",
                "Contratación del servicio y pago del mismo",
                "Angular");
            Container location = fastPorte.AddContainer(
                "Ubicación del transportista",
                "Ubicación del transportista en tiempo real",
                "Angular");

            Container database = fastPorte.AddContainer("Database", "", "MySQL");

            client.Uses(mobileApplication, "Consulta");
            client.Uses(webApplication, "Consulta");
            client.Uses(landingPage, "Consulta");

            admin.Uses(mobileApplication, "Consulta");
            admin.Uses(webApplication, "Consulta");
            admin.Uses(landingPage, "Consulta");

            carrier.Uses(mobileApplication, "Consulta");
            carrier.Uses(webApplication, "Consulta");
            carrier.Uses(landingPage, "Consulta");

            mobileApplication.Uses(apiRest, "API Request", "JSON/HTTPS");
            webApplication.Uses(apiRest, "API Request", "JSON/HTTPS");

            apiRest.Uses(personalData, "", "");
            apiRest.Uses(search, "", "");
            apiRest.Uses(hiring, "", "");
            apiRest.Uses(location, "", "");

            personalData.Uses(database, "", "");
            search.Uses(database, "", "");
            hiring.Uses(database, "", "");
            location.Uses(database, "", "");

            location.Uses(googleMaps, "API Request", "JSON/HTTPS");

            hiring.Uses(payment, "API Request", "JSON/HTTPS");
            hiring.Uses(email, "API Request", "JSON/HTTPS");

            apiRest.Uses(reniec, "API Request", "JSON/HTTPS");

            // Tags
            mobileApplication.AddTags("MobileApp");
            webApplication.AddTags("WebApp");
            landingPage.AddTags("LandingPage");
            apiRest.AddTags("APIRest");
            database.AddTags("Database");

            string contextTag = "Context";

            personalData.AddTags(contextTag);
            search.AddTags(contextTag);
            hiring.AddTags(contextTag);
            location.AddTags(contextTag);

            styles.Add(new ElementStyle("MobileApp")
            {
                Background = "#9d33d6",
                Color = "#ffffff",
                Shape = Shape.MobileDevicePortrait,
                Icon = ""
            });
            styles.Add(new ElementStyle("WebApp")
            {
                Background = "#7be3af",
                Color = "#ffffff",
                Shape = Shape.WebBrowser,
                Icon = ""
            });
            styles.Add(new ElementStyle("LandingPage")
            {
                Background = "#929000",
                Color = "#ffffff",
                Shape = Shape.WebBrowser,
                Icon = ""
            });
            styles.Add(new ElementStyle("APIRest")
            {
                Background = "#0000ff",
                Color = "#ffffff",
                Shape = Shape.RoundedBox,
                Icon = ""
            });
            styles.Add(new ElementStyle("Database")
            {
                Background = "#ff0000",
                Color = "#ffffff",
                Shape = Shape.Cylinder,
                Icon = ""
            });
            styles.Add(new ElementStyle(contextTag)
            {
                Background = "#facc2e",
                Shape = Shape.Hexagon,
                Icon = ""
            });

            ContainerView containerView =
                viewSet.CreateContainerView(fastPorte, "Contenedor", "Diagrama de contenedores");
            contextView.PaperSize = PaperSize.A4_Landscape;
            containerView.AddAllElements();

            // 3. Diagrama de Componentes

            // Hiring

            Component domainLayerH =
                hiring.AddComponent("Domain Layer", "", "Angular");
            Component hiringController = hiring.AddComponent(
                "HiringController",
                "REST API endpoints de contratación y pagos de servicios.",
                "REST Controller");
            Component hiringApplicationService = hiring.AddComponent(
                "HiringApplicationService",
                "Provee métodos para la contración de servicios de transporte.",
                "Component");
            Component paymentApplicationService = hiring.AddComponent(
                "PaymentApplicationService",
                "Provee métodos para el pago de contratos",
                "Component");
            Component hiringRepository = hiring.AddComponent(
                "HiringRepository",
                "Información de los contratos",
                "Component");
            Component paymentFacade =
                hiring.AddComponent("Payment Facade", "", "Component");

            apiRest.Uses(hiringController, "", "JSON/HTTPS");

            hiringController.Uses(paymentApplicationService, "Invoca métodos de pago de contratos", "");
            hiringController.Uses(hiringApplicationService, "Invoca métodos de contratación de servicios de trasnporte", "");

            hiringApplicationService.Uses(domainLayerH, "Usa", "");

            paymentApplicationService.Uses(domainLayerH, "Usa", "");
            paymentApplicationService.Uses(paymentFacade, "Usa");
            paymentApplicationService.Uses(hiringRepository, "", "");

            hiringRepository.Uses(database, "", "");

            paymentFacade.Uses(payment, "JSON/HTTPS");

            // Tags
            domainLayerH.AddTags("DomainLayerH");
            hiringController.AddTags("HiringController");
            paymentApplicationService.AddTags("PaymentApplicationService");
            hiringApplicationService.AddTags("HiringApplicationService");
            hiringRepository.AddTags("PaymentRepository");
            paymentFacade.AddTags("PaymentFacade");

            styles.Add(new ElementStyle("DomainLayerCE")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("HiringController")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("PaymentApplicationService")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("HiringApplicationService")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("PaymentRepository")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("PaymentFacade")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });

            ComponentView componentViewH =
                viewSet.CreateComponentView(hiring, "Hiring", "Component Diagram");
            componentViewH.PaperSize = PaperSize.A4_Landscape;
            componentViewH.Add(webApplication);
            componentViewH.Add(apiRest);
            componentViewH.Add(database);
            componentViewH.Add(payment);
            componentViewH.Add(googleMaps);
            componentViewH.AddAllComponents();

            // Search

            Component domainLayerS =
                search.AddComponent("Domain Layer", "", "Angular");
            Component searchController = search.AddComponent(
                "SearchController",
                "REST API endpoints de búsqueda de servicios.",
                "REST Controller");
            Component searchService = search.AddComponent(
                "SearchApplicationService",
                "Provee métodos para la búsqueda de servicios",
                "Component");
            Component transportServiceRepository = search.AddComponent(
                "TransportServiceRepository",
                "Información de los servicios de transporte",
                "Component");

            apiRest.Uses(searchController, "", "JSON/HTTPS");
            searchController.Uses(searchService, "Invoca métodos de búsqueda");

            searchService.Uses(domainLayerS, "Usa", "");
            searchService.Uses(transportServiceRepository, "", "");

            transportServiceRepository.Uses(database, "", "");

            // Tags
            domainLayerS.AddTags("DomainLayerS");
            searchController.AddTags("SearchController");
            searchService.AddTags("SearchApplicationService");
            transportServiceRepository.AddTags("TransportServiceRepository");

            styles.Add(new ElementStyle("DomainLayerS")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("searchController")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("searchApplicationService")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("transportServiceRepository")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });

            ComponentView componentViewS = viewSet.CreateComponentView(search, "Search", "Component Diagram");
            componentViewS.PaperSize = PaperSize.A4_Landscape;
            componentViewS.Add(webApplication);
            componentViewS.Add(apiRest);
            componentViewS.Add(database);
            componentViewS.AddAllComponents();

            // Location

            Component domainLayerL = location.AddComponent("Domain Layer", "", "");
            Component locationController = location.AddComponent(
                "SeguridadController",
                "REST API endpoints de ubicación.",
                "REST Controller");
            Component locationService = location.AddComponent(
                "LocationApplicationService",
                "Provee métodos para la localización del usuario transportista.",
                "Component");
            Component carrierRepository = location.AddComponent(
                "CarrierRepository",
                "Información de los usuarios transportistas",
                "Component");

            apiRest.Uses(locationController, "", "JSON/HTTPS");
            locationController.Uses(locationService, "Invoca métodos de localización");

            locationService.Uses(domainLayerL, "Usa", "");
            locationService.Uses(carrierRepository, "", "");

            carrierRepository.Uses(database, "", "");

            carrierRepository.Uses(googleMaps, "", "JSON/HTTPS");

            // Tags
            domainLayerL.AddTags("DomainLayerL");
            locationController.AddTags("LocationController");
            locationService.AddTags("LocationApplicationService");
            carrierRepository.AddTags("CarrierRepository");

            styles.Add(new ElementStyle("DomainLayerL")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("LocationController")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("LocationApplicationService")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("CarrierRepository")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });

            ComponentView componentViewL =
                viewSet.CreateComponentView(location, "Location", "Component Diagram");
            componentViewL.PaperSize = PaperSize.A4_Landscape;
            componentViewL.Add(webApplication);
            componentViewL.Add(apiRest);
            componentViewL.Add(database);
            componentViewL.Add(googleMaps);
            componentViewL.AddAllComponents();

            // Personal Data

            Component domainLayerPD = personalData.AddComponent("Domain Layer", "", "");
            Component personalDataController = personalData.AddComponent(
                "PersonalDataController",
                "REST API endpoints de información personal del usuario.",
                "REST Controller");
            Component personalDataService = personalData.AddComponent(
                "PersonalDatApplicationService",
                "Provee métodos para el apartado de información personal del usuario",
                "Component");
            Component carrierPDRepository = personalData.AddComponent(
                "CarrierRepository",
                "Información personal de los usuarios transportistas",
                "Component");
            Component clientPDRepository = personalData.AddComponent(
                "ClientRepository",
                "Información personal de los usuarios clientes",
                "Component");

            apiRest.Uses(personalDataController, "", "JSON/HTTPS");
            personalDataController.Uses(personalDataService, "Invoca métodos de información personal");

            personalDataService.Uses(domainLayerPD, "Usa", "");
            personalDataService.Uses(carrierPDRepository, "", "");
            personalDataService.Uses(clientPDRepository, "", "");

            carrierPDRepository.Uses(database, "", "");
            clientPDRepository.Uses(database, "", "");

            // Tags
            domainLayerPD.AddTags("DomainLayerPD");
            personalDataController.AddTags("PersonalDataController");
            personalDataService.AddTags("PersonalDataApplicationService");
            carrierPDRepository.AddTags("CarrierPDRepository");
            clientPDRepository.AddTags("ClientPDRepository");

            styles.Add(new ElementStyle("DomainLayerPD")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("PersonalDataController")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("PersonalDataApplicationService")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("CarrierPDRepository")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });
            styles.Add(new ElementStyle("ClientPDRepository")
            {
                Shape = Shape.Component,
                Background = "#facc2e",
                Icon = ""
            });

            ComponentView componentViewPD = viewSet.CreateComponentView(
                personalData, "Información personal", "Component Diagram");
            componentViewPD.PaperSize = PaperSize.A4_Landscape;
            componentViewPD.Add(webApplication);
            componentViewPD.Add(apiRest);
            componentViewPD.Add(database);
            componentViewPD.AddAllComponents();

            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}
