.PHONY: dev husky format api build remove 

dev:
	dotnet watch

husky:
	chmod +x formatter.sh
	
format:
	./formatter.sh .

api:
	clear; nodemon ./fake_api

build:
	clear; docker-compose up --build -d

remove:
	clear; docker-compose down
