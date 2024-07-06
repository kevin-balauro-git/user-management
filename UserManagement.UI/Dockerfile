FROM node:latest AS build
WORKDIR /app
COPY ./UserManagement.UI/package*.json  ./
COPY ./UserManagement.UI/ .
RUN npm install 
RUN npm run build

FROM nginx:stable
COPY ./UserManagement.UI/nginx.conf ./etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/user-management.ui/browser /usr/share/nginx/html
EXPOSE 80