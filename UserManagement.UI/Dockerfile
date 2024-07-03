FROM node:22-alpine AS build
WORKDIR /app
COPY package*.json  ./
RUN npm ci
RUN npm install -g @angular/cli
COPY . .
RUN npm run build

FROM nginx:stable
COPY /nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/user-management.ui/browser /usr/share/nginx/html
EXPOSE 80