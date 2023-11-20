FROM node:20-bullseye

WORKDIR /usr/dashboard
COPY package*.json ./

RUN npx browserslist@latest --update-db
RUN npm ci

COPY . .

RUN npm run build

EXPOSE 3000

CMD ["npm", "run", "dev"]
