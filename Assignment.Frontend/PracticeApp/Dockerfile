# Use the official Node.js image for building the application
FROM node:18 AS build
WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application files
COPY . ./

# Build the Angular application
RUN npm run build --prod

# Use the official Nginx image for serving the application
FROM nginx:alpine AS runtime
WORKDIR /usr/share/nginx/html

# Copy the built application files
COPY --from=build /app/dist /usr/share/nginx/html

# Expose the default Nginx port
EXPOSE 80

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]