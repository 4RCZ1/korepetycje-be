# Instrukcja dla nowicjuszy backendu systemu Tutorea

## Instalacja
> git clone <url niniejszego repozytorium>
> git checkout dev

Należy otworzyć w wybranym IDE (zalecane: JetBrains Rider) główny plik projektu czyli `korepetycje-be.sln`.
Przydatna jest wtyczka Entity Framework Core UI.
Trzeba zainstalować AWS SAM (Serverless Application Model), jeśli jeszcze nie ma się go w systemie.
Należy wybrać rejon `europe-central-1` (Frankfurt).
Należy być uwierzytelnionym w AWS IAM (Identity and Access Management).

## Kompilacja i uruchomienie

Aby sprawdzić, czy kod się kompiluje, wystarczy budowanie ze środowiska.
Potem, aby uruchomić projekt, uruchamia się komendę:

> sam build --parallel --no-cached && sam local start-api --warm-containers LAZY

Uruchomienie wymaga chodzącego Dockera (na Windowsie wystarczy uruchomić aplikację Docker Desktop).
Musi być ustawiona zmienna środowiskowa `S3_BUCKET_NAME` na nazwę (istniejącego)
bucket'a AWS S3.

Domyślnie pracuje się na chmurowej instancji PostgreSQL, w celu zamiany na lokalną
należy ustawić zmienną środowiskową `KOREPETYCJE_POSTGRES` na connection string'a do localhost.
Zamiast `localhost` należy tam podać `host.docker.internal`.
Instancja PostgreSQL powinna mieć wykonane migracje bazodanowe, co można
osiągnąć wtyczką Entity Framework Core UI, i sprawdzić w tabeli `__EFMigrationsHistory`.

## Przechadzka po repozytorium

`template.yaml` zawiera między listę endpointów (lambd AWS Lambda) i inne sprawy konfiguracyjne AWS.
`docs/` zawiera opisy niektórych endpointów jako pliki tekstowe.
`test/` i `src/` to foldery z kodem C#, odpowiednio testami jednostkowymi i implementacją.
Podfoldery `src/` są opisane poniżej.

### Moduły

- `Authentication` - komunikacja z Cognito.
- `Database` - klasy odpowiedzialne za operacje na bazie danych.
- `Database.Entities` - encje (klasy 1:1 z tabelami). Zmiany w nich wymagają utworzenia migracji w folderze `/src/Database/Migrations`, np. wspomnianą już wtyczką Entity Framework Core UI.
- `Endpoints` - moduł związany z AWS API Gateway. większość klas tutaj odpowiada 1:1 endpointom.
- `Endpoints.Dto` - schematy danych przesyłanych pomiędzy frontendem i backendem.
- `Endpoints.Interfaces` - interfejsy używane przez moduł Endpoints, implementowane w Authentication i Services.
- `FileStorage` - komunikacja z AWS S3.
- `Services` - kod logiki aplikacji.
- `Services.Interfaces` - interfejsy używane przez moduł Services, implementowane w Database i FileStorage.
