FROM node:12.13.1-slim AS build
WORKDIR /src
COPY . .
RUN npm install --registry=https://registry.npm.taobao.org 
RUN npm run build:prod
FROM nginx:latest
WORKDIR /app
COPY --from=build /src/dist /usr/share/nginx/html/