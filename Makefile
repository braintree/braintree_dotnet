.PHONY: console mono core build build-mono build-core

console: mono

mono: build-mono
	docker run -it -v="$(PWD):/braintree-dotnet" --net="host" braintree-dotnet-mono /bin/bash -l

core: build-core
	docker run -it -v="$(PWD):/braintree-dotnet" --net="host" braintree-dotnet-core /bin/bash -l -c "dotnet restore;bash"

build: build-mono build-core

build-mono:
	docker build -t braintree-dotnet-mono -f Dockerfile-mono .

build-core:
	docker build -t braintree-dotnet-core -f Dockerfile-core .
