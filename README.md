# Instruções

1 - Fazer o clone de ambos os repositórios em uma pasta chamada deploy  
2 - Criar o docker-compose.yml dentro da pasta deploy com os dados no final desse arquivo  
3 - Rodar o comando docker-compose up -d

### Repositórios:

git clone https://github.com/lucasphill/cs-project-amplo-dadosambientais.git  
git clone https://github.com/lucasphill/react-project-amplo-dadosambientais.git

## 1 - Build individual com Dockerfile:

### API (requer postgres em localhost):

Path: "deploy\cs-project-amplo-dadosambientais\cs-project-amplo-dadosambientais"  
docker build -t api-amplo .  
docker run -d -p 80:80 api-amplo

### React:

Path: "deploy\react-project-amplo-dadosambientais"  
docker build -t react-amplo .  
docker run -d -p 80:80 react-amplo

### Build com docker-compose

Path: "deploy" (pasta raiz onde foi realizado git clone)  
docker-compose up -d

## 2 - Ubuntu (após instalação do docker):

mkdir deploy  
cd deploy/  
git clone https://github.com/lucasphill/cs-project-amplo-dadosambientais.git  
git clone https://github.com/lucasphill/react-project-amplo-dadosambientais.git  
sudo nano docker-compose.yml  
sudo nano react-project-amplo-dadosambientais/.env  
docker-compose up -d

ajustar o nginx para não ter erro ao recarregar a pagina  
docker exec -it `<codigo do container do front end>` sh  

apk add nano  
cd etc/nginx/conf.d/  
nano default.conf (conforme exemplo abaixo)  
exit  
reinicie o container  

```
location / {
    root   /usr/share/nginx/html;
    index  index.html index.htm;
    try_files $uri $uri/ /index.html;  # Adicione esta linha
}
```

```docker-compose
services:
  db:
    image: postgres:latest
    container_name: db
    ports:
      - 5432:5432
    environment:
      - POSTGRES_PASSWORD=postpassword
    networks:
      - app-network

  api:
    build:
      context: ./cs-project-amplo-dadosambientais/cs-project-amplo-dadosambientais
      dockerfile: Dockerfile
    container_name: api
    restart: always
    ports:
      - 8080:8080
    networks:
      - app-network
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__PsqlConnectionString=Host=db;Username=postgres;Password=postpassword;Database=amploengenharia
    depends_on:
      - db

  frontend:
    build:
      context: ./react-project-amplo-dadosambientais
      dockerfile: Dockerfile
    container_name: frontend
    ports:
      - 80:80
    networks:
      - app-network

networks:
  app-network:
    driver: bridge
```
