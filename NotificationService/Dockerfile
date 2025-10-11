# NotificationService/Dockerfile
FROM node:20-alpine

WORKDIR /usr/src/app

# Copy package files first for caching
COPY package*.json ./

# Install only production dependencies in image
RUN npm ci --omit=dev

# Copy application code
COPY . .

# Expose dev port (Heroku assigns real port via $PORT)
EXPOSE 5001

# Start the app (Heroku will set PORT env var)
CMD ["node", "index.js"]
